# Utilisez l'image SDK pour construire, publier, et tester
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copiez les fichiers de projet
COPY API_Clients/API_Clients.csproj API_Clients/
COPY API_Clients.Tests/API_Clients.Tests.csproj API_Clients.Tests/

# Restaurer les dépendances
RUN dotnet restore API_Clients/API_Clients.csproj
RUN dotnet restore API_Clients.Tests/API_Clients.Tests.csproj

# Copiez tous les fichiers sources
COPY . .

# Construisez les projets
WORKDIR /src/API_Clients
RUN dotnet build -c Release -o /app/build

WORKDIR /src/API_Clients.Tests
RUN dotnet build -c Release -o /app/build

# Exécutez les tests
WORKDIR /src/API_Clients.Tests
RUN dotnet test --no-restore --verbosity normal

# Publiez le projet API_Clients
WORKDIR /src/API_Clients
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Utilisez l'image de base .NET pour ASP.NET Core pour la phase finale
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API_Clients.dll"]
