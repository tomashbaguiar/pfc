import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MqttSubscriberComponent } from './mqtt-subscriber/mqtt-subscriber.component';

const routes: Routes = [
  { path: 'real-time', component: MqttSubscriberComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
