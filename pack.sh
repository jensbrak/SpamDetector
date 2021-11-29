#!/usr/bin/env bash

# Build release version
dotnet restore
dotnet clean
dotnet build -c Release

# Create NuGet packages
dotnet pack Zon3.SpamDetector/Zon3.SpamDetector.csproj --no-build -c Release -o ./artifacts
dotnet pack Zon3.SpamDetector.Localization/Zon3.SpamDetector.Localization.csproj --no-build -c Release -o ./artifacts
