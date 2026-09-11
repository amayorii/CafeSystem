@echo off
echo Starting CafeSystem Server...
start cmd /k "cd CafeSystem.Server && dotnet run"

echo Waiting 3 seconds to avoid file lock conflict...
timeout /t 3 /nobreak > NUL

echo Starting CafeSystem Client...
start cmd /k "cd CafeSystem.Client && dotnet run"

echo Both services are starting!
