# Regenerates src\SqlNotebook\Resources\sqlite-doc.zip

$ErrorActionPreference = "Stop"
Set-StrictMode -Version 3
Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem

$ps1Dir = $PSScriptRoot
$rootDir = Resolve-Path (Join-Path $ps1Dir "..\")
$zipFilePath = Join-Path $rootDir "src\SqlNotebook\Resources\sqlite-doc.zip"
$sqliteDocTxtFilePath = Join-Path $rootDir "ext\sqlite\sqlite-doc.txt"
$docDir = Join-Path $rootDir "doc"
$artDir = Join-Path $rootDir "doc\art"
[System.IO.File]::Delete($zipFilePath)

$utf8 = New-Object System.Text.UTF8Encoding($False)

try {
	$archive = [System.IO.Compression.ZipFile]::Open($zipFilePath, [System.IO.Compression.ZipArchiveMode]::Create)

	# add SQLite official documentation to the zip
    Write-Host "Adding: sqlite-doc.txt"
	$data = [System.IO.File]::ReadAllText($sqliteDocTxtFilePath)
	$entry = $archive.CreateEntry("sqlite-doc.txt", [System.IO.Compression.CompressionLevel]::Optimal)
	$stream = $entry.Open()
	$bytes = $utf8.GetBytes($data)
	$stream.Write($bytes, 0, $bytes.Length)
	$stream.Close()
	
	# add our own documentation to the zip
	$docFilenames = Get-ChildItem -file $docDir | Select -Expand "Name"
	ForEach ($docFilename in $docFilenames) {
        Write-Host "Adding: $docFilename"
		$data = [System.IO.File]::ReadAllBytes([System.IO.Path]::Combine($docDir, $docFilename))
		$entry = $archive.CreateEntry($docFilename, [System.IO.Compression.CompressionLevel]::Optimal)
		$stream = $entry.Open()
		$stream.Write($data, 0, $data.Length)
		$stream.Close()
	}

	$docArtFilenames = Get-ChildItem -file ([System.IO.Path]::Combine($artDir, (Join-Path $artDir "*.png"))) | Select -Expand "Name"
	ForEach ($docArtFilename in $docArtFilenames) {
        Write-Host "Adding: $docArtFilename"
		$data = [System.IO.File]::ReadAllBytes([System.IO.Path]::Combine($artDir, $docArtFilename))
		$entry = $archive.CreateEntry($docArtFilename, [System.IO.Compression.CompressionLevel]::NoCompression)
		$stream = $entry.Open()
		$stream.Write($data, 0, $data.Length)
		$stream.Close()
	}

	$docArtFilenames = Get-ChildItem -file ([System.IO.Path]::Combine($artDir, (Join-Path $artDir "*.svg"))) | Select -Expand "Name"
	ForEach ($docArtFilename in $docArtFilenames) {
        Write-Host "Adding: $docArtFilename"
		$data = [System.IO.File]::ReadAllBytes([System.IO.Path]::Combine($artDir, $docArtFilename))
		$entry = $archive.CreateEntry($docArtFilename, [System.IO.Compression.CompressionLevel]::NoCompression)
		$stream = $entry.Open()
		$stream.Write($data, 0, $data.Length)
		$stream.Close()
	}

	$archive.Dispose()
    Write-Host "Generated sqlite-doc.zip"
} catch {
	[System.IO.File]::Delete($zipFilePath)

    Write-Host "Error generating sqlite-doc.zip:"
	Write-Host $_.Exception.Message
}
