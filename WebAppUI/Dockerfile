#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8091
EXPOSE 8090

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebAppUI/WebAppUI.csproj", "WebAppUI/"]
COPY ["WebApplicationApi/WebApplicationApi.csproj", "WebApplicationApi/"]

RUN dotnet restore "./WebAppUI/./WebAppUI.csproj"
RUN dotnet restore "./WebApplicationApi/WebApplicationApi.csproj"

COPY WebAppUI/. ./WebAppUI/
COPY WebApplicationApi/. ./WebApplicationApi/

WORKDIR "/src/WebAppUI"
RUN dotnet build "./WebAppUI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebAppUI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY ["./localhost.pfx", "/https/localhost.pfx"]

# Environment variable for the HTTPS certificate
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="123456"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/https/localhost.pfx"

ENTRYPOINT ["dotnet", "WebAppUI.dll"]