import 'IJsonGoModel.dart';
import 'Deserializer.dart';
import 'package:hello_world/ServerProvider/Runtime/TypeManager.dart';

class ObjectModel implements IJsonGoModel {
  Map<String, IJsonGoModel> properties = new Map<String, IJsonGoModel>();

  void add(String nameOrValue, IJsonGoModel value) {
    properties[nameOrValue] = value;
  }

  Object generate(Type type, Deserializer deserializer) {
    if (type == null) return null;
    String idName = deserializer.setting.idRefrencedTypeNameNoQuot;
    String referenceName = deserializer.setting.refRefrencedTypeNameNoQuot;
    String valuesName = deserializer.setting.valuesRefrencedTypeNameNoQuot;
    Object obj;
    if (properties.containsKey(referenceName)) {
      var key = properties[referenceName].generate(String, deserializer);
      return deserializer.deSerializedObjects[key];
    } else if (properties.containsKey(valuesName)) {
      obj = properties[valuesName].generate(type, deserializer);
      for (var key in properties.keys) {
        var item = properties[key];
        if (key == idName) {
          String value = item.generate(String, deserializer);
          deserializer.deSerializedObjects[value] = obj;
          break;
        }
      }
      return obj;
    } else {
      var typeInfo = TypeManager.current.getTypeInfo(type);
      if (typeInfo == null) return null;
      obj = typeInfo.createInstanceFunction();
      for (var key in properties.keys) {
        var item = properties[key];
        if (key == idName) {
          String value = item.generate(String, deserializer);
          deserializer.deSerializedObjects[value] = obj;
        } else {
          var keyType = deserializer.getKeyType(obj, key);
          if (keyType != null) {
            Object value = item.generate(keyType.getKeyType(), deserializer);
            deserializer.setValue(obj, value, key);
          }
        }
      }
      return obj;
    }
  }
}
