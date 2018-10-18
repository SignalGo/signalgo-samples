import { Component } from '@angular/core';
import { HelloworldService } from './Services/helloworldService';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'signalgo-angularSample';
  constructor(private helloworldService: HelloworldService) {

  }
  ngInit() {

  }

  async HelloClick() {
    try {
      var result = await this.helloworldService.Login("Ali");
      alert("result success: " + result);
    }
    catch (ex) {
      alert("are you start your server? I'm trying to " + environment.serverUrl);
    }
  }

  async ComplexClick() {
    try {
      var result = await this.helloworldService.GetUserInfoes();
      alert("result success to see your object open console");
      console.log(result);
    }
    catch (ex) {
      alert("are you start your server? I'm trying to " + environment.serverUrl);
    }
  }
}
