import 'MethodInfo.dart';
import 'PropertyBuilder.dart';
import 'ParameterInfo.dart';
import 'TypeInfo.dart';
import 'TypeMode.dart';
import 'TypeManager.dart';

/// <summary>
/// build your types
/// </summary>
class TypeBuilder<T> {
  /// <summary>
  /// created insatnce function
  /// </summary>
  Function createInstanceFunction;

  /// <summary>
  /// created insatnce function for list<T>
  /// </summary>
  Function getTypeFromcreateInstanceFunction;

  /// <summary>
  /// all of properties of type
  /// </summary>
  Map<String, PropertyBuilder<dynamic>> properties =
      new Map<String, PropertyBuilder<dynamic>>();

  /// <summary>
  /// all of methods of type
  /// </summary>
  Map<String, MethodInfo> methods = new Map<String, MethodInfo>();

  /// <summary>
  /// all of generic arguments
  /// </summary>
  List<TypeInfo> genericArguments = new List<TypeInfo>();

  /// <summary>
  /// all of base classes
  /// </summary>
  List<TypeInfo> baseClasses = new List<TypeInfo>();

  /// <summary>
  /// mode of type
  /// </summary>
  TypeMode typeMode = TypeMode.Unknown;

  /// <summary>
  /// function of create instance of object
  /// </summary>
  /// <param name="createInstance"></param>
  /// <returns></returns>
  TypeBuilder<T> createInstance(Function createInstance) {
    createInstanceFunction = createInstance;
    return this;
  }

  TypeBuilder<T> getTypeFromcreateInstance(Function createInstance) {
    getTypeFromcreateInstanceFunction = createInstance;
    return this;
  }

  static TypeBuilder<T> make<T>(TypeMode _typeMode) {
    var result = new TypeBuilder<T>();
    result.typeMode = _typeMode;
    return result;
  }

  /// <summary>
  /// add property to type
  /// </summary>
  /// <param name="name"></param>
  /// <param name="typeMode"></param>
  /// <param name="getValue"></param>
  /// <param name="setValue"></param>
  /// <returns></returns>
  TypeBuilder<T> addProperty<X>(
      String name, TypeMode typeMode, Function getValue, Function setValue) {
    var value = new PropertyBuilder<X>();
    value.name = name;
    value.type = typeMode;
    value.getValue = getValue;
    value.setValue = setValue;
    properties[name] = value;
    return this;
  }

  TypeBuilder<T> addMethod<X>(
      String name, List<ParameterInfo> parameters, Function invoke) {
    var value = new MethodInfo();
    value.name = name.toLowerCase();
    value.invokeMethod = invoke;
    value.parameters = parameters;
    methods[value.name] = value;
    return this;
  }

  TypeBuilder<T> addPropertyWithInstance<X>(String name, TypeMode typeMode,
      Function getValue, Function setValue, Function createIntance) {
    var value = new PropertyBuilder<X>();
    value.name = name;
    value.type = typeMode;
    value.getValue = getValue;
    value.setValue = setValue;
    value.createInstance = createIntance;
    properties[name] = value;
    return this;
  }

  TypeBuilder<T> addGenericArgument(TypeInfo typeInfo) {
    genericArguments.add(typeInfo);
    return this;
  }

  TypeBuilder<T> addBaseClass(TypeInfo typeInfo) {
    baseClasses.add(typeInfo);
    return this;
  }

  /// <summary>
  /// build a type
  /// </summary>
  TypeInfo<T> build() {
    TypeInfo<T> typeInfo = new TypeInfo<T>();
    if (getTypeFromcreateInstanceFunction != null)
      typeInfo.type = getTypeFromcreateInstanceFunction().runtimeType;
    else
      typeInfo.type = T;
    typeInfo.createInstanceFunction = createInstanceFunction;
    typeInfo.getTypeFromcreateInstanceFunction = getTypeFromcreateInstance;
    typeInfo.properties = properties;
    typeInfo.methods = methods;
    typeInfo.genericArguments = genericArguments;
    typeInfo.typeMode = typeMode;
    typeInfo.baseClasses = baseClasses;
    TypeManager.current.add(typeInfo);
    return typeInfo;
  }
}
