user="user"
password="password"
container="ugh_db_1"
dbname="db"

sudo docker exec $container mysqldump --no-tablespaces --opt --skip-extended-insert -u$user -p$password db > "$1backup.sql"

