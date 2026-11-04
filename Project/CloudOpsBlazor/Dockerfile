
# BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY CloudOpsBlazor.csproj ./
RUN dotnet restore "CloudOpsBlazor.csproj"

COPY . .
RUN dotnet publish "CloudOpsBlazor.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# RUNTIME STAGE
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CloudOpsBlazor.dll"]
