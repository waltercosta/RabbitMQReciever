FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env

WORKDIR /core

COPY *.csproj .

RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /core

COPY --from=0 /core/out .

EXPOSE 443/tcp
EXPOSE 80/tcp

ENTRYPOINT [ "dotnet", "basic.dll"]