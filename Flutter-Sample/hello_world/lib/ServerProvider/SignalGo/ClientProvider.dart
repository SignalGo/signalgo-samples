import 'dart:async';
import 'dart:io';

import 'dart:typed_data';
import 'dart:convert';
import 'package:hello_world/ServerProvider/Json/Deserialize/Deserializer.dart';
import 'package:hello_world/ServerProvider/Json/Serializer.dart';
import 'package:hello_world/ServerProvider/PostJsonToServerService.dart';
import 'package:hello_world/ServerProvider/Runtime/TypeManager.dart';
import 'package:hello_world/ServerProvider/SignalGo/MethodCallInfo.dart';
import 'package:hello_world/ServerProvider/SignalGo/MethodCallbackInfo.dart';
import 'package:hello_world/ServerProvider/SignalGo/PipeNetworkStream.dart';
import 'package:hello_world/ServerProvider/SignalGo/enums.dart';

class ClientProvider {
  Socket _socket;
  String newLine = "\r\n";
  int maximumReceiveDataBlock = 1024 * 1024;
  Map<String, Completer> waitedMethodsForResponse =
      new Map<String, Completer>();
  Map<String, Object> callbacks = new Map<String, Object>();

  PipeNetworkStream stream;
  String _address;
  int _port;
  Function _changed;
  void registerService<T>(String serviceName) {
    var builder = TypeManager.current.getTypeInfo(T);
    var instance = builder.createInstanceFunction();
    callbacks[serviceName] = instance;
  }

  void autoReconnectAsync(String address, int port, Function changed) async {
    _changed = changed;
    _address = address;
    _port = port;

    stream = new PipeNetworkStream();
    try {
      _socket =
          await Socket.connect(address, port, timeout: Duration(seconds: 5));
      _socket.write("SignalGo/4.0 $address:$port$newLine");
      changed(true);
      onDataReceiver();
      _socket.listen(dataHandler,
          onError: errorHandler, onDone: doneHandler, cancelOnError: false);
    } catch (e) {
      print("Unable to connect: $e");
      changed(false);
      doneHandler();
    }

    //Connect standard in to the socket
    //_socket.write(obj)
    //stdin.listen(onData);
  }

  bool isBreakFromWhile = true;
  bool isStop = true;
  void onDataReceiver() async {
    try {
      isStop = false;
      isBreakFromWhile = false;
      while (!isStop) {
        var dataTypeByte = await stream.readOneByteAsync();
        DataType dataType = DataType.values[dataTypeByte];
        if (dataType == DataType.PingPong) {
          //PingAndWaitForPong.Set();
          continue;
        }

        int compressModeByte = await stream.readOneByteAsync();
        CompressMode compresssMode = CompressMode.values[compressModeByte];

        // server is called client method
        if (dataType == DataType.CallMethod) {
          var bytes = await stream.readBlockToEndAsync(
              compresssMode, maximumReceiveDataBlock);
          String json = utf8.decode(bytes);
          Deserializer deserializer = new Deserializer();
          MethodCallInfo callInfo =
              deserializer.deserialize<MethodCallInfo>(json);
          if (callInfo.type == MethodType.User) callMethod(callInfo);
        }
        //after client called server method, server response to client
        else if (dataType == DataType.ResponseCallMethod) {
          var bytes = await stream.readBlockToEndAsync(
              compresssMode, maximumReceiveDataBlock);
          String json = utf8.decode(bytes);
          Deserializer deserializer = new Deserializer();
          MethodCallbackInfo callback =
              deserializer.deserialize<MethodCallbackInfo>(json);

          if (waitedMethodsForResponse.containsKey(callback.guid)) {
            var keyValue = waitedMethodsForResponse[callback.guid];
            if (callback.isException) {
              keyValue.completeError(new Exception(callback.data));
            } else
              print(callback.data);
              keyValue.complete(callback);
          }
        } else {
          disconnect();
          break;
        }
      }
    } catch (ex) {
      print(ex);
      disconnect();
    } finally {
      isBreakFromWhile = true;
    }
  }

  void callMethod(MethodCallInfo callInfo) async {
    MethodCallbackInfo callback = new MethodCallbackInfo()
      ..guid = callInfo.guid;
    Serializer serializer = new Serializer();
    try {
      if (!callbacks.containsKey(callInfo.serviceName.toLowerCase()))
        throw new Exception(
            "Callback service {callInfo.ServiceName} not found or not registred in client side!");
      Object service = callbacks[callInfo.serviceName.toLowerCase()];
      var serviceType = TypeManager.current.getTypeInfo(service.runtimeType);
      var method = serviceType.findMethod(callInfo.methodName);

      if (method == null)
        throw new Exception(
            "Method ${callInfo.methodName} from service ${callInfo.serviceName} not found! serviceType ok?: ${serviceType != null}");
      List<Object> parameters = new List<Object>();
      int index = 0;
      for (var parameter in method.parameters) {
        Deserializer deserializer = new Deserializer();
        parameters.add(deserializer.deserializeWithType(
            callInfo.parameters[index].value, parameter.parameterType));
        index++;
      }
      var data = method.invoke(service, parameters);
      callback.data = data == null ? null : serializer.serialize(data);
    } catch (ex) {
      callback.isException = true;
      callback.data = serializer.serialize(ex.ToString());
      print(ex);
    }
    sendCallbackData(callback);
  }

  void sendCallbackData(MethodCallbackInfo callback) async {
    try {
      Serializer serializer = new Serializer();
      String json = serializer.serialize(callback);
      List<int> bytes = utf8.encode(json);
      List<int> len = PostJsonToServerService.getBytes(bytes.length);
      List<int> data = new List<int>();
      data.add(DataType.ResponseCallMethod.index);
      data.add(CompressMode.None.index);
      data.addAll(len);
      data.addAll(bytes);
      if (data.length > maximumReceiveDataBlock) {
        throw new Exception(
            "SendCallbackData data length is upper than MaximumSendDataBlock");
      }
      var sendBytes = new Uint8List(data.length);
      for (int i = 0; i < data.length; i++) {
        sendBytes[i] = data[i];
      }
      sendData(sendBytes);
    } catch (ex) {}
  }

  void sendData(Uint8List data) {
    _socket.add(data);
  }

  void dataHandler(data) {
    stream.saveNewData(data);
  }

  void errorHandler(error, StackTrace trace) {
    disconnect();
    print("socket error:");
    print(error);
  }

  void doneHandler() async {
    try {
      disconnect();
      print("socket done");
      do {
        await new Future.delayed(const Duration(seconds: 1), () => "1");
      } while (!isBreakFromWhile);
      autoReconnectAsync(_address, _port, _changed);
    } catch (ex) {
      print(ex);
    }
  }

  void disconnect() async {
    try {
      stream.dispose();
      isStop = true;
      _socket.close();
      _socket.destroy();
    } catch (ex) {}
  }
}
