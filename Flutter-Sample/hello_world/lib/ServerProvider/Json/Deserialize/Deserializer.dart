//import 'package:behta/dev/dc_server/ServerProviders/Json/Deserialize/CustomDateTime.dart';
import 'package:hello_world/ServerProvider/Json/JsonSettingInfo.dart';
import 'IJsonGoModel.dart';
import 'ObjectModel.dart';
import 'ArrayModel.dart';
import 'ValueModel.dart';
import 'package:hello_world/ServerProvider/Runtime/TypeManager.dart';
import 'package:hello_world/ServerProvider/Runtime/PropertyBuilder.dart';
import 'package:hello_world/ServerProvider/Runtime/TypeInfo.dart';
import 'package:hello_world/ServerProvider/Runtime/TypeMode.dart';

class Deserializer {
  String supportedValue = "0123456789.truefalsTRUEFALS-";

  /// <summary>
  /// save deserialize objects for referenced type
  /// </summary>
  Map<String, Object> deSerializedObjects = new Map<String, Object>();

  /// <summary>
  /// default setting of serializer
  /// </summary>
  JsonSettingInfo setting = new JsonSettingInfo();
  int _indexOf;

  /// <summary>
  /// deserialize a json to a type
  /// </summary>
  /// <typeparam name="T">type of deserialize</typeparam>
  /// <param name="json">json to deserialize</param>
  /// <returns>deserialize type</returns>
  T deserialize<T>(String json) {
    _indexOf = 0;
    deSerializedObjects.clear();
    IJsonGoModel jsonModel = extract(json);
    return jsonModel.generate(T, this);
  }

  Object deserializeWithType(String json, Type type) {
    _indexOf = 0;
    deSerializedObjects.clear();
    IJsonGoModel jsonModel = extract(json);
    return jsonModel.generate(type, this);
  }

  static String trim(String text, String textTrim) {
    if (text == null || textTrim == null) return text;
    while (text.startsWith(textTrim)) {
      text = text.substring(textTrim.length);
    }
    while (text.endsWith(textTrim)) {
      text = text.substring(0, text.length - textTrim.length);
    }
    return text;
  }

  /// <summary>
  /// deserialize json
  /// </summary>
  /// <param name="json">json value</param>
  /// <param name="type">type to deserialize</param>
  /// <returns>value deserialize</returns>
  IJsonGoModel extract(String json) {
    IJsonGoModel jsonGoModel;
    bool canSkip = true;
    for (int i = _indexOf; i < json.length; i++) {
      _indexOf = i;
      var character = json[i];
      if (canSkip && isWhiteSpace(character)) continue;
      if (character == '{') {
        jsonGoModel = new ObjectModel();
        _indexOf++;
        var result = extractObject(json);
        for (var key in result.keys) {
          var item = result[key];
          jsonGoModel.add(key, item);
        }

        return jsonGoModel;
      } else if (character == '[') {
        jsonGoModel = new ArrayModel();
        _indexOf++;
        for (IJsonGoModel item in extractArray(json)) {
          jsonGoModel.add(null, item);
        }
        return jsonGoModel;
      } else if (character == '\"') {
        jsonGoModel = new ValueModel(extractString(json));
        return jsonGoModel;
      } else {
        jsonGoModel = new ValueModel(extractValue(json));
        return jsonGoModel;
      }
    }
    return jsonGoModel;
  }

  List<IJsonGoModel> extractArray(String json) {
    List<IJsonGoModel> result = new List<IJsonGoModel>();
    for (int i = _indexOf; i < json.length; i++) {
      _indexOf = i;
      var character = json[i];
      if (isWhiteSpace(character)) continue;
      if (character == '{') {
        ObjectModel jsonGoModel = new ObjectModel();
        _indexOf++;
        var keyValues = extractObject(json);
        for (var key in keyValues.keys) {
          var value = keyValues[key];
          jsonGoModel.add(key, value);
        }
        i = _indexOf;
        result.add(jsonGoModel);
      } else if (character == '[') {
        ArrayModel jsonGoModel = new ArrayModel();
        _indexOf++;
        for (IJsonGoModel item in extractArray(json)) {
          jsonGoModel.add(null, item);
        }
        i = _indexOf;
        result.add(jsonGoModel);
      } else if (character == ',') {
        continue;
      } else if (character == ']') {
        break;
      } else if (character == '"') {
        var resultString = extractString(json);
        ValueModel valueModel = new ValueModel(resultString);
        result.add(valueModel);
        i = _indexOf;
      } else
        throw new Exception(
            "end of character not support '${character}' index of {$i} i think i must find '}' character");
    }
    return result;
  }

  /// <summary>
  /// extract list of properties from object
  /// </summary>
  /// <param name="json"></param>
  /// <returns></returns>
  Map<String, IJsonGoModel> extractObject(String json) {
    Map<String, IJsonGoModel> result = new Map<String, IJsonGoModel>();
    var canSkip = true;
    var findKey = false;
    var findStartOfValue = false;
    String keyBuilder = "";
    for (int i = _indexOf; i < json.length; i++) {
      _indexOf = i;
      var character = json[i];
      if (canSkip && isWhiteSpace(character)) continue;
      if (findKey) {
        keyBuilder += character;
        if (character == '\"') {
          findKey = false;
          findStartOfValue = true;
          canSkip = true;
        }
        //else
        //    throw new Exception($"I tried to find start of key, I think here must be '\"' char instead of '{character}' char index of {i} from json {json}");
      } else if (findStartOfValue) {
        if (character == ':') {
          _indexOf++;
          IJsonGoModel value = extract(json);
          result[trim(keyBuilder, '"')] = value;
          keyBuilder = "";
          i = _indexOf;
          canSkip = true;
          findKey = false;
          findStartOfValue = false;
        } else
          throw new Exception(
              "I tried to find start of value, I think here must be ':' char instead of '${character}' char index of ${i} from json ${json}");
      } else if (character == '\"') {
        if (!findKey) {
          findKey = true;
          keyBuilder += character;
          canSkip = false;
        }
      } else if (character == '}') {
        break;
      } else if (character == '{')
        continue;
      else if (character != ',')
        throw new Exception(
            "I tried to find start of object, I think here must be '{' char instead of '${character}' char index of ${i} from json ${json}");
    }
    return result;
  }

  /// <summary>
  /// get type of a json parameter name
  /// </summary>
  /// <param name="obj">object</param>
  /// <param name="key">json parameter name</param>
  /// <returns>type of json parameter</returns>
  PropertyBuilder getKeyType(Object obj, String key) {
    if (obj == null) return null;
    key = trim(key, '"');
    var typeInfo = TypeManager.current.getTypeInfo(obj.runtimeType);
    if (typeInfo == null) return null;
    var properties = typeInfo.properties;

    for (var pKey in properties.keys) {
      var property = properties[pKey];
      if (property.name.toLowerCase() == key.toLowerCase()) {
        return property;
      }
    }
    for (var base in typeInfo.baseClasses) {
      for (var pKey in base.properties.keys) {
        var property = base.properties[pKey];
        if (property.name.toLowerCase() == key.toLowerCase()) {
          return property;
        }
      }
    }

    return null;
  }

  /// <summary>
  /// extract string from inside of double " char
  /// </summary>
  /// <param name="json"></param>
  /// <returns></returns>
  String extractString(String json) {
    String result = "";
    var started = false;
    bool canSkipOneTime = false;
    for (int i = _indexOf; i < json.length; i++) {
      _indexOf = i;
      var character = json[i];
      if (started &&
          character == '\\' &&
          json.length > i + 1 &&
          json[i + 1] == '"') {
        canSkipOneTime = true;
        continue;
      }
      result += character;
      if (character == '\"') {
        if (canSkipOneTime) {
          canSkipOneTime = false;
          continue;
        }
        if (!started)
          started = true;
        else
          break;
      }
    }

    return result;
  }

  String extractValue(String json) {
    String result = "";
    var started = false;
    for (int i = _indexOf; i < json.length; i++) {
      var character = json[i];
      if (supportedValue.contains(character)) {
        if (!started) started = true;
        result += character;
        _indexOf = i;
      } else
        break;
    }

    return result;
  }

  /// <summary>
  /// set value of json parameter key to an instance of object
  /// </summary>
  /// <param name="obj">object to change parameter</param>
  /// <param name="value">value to set</param>
  /// <param name="key">parameter name of object</param>
  void setValue(Object obj, Object value, String key) {
    if (obj == null) return;
    key = trim(key, '"');
    var typeInfo = TypeManager.current.getTypeInfo(obj.runtimeType);
    var properties = typeInfo.properties;
    for (var pKey in properties.keys) {
      var property = properties[pKey];
      if (property.name.toLowerCase() == key.toLowerCase()) {
        try {
          property.setValue(obj, value);
          break;
        } catch (ex) {
          print(ex);
        }
      }
    }
    for (var base in typeInfo.baseClasses) {
      for (var pKey in base.properties.keys) {
        var property = base.properties[pKey];
        if (property.name.toLowerCase() == key.toLowerCase()) {
          try {
            property.setValue(obj, value);
            break;
          } catch (ex) {
            print(ex);
          }
        }
      }
    }
  }

  Object getValue(TypeInfo typeInfo, Type type, Object value) {
    if (value == null) return null;
    TypeMode typeMode = TypeMode.Unknown;
    if (typeInfo != null) typeMode = typeInfo.typeMode;

    if (typeMode == TypeMode.Int || type == int)
      return int.parse(value);
    else if (typeMode == TypeMode.Double || type == double)
      return double.parse(value);
    else if (typeMode == TypeMode.String || type == String)
      return value.toString();
    else if (typeMode == TypeMode.DateTime || type == DateTime) {
      //var result = CustomDateTime.parse(value);
      //var result = DateTime.parse(value);
      try{
        var text = value.toString();
        if (text.contains("+"))
          text = text.substring(0, text.indexOf('.')) +
              text.substring(text.indexOf('+'));
        else
          text = text.substring(0, text.indexOf('.'));
        return DateTime.parse(text);
      }
      catch(ex){
        return DateTime.now();

      }
    } else if (typeMode == TypeMode.Double || type == double)
      return double.parse(value);
    else if (typeMode == TypeMode.Boolean || type == bool)
      return value.toString().toLowerCase() == 'true';
    else if (typeMode == TypeMode.Enum) {
      var instance = typeInfo.createInstanceFunction()[int.parse(value)];
      return instance;
    }

//    else if (instance_mirror.type.isEnum) {
//      var dy = data as dynamic;
//      return '\"' + dy.index.toString() + '\"';
//    }
//    if (type) {
//      value = Convert.ChangeType(value, typeof(int));
//      value = Enum.ToObject(type, (int)value);
//    }
//    else
//      value = Convert.ChangeType(value, type);
    return value;
  }

  /// <summary>
  /// check if a character is whitespace or empty
  /// </summary>
  /// <param name="value">character to check</param>
  /// <returns>is char is white space</returns>
  bool isWhiteSpace(String value) {
    return value == '\b' ||
        value == '\f' ||
        value == '\n' ||
        value == '\r' ||
        value == '\t' ||
        value == ' ';
  }
}
