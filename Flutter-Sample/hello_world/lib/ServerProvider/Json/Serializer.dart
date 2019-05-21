import 'JsonSettingInfo.dart';
import 'package:hello_world/ServerProvider/Runtime/TypeManager.dart';
import 'package:hello_world/ServerProvider/Runtime/TypeMode.dart';

class Serializer {
  /// <summary>
  /// save serialized objects to skip stackoverflow exception and for referenced type
  /// </summary>
  Map<Object, String> serializedObjects = new Map<Object, String>();

  /// <summary>
  /// default setting of serializer
  /// </summary>
  JsonSettingInfo setting = new JsonSettingInfo();

  /// <summary>
  /// start of referenced index
  /// </summary>
  int referencedIndex = 1;

  /// <summary>
  /// serialize an object to a json string
  /// </summary>
  /// <param name="data">any object to serialize</param>
  /// <returns>json that serialized from you object</returns>
  String serialize(Object data) {
    referencedIndex = 1;
    serializedObjects.clear();
    return serializeObject(data);
  }

  /// <summary>
  /// serialize an object to a json string
  /// </summary>
  /// <param name="data">any object to serialize</param>
  /// <returns>json that serialized from you object</returns>
  String serializeObject(Object data) {
    if (data == null) {
      throw new Exception("data is null to serialize");
    }
    var typeInfo = TypeManager.current.getTypeInfo(data.runtimeType);
    if (typeInfo == null) {
      var typeMode = TypeManager.current.getTypeMode(data.runtimeType);
      if (typeMode != TypeMode.Object && typeMode != TypeMode.Array) {
        if (typeMode == TypeMode.String)
          return '\"' + serializeString(data.toString()) + '\"';
        else
          return '\"' + data.toString() + '\"';
      } else if (data.runtimeType
          .toString()
          .contains("_InternalLinkedHashMap")) {
        var map = data as Map;
        String stringBuilder = "";
        stringBuilder += "{";
        for (var key in map.keys) {
          stringBuilder += "\"${key}\":${serializeObject(map[key])}";
          stringBuilder += ',';
        }
        if (stringBuilder[stringBuilder.length - 1] == ',')
          stringBuilder = stringBuilder.substring(0, stringBuilder.length - 1);
        stringBuilder += "}";
        return stringBuilder;
      }
      return null;
    }

    if (typeInfo.typeMode == TypeMode.Enum) {
      var dy = data as dynamic;
      return '\"' + dy.index.toString() + '\"';
    }
    String referencedId = "";
    if (!serializedObjects.containsKey(data)) {
      referencedId = referencedIndex.toString();
      serializedObjects[data] = referencedId;
      referencedIndex++;
    } else if (setting.hasGenerateRefrencedTypes) {
      referencedId = serializedObjects[data];
      String stringBuilder = "";
      stringBuilder += "{";
      stringBuilder += "${setting.refRefrencedTypeName}:\"${referencedId}\"";
      stringBuilder += "}";
      return stringBuilder.toString();
    } else
      return null;

    if (typeInfo.typeMode == TypeMode.Array) {
      String stringBuilder = "";
      if (setting.hasGenerateRefrencedTypes)
        stringBuilder =
            "{${setting.idRefrencedTypeName}:\"${referencedId}\",${setting.valuesRefrencedTypeName}:";

      stringBuilder += '[';
      for (var item in data) {
        if (item != null) {
          var serialized = serializeObject(item);
          if (serialized != null) {
            stringBuilder += serialized;
            stringBuilder += ',';
          }
        }
      }
      if (stringBuilder[stringBuilder.length - 1] == ',')
        stringBuilder = stringBuilder.substring(0, stringBuilder.length - 1);
      stringBuilder += ']';
      if (setting.hasGenerateRefrencedTypes) stringBuilder += '}';
      return stringBuilder.toString();
    } else {
      String stringBuilder = "";

      var properties = typeInfo.properties;

      stringBuilder += '{';
      if (setting.hasGenerateRefrencedTypes)
        stringBuilder += "${setting.idRefrencedTypeName}:\"${referencedId}\",";

      for (var key in properties.keys) {
        var property = properties[key];
        var name = property.name;
        var value = property.getValue(data);
        if (value == null) continue;
        if (value != null) {
          var serializedValue = serializeObject(value);
          if (serializedValue != null) {
            stringBuilder += '\"';
            stringBuilder += name;
            stringBuilder += '\"';
            stringBuilder += ':';
            stringBuilder += serializedValue;
            stringBuilder += ',';
          }
        }
      }
      if (stringBuilder[stringBuilder.length - 1] == ',')
        stringBuilder = stringBuilder.substring(0, stringBuilder.length - 1);
      stringBuilder += '}';
      return stringBuilder;
    }
  }

  String serializeString(String value) {
    String result = "";
    for (int i = 0; i < value.length; i++) {
      var ch = value[i];
      if (ch == '"') result += '\\';
      result += ch;
    }
    return result;
  }
}
