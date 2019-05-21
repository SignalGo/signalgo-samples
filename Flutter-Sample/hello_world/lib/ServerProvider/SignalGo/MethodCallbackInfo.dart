/// <summary>
/// call is return back from client or server
/// </summary>
class MethodCallbackInfo {
  /// <summary>
  /// method access code
  /// </summary>
  String guid;

  /// <summary>
  /// json data
  /// </summary>
  String data;

  /// <summary>
  /// data is exception
  /// </summary>
  bool isException = false;

  /// <summary>
  /// if client have not permision to call method
  /// </summary>
  bool isAccessDenied = false;

  /// <summary>
  /// part number
  /// </summary>
  int partNumber;
}
