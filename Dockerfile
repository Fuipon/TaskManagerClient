# Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet publish -c Release -o out

# Используем Nginx для сервинга статических файлов
FROM nginx:alpine
COPY --from=build /app/out/wwwroot /usr/share/nginx/html
