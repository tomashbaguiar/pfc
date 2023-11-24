#!/bin/bash

echo "Creating networks..."
docker network create mqtt_network
docker network create template_network
docker network create transformation_network

echo "Creating containers..."
echo "MQTT Broker"
docker run -itd --network mqtt_network -p 1883:1883 -p 9001:9001 --name mqtt_broker toke/mosquitto
echo "Zeebe and Operate"
docker compose -f ~/pfc/infrastructure/docker-compose-zeebe.yaml up -d
sleep 20
echo "Template Database"
docker run -itd --network template_network --name template_db mongodb/mongodb-community-server
echo "Template API"
docker run -itd --network template_network -p 5010:5010 --name template_api template-api
docker network connect transformation_network template_api
echo "Data Transformer"
docker run -itd --network camunda-platform_camunda-platform --name data_transformer data-transformer
docker network connect transformation_network data_transformer
echo "Message Gateway"
docker create -it --network camunda-platform_camunda-platform --name message_gateway message-gateway
docker network connect mqtt_network message_gateway
docker start message_gateway
echo "Event Notifier"
docker run -itd --network camunda-platform_camunda-platform --name event_notifier event-notifier
docker network connect mqtt_network event_notifier
echo "Monitoring Application"
docker run -itd --network mqtt_network -p 8090:80 --name monitoring_app monitoring-app

echo "Done"
