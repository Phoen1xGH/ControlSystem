# Базовый образ microsoft
# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Устанавливаем часовой пояс 
#RUN apt-get update && apt-get install -y tzdata
#ENV TZ=Europe/Moscow

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