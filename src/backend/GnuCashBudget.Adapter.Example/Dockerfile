FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GnuCashBudget.Adapter.Example/GnuCashBudget.Adapter.Example.csproj", "GnuCashBudget.Adapter.Example/"]
COPY ["GnuCashBudget.Adapter.Abstractions/GnuCashBudget.Adapter.Abstractions.csproj", "GnuCashBudget.Adapter.Abstractions/"]
RUN dotnet restore "GnuCashBudget.Adapter.Example/GnuCashBudget.Adapter.Example.csproj"
COPY . .
WORKDIR "/src/GnuCashBudget.Adapter.Example"
RUN dotnet build "GnuCashBudget.Adapter.Example.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GnuCashBudget.Adapter.Example.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GnuCashBudget.Adapter.Example.dll"]
