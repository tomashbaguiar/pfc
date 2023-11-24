#!/bin/bash

echo "Stopping Zeebe and Operate"
docker compose -f ~/pfc/infrastructure/docker-compose-zeebe.yaml down

echo "Stopping containers..."
docker stop $(docker ps -a -q)

echo "Removing containers..."
docker rm $(docker ps -a -q)

echo "Removing volumes..."
docker volume rm $(docker volume ls -q)

echo "Removing networks..."
docker network rm $(docker network ls -q)

echo "Done"
