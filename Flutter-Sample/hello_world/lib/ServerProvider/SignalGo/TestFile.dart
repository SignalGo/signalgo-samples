bool isConnectionInit = false;


enum ServerConectionStatus {
  NotInit,
  Connecting,
  Connected,
  Disconecting,
  Disconected,
  Fialed,
  ConnectionTryCountExceet,
}

ServerConectionStatus currentStatus = ServerConectionStatus.NotInit;
ServerConectionStatus latestStatus = ServerConectionStatus.NotInit;



void connectionFaild(){
      latestStatus = currentStatus;  
    currentStatus = ServerConectionStatus.Fialed;
}

void disconnect(){
      latestStatus = currentStatus;  
    currentStatus = ServerConectionStatus.Disconected;
}


void _internalConnect({String ip, int port}) {
  // pure logic
  
}

void _initConnect() {
  //log logic
  isConnectionInit = true;
}


bool isFirstConnect(){
  return latestStatus == ServerConectionStatus.NotInit && currentStatus ==ServerConectionStatus.Connected;
}



void connect({String ip, int port, bool autoReconnect = true, int reconnectDelay}) async {

   tryReconnect() async {
    if (!autoReconnect) return;

    await Future.delayed(Duration(seconds: reconnectDelay));

    if (currentStatus == ServerConectionStatus.Connecting ||
     currentStatus == ServerConectionStatus.Connected) {
     
      tryReconnect();
    } else {
        connect(ip: ip, port: port);
    }
   }
   

    currentStatus = ServerConectionStatus.Connecting;

   if (!isConnectionInit) {
      _initConnect();
    }

 
  try {

    _internalConnect(ip:ip,port: port);
   latestStatus = currentStatus;  
    currentStatus = ServerConectionStatus.Connected;
  } catch (e) {

    // log

    connectionFaild();
        // what was latest ????
        // tryCount++
  } 
  finally {
    tryReconnect();
  }

}

