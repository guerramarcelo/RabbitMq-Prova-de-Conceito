# RabbitMq Prova de Conceito
Prova de conceito usando conceito de filas e rabbitmq

# Arquitetura da prova de conceito utilizando filas
![Arquitetura](/imagens/diagrama-integracao.jpeg)

# Instalando e Executando Docker Container do RabbitMQ com Manager
- docker pull rabbitmq:management
- docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 --restart=always --hostname rabbitmq-master -v /docker/rabbitmq/data:/var/lib/rabbitmq rabbitmq:management
