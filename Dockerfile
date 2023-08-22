# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build app
COPY . ./

EXPOSE 5000
EXPOSE 5001
ENV ASPNETCORE_URLS=http://*:5000;https://*:5001
RUN dotnet dev-certs https --trust
ENTRYPOINT ["dotnet", "run"]
