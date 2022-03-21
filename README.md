# dotnet-tutorial-webapi-todo-items

Tutorial: [Create a web API with controllers](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio-code)

## Debugging

1. Trust the HTTP development certificate:

   `dotnet dev-certs https --trust`

## Docker Setup

1. Build the docker image:

   `docker build -t dotnet-tutorial-webapi-todo-items .`

1. Create a `.env` file with the required environment variables:

   ```
   TODOITEMSAPI_Execution__ReadOnly=false
   ```

1. Deploy the app

   1. As a docker container:

      `docker run --name {APP_NAME} -dp {PORT}:80 --env-file ./.env dotnet-tutorial-webapi-todo-items`

   1. As a docker swarm service on a manager node:

      `docker service create --name {SERVICE_NAME} -p {PORT}:80 --env-file ./.env --replicas {NUMBER_OF_REPLICAS} dotnet-tutorial-webapi-todo-items`

1. Checkout the todo items api at `/api/todoitems`
