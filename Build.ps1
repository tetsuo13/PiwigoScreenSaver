$ErrorActionPreference = "Stop"

$binDir = "PiwigoScreenSaver\bin\Release"
$projectFile = "PiwigoScreenSaver\PiwigoScreenSaver.csproj"

$csproj = [Xml](Get-Content $projectFile)
$assemblyVersion = [Version]$csproj.Project.PropertyGroup.AssemblyVersion
$versionString = "{0}.{1}.{2}" -f $assemblyVersion.Major, $assemblyVersion.Minor, $assemblyVersion.Build

Write-Host "Building version:" $versionString
Write-Host "dotnet --version:" (dotnet --version)

if (Test-Path $binDir) {
    Remove-Item -Path $binDir -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Host "Building $projectFile" -ForegroundColor Magenta
dotnet publish --runtime win-x64 --configuration Release /p:PublishSingleFile=true $projectFile
Write-Host "Done building." -ForegroundColor Green

$publishDir = "$binDir\netcoreapp3.1\win-x64\publish"

Write-Host "Creating archive in $publishDir..." -ForegroundColor Magenta

$screenSaverFileName = "PiwigoScreenSaver.scr"
$downloadFileName = "PiwigoScreenSaver-$versionString.zip"

Rename-Item -Path $publishDir\PiwigoScreenSaver.exe -NewName $screenSaverFileName

Compress-Archive -LiteralPath $publishDir\$screenSaverFileName -DestinationPath $publishDir\$downloadFileName

$filesToFash = @($screenSaverFileName, $downloadFileName)

Write-Host
Write-Host "Hashes for the release notes:" -ForegroundColor Magenta
Write-Host "| File | SHA-256 |"
Write-Host "| ---- | ------- |"

$filesToFash | ForEach-Object {
    "| " + $_ + " | " + (Get-FileHash $publishDir\$_ -Algorithm SHA256).Hash.ToLower() + " |"
}
