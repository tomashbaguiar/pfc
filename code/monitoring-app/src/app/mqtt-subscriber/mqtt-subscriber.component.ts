import { Component, OnDestroy, OnInit, ViewChild  } from '@angular/core';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';
import { IMqttMessage, MqttService } from 'ngx-mqtt';
import { Subscription } from 'rxjs';
import { BaseChartDirective } from 'ng2-charts';
import * as moment from 'moment';

@Component({
  selector: 'app-mqtt-subscriber',
  templateUrl: './mqtt-subscriber.component.html',
  styleUrls: ['./mqtt-subscriber.component.sass']
})
export class MqttSubscriberComponent implements OnInit, OnDestroy {
  private subscription: Subscription | undefined;
  private length: number = 25;

  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  lineChartType: ChartType = 'line';
  lineChartOptions: ChartConfiguration['options'] = {
    responsive: true,
  };
  lineChartLabels: string[] = [];
  lineChartData: ChartData<'line'> = {
    labels: this.lineChartLabels,
    datasets: [],
  };

  constructor(private _mqttService: MqttService) { }

  ngOnInit(): void {
    this.subscription = this._mqttService
      .observe('spaceship/monitoring')
      .subscribe((message: IMqttMessage) => {
        let payload: string = message.payload.toString();
        let keyValuePair = JSON.parse(payload);
          
        let date: string = moment().format('hh:mm:ss')
        this.updateLabels(date);

        let reporting: string[] = [];
        Object.keys(keyValuePair).forEach((key: string) => {
          reporting.push(key);

          if (!this.lineChartData.datasets.some(o => o.label === key)) {
            this.createNewDataSet(key);
          }
        });

        this.lineChartData.datasets.forEach(d => {
          let key = d.label!;
          if (!reporting.includes(key)) return;

          let value = { x: date, y: keyValuePair[key] };

          this.updateData(key, value);
        });
      });
  }

  private createNewDataSet(key: string): void {
    this.lineChartData.datasets.push({
      data: [],
      label: key,
      pointRadius: 10,
    });
    this.chart?.update();
  }

  private updateData(key: string, value: any): void {
    if (this.lineChartData.datasets.find(o => o.label === key)!.data.length >= this.length) {
      this.lineChartData.datasets.find(o => o.label === key)!.data.shift();
    }
    this.lineChartData.datasets.find(o => o.label === key)!.data.push(value);

    this.chart?.update();
  }

  private updateLabels(date: string): void {
    if (this.lineChartLabels.length >= this.length) {
      this.lineChartLabels.shift();
    }
    this.lineChartLabels.push(date);
    this.chart?.update();
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
