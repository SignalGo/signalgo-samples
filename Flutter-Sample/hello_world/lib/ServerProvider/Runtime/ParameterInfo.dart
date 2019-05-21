abstract class ParameterInfoBase {
  String name;
}

class ParameterInfo<T> extends ParameterInfoBase {
  ParameterInfo() {
    parameterType = T;
  }
  Type parameterType;
  T value;
}
