FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

COPY . ./

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App

RUN apt-get update && apt-get install python3 ffmpeg libopus0 libopus-dev libsodium23 libsodium-dev -y

COPY --from=build-env /App/out .
COPY .env ./
COPY downloaded-binaries/* ./downloaded-binaries/

ENTRYPOINT ["dotnet", "App.dll"]