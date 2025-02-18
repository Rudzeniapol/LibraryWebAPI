FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["LibraryAPI/LibraryAPI.csproj", "LibraryAPI/"]
RUN dotnet restore "LibraryAPI/LibraryAPI.csproj"

COPY . .
WORKDIR "/src/LibraryAPI"
RUN dotnet publish "LibraryAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LibraryAPI.dll"]