# Etapa base para o runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Auth.sln", "."]
COPY ["Auth.API/Auth.API.csproj", "Auth.API/"]
COPY ["Auth.Domain/Auth.Domain.csproj", "Auth.Domain/"]
COPY ["Auth.Application/Auth.Application.csproj", "Auth.Application/"]
COPY ["Auth.Data/Auth.Data.csproj", "Auth.Data/"]
COPY ["Auth.Tests/Auth.Tests.csproj", "Auth.Tests/"]

# Restaura as dependências
RUN dotnet restore "Auth.sln"

# Copia o restante do código e realiza o build
COPY . .
WORKDIR "/src/Auth.API"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Etapa para publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final para execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.API.dll"]
