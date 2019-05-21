/// <summary>
/// methods call and callback type
/// </summary>
enum DataType {
  /// <summary>
  /// correct byte
  /// </summary> = 0
  Unkwnon,

  /// <summary>
  /// method must call
  /// </summary> = 1
  CallMethod,

  /// <summary>
  /// response called method
  /// </summary> = 2
  ResponseCallMethod,

  /// <summary>
  /// register a file connection for download
  /// </summary>= 3
  RegisterFileDownload,

  /// <summary>
  /// register a file connection for upload
  /// </summary> = 4
  RegisterFileUpload,

  /// <summary>
  /// ping pong between client and server
  /// </summary> = 5
  PingPong,

  /// <summary>
  /// get details of service like methods
  /// </summary> = 6
  GetServiceDetails,

  /// <summary>
  /// get details of method parameters
  /// </summary> = 7
  GetMethodParameterDetails,

  /// <summary>
  /// flush stream for client side to get position of upload file
  /// </summary> = 8
  FlushStream,

  /// <summary>
  /// request to get client id from server
  /// </summary> = 9
  GetClientId
}

/// <summary>
/// compress mode byte
/// </summary>
enum CompressMode {
  /// <summary>
  /// no compress
  /// </summary>
  None,

  /// <summary>
  /// zip compress
  /// </summary>
  Zip
}
