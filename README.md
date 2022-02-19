# dotnet-tutorial-webapi-todo-items

Tutorial: [Create a web API with controllers](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio-code)

## Debugging

1. Trust the HTTP development certificate:

    `dotnet dev-certs https --trust`


## Docker

1. Build the docker image:

    `docker build -t dotnet-tutorial-webapi-todo-items .`

1. Create and run the docker container:

    `docker run -dp 5000:80 dotnet-tutorial-webapi-todo-items`

1. Checkout the [todo items api](http://localhost:5000/api/todoitems)
