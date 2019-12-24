if (Test-Path PiwigoScreenSaver\bin\Release) {
    Remove-Item -Path PiwigoScreenSaver\bin\Release -Recurse -Force -ErrorAction SilentlyContinue
}

dotnet publish --runtime win-x64 --configuration Release /p:PublishSingleFile=true .\PiwigoScreenSaver\PiwigoScreenSaver.csproj

$publishDir = "PiwigoScreenSaver\bin\Release\netcoreapp3.1\win-x64\publish"

Rename-Item -Path $publishDir\PiwigoScreenSaver.exe -NewName PiwigoScreenSaver.scr

Compress-Archive -LiteralPath $publishDir\PiwigoScreenSaver.scr -DestinationPath $publishDir\PiwigoScreenSaver.zip
