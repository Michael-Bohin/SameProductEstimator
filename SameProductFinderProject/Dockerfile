#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SameProductFinderProject/SameProductEstimator.csproj", "SameProductFinderProject/"]
RUN dotnet restore "./SameProductFinderProject/SameProductEstimator.csproj"
COPY . .
WORKDIR "/src/SameProductFinderProject"
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SameProductEstimator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SameProductEstimator.dll"]