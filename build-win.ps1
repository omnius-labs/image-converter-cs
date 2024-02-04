Remove-Item ./pub/win-x64/* -Recurse

$Env:NativeDepsPlatform="Windows"
$Env:PlatformTarget="x64"

dotnet publish ./src/ImageConverter -p:PublishSingleFile=true -p:PublishTrimmed=true --runtime win-x64 --configuration Release --self-contained true --output ./pub/win-x64/
