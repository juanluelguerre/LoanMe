@echo off

echo BUILDING rabitmq ...

docker build -t rabitmq .

docker run -d --hostname my-rabbit --name loanme-rabbitmq -e RABBITMQ_DEFAULT_USER=loanme -e RABBITMQ_DEFAULT_PASS=Password12! rabbitmq:3

REM docker run -d --hostname my-rabbit --name loanme-rabbitmq rabbitmq:3-management