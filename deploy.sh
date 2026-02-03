#!/bin/bash

echo "Starting deployment..."

# Git pull
echo "Pulling latest changes from git..."
git pull

# Docker compose down
echo "Stopping containers..."
docker compose down

# Docker compose pull
echo "Pulling latest images..."
docker compose pull

# Docker compose up
echo "Starting containers..."
docker compose up -d

echo "Deployment completed!"