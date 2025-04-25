@echo off
echo TimeTracker Deployment Script
echo ============================

echo 1. Building solution...
dotnet build TimeTracker.sln --configuration Release

echo 2. Starting server...
start cmd /k "dotnet run --project TimeTracker.Server/TimeTracker.Server.csproj --configuration Release"

echo 3. Waiting for server to start...
timeout /t 5

echo 4. Starting client...
start cmd /k "dotnet run --project TimeTracker.Client/TimeTracker.Client.csproj --configuration Release"

echo Deployment completed successfully!
echo Server is running on http://localhost:5033
echo Swagger is available at http://localhost:5033/swagger