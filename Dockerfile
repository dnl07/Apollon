FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Apollon.Api/Apollon.Api.csproj", "Apollon.Api/"]
COPY ["Apollon.Core/Apollon.Core.csproj", "Apollon.Core/"]
COPY ["Apollon.Models/Apollon.Models.csproj", "Apollon.Models/"]

RUN dotnet restore "Apollon.Api/Apollon.Api.csproj"

COPY ["Apollon.Api/", "Apollon.Api/"]
COPY ["Apollon.Core/", "Apollon.Core/"]
COPY ["Apollon.Models/", "Apollon.Models/"]

RUN dotnet publish "Apollon.Api/Apollon.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:GenerateAssemblyInfo=false \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish ./

EXPOSE 5000
ENTRYPOINT [ "dotnet", "Apollon.Api.dll" ] 