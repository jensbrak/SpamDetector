#!/usr/bin/env bash

# Build release version
dotnet restore
dotnet clean
dotnet build -c Release

# Create NuGet packages
dotnet pack Zon3.SpamDetector/Zon3.SpamDetector --no-build -c Release -o ./artifacts
