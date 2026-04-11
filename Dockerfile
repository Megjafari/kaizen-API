FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Kaizen.API/Kaizen.API.csproj", "Kaizen.API/"]
RUN dotnet restore "Kaizen.API/Kaizen.API.csproj"
COPY . .
WORKDIR "/src/Kaizen.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kaizen.API.dll"]