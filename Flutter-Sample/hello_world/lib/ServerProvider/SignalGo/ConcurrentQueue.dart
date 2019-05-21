import 'package:threading/threading.dart';

class ConcurrentQueue<T> {
  Lock lock = new Lock();
  List<T> _items = new List<T>();
  bool isEmpty() {
    return _items.length == 0;
  }

  Future enqueue(T item) async {
    try {
      await lock.tryAcquire();
      _items.add(item);
    } finally {
      await lock.release();
    }
  }

  Future<T> peek() async {
    try {
      await lock.tryAcquire();
      if (isEmpty()) return null;
      return _items.first;
    } finally {
      await lock.release();
    }
  }

  Future dequeue(T item) async {
    try {
      await lock.tryAcquire();
      if (isEmpty()) return null;
      _items.remove(item);
    } finally {
      await lock.release();
    }
  }
    void dispose() {
    _items.clear();
    lock.release();
  }
}
