#!/bin/sh

echo "Cleaning old build..."
rm -rf bin obj

echo "Restoring packages..."
dotnet restore

echo "Building bot..."
dotnet build -c Debug

echo "Running bot..."
dotnet run --framework net10.0 --no-build