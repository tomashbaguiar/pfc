#!/bin/bash

for i in {1..10}; do
	TEMP=$(($RANDOM % (39 + 1 - 36) + 36))
	HR=$(($RANDOM % (160 + 1 - 55) + 55))
	OXI=$(($RANDOM % (100 + 1 - 95) + 95))
	mosquitto_pub -h localhost -t spaceship/temperature -m ${TEMP}
	sleep 2
	mosquitto_pub -h localhost -t spaceship/heartrate -m ${HR}
	sleep 2
	mosquitto_pub -h localhost -t spaceship/oximetry -m ${OXI}
	sleep 10
done
