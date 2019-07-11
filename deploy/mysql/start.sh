# https://platzi.com/tutoriales/1432-docker/3268-como-crear-un-contenedor-con-docker-mysql-y-persistir-la-informacion/
echo "BUILDING mysql ..."

docker build -t mysql-db .
docker rm -f mysql-db
docker run -d -p 33060:3306 --name mysql-db  -e MYSQL_ROOT_PASSWORD=Password12! -e MYSQL_DATABASE=Customers mysql

# docker run -d -p 33060:3306 --name mysql-db  -e MYSQL_ROOT_PASSWORD=Password12! -e MYSQL_DATABASE=Customers --mount src=mysql-db-data,dst=/var/lib/mysql mysql
#  docker exec -it mysql-db mysql -p