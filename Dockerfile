FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy the csproj files and restore dependecies (via Nuget)
COPY "FinancialRiskSimulator.sln" "."
COPY "RiskSimulator.WebApi/*.csproj" "RiskSimulator.WebApi/"
COPY "RiskSimulator.Application/*.csproj" "RiskSimulator.Application/"
COPY "RiskSimulator.Data.InMemory/*.csproj" "RiskSimulator.Data.InMemory/"
COPY "RiskSimulator.Infrastructure/*.csproj" "RiskSimulator.Infrastructure/"
COPY "FinancialRiskSimulator.Tests/*.csproj" "FinancialRiskSimulator.Tests/"
RUN dotnet restore

# Copy the project files and build release
COPY "RiskSimulator.WebApi/*.csproj" "RiskSimulator.WebApi/"
COPY "RiskSimulator.Application/*.csproj" "RiskSimulator.Application/"
COPY "RiskSimulator.Data.InMemory/*.csproj" "RiskSimulator.Data.InMemory/"
COPY "RiskSimulator.Infrastructure/*.csproj" "RiskSimulator.Infrastructure/"
COPY "FinancialRiskSimulator.Tests/*.csproj" "FinancialRiskSimulator.Tests/"

RUN dotnet build "RiskSimulator.WebApi/RiskSimulator.WebApi.csproj"
RUN dotnet test  "FinancialRiskSimulator.Tests/FinancialRiskSimulator.Tests.csproj"
RUN dotnet publish "RiskSimulator.WebApi/RiskSimulator.WebApi.csproj" -o /app/published-app

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
EXPOSE 80 443
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "/app/RiskSimulator.WebApi.dll" ]


