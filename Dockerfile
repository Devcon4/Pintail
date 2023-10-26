FROM mcr.microsoft.com/dotnet/sdk:8.0 as api

ARG VERSION=1.0.0

WORKDIR /build

COPY api/Pintail.WebApi/Pintail.WebApi.csproj ./Pintail.WebApi/Pintail.WebApi.csproj
COPY api/Pintail.Domain/Pintail.Domain.csproj ./Pintail.Domain/Pintail.Domain.csproj
COPY api/Pintail.Infrastructure/Pintail.Infrastructure.csproj ./Pintail.Infrastructure/Pintail.Infrastructure.csproj
COPY api/Pintail.BusinessLogic/Pintail.BusinessLogic.csproj ./Pintail.BusinessLogic/Pintail.BusinessLogic.csproj

RUN dotnet restore ./Pintail.WebApi/Pintail.WebApi.csproj

COPY api/ ./

RUN dotnet publish -c Release --no-restore -o /build/publish /p:Version=${VERSION} ./Pintail.WebApi/Pintail.WebApi.csproj

FROM node:20-alpine as client
WORKDIR /build

COPY client/package.json ./
COPY client/package-lock.json ./

RUN npm ci;

COPY client/ ./

RUN npm run build;

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as entry
ARG USER=clientuser
ARG UID=1001
ARG GID=1001

RUN groupadd -g $GID $USER
RUN useradd --uid $UID --gid $GID --create-home --shell /bin/bash $USER
RUN mkdir /client && chown -R $USER:$USER /client

USER $USER

WORKDIR /client
COPY --chown=$USER:$USER --from=api /build/publish .
COPY --chown=$USER:$USER --from=client /build/dist/pintail ./wwwroot/

# Have to use higher port due to non-root user
ENV ASPNETCORE_URLS="http://*:8080"

EXPOSE 8080
ENTRYPOINT ["dotnet", "Pintail.WebApi.dll"]