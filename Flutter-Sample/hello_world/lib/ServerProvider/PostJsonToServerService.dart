import 'dart:convert';
import 'dart:io';
import 'dart:async';
import 'dart:typed_data';
import 'package:hello_world/ServerProvider/Json/Deserialize/Deserializer.dart';
import 'package:hello_world/ServerProvider/Json/Serializer.dart';
import 'package:hello_world/ServerProvider/Runtime/TypeBuilder.dart';
import 'package:hello_world/ServerProvider/Runtime/TypeMode.dart';
import 'package:hello_world/ServerProvider/SignalGo/ClientProvider.dart';
import 'package:hello_world/ServerProvider/SignalGo/MethodCallInfo.dart';
import 'package:hello_world/ServerProvider/SignalGo/MethodCallbackInfo.dart';
import 'package:hello_world/ServerProvider/SignalGo/ParameterInfo.dart';
import 'package:hello_world/ServerProvider/SignalGo/enums.dart';

class PostJsonToServerService {
  static String baseUrl = "http://behta.net:6578";
  static Serializer serializer = new Serializer();
  static String cookie;
  static ClientProvider clientProvider;

  static Future<T> post<T>(String url, Map map) async {
    if (baseUrl[baseUrl.length - 1] != '/') baseUrl += '/';
    HttpClient httpClient = new HttpClient();
    HttpClientRequest request =
        await httpClient.postUrl(Uri.parse(baseUrl + url));
    request.headers.set('content-type', 'application/json');
    if (cookie != null) {
      request.headers.set("cookie", cookie);
    }
    if (map != null) {
      var serialized = serializer.serialize(map);
      var body = utf8.encode(serialized);
      request.headers.set('Content-Length', body.length.toString());
      request.add(body);
    } else {
      request.headers.set('Content-Length', "0");
    }

    HttpClientResponse response = await request.close();
    if (response.headers["set-cookie"] != null) {
      cookie = response.headers["set-cookie"].first;
    }
    String reply = await response.transform(utf8.decoder).join();
    httpClient.close();

    Deserializer deserializer = new Deserializer();
    return deserializer.deserialize<T>(reply);
  }

  static Future<T> send<T>(
      String serviceName, String methodName, Map map) async {
    List<int> data = new List<int>();
    data.add(DataType.CallMethod.index);
    data.add(CompressMode.None.index);
    MethodCallInfo methodCallInfo = new MethodCallInfo();
    methodCallInfo.guid = MethodCallInfo.generateV4();
    methodCallInfo.serviceName = serviceName;
    methodCallInfo.methodName = methodName;
    if (map != null) {
      for (var key in map.keys) {
        var value = map[key];
        var parameter = new ParameterInfo();
        parameter.name = key;
        parameter.value = serializer.serialize(value);
        methodCallInfo.parameters.add(parameter);
      }
    }
    var serialized = serializer.serialize(methodCallInfo);
    var body = utf8.encode(serialized);

    var bodyLengthBytes = getBytes(body.length);
    data.addAll(bodyLengthBytes);
    data.addAll(body);
    var bytes = new Uint8List(data.length);
    for (int i = 0; i < data.length; i++) {
      bytes[i] = data[i];
    }
    Completer<MethodCallbackInfo> resultFuture =
        new Completer<MethodCallbackInfo>();
    clientProvider.waitedMethodsForResponse[methodCallInfo.guid] = resultFuture;
    clientProvider.sendData(bytes);
    var resultCallback = await resultFuture.future;
    if (!resultCallback.isAccessDenied && !resultCallback.isException) {
      Deserializer deserializer = new Deserializer();
      var result = deserializer.deserialize<T>(resultCallback.data);
      return result;
    }
    throw new Exception(
        "exception isAccessDenied: ${resultCallback.isAccessDenied} isException: ${resultCallback.isException}");
  }

  static Uint8List getBytes(int value) {
    int number = value;

    Uint8List buffer = new Uint8List(4);
    int i = 0;
    while (number / 256 != 0) {
      double newByte = number / 256;
      number = number % 256;
      buffer[i] = number;
      number = newByte.toInt();
      i++;
    }
    buffer[i] = number;
    return buffer;
  }

  static int getInt(List<int> numRef) {
    return (((numRef[0] | (numRef[1] << 8)) | (numRef[2] << 0x10)) |
        (numRef[3] << 0x18));
  }

  static void initialize(ClientProvider _clientProvider) {
    clientProvider = _clientProvider;
    TypeBuilder.make<MethodCallInfo>(TypeMode.Object)
        .addProperty<String>(
            "guid",
            TypeMode.String,
            (MethodCallInfo x) => x.guid,
            (MethodCallInfo x, String value) => x.guid = value)
        .addProperty<String>(
            "serviceName",
            TypeMode.String,
            (MethodCallInfo x) => x.serviceName,
            (MethodCallInfo x, String value) => x.serviceName = value)
        .addProperty<String>(
            "methodName",
            TypeMode.String,
            (MethodCallInfo x) => x.methodName,
            (MethodCallInfo x, String value) => x.methodName = value)
        .addProperty<String>(
            "data",
            TypeMode.String,
            (MethodCallInfo x) => x.data,
            (MethodCallInfo x, String value) => x.data = value)
        .addPropertyWithInstance<List<ParameterInfo>>(
            "parameters",
            TypeMode.Array,
            (MethodCallInfo x) => x.parameters,
            (MethodCallInfo x, List<ParameterInfo> value) =>
                x.parameters = value,
            () => new List<ParameterInfo>())
        .addProperty<MethodType>(
            "type",
            TypeMode.Enum,
            (MethodCallInfo x) => x.type,
            (MethodCallInfo x, MethodType value) => x.type = value)
        .addProperty<int>(
            "partNumber",
            TypeMode.Int,
            (MethodCallInfo x) => x.partNumber,
            (MethodCallInfo x, int value) => x.partNumber = value)
        .createInstance(() => new MethodCallInfo())
        .build();

    TypeBuilder.make<MethodCallbackInfo>(TypeMode.Object)
        .addProperty<String>(
            "guid",
            TypeMode.String,
            (MethodCallbackInfo x) => x.guid,
            (MethodCallbackInfo x, String value) => x.guid = value)
        .addProperty<String>(
            "data",
            TypeMode.String,
            (MethodCallbackInfo x) => x.data,
            (MethodCallbackInfo x, String value) => x.data = value)
        .addProperty<String>(
            "isAccessDenied",
            TypeMode.Boolean,
            (MethodCallbackInfo x) => x.isAccessDenied,
            (MethodCallbackInfo x, bool value) => x.isAccessDenied = value)
        .addProperty<String>(
            "isException",
            TypeMode.Boolean,
            (MethodCallbackInfo x) => x.isException,
            (MethodCallbackInfo x, bool value) => x.isException = value)
        .addProperty<List<ParameterInfo>>(
            "partNumber",
            TypeMode.Int,
            (MethodCallbackInfo x) => x.partNumber,
            (MethodCallbackInfo x, int value) => x.partNumber = value)
        .createInstance(() => new MethodCallbackInfo())
        .build();
    var parameterTypeInfo = TypeBuilder.make<ParameterInfo>(TypeMode.Object)
        .addProperty<String>(
            "name",
            TypeMode.String,
            (ParameterInfo x) => x.name,
            (ParameterInfo x, String value) => x.name = value)
        .addProperty<String>(
            "value",
            TypeMode.String,
            (ParameterInfo x) => x.value,
            (ParameterInfo x, String value) => x.value = value)
        .createInstance(() => new ParameterInfo())
        .build();

    TypeBuilder.make<List<ParameterInfo>>(TypeMode.Array)
        .addProperty<ParameterInfo>("Add", TypeMode.Array, null,
            (List<ParameterInfo> x, ParameterInfo value) => x.add(value))
        .addGenericArgument(parameterTypeInfo)
        .createInstance(() => new List<ParameterInfo>())
        .getTypeFromcreateInstance(() => new List<ParameterInfo>())
        .build();

    TypeBuilder.make<MethodType>(TypeMode.Enum)
        .createInstance(() => MethodType.values)
        .build();
  }
}
