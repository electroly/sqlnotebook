$ErrorActionPreference = "Stop"

$packageName = 'SQL Notebook'
$installerType = 'msi'
$url = 'https://github.com/electroly/sqlnotebook/releases/download/v1.2.1/SQLNotebook-1.2.1.msi'
$silentArgs = '/quiet'
$validExitCodes = @(0)
$checksum = 'DEF5C511AE68EF79DC62138D09632BB834448355DD46E25CAB7B1228916EA792'

Install-ChocolateyPackage "$packageName" "$installerType" "$silentArgs" "$url" -validExitCodes $validExitCodes -checksumType 'sha256' -checksum $checksum
