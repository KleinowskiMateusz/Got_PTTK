#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KsiazeczkaPTTK.API/KsiazeczkaPTTK.API.csproj", "KsiazeczkaPTTK.API/"]
COPY ["KsiazeczkaPTTK.Logic/KsiazeczkaPTTK.Logic.csproj", "KsiazeczkaPTTK.Logic/"]
COPY ["KsiazeczkaPTTK.DAL/KsiazeczkaPTTK.DAL.csproj", "KsiazeczkaPTTK.DAL/"]
COPY ["KsiazeczkaPTTK.Domain/KsiazeczkaPTTK.Domain.csproj", "KsiazeczkaPTTK.Domain/"]
RUN dotnet restore "KsiazeczkaPTTK.API/KsiazeczkaPTTK.API.csproj"
COPY . .
WORKDIR "/src/KsiazeczkaPTTK.API"
RUN dotnet build "KsiazeczkaPTTK.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KsiazeczkaPTTK.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KsiazeczkaPTTK.API.dll"]