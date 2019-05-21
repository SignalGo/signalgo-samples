import 'Deserializer.dart';

/// <summary>
/// json go model before deserialize
/// </summary>
abstract class IJsonGoModel {
  /// <summary>
  /// generate type to object
  /// </summary>
  /// <param name="type"></param>
  /// <param name="deserializer"></param>
  /// <returns></returns>
  Object generate(Type type, Deserializer deserializer);

  /// <summary>
  /// add property,item,value to model
  /// </summary>
  /// <param name="nameOrValue"></param>
  /// <param name="value"></param>
  void add(String nameOrValue, IJsonGoModel value);
}
