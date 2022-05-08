$ErrorActionPreference = "Stop"

$packageName = 'SQL Notebook'
$installerType = 'msi'
$url = 'https://github.com/electroly/sqlnotebook/releases/download/v1.2.0/SQLNotebook-1.2.0.msi'
$silentArgs = '/quiet'
$validExitCodes = @(0)
$checksum = 'F2BB43F9E7F8C67D71370BEC28B1B0AC1A3319DC68967F6F121DA9ED09296723'

Install-ChocolateyPackage "$packageName" "$installerType" "$silentArgs" "$url" -validExitCodes $validExitCodes -checksumType 'sha256' -checksum $checksum
