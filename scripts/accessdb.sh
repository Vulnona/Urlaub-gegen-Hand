#!/bin/bash
user="user"
password="password"
container="ugh_db_1"
#container="ugh-iteration_bugs_db_1"
#dbname="urlaubgegenhand"
dbname="db"
if [ -z $1 ] ;then
   echo -e "usage:\n$0 u\t\t\t list users\n$0 m\t\t\t list memberships\n"$0 o"\t\t\t open db"
   exit 0
fi
if [ $1 = "u" ];then
    table="Users"
elif [ $1 = "m" ];then
     table="Memberships"
elif [ $1 = "o" ];then
     docker exec -it $container mysql -u$user -p$password -D $dbname
     exit 0
else
    echo usage: "parameter not recognized."
    exit 0
fi
docker exec $container mysql -u$user -p$password -D $dbname -e "select * from $table;"
