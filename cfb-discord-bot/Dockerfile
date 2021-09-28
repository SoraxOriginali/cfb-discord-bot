#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["cfb-discord-bot/cfb-discord-bot.csproj", "cfb-discord-bot/"]
RUN dotnet restore "cfb-discord-bot/cfb-discord-bot.csproj"
COPY . .
WORKDIR "/src/cfb-discord-bot"
RUN dotnet build "cfb-discord-bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cfb-discord-bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cfb-discord-bot.dll"]