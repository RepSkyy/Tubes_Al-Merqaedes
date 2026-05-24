@echo off

echo =====================================
echo Running Leopard2RI Bot
echo =====================================

echo Cleaning...
rmdir /s /q bin
rmdir /s /q obj

echo Restoring packages...
dotnet restore

echo Building bot...
dotnet build -c Debug

echo Starting bot...
dotnet run --framework net10.0 --no-build

echo.
echo =====================================
echo Process finished
echo =====================================

pause