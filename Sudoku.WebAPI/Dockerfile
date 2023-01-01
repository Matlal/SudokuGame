FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MutliWebApp/MutliWebApp.csproj", "MutliWebApp/"]
RUN dotnet restore "MutliWebApp/MutliWebApp.csproj"
COPY . .
WORKDIR "/src/MutliWebApp"
RUN dotnet build "MutliWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MutliWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MutliWebApp.dll"]
