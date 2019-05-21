import 'IJsonGoModel.dart';
import 'Deserializer.dart';
import 'package:hello_world/ServerProvider/Runtime/TypeManager.dart';

class ValueModel implements IJsonGoModel {
  ValueModel(String _value) {
    value = _value;
  }

  String value;

  void add(String nameOrValue, IJsonGoModel jsonModel) {
    value = nameOrValue;
  }

  Object generate(Type type, Deserializer deserializer) {
    if (type == null) return null;
    var typeInfo = TypeManager.current.getTypeInfo(type);
    return deserializer.getValue(typeInfo, type, Deserializer.trim(value, '"'));
  }
}
