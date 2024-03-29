FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

COPY . ./

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App

RUN apt-get update
RUN apt-get install python3 -y
RUN apt-get install ffmpeg -y
RUN apt-get install libopus0 libopus-dev -y
RUN apt-get install libsodium23 libsodium-dev -y

COPY --from=build-env /App/out .
COPY .env ./
COPY downloaded-binaries/* ./downloaded-binaries/

ENTRYPOINT ["dotnet", "App.dll"]