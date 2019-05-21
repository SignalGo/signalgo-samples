import 'dart:async';

import "package:threading/threading.dart";

class ConcurrentBlockingCollection<T> {
  ConcurrentBlockingCollection() {
    _taskCompletionSource = createNewTask();
  }

  Lock _addLock = new Lock();
  Lock _takeLock = new Lock();

  List<Object> _items = new List<Object>();
  Completer _taskCompletionSource;
  bool _isTakeTaskResult = false;
  int count() {
    return _items.length;
  }

  Future addAsync(T item) async {
    try {
      if (_isCanceled) return;
      checkDispose();
      await _addLock.acquire();
      checkDispose();
      _items.add(item);
      //Console.WriteLine("added" + item);
      if (!_taskCompletionSource.isCompleted) {
        completeTask();
      } else {
        //Console.WriteLine("wrong status 2 : " + _taskCompletionSource.Task.Status);
      }
    } finally {
      _addLock.release();
    }
  }

  Completer<T> createNewTask() {
    Completer<T> tcs = new Completer<T>();
    return tcs;
  }

  void completeTask() {
    if (_isCanceled && _items.length == 0) return;
    if (!_taskCompletionSource.isCompleted) {
      Object find = _items.length == 0 ? null : _items.first;
      if (find != null) {
        _items.removeAt(0);
        _taskCompletionSource.complete(find);
      }
    } else {
      if (_isTakeTaskResult) {
        Object find = _items.length == 0 ? null : _items.first;
        if (find != null) {
          _items.removeAt(0);
          _taskCompletionSource = createNewTask();
          _taskCompletionSource.complete(find);
        } else
          _taskCompletionSource = createNewTask();
        _isTakeTaskResult = false;
      }
    }
  }

  bool _isCanceled = false;
  Future cancelAsync() async {
    try {
      checkDispose();
      await _addLock.acquire();
      checkDispose();
      _isCanceled = true;
      Object find = _items.length == 0 ? null : _items.first;
      _taskCompletionSource.complete(find);
    } finally {
      _addLock.release();
    }
  }

  Future<T> takeAsync() async {
    try {
      checkDispose();
      await _addLock.acquire();
      checkDispose();
      completeTask();
    } finally {
      _addLock.release();
    }

    try {
      checkDispose();
      await _takeLock.acquire();
      checkDispose();
      Future<T> result = _taskCompletionSource.future;
      _isTakeTaskResult = true;
      return await result;
    } finally {
      _takeLock.release();
    }

    //CompleteTask();
  }

  bool isDispose = false;
  void dispose() {
    _isCanceled = true;
    isDispose = true;
    _addLock.release();
    _takeLock.release();
    if (_taskCompletionSource != null)
      _taskCompletionSource.completeError(new Exception("disposed"));
  }

  void checkDispose() {
    if (isDispose) throw new Exception("disposed");
  }
}
