$archive = 0
$path = [System.IO.Path]::GetDirectoryName($PSCommandPath)
$zipPath = [System.IO.Path]::Combine($path, "sqlite-doc.zip")
if (Test-Path $zipPath) {
	break
}

try {
	Add-Type -AssemblyName System.IO.Compression
	Add-Type -AssemblyName System.IO.Compression.FileSystem

	Remove-Item $zipPath -ErrorAction SilentlyContinue
	$archive = [System.IO.Compression.ZipFile]::Open($zipPath, [System.IO.Compression.ZipArchiveMode]::Create)
	$utf8 = New-Object System.Text.UTF8Encoding($False)

	# add SQLite official documentation to the zip
	$sourcePath = Resolve-Path ([System.IO.Path]::Combine($path, "..\..\..\ext\sqlite\sqlite-doc.txt"))
	$data = [System.IO.File]::ReadAllText($sourcePath)
	$entry = $archive.CreateEntry("sqlite-doc.txt", [System.IO.Compression.CompressionLevel]::Optimal)
	$stream = $entry.Open()
	$bytes = $utf8.GetBytes($data)
	$stream.Write($bytes, 0, $bytes.Length)
	$stream.Close()
	
	# add our own documentation to the zip
	$docPath = Resolve-Path ([System.IO.Path]::Combine($path, "..\..\..\doc"))
	$docFilenames = ls $docPath | Select -Expand "Name"
	ForEach ($docFilename in $docFilenames) {
		$data = [System.IO.File]::ReadAllText([System.IO.Path]::Combine($docPath, $docFilename))
		$entry = $archive.CreateEntry($docFilename, [System.IO.Compression.CompressionLevel]::Optimal)
		$stream = $entry.Open()
		$bytes = $utf8.GetBytes($data)
		$stream.Write($bytes, 0, $bytes.Length)
		$stream.Close()
	}

	$archive.Dispose()
    Write-Host "Generated sqlite-doc.zip"
} catch {
	if ($archive -ne 0) {
		$archive.Dispose()
	}
	[System.IO.File]::Delete($zipPath)

    Write-Host "Error generating sqlite-doc.zip:"
	Write-Host $_.Exception.Message
}
