FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Source/Services/Messages/SignalRNotifier/SignalRNotifier.csproj", "Source/Services/Messages/SignalRNotifier/"]
RUN dotnet restore "Source/Services/Messages/SignalRNotifier/SignalRNotifier.csproj"
COPY . .
WORKDIR "/src/Source/Services/Messages/SignalRNotifier"
RUN dotnet build "SignalRNotifier.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SignalRNotifier.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalRNotifier.dll"]
