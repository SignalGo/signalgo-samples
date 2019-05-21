import 'TypeInfo.dart';
import 'TypeMode.dart';

/// <summary>
/// manage types
/// </summary>
class TypeManager {
  /// <summary>
  /// current type manager
  /// </summary>
  static TypeManager current = new TypeManager();

  /// <summary>
  /// compiled types in memory
  /// </summary>
  Map<Type, TypeInfo> compiledTypes = new Map<Type, TypeInfo>();

  /// <summary>
  /// add a type
  /// </summary>
  /// <param name="typeInfo"></param>
  void add(TypeInfo typeInfo) {
    compiledTypes[typeInfo.type] = typeInfo;
  }

  /// <summary>
  /// get type info
  /// </summary>
  /// <param name="type"></param>
  /// <returns></returns>
  TypeInfo getTypeInfo(Type type) {
    var result = compiledTypes[type];
    if (result == null) {
      for (var key in compiledTypes.keys) {
        if (compiledTypes[key].type2 == type) {
          return compiledTypes[key];
        }
      }
    }
    return result;
  }

  TypeMode getTypeMode(Type type) {
    if (type == bool)
      return TypeMode.Boolean;
    else if (type == int)
      return TypeMode.Int;
    else if (type == String)
      return TypeMode.String;
    else if (type == double)
      return TypeMode.Double;
    else if (type == DateTime)
      return TypeMode.DateTime;
    else if (type.toString().contains("_GrowableList")) return TypeMode.Array;
    return TypeMode.Object;
  }
}
