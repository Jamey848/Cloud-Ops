

# ==========================================
# BUILD STAGE
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy project file
COPY Recommendation_engine.csproj ./

# Restore dependencies
RUN dotnet restore Recommendation_engine.csproj

# Copy application source
COPY . .

# Publish application
RUN dotnet publish Recommendation_engine.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ==========================================
# RUNTIME STAGE
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .

EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "Recommendation_engine.dll"]
