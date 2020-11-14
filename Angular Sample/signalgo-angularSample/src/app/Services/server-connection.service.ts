import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Type } from '@angular/compiler/src/core';
import { Extractor } from 'node_modules/@angular/compiler';
import { Observable } from 'rxjs';
//import * as moment from 'jalali-moment';
@Injectable({
  providedIn: 'root'
})
export class ServerConnectionService {


  constructor(private http: HttpClient) { }

  postFile<T>(url: string, ...params: any[]): Promise<T> {
    var element = document.createElement('input');
    element.setAttribute('id', 'importFile');
    element.setAttribute('type', 'file');
    element.setAttribute('style', 'visibility:hidden;');
    var dateParser = function (key, value) {
      if (typeof value === 'string') {
        var date = Date.parse(value);
        if (date) {
          return new Date(date);
        }
      }
      return value;
    };

    return new Promise<T>((resolve, reject) => {
      element.value = '';
      element.onchange = (event: any) => {
        var formData = new FormData();
        var input = event.target;
        params.forEach(element => {
          formData.append(element.Name, element.Value);
        });
        formData.append('userfile', input.files[0]);
        let headers = new HttpHeaders();
        headers.append('Content-Type', 'multipart/form-data');
        this.http.post(environment.serverUrl + url, formData, {
          headers: headers,
          withCredentials: true,
        }).subscribe((response: any) => {
          try {
            var json = JSON.stringify(response);
            var result = JSON.parse(json, dateParser);
            result = this.deserializeReferences(result, {}, []);
            resolve(result);
          } catch (ex) {
            reject(ex);
            console.log("fix reference error:", ex);
          }
        }, (error) => {
          console.log("error response: ", error);
        });
      };
      element.click();
    });




    // var dateParser = function (key, value) {
    //   if (typeof value === 'string') {
    //     var date = Date.parse(value);
    //     if (date) {
    //       //console.log("date not NaN: ", date,value);
    //       return new Date(date);
    //     }
    //   }
    //   return value;
    // };
    // try {
    //   this.fixDateTimeProperties(data, []);
    // }
    // catch (ex) {
    //   console.log("fix date time error:", ex);
    // }
    // try {
    //   data = this.serializeReferences(data, true, []);
    //   //alert("url:" + url + " serializeReferences :" + JSON.stringify(data));
    // }
    // catch (ex) {
    //   console.log("url:" + url + " serializeReferences error:", ex);
    // }

  }


  postFileWithData<T>(url: string, fileData: any, ...params: any[]): Promise<T> {
    var dateParser = function (key, value) {
      if (typeof value === 'string') {
        var date = Date.parse(value);
        if (date) {
          return new Date(date);
        }
      }
      return value;
    };

    return new Promise<T>((resolve, reject) => {
      var formData = new FormData();
      // var input = event.target;
      params.forEach(element => {
        formData.append(element.Name, element.Value);
      });
      formData.append('userfile', fileData);
      let headers = new HttpHeaders();
      headers.append('Content-Type', 'multipart/form-data');
      this.http.post(environment.serverUrl + url, formData, {
        headers: headers,
        withCredentials: true,
      }).subscribe((response: any) => {
        try {
          var json = JSON.stringify(response);
          var result = JSON.parse(json, dateParser);
          result = this.deserializeReferences(result, {}, []);
          resolve(result);
        } catch (ex) {
          reject(ex);
          console.log("fix reference error:", ex);
        }
      }, (error) => {
        console.log("error response: ", error);
      });
    });
  }

  post<T>(url: string, data: any): Observable<T> {
    var dateParser = function (key, value) {
      if (typeof value === 'string') {
        var date = Date.parse(value);
        if (date && (value.indexOf('/') >= 0 || value.indexOf('-') >= 0) && value.indexOf("http") == -1) {
          //console.log("date not NaN: ", date, value);
          return new Date(date);
        }
      }
      return value;
    };
    try {
      this.fixDateTimeProperties(data, []);
    }
    catch (ex) {
      console.log("fix date time error:", ex);
    }
    try {
      data = this.serializeReferences(data, true, []);
      //alert("url:" + url + " serializeReferences :" + JSON.stringify(data));
    }
    catch (ex) {
      console.log("url:" + url + " serializeReferences error:", ex);
    }
    return new Observable<T>((observer) => {

      return this.http.post(environment.serverUrl + url, data, { withCredentials: true }).subscribe((response: any) => {
        try {
          var json = JSON.stringify(response);
          var result = JSON.parse(json, dateParser);
          result = this.deserializeReferences(result, {}, []);

          observer.next(result);
        } catch (ex) {
          console.log("fix reference error:", ex);
        }
      }, (error) => {
        console.log("error response: ", error);
        observer.error(error);
      });
    });
  }

  toCamelCase(o: any) {
    var origKey, newKey, value;
    if (o instanceof Array) {
      return o;
    } else {
      for (origKey in o) {
        if (o.hasOwnProperty(origKey)) {
          newKey = (origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey).toString()
          value = o[origKey]
          o[newKey] = value;
          delete o[origKey];
        }
      }
    }
  }

  public clone(obj: any): any {
    return this.deserializeReferences(this.serializeReferences(obj, true, []), {}, []);
  }
  fixDateTimeProperties<T>(data: any, oneTimeList: Array<T>) {
    if (data == null || data == undefined || oneTimeList.find(x => x == data))
      return;
    oneTimeList.push(data);
    let properties = Object.getOwnPropertyNames(data);
    //alert("properties: " + properties);
    properties.forEach(x => {
      let pData = data[x];
      if (pData == null || pData == undefined || oneTimeList.find(x => x == pData))
        return;
      // alert(pData + " : " + (typeof Date));
      if (pData instanceof Date) {
        data[x] = new Date(Date.UTC(pData.getFullYear(), pData.getMonth(), pData.getDate(), pData.getHours(), pData.getMinutes(), pData.getSeconds(), pData.getMilliseconds()));
      }
      this.fixDateTimeProperties(pData, oneTimeList);
    });
  }

  deserializeReferences<T>(obj: any, ids: {}, mappedObjects: Array<T>): any {
    var type = typeof obj;
    if (type == "string" || type == "number" || obj === Date || obj == null || obj == undefined) {
      return obj;
    }
    if (mappedObjects.indexOf(obj) >= 0 && !obj.$values) {
      return obj;
    }
    mappedObjects.push(obj);
    if (obj.$id) {
      if (obj.$values)
        ids[obj.$id] = obj.$values;
      else
        ids[obj.$id] = obj;
      delete obj.$id;
    }

    this.toCamelCase(obj);

    if (obj.$ref) {
      var ref = obj.$ref;
      obj = ids[obj.$ref];
      return obj;
    }
    if (obj instanceof Array) {
      var newArray = [];
      obj.forEach(x => {
        if (x.$ref) {
          x = ids[x.$ref];
          if (x) {
            newArray.push(x);
            delete x.$ref;
          }
        }
        else {
          newArray.push(this.deserializeReferences(x, ids, mappedObjects));
        }
      });
      return newArray;
    }
    else if (obj.$values) {
      var newArray = [];
      obj.$values.forEach(x => {
        if (x.$ref) {
          x = ids[x.$ref];
          if (x) {
            newArray.push(x);
            delete x.$ref;
          }
        }
        else {
          newArray.push(this.deserializeReferences(x, ids, mappedObjects));
        }
      });
      delete obj.$values;
      return newArray;
    }
    else {
      var properties = Object.getOwnPropertyNames(obj);
      for (var i = 0; i < properties.length; i++) {
        obj[properties[i]] = this.deserializeReferences(obj[properties[i]], ids, mappedObjects);
      }
    }

    return obj;
  }
  serializeReferences<T>(obj: any, isMainObject: boolean, mappedObjects: any): any {
    if (obj == null || obj == undefined)
      return obj;
    var type = typeof obj;
    if (type != "object" || obj instanceof Date)
      return obj;
    // else if (obj instanceof moment || obj._isAMomentObject) {
    //   return new Date(obj);
    // }
    var newObj = null;
    newObj = {};
    let find = mappedObjects.indexOf(obj);
    if (!isMainObject) {
      let currentId = mappedObjects.length + 1;
      newObj["$id"] = currentId;
      mappedObjects.push(obj);
    }
    if (obj instanceof Array) {
      newObj.$values = [];
      if (find != -1) {
        newObj.$values = { $ref: find + 1 };
      }
      else {
        obj.forEach(x => {
          let find = mappedObjects.indexOf(x);
          if (find != -1) {
            newObj.$values.push({ $ref: find + 1 });
          }
          else {
            if (isMainObject)
              newObj.$values.push(this.serializeReferences(x, false, []));
            else
              newObj.$values.push(this.serializeReferences(x, false, mappedObjects));
          }
        });
      }
    }
    else {
      var properties = Object.getOwnPropertyNames(obj);
      for (var i = 0; i < properties.length; i++) {
        let propertyValue = obj[properties[i]];
        let find = mappedObjects.indexOf(propertyValue);
        newObj[properties[i]] = undefined;
        if (find != -1) {
          newObj[properties[i]] = { $ref: find + 1 };
        }
        else {
          if (isMainObject)
            newObj[properties[i]] = this.serializeReferences(propertyValue, false, []);
          else
            newObj[properties[i]] = this.serializeReferences(propertyValue, false, mappedObjects);
        }
      }
    }
    return newObj;
  }
}
