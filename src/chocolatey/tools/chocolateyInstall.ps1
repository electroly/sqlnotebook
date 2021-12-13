$ErrorActionPreference = "Stop"

$packageName = 'SQL Notebook'
$installerType = 'msi'
$url = 'https://github.com/electroly/sqlnotebook/releases/download/v1.0.0/SQLNotebook-1.0.0.msi'
$silentArgs = '/quiet'
$validExitCodes = @(0)
$checksum = '07F819E6328AB121EAA9DE0A2355219A3E2C1142E7B06F43ABEC77B8AEA36459'

Install-ChocolateyPackage "$packageName" "$installerType" "$silentArgs" "$url" -validExitCodes $validExitCodes -checksumType 'sha256' -checksum $checksum
