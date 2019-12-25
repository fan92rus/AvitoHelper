FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
COPY AvitoHelper.csproj Build/
WORKDIR /Build
RUN dotnet restore
COPY . .
RUN dotnet build "AvitoHelper.csproj" -c Release -o /app/build
VOLUME /app/static/
FROM build AS publish
RUN dotnet publish "AvitoHelper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AvitoHelper.dll"]
