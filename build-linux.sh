#!/bin/bash
set -euo pipefail

rm -rf ./pub/linux-x64/*

export NativeDepsPlatform=linux
export PlatformTarget=x64

dotnet publish ./src/ImageConverter -p:PublishSingleFile=true -p:PublishTrimmed=true --runtime linux-x64 --configuration Release --self-contained true --output ./pub/linux-x64/
