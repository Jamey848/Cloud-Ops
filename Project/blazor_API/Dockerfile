
# ==========================================
# BUILD STAGE
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 
# Fetch offical sdk image. Basically the engine to:
# build the application,
# fetch nuget dependancies
# compile c# code
# and so on

WORKDIR /src # Define working directory in the build container.

COPY Recommendation_engine.csproj ./ 
# Fetch the csproj file. Basically the DNA of your project:
# Target .NET framework, NuGet packages, project configurations.

RUN dotnet restore "Recommendation_engine.csproj" 
# Download all dependancies & NuGet packages. Makes it available for build.

COPY . . 
# Copies all the application files from the host machine into the src working dir.
# Hopefully dockerignore now filters out unecessary files like bin, git, etc.

RUN dotnet publish "Recommendation_engine.csproj" \     
	-c Release \ 
        -o /app/publish \ 
        --no-restore # Skip depedanncy restoration


# ==========================================
# RUNTIME STAGE
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Recommendation_engine.dll"] # Just like with a car: all the work is done, but to get it moving
# you have to put in the key and rev the engine. That's essentialy this.