FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/DiscordialBot/DiscordialBot.csproj", "DiscordialBot/"]
RUN dotnet restore "src/DiscordialBot/DiscordialBot.csproj"
COPY . .
WORKDIR "/src/DiscordialBot"
RUN dotnet build "DiscordialBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscordialBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscordialBot.dll"]
