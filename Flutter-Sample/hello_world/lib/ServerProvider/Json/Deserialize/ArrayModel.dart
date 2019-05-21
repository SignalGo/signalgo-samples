import 'IJsonGoModel.dart';
import 'Deserializer.dart';
import 'package:hello_world/ServerProvider/Runtime/TypeManager.dart';
import 'package:hello_world/ServerProvider/Runtime/PropertyBuilder.dart';

class ArrayModel implements IJsonGoModel {
  List<IJsonGoModel> items = new List<IJsonGoModel>();

  void add(String nameOrValue, IJsonGoModel value) {
    items.add(value);
  }

  Object generate(Type type, Deserializer deserializer) {
    if (type == null) return null;

    var typeInfo = TypeManager.current.getTypeInfo(type);
    if (typeInfo == null) return null;
    Object obj = typeInfo.createInstanceFunction();
    var properties = typeInfo.properties;
    PropertyBuilder propertyBuilder;
    for (var m in properties.keys) {
      if (m == "Add") {
        propertyBuilder = properties[m];
      }
    }
    for (var item in items) {
      var elementType = typeInfo.genericArguments[0];
      if (elementType != null) {
        var value = item.generate(elementType.type, deserializer);
        propertyBuilder.setValue(obj, value);
      }
    }
    return obj;
  }
}
