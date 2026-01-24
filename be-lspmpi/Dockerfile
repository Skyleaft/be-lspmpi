FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["be-lemdiklat-permapendis/be-lemdiklat-permapendis.csproj", "be-lemdiklat-permapendis/"]
RUN dotnet restore "be-lemdiklat-permapendis/be-lemdiklat-permapendis.csproj"
COPY . .
WORKDIR "/src/be-lemdiklat-permapendis"
RUN dotnet build "./be-lemdiklat-permapendis.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./be-lemdiklat-permapendis.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/uploads/profile-photos
ENTRYPOINT ["dotnet", "be-lemdiklat-permapendis.dll"]
