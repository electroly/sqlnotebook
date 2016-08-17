$ErrorActionPreference = "Stop"

$packageName = 'SQL Notebook'
$installerType = 'msi'
$url = 'https://sqlnotebook.com/install/SQLNotebook_0_6_0.msi'
$silentArgs = '/quiet'
$validExitCodes = @(0)

Install-ChocolateyPackage "$packageName" "$installerType" "$silentArgs" "$url" -validExitCodes $validExitCodes
