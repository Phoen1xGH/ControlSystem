# Базовый образ microsoft
# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

# Сборка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем проектные файлы
COPY ControlSystem.Domain/*.csproj ./ControlSystem.Domain/
COPY ControlSystem.Services/*.csproj ./ControlSystem.Services/
COPY ControlSystem.DAL/*.csproj ./ControlSystem.DAL/
COPY ControlSystem.MainApp/*.csproj ./ControlSystem.MainApp/

# Восстановление зависимостей для всех проектов
RUN dotnet restore "./ControlSystem.MainApp/ControlSystem.MainApp.csproj"

# Копирование исходники только нужных проектов
COPY ControlSystem.MainApp/ ControlSystem.MainApp/
COPY ControlSystem.DAL/ ControlSystem.DAL/
COPY ControlSystem.Services/ ControlSystem.Services/
COPY ControlSystem.Domain/ ControlSystem.Domain/

# Релизная сборкка проекта
WORKDIR "/src/ControlSystem.MainApp"
RUN dotnet build "ControlSystem.MainApp.csproj" -c Release -o /app/build

# Публикация
FROM build AS publish
RUN dotnet publish "ControlSystem.MainApp.csproj" -c Release -o /app/publish

# Запуск
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ControlSystem.MainApp.dll"]