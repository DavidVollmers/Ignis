FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

EXPOSE 80

COPY bin/Release/net6.0/publish/ .

ENTRYPOINT dotnet Ignis.Website.dll
