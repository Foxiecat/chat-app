FROM mcr.microsoft/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /chat-app.Api
COPY ["chat-app.Api/chat-app.Api.csproj", "chat-app.Api/"]
RUN dotnet restore "chat-app.Api/chat-app.Api.csproj"
COPY . .
RUN dotnet build "./chat-app.Api.csproj"-c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN publish "./chat-app.Api.csproj" -c $BUILD_CONFIGURATION .o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "chat-app.Api.dll"]
