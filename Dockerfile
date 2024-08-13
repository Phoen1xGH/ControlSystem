# Базовый образ microsoft
# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Сборка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл решения
COPY ControlSystem.sln ./

# Копируем проектные файлы и восстанавливаем зависимости для всех проектов
COPY ControlSystem.Domain/*.csproj ./ControlSystem.Domain/
COPY ControlSystem.Services/*.csproj ./ControlSystem.Services/
COPY ControlSystem.DAL/*.csproj ./ControlSystem.DAL/
COPY ControlSystem.MainApp/*.csproj ./ControlSystem.MainApp/
COPY ControlSystem.Tests/*.csproj ./ControlSystem.Tests/

RUN dotnet restore 

COPY . .

WORKDIR "/src/ControlSystem.MainApp"
RUN dotnet build "ControlSystem.MainApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ControlSystem.MainApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ControlSystem.MainApp.dll"]















## Базовый образ microsoft
## Сборка
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#
## Копируем файл решения
#COPY ControlSystem.sln ./
#
## Копируем проектные файлы и восстанавливаем зависимости для всех проектов
#COPY ControlSystem.Domain/*.csproj ./ControlSystem.Domain/
#COPY ControlSystem.Services/*.csproj ./ControlSystem.Services/
#COPY ControlSystem.DAL/*.csproj ./ControlSystem.DAL/
#COPY ControlSystem.MainApp/*.csproj ./ControlSystem.MainApp/
#COPY ControlSystem.Tests/*.csproj ./ControlSystem.Tests/
#
## Восстанавливаем зависимости
#RUN dotnet restore
#
## Копируем остальные файлы
#COPY . .
#
## Сборка и публикация приложения
#WORKDIR /src/ControlSystem.MainApp
#RUN dotnet publish -c Release -o /app/publish
#
## Этап выполнения
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
#WORKDIR /app
#COPY --from=build /app/publish .
#
## Копируем скрипт для применения миграций
#COPY apply-migrations.sh ./
#RUN chmod +x apply-migrations.sh
#
## Запуск приложения
#ENTRYPOINT ["./apply-migrations.sh"]
#