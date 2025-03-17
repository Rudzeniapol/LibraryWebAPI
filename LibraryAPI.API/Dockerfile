FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN apt-get update && \
    apt-get install -y --no-install-recommends libkrb5-3 krb5-user libgssapi-krb5-2 && \
    rm -rf /var/lib/apt/lists/*

COPY ["LibraryAPI.API/LibraryAPI.API.csproj", "LibraryAPI.API/"]
COPY ["LibraryAPI.Application/LibraryAPI.Application.csproj", "LibraryAPI.Application/"]
COPY ["LibraryAPI.Domain/LibraryAPI.Domain.csproj", "LibraryAPI.Domain/"]
COPY ["LibraryAPI.Persistence/LibraryAPI.Persistence.csproj", "LibraryAPI.Persistence/"]
RUN dotnet restore "LibraryAPI.API/LibraryAPI.API.csproj"

COPY . .
WORKDIR "/src/LibraryAPI.API"
RUN dotnet build "LibraryAPI.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LibraryAPI.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

RUN apt-get update && \
    apt-get install -y --no-install-recommends libkrb5-3 krb5-user libgssapi-krb5-2 && \
    rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "LibraryAPI.API.dll"]