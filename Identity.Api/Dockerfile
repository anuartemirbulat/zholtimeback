FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Identity.Api/Identity.Api.csproj", "Identity.Api/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["DataContracts/DataContracts.csproj", "DataContracts/"]
COPY ["Constants/Constants.csproj", "Constants/"]
COPY ["Identity.DAL/Identity.DAL.csproj", "Identity.DAL/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Identity.Services/Identity.Services.csproj", "Identity.Services/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Identity.DataContract/Identity.DataContract.csproj", "Identity.DataContract/"]
RUN dotnet restore "Identity.Api/Identity.Api.csproj"
COPY . .
WORKDIR "/src/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:EnvironmentName=Staging

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]
