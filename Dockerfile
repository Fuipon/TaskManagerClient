# Стейдж 1: билд
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Копируем sln и проект
COPY TaskManagerClient.sln ./
COPY TaskManagerClient/ ./TaskManagerClient/

# Восстанавливаем зависимости
RUN dotnet restore "TaskManagerClient/TaskManagerClient.csproj"

# Сборка
RUN dotnet publish "TaskManagerClient/TaskManagerClient.csproj" -c Release -o /app/publish

# Стейдж 2: рантайм
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "TaskManagerClient.dll"]
