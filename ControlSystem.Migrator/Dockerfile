FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Устанавливаем PostgreSQL client
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем проекты, которые нужны мигратору
COPY ["ControlSystem.Migrator/ControlSystem.Migrator.csproj", "ControlSystem.Migrator/"]
COPY ["ControlSystem.DAL/ControlSystem.DAL.csproj", "ControlSystem.DAL/"]
COPY ["ControlSystem.Domain/ControlSystem.Domain.csproj", "ControlSystem.Domain/"]

RUN dotnet restore "ControlSystem.Migrator/ControlSystem.Migrator.csproj"

# Копируем исходники только этих проектов
COPY ControlSystem.Migrator/ ControlSystem.Migrator/
COPY ControlSystem.DAL/ ControlSystem.DAL/
COPY ControlSystem.Domain/ ControlSystem.Domain/

WORKDIR "/src/ControlSystem.Migrator"
RUN dotnet publish "./ControlSystem.Migrator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ControlSystem.Migrator.dll"]
