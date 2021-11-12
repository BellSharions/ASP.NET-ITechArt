#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ASP-NET/ASP-NET.csproj", "ASP-NET/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Business/Business.csproj", "Business/"]
RUN dotnet restore "ASP-NET/ASP-NET.csproj"
COPY . .
WORKDIR "/src/ASP-NET"
RUN dotnet build "ASP-NET.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ASP-NET.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASP-NET.dll"]
