# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG PORT

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE ${PORT}


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MovieDB.REST-API/MovieDB.REST-API.csproj", "MovieDB.REST-API/"]
COPY ["Database/Database.csproj", "Database/"]
COPY ["MovieDB.SharedModels/MovieDB.SharedModels.csproj", "MovieDB.SharedModels/"]
RUN dotnet restore "./MovieDB.REST-API/MovieDB.REST-API.csproj"
COPY . .
WORKDIR "/src/MovieDB.REST-API"
RUN dotnet build "./MovieDB.REST-API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MovieDB.REST-API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieDB.REST-API.dll"]
# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.