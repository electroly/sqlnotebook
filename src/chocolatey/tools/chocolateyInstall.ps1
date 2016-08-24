$ErrorActionPreference = "Stop"

$packageName = 'SQL Notebook'
$installerType = 'msi'
$url = 'https://sqlnotebook.com/install/SQLNotebook_0_6_0.msi'
$silentArgs = '/quiet'
$validExitCodes = @(0)
$checksum = '336E6C78C2E7A01C9A983FAE96FFA032AB2864CAC8FC58809205F587F0E40369'

Install-ChocolateyPackage "$packageName" "$installerType" "$silentArgs" "$url" -validExitCodes $validExitCodes -checksumType 'sha256' -checksum $checksum
