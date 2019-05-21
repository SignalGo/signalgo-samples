import 'package:hello_world/ServerProvider/PostJsonToServerService.dart';
import 'package:hello_world/ServerProvider/SignalGo/BufferSegment.dart';
import 'package:hello_world/ServerProvider/SignalGo/ConcurrentBlockingCollection.dart';
import 'package:hello_world/ServerProvider/SignalGo/ConcurrentQueue.dart';
import 'package:hello_world/ServerProvider/SignalGo/Tuple.dart';
import 'package:hello_world/ServerProvider/SignalGo/enums.dart';
import 'package:threading/threading.dart';
import 'dart:convert';

class PipeNetworkStream {
  int bufferToRead = 512;
  PipeNetworkStream() {
    blockBuffers = new ConcurrentBlockingCollection<BufferSegment>();
  }

  bool isClosed = false;

  ConcurrentBlockingCollection<BufferSegment> blockBuffers;
  ConcurrentQueue<BufferSegment> queueBuffers =
      new ConcurrentQueue<BufferSegment>();

  void saveNewData(List<int> data) async {
    await blockBuffers.addAsync(new BufferSegment()
      ..buffer = data
      ..position = 0);
  }

  bool isWaitToRead = false;

  Lock lockWaitToRead = new Lock();

  Future<Tuple2<List<int>, int>> readAsync(int count) async {
    if (isClosed && queueBuffers.isEmpty() && blockBuffers.count() == 0)
      throw new Exception("read zero buffer! client disconnected");

    BufferSegment bufferSegment;
    if (queueBuffers.isEmpty()) {
      bufferSegment = await blockBuffers.takeAsync();
      if (isClosed && blockBuffers.count() == 0)
        throw new Exception("read zero buffer! client disconnected");
      await queueBuffers.enqueue(bufferSegment);
    } else {
      bufferSegment = await queueBuffers.peek();
      if (bufferSegment == null) return await readAsync(count);
    }

    if (bufferSegment.isFinished()) {
      await queueBuffers.dequeue(bufferSegment);
      return await readAsync(count);
    } else {
      var readBytes = bufferSegment.readBufferSegment(count);
      if (bufferSegment.isFinished()) await queueBuffers.dequeue(bufferSegment);
      return readBytes;
    }
  }

  Future<Tuple2<List<int>, int>> readExistingAsync(List<int> exitBytes) async {
    if (isClosed && queueBuffers.isEmpty() && blockBuffers.count() == 0)
      throw new Exception("read zero buffer! client disconnected");
    Tuple2<List<int>, int> result = new Tuple2<List<int>, int>();
    result.value1 = new List<int>();

    while (true) {
      BufferSegment bufferSegment;
      if (queueBuffers.isEmpty()) {
        bufferSegment = await blockBuffers.takeAsync();

        if (isClosed && blockBuffers.count() == 0)
          throw new Exception("read zero buffer! client disconnected");
        await queueBuffers.enqueue(bufferSegment);
      } else {
        bufferSegment = await queueBuffers.peek();
        if (bufferSegment == null) return await readExistingAsync(exitBytes);
      }

      if (bufferSegment.isFinished()) {
        await queueBuffers.dequeue(bufferSegment);
        return await readExistingAsync(exitBytes);
      } else {
        if (result.value1.length > 0 &&
            result.value1.last == exitBytes.first &&
            bufferSegment.whatIsFirstByte() == exitBytes.last)
          exitBytes = [exitBytes.last];
        var readNew = bufferSegment.read(exitBytes);
        result.value1.addAll(readNew.value1);
        if (bufferSegment.isFinished())
          await queueBuffers.dequeue(bufferSegment);
        if (readNew.value2) break;
      }
    }
    return result;
  }

  Future<int> readOneByteAsync() async {
    if (isClosed && queueBuffers.isEmpty() && blockBuffers.count() == 0)
      throw new Exception("read zero buffer! client disconnected");
    BufferSegment result;
    if (queueBuffers.isEmpty()) {
      result = await blockBuffers.takeAsync();

      if (isClosed && blockBuffers.count() == 0)
        throw new Exception("read zero buffer! client disconnected");
      await queueBuffers.enqueue(result);
    } else {
      result = await queueBuffers.peek();
      if (result == null) return await readOneByteAsync();
    }

    if (result.isFinished()) {
      await queueBuffers.dequeue(result);
      return await readOneByteAsync();
    } else {
      var b = result.readFirstByte();
      if (result.isFinished()) await queueBuffers.dequeue(result);
      return b;
    }
  }

  Future<String> readLineAsync() async {
    if (isClosed && queueBuffers.isEmpty() && blockBuffers.count() == 0)
      throw new Exception("read zero buffer! client disconnected");
    return String.fromCharCodes((await readExistingAsync([13, 10])).value1);
  }

  Future<String> readExistingLineAsync(String endOfLine) async {
    if (isClosed && queueBuffers.isEmpty() && blockBuffers.count() == 0)
      throw new Exception("read zero buffer! client disconnected");
    var bytes = await readExistingAsync(ascii.encode(endOfLine).toList());
    return String.fromCharCodes(bytes.value1);
  }

  Future<List<int>> readBlockToEndAsync(
      CompressMode compress, int maximum) async {
    //first 4 bytes are size of block
    List<int> dataLenByte = await readBlockSizeAsync(4);
    //convert bytes to int
    int dataLength = PostJsonToServerService.getInt(dataLenByte);
    if (dataLength > maximum)
      throw new Exception(
          "dataLength is upper than maximum :" + dataLength.toString());
    //read a block
    var dataBytes = await readBlockSizeAsync(dataLength);
    return dataBytes;
  }

  Future<List<int>> readBlockSizeAsync(int count) async {
    List<int> bytes = new List<int>();
    int lengthReaded = 0;

    while (lengthReaded < count) {
      int countToRead = count;
      if (lengthReaded + countToRead > count) {
        countToRead = count - lengthReaded;
      }
      var readed = await readAsync(countToRead);
      if (readed.value2 <= 0)
        throw new Exception("read zero buffer! client disconnected: " +
            readed.value2.toString());
      lengthReaded += readed.value2;
      bytes.addAll(readed.value1);
    }
    return bytes;
  }

  void dispose(){
    blockBuffers.dispose();
    queueBuffers.dispose();
  }
}
