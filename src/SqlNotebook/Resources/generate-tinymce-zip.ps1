$ErrorActionPreference = "Stop"

function AddToZip($archive, $dir, $filename) {
    $data = [System.IO.File]::ReadAllBytes([System.IO.Path]::Combine($dir, $filename))
    $entry = $archive.CreateEntry($filename, [System.IO.Compression.CompressionLevel]::NoCompression)
    $stream = $entry.Open()
    $stream.Write($data, 0, $data.Length)
    $stream.Close()
}

Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem

$path = [System.IO.Path]::GetDirectoryName($PSCommandPath)
Push-Location $path
$tmPath = Resolve-Path ..\..\..\ext\tinymce\deploy
$zipFilePath = [System.IO.Path]::Combine((Resolve-Path .), "tinymce.zip")
$fatcowPath = Resolve-Path ..\..\..\ext\fatcow
Pop-Location

if (Test-Path $zipFilePath) {
    break
}

[System.IO.Compression.ZipFile]::CreateFromDirectory($tmPath, $zipFilePath, [System.IO.Compression.CompressionLevel]::Optimal, $false)

# add the icons that the editor needs
$archive = [System.IO.Compression.ZipFile]::Open($zipFilePath, [System.IO.Compression.ZipArchiveMode]::Update)
AddToZip $archive $fatcowPath "text_align_left.png"
AddToZip $archive $fatcowPath "text_align_center.png"
AddToZip $archive $fatcowPath "text_align_right.png"
AddToZip $archive $fatcowPath "text_indent.png"
AddToZip $archive $fatcowPath "text_indent_remove.png"
AddToZip $archive $fatcowPath "text_list_bullets.png"
AddToZip $archive $fatcowPath "text_list_numbers.png"
AddToZip $archive $fatcowPath "table.png"
AddToZip $archive $fatcowPath "hrule.png"
AddToZip $archive $fatcowPath "text_bold.png"
AddToZip $archive $fatcowPath "text_italic.png"
AddToZip $archive $fatcowPath "text_underline.png"
AddToZip $archive $fatcowPath "page_white_copy.png"
AddToZip $archive $fatcowPath "cut.png"
AddToZip $archive $fatcowPath "page_white_paste.png"
AddToZip $archive $fatcowPath "font_colors.png"
$archive.Dispose()
