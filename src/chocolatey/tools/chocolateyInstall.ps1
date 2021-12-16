$ErrorActionPreference = "Stop"

$packageName = 'SQL Notebook'
$installerType = 'msi'
$url = 'https://github.com/electroly/sqlnotebook/releases/download/v1.0.1/SQLNotebook-1.0.1.msi'
$silentArgs = '/quiet'
$validExitCodes = @(0)
$checksum = 'DD11217155D66038DF2A3846862622F4EE9FE2A027B46D430B07349B4A383029'

Install-ChocolateyPackage "$packageName" "$installerType" "$silentArgs" "$url" -validExitCodes $validExitCodes -checksumType 'sha256' -checksum $checksum
