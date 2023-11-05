$ErrorActionPreference = "Stop"

$packageName = 'SQL Notebook'
$installerType = 'msi'
$url = 'https://github.com/electroly/sqlnotebook/releases/download/v1.2.2/SqlNotebook-64bit-1.2.2.msi'
$silentArgs = '/quiet'
$validExitCodes = @(0)
$checksum = 'F13F50F24637E0CAD2A3531B0A57DF0617BEC983B20208B1B056933D89D5AF0C'

Install-ChocolateyPackage "$packageName" "$installerType" "$silentArgs" "$url" -validExitCodes $validExitCodes -checksumType 'sha256' -checksum $checksum
