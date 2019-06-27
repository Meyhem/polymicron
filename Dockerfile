FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

WORKDIR /src
COPY . .

RUN dotnet restore PolyMicron
RUN dotnet publish PolyMicron --configuration Release -o publish-bin

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app

COPY --from=build /src/PolyMicron/publish-bin .

EXPOSE 3000
ENTRYPOINT ["dotnet", "PolyMicron.dll"]

