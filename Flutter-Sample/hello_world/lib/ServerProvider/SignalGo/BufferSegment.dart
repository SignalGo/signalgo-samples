import 'package:hello_world/ServerProvider/SignalGo/Tuple.dart';

class BufferSegment {
  List<int> buffer;
  int position = 0;
  bool isFinished() {
    return position == buffer.length;
  }

  int readFirstByte() {
    int result = buffer[position];
    position++;
    return result;
  }

  int whatIsFirstByte() {
    return buffer[position];
  }

  Tuple2<List<int>, int> readBufferSegment(int count) {
    Tuple2<List<int>, int> tuple = new Tuple2<List<int>, int>();
    if (count > buffer.length) {
      tuple.value1 = buffer.skip(position).toList();
      tuple.value2 = tuple.value1.length;
      position = buffer.length;
      return tuple;
    } else {
      tuple.value1 = buffer.skip(position).take(count).toList();
      tuple.value2 = tuple.value1.length;
      position += tuple.value2;
      return tuple;
    }
  }

  Tuple2<List<int>, bool> read(List<int> exitBytes) {
    Tuple2<List<int>, bool> result = new Tuple2<List<int>, bool>();
    result.value2 = false;
    int startPosition = position;
    for (int i = position; i < buffer.length; i++) {
      if (sequenceEqual(buffer.skip(i).take(exitBytes.length), exitBytes)) {
        result.value2 = true;
        position += exitBytes.length;
        break;
      }
      position++;
    }
    int endPosition = position;
    result.value1 =
        buffer.skip(startPosition).take(endPosition - startPosition).toList();
    return result;
  }

  static bool sequenceEqual(List<int> list1, List<int> List) {
    if (list1 == List)
      return true;
    else if (list1 == null || List == null)
      return false;
    else if (list1.length != List.length) return false;
    for (int i = 0; i < list1.length; i++) {
      if (list1[i] != List[i]) return false;
    }
    return true;
  }
}
