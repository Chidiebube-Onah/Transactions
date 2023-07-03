# Microservice Transaction Update Project

The Microservice Transaction Update Project is a solution designed to handle transaction update requests in a distributed and scalable manner. It consists of two microservices: Client and Transactions. The Client microservice simulates sending the UpdateTransaction command and acts as a mock for the crypto third-party API. The Transactions microservice processes the update requests and provides endpoints for fetching all transactions.

## Overview

The project leverages microservice architecture to decouple the functionality into separate services, allowing for independent development, deployment, and scalability. The Client microservice acts as a client-facing API, while the Transactions microservice handles the business logic and data persistence.

### Client Microservice

The Client microservice provides APIs to simulate sending the UpdateTransaction command. It exposes a Swagger documentation interface accessible at http://localhost:8000/swagger, allowing developers to explore and interact with the available endpoints. This microservice acts as a bridge between the client application and the Transactions microservice.

### Transactions Microservice

The Transactions microservice is responsible for processing the UpdateTransaction requests received from the Client microservice. It utilizes technologies such as .NET 6 and Entity Framework Core for data persistence. The microservice provides endpoints for fetching all transactions and exposes a Swagger documentation interface accessible at http://localhost:8001/swagger.

### Communication and Messaging

The communication between the microservices is facilitated by MassTransit and RabbitMQ. MassTransit provides a robust messaging framework, enabling reliable message-based communication between the microservices. RabbitMQ acts as the message broker, ensuring the reliable delivery of messages between the microservices.

### Containerization and Deployment

The project is containerized using Docker, allowing for easy deployment and scalability. The Docker Compose file provided in the project sets up the necessary containers for the microservices, MSSQL Server, and RabbitMQ. This enables seamless deployment and management of the application in a containerized environment.

## Technologies Used

- .NET 6
- Entity Framework Core (EF Core)
- Docker
- MSSQL Server
- MassTransit
- RabbitMQ

## Setup

To set up and run the project, follow these steps:

1. Clone the repository to your local machine.
2. Ensure that Docker is installed and running.
3. Open a terminal or command prompt and navigate to the project directory.
4. Run the following command to build and start the Docker containers:

   ```
   docker-compose up -d
   ```

   This command will build the Docker images and start the containers specified in the Docker Compose file.

5. Wait for the containers to start. You can monitor the container logs to see if there are any errors:

   ```
   docker-compose logs -f
   ```

6. Once the containers are up and running, you can access the Client microservice Swagger documentation at http://localhost:8000/swagger and the Transactions microservice Swagger documentation at http://localhost:8001/swagger.

Feel free to explore the APIs and test the functionality provided by the microservices.

Cheers!
