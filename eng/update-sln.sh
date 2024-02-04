#!/bin/bash
set -euo pipefail

dotnet new sln --force -n image-converter-cs
dotnet sln add ./src/**/*.csproj
