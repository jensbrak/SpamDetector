#!/usr/bin/env bash

# Prepare
cd ..

# Build release version
dotnet restore
dotnet clean
dotnet build -c Release

# Create NuGet packages
dotnet pack Zon3.SpamDetector/Zon3.SpamDetector --no-build -c Release -o ./artifacts
dotnet pack Zon3.SpamDetector.Localization/Zon3.SpamDetector.Localization --no-build -c Release -o ./artifacts
