FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o published-app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/published-app .
ENTRYPOINT ["dotnet", "dotnet-tutorial-webapi-todo-items.dll"]
