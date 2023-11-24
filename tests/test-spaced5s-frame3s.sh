#!/bin/bash

for i in {1..10}; do
	TEMP=$((100 * ($RANDOM % (39 + 1 - 36) + 36) / 39))
	HR=$((100 * ($RANDOM % (160 + 1 - 55) + 55) / 160))
	OXI=$(($RANDOM % (100 + 1 - 95) + 95))

	HR_TEMP=$((HR / TEMP))
	OXI_HR=$((OXI / HR))

	mosquitto_pub -h localhost -t spaceship/temperature -m ${TEMP}
	mosquitto_pub -h localhost -t spaceship/heartrate -m ${HR}
	mosquitto_pub -h localhost -t spaceship/oximetry -m ${OXI}

	mosquitto_pub -h localhost -t spaceship/hrtemp -m ${HR_TEMP}
	mosquitto_pub -h localhost -t spaceship/oxihr -m ${OXI_HR}

	sleep 3
done
