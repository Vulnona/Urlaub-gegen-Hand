#!/bin/bash

sleep 10

set -e

# Wait for MySQL to be fully up and running
host="db"
user="root"
password="password"
database="db"

max_retries=30
retry_interval=2

for i in $(seq 1 $max_retries); do
  if mysql -h "$host" -u "$user" -p"$password" -e "SELECT 1" &> /dev/null; then
    echo "MySQL is available - executing init-db.sh"
    mysql -h "$host" -u "$user" -p"$password" "$database" < /docker-entrypoint-initdb.d/data.sql
    echo "Database initialized"
    exit 0
  fi
  echo "MySQL is unavailable - sleeping"
  sleep $retry_interval
done

echo "MySQL did not become available in time - exiting"
exit 1

