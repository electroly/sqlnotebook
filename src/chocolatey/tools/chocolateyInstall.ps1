$ErrorActionPreference = "Stop"

$packageName = 'SQL Notebook'
$installerType = 'msi'
$silentArgs = '/quiet'
$validExitCodes = @(0)

if ($env:PROCESSOR_ARCHITECTURE -eq 'ARM64') {
    $url = 'https://github.com/electroly/sqlnotebook/releases/download/v1.2.3/SqlNotebook-arm64-1.2.3.msi'
    $checksum = '1A1660757E4E494F1A4DF8B9819939F51F628BBB1E1BFD7F966839C46CC5405D'
} elseif (Get-OSArchitectureWidth 32) {
    $url = 'https://github.com/electroly/sqlnotebook/releases/download/v1.2.3/SqlNotebook-32bit-1.2.3.msi'
    $checksum = '8CE204214924ABB8B8499C8C52AE54035A2D504DC7609C6B29A6B60CBA7CFB34'
} else {
    $url = 'https://github.com/electroly/sqlnotebook/releases/download/v1.2.3/SqlNotebook-64bit-1.2.3.msi'
    $checksum = '94ED3828EA4E6C7F75E7D57ED8A73EEE3D8CFBAF0CD4DF049D6E9DBA3A979210'
}

Install-ChocolateyPackage "$packageName" "$installerType" "$silentArgs" "$url" -validExitCodes $validExitCodes -checksumType 'sha256' -checksum $checksum
