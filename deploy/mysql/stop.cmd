@echo off

docker stop mysql-db
docker rm mysql-db
docker image rm mysql-db
