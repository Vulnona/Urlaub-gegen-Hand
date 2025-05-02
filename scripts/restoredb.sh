#!/bin/bash
user="user"
root="root"
password="password"
container="ugh_db_1"
dbname="db"
#warning untested
if [ -z $1 ]
then
    echo "no backup file given"
    exit 0
fi
sudo docker cp $1 $container:/backup.sql
sudo docker exec $container mysql -u$user -p$password -e "drop database $dbname"
sudo docker exec $container mysql -u$user -p$password -e "create database $dbname"
sudo docker exec $container sh -c "mysql -u$root -p$password -A $dbname < /backup.sql"
