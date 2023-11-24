import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MqttSubscriberComponent } from './mqtt-subscriber.component';

describe('MqttSubscriberComponent', () => {
  let component: MqttSubscriberComponent;
  let fixture: ComponentFixture<MqttSubscriberComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MqttSubscriberComponent]
    });
    fixture = TestBed.createComponent(MqttSubscriberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
