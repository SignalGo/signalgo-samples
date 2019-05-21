import 'dart:math';

import 'package:hello_world/ServerProvider/SignalGo/ParameterInfo.dart';

/// <summary>
/// signal go call method type
/// </summary>
enum MethodType {
  /// <summary>
  /// programmer type
  /// </summary>
  User,

  /// <summary>
  /// signalGo type
  /// </summary>
  SignalGo
}

/// <summary>
/// call info class is data for call client or server method
/// </summary>
class MethodCallInfo {
  /// <summary>
  /// method access code
  /// </summary>
  String guid;

  /// <summary>
  /// service name in client or server from ServiceContract class
  /// </summary>
  String serviceName;

  /// <summary>
  /// method name in client or server from ServiceContract class
  /// </summary>
  String methodName;

  /// <summary>
  /// data to send
  /// </summary>
  String data;

  /// <summary>
  /// method parameters
  /// </summary>
  List<ParameterInfo> parameters = new List<ParameterInfo>();

  /// <summary>
  /// sender of call from ignalGo service or not
  /// </summary>
  MethodType type = MethodType.User;

  /// <summary>
  /// Part number of call method
  /// </summary>
  int partNumber;

  static final Random _random = new Random();
  static String generateV4() {
    // Generate xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx / 8-4-4-4-12.
    int special = 8 + _random.nextInt(4);

    return '${_bitsDigits(16, 4)}${_bitsDigits(16, 4)}-'
        '${_bitsDigits(16, 4)}-'
        '4${_bitsDigits(12, 3)}-'
        '${_printDigits(special, 1)}${_bitsDigits(12, 3)}-'
        '${_bitsDigits(16, 4)}${_bitsDigits(16, 4)}${_bitsDigits(16, 4)}';
  }

  static String _bitsDigits(int bitCount, int digitCount) =>
      _printDigits(_generateBits(bitCount), digitCount);

  static int _generateBits(int bitCount) => _random.nextInt(1 << bitCount);

  static String _printDigits(int value, int count) =>
      value.toRadixString(16).padLeft(count, '0');
}
