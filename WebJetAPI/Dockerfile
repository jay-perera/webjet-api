FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["WebJetAPI/WebJetAPI.csproj", "WebJetAPI/"]
RUN dotnet restore "WebJetAPI/WebJetAPI.csproj"
COPY . .
WORKDIR "/src/WebJetAPI"
RUN dotnet build "WebJetAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebJetAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebJetAPI.dll"]