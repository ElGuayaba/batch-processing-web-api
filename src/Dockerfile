#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 1433
EXPOSE 5431

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["BatchProcessingApp.API/BatchProcessingApp.API.csproj", "BatchProcessingApp.API/"]
RUN dotnet restore "BatchProcessingApp.API/BatchProcessingApp.API.csproj"
COPY . .
WORKDIR "/src/BatchProcessingApp.API"
RUN dotnet build "BatchProcessingApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BatchProcessingApp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BatchProcessingApp.API.dll"]