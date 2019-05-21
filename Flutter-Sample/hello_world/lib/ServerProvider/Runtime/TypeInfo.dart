import 'MethodInfo.dart';
import 'PropertyBuilder.dart';
import 'TypeMode.dart';

/// <summary>
/// details of a type
/// </summary>
class TypeInfo<T> {
  /// <summary>
  /// type of builder
  /// </summary>
  Type type;

  /// <summary>
  /// type 2 of builder
  /// </summary>
  Type type2;

  /// <summary>
  /// all of generic arguments
  /// </summary>
  List<TypeInfo> genericArguments = new List<TypeInfo>();

  /// <summary>
  /// all of base classes
  /// </summary>
  List<TypeInfo> baseClasses = new List<TypeInfo>();

  /// <summary>
  /// create instance of type
  /// </summary>
  Function createInstanceFunction;
  Function getTypeFromcreateInstanceFunction;

  ///mode of type
  TypeMode typeMode = TypeMode.Unknown;

  /// <summary>
  /// all of properties of type
  /// </summary>
  Map<String, PropertyBuilder<dynamic>> properties =
      new Map<String, PropertyBuilder<dynamic>>();

  /// <summary>
  ///list of methods
  /// </summary>
  Map<String, MethodInfo> methods = new Map<String, MethodInfo>();

  /// find method
  MethodInfo findMethod(String name) {
    return methods[name.toLowerCase()];
  }
}
