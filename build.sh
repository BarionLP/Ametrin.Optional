#!/usr/bin/env bash
set -e
# dotnet clean
rm -rf "$HOME/.nuget/packages/ametrin.optional.analyzer"
rm -rf "$HOME/.nuget/packages/ametrin.optional"
# rm -rf "$HOME/.nuget/packages/ametrin.optional.testing.tunit"
dotnet pack ./analyzer/ -c Release
dotnet pack ./src/ -c Release
dotnet pack ./testing/TUnit/ -c Release
dotnet build
dotnet run --project .\test\