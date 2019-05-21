/// <summary>
/// default setting of json serialize and deserialier
/// </summary>
class JsonSettingInfo {
  /// <summary>
  /// $Id refrenced type name
  /// </summary>
  String idRefrencedTypeName = "\"\$id\"";
  String idRefrencedTypeNameNoQuot = "\$id";
  /// <summary>
  /// $Ref refrenced type name
  /// </summary>
  String refRefrencedTypeName = "\"\$ref\"";
  String refRefrencedTypeNameNoQuot = "\$ref";

  /// <summary>
  /// $Values refrenced type name
  /// </summary>
  String valuesRefrencedTypeName = "\"\$values\"";
  String valuesRefrencedTypeNameNoQuot = "\$values";

  /// <summary>
  /// support for $id,$ref,$values for serialization
  /// </summary>
  bool hasGenerateRefrencedTypes = true;
}
