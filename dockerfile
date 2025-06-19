# Etapa base para o runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["VanFinder.sln", "."]
COPY ["VanFinder.API/VanFinder.API.csproj", "VanFinder.API/"]
COPY ["VanFinder.Domain/VanFinder.Domain.csproj", "VanFinder.Domain/"]
COPY ["VanFinder.Application/VanFinder.Application.csproj", "VanFinder.Application/"]
COPY ["VanFinder.Data/VanFinder.Data.csproj", "VanFinder.Data/"]
COPY ["VanFinder.Tests/VanFinder.Tests.csproj", "VanFinder.Tests/"]

# Restaura as dependências
RUN dotnet restore "VanFinder.sln"

# Copia o restante do código e realiza o build
COPY . .
WORKDIR "/src/VanFinder.API"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Etapa para publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final para execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VanFinder.API.dll"]
