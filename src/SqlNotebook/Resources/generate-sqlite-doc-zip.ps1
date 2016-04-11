try {
    $path = [System.IO.Path]::GetDirectoryName($PSCommandPath)
	$zipPath = [System.IO.Path]::Combine($path, "sqlite-doc.zip")
	if (Test-Path $zipPath) {
		break
	}

	Add-Type -AssemblyName System.IO.Compression
	Add-Type -AssemblyName System.IO.Compression.FileSystem

	$sourcePath = Resolve-Path ([System.IO.Path]::Combine($path, "..\..\..\ext\sqlite\sqlite-doc.txt"))
	$data = [System.IO.File]::ReadAllText($sourcePath)

	Remove-Item $zipPath -ErrorAction SilentlyContinue
	$archive = [System.IO.Compression.ZipFile]::Open($zipPath, [System.IO.Compression.ZipArchiveMode]::Create)
	$entry = $archive.CreateEntry("sqlite-doc.txt", [System.IO.Compression.CompressionLevel]::Optimal)
	$stream = $entry.Open()
	$utf8 = New-Object System.Text.UTF8Encoding($False)
	$bytes = $utf8.GetBytes($data)
	$stream.Write($bytes, 0, $bytes.Length)
	$stream.Close()
	$archive.Dispose()
    Write-Host "Generated sqlite-doc.zip"
} catch {
    Write-Host "Error generating sqlite-doc.zip:"
	Write-Host $_.Exception.Message
}
