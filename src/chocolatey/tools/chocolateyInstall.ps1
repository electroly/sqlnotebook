$ErrorActionPreference = "Stop"

$packageName = 'SQL Notebook'
$installerType = 'msi'
$url = 'https://github.com/electroly/sqlnotebook/releases/download/v1.1.0/SQLNotebook-1.1.0.msi'
$silentArgs = '/quiet'
$validExitCodes = @(0)
$checksum = 'D6D94DBBAA2B6FC59FBF79D906A8A7C4943F383E192A600308BFAC50F5B8A87D'

Install-ChocolateyPackage "$packageName" "$installerType" "$silentArgs" "$url" -validExitCodes $validExitCodes -checksumType 'sha256' -checksum $checksum
