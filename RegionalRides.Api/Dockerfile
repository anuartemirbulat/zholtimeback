FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RegionalRides.Api/RegionalRides.Api.csproj", "RegionalRides.Api/"]
COPY ["Constants/Constants.csproj", "Constants/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["RegionalRides.DAL/RegionalRides.DAL.csproj", "RegionalRides.DAL/"]
COPY ["RegionalRides.Services/RegionalRides.Services.csproj", "RegionalRides.Services/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["DataContracts/DataContracts.csproj", "DataContracts/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["RegionalRides.DataContracts/RegionalRides.DataContracts.csproj", "RegionalRides.DataContracts/"]
RUN dotnet restore "RegionalRides.Api/RegionalRides.Api.csproj"
COPY . .
WORKDIR "/src/RegionalRides.Api"
RUN dotnet build "RegionalRides.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RegionalRides.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:EnvironmentName=Staging

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RegionalRides.Api.dll"]
