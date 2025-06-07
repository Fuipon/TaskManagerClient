# ===== Stage 1: Build =====
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY TaskManagerClient.sln ./
COPY TaskManagerClient/ ./TaskManagerClient/

RUN dotnet publish "TaskManagerClient/TaskManagerClient.csproj" -c Release -o /app/publish

# ===== Stage 2: Serve =====
FROM nginx:alpine AS final
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
