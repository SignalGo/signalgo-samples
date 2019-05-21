import 'TypeMode.dart';

/// <summary>
/// property info of type
/// </summary>
class PropertyBuilder<T> {
  /// <summary>
  /// name of property
  /// </summary>
  String name;

  /// <summary>
  /// type of prtoperty
  /// </summary>
  TypeMode type;

  /// <summary>
  /// get Value
  /// </summary>
  Function getValue;

  /// <summary>
  /// set Value
  /// </summary>
  Function setValue;

  /// <summary>
  /// create instance for get type
  /// </summary>
  Function createInstance;
  //get T type
  Type getKeyType() {
    if (createInstance != null) return createInstance().runtimeType;
    return T;
  }
}
