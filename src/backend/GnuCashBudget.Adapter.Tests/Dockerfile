FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GnuCashBudget.Adapter.Tests/GnuCashBudget.Adapter.Tests.csproj", "GnuCashBudget.Adapter.Tests/"]
COPY ["GnuCashBudget.Adapter.Abstractions/GnuCashBudget.Adapter.Abstractions.csproj", "GnuCashBudget.Adapter.Abstractions/"]
RUN dotnet restore "GnuCashBudget.Adapter.Tests/GnuCashBudget.Adapter.Tests.csproj"
COPY . .
WORKDIR "/src/GnuCashBudget.Adapter.Tests"
RUN dotnet build "GnuCashBudget.Adapter.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build
