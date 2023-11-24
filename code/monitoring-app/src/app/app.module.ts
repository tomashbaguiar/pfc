import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MqttSubscriberComponent } from './mqtt-subscriber/mqtt-subscriber.component';
import { IMqttServiceOptions, MqttModule } from 'ngx-mqtt';
import { NgChartsModule } from 'ng2-charts';

export const MQTT_SERVICE_OPTIONS: IMqttServiceOptions = {
  hostname: 'localhost',
  port: 9001,
  path: '/mqtt',
};

@NgModule({
  declarations: [
    AppComponent,
    MqttSubscriberComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MqttModule.forRoot(MQTT_SERVICE_OPTIONS),
    NgChartsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
