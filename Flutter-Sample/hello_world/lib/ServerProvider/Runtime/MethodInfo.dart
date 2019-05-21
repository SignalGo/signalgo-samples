import 'ParameterInfo.dart';

class MethodInfo {
  String name;
  //Type returnType;
  List<ParameterInfo> parameters = new List<ParameterInfo>();
  Function invokeMethod;
  Object invoke(Object service, List<Object> values) {
    if (values == null || values.length == 0)
      return invokeMethod(service);
    else if (values.length == 1)
      return invokeMethod(service, values[0]);
    else if (values.length == 2)
      return invokeMethod(service, values[0], values[1]);
    else if (values.length == 3)
      return invokeMethod(service, values[0], values[1], values[2]);
    else if (values.length == 4)
      return invokeMethod(service, values[0], values[1], values[2], values[3]);
    else if (values.length == 5)
      return invokeMethod(
          service, values[0], values[1], values[2], values[3], values[4]);
    else if (values.length == 6)
      return invokeMethod(service, values[0], values[1], values[2], values[3],
          values[4], values[5]);
    else if (values.length == 7)
      return invokeMethod(service, values[0], values[1], values[2], values[3],
          values[4], values[5], values[6]);
    else if (values.length == 8)
      return invokeMethod(service, values[0], values[1], values[2], values[3],
          values[4], values[5], values[6], values[7]);
    else if (values.length == 9)
      return invokeMethod(service, values[0], values[1], values[2], values[3],
          values[4], values[5], values[6], values[7], values[8]);
    else if (values.length == 10)
      return invokeMethod(service, values[0], values[1], values[2], values[3],
          values[4], values[5], values[6], values[7], values[8], values[9]);
    throw new Exception(
        "not support ${values.length} parameter count in TypeBuilder Method invoke");
  }
}
