#!/bin/bash
# todo write a safe version in powershell to ensure a backup before the db server is burned to the ground.
docker stop ugh-db
docker rm ugh-db
# volume probably named incorrectly for windows.
docker volume rm ugh_db-data
