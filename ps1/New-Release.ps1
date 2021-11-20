$ErrorActionPreference = "Stop"
Set-StrictMode -Version 3
Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem

$wixDir = "C:\Program Files (x86)\WiX Toolset v3.11\bin"
if (-not (Test-Path $wixDir)) {
    throw "WiX not found!"
}

$signtool = "C:\Program Files (x86)\Windows Kits\10\bin\10.0.19041.0\x86\signtool.exe"
if (-not (Test-Path $signtool)) {
    throw "Signtool not found!"
}

$ps1Dir = $PSScriptRoot
$rootDir = (Resolve-Path (Join-Path $ps1Dir "..\")).Path
$srcDir = Join-Path $rootDir "src"
$binDir = Join-Path $srcDir "SqlNotebook\bin"
$relDir = Join-Path $srcDir "SqlNotebook\bin\publish"
$tempWxsFilePath = "$srcDir\SqlNotebook\bin\temp.wxs"

Remove-Item "$relDir\portable" -Recurse -ErrorAction SilentlyContinue
Remove-Item "$relDir\*.pdb" -ErrorAction SilentlyContinue
Remove-Item "$relDir\*.xml" -ErrorAction SilentlyContinue
Remove-Item "$relDir\*.wixpdb" -ErrorAction SilentlyContinue
Remove-Item "$relDir\*.wixobj" -ErrorAction SilentlyContinue
Remove-Item "$relDir\*.wxs" -ErrorAction SilentlyContinue
Copy-Item -Force "$rootDir\src\SqlNotebookDb\bin\x64\Release\sqlite3.dll" "$relDir\sqlite3.dll"

Push-Location $relDir

$msiFilename = "SQLNotebook.msi"
$msiFilePath = "$binDir\$msiFilename"
$zipFilePath = "$binDir\SQLNotebook.zip"

rm "$relDir\$msiFilename" -ErrorAction SilentlyContinue
rm $zipFilePath -ErrorAction SilentlyContinue
rm "$relDir\SqlNotebook.wixobj" -ErrorAction SilentlyContinue
rm "$relDir\SqlNotebook.wxs" -ErrorAction SilentlyContinue

& $signtool sign /n "Brian Luft" /tr http://timestamp.digicert.com "$relDir\sqlite3.dll" | Write-Output
& $signtool sign /n "Brian Luft" /tr http://timestamp.digicert.com "$relDir\SqlNotebook.exe" | Write-Output
Copy-Item -Force "$srcdir\SqlNotebook\SqlNotebookIcon.ico" "$relDir\SqlNotebookIcon.ico"

#
# Generate portable ZIP
#

[System.IO.Compression.ZipFile]::CreateFromDirectory($relDir, $zipFilePath)

#
# Generate MSI
#

# Replace <!--FILES--> in the wxs file with <File> entries
$filesXml = ""
$refsXml = ""

Write-Host "`"$wixDir\heat.exe`" dir -o `"$tempWxsFilePath`" -cg SqlNotebookComponentGroup -sfrag -gg -g1 -sreg -svb6 -scom"
& "$wixDir\heat.exe" dir . -o "$tempWxsFilePath" -cg SqlNotebookComponentGroup -sfrag -gg -g1 -sreg -svb6 -scom -suid
$heatLines = [System.IO.File]::ReadAllLines($tempWxsFilePath)
Remove-Item $tempWxsFilePath
$doneWithFilesXml = $false
for ($i = 5; $i -lt $heatLines.Length; $i++) {
    if ($heatLines[$i].Contains("</DirectoryRef>")) {
        $doneWithFilesXml = $true
    }
    if (-not $doneWithFilesXml) {
        $filesXml += $heatLines[$i] + "`r`n"
    }
    if ($heatLines[$i].Contains("<ComponentRef")) {
        $refsXml += $heatLines[$i] + "`r`n"
    }
}
$filesXml = $filesXml.Substring(0, $filesXml.LastIndexOf('</Directory>')).Replace("<Component ", '<Component Win64="yes" ')

$wxs = (Get-Content "$srcdir\SqlNotebook.wxs").Replace("<!--FILES-->", $filesXml).Replace("<!--REFS-->", $refsXml)
Set-Content "$relDir\SqlNotebook.wxs" $wxs

& "$wixDir\candle.exe" -nologo -pedantic "$relDir\SqlNotebook.wxs" | Write-Output
if (-not (Test-Path "$relDir\SqlNotebook.wixobj")) {
    throw "candle failed to produce SqlNotebook.wixobj"
}

& "$wixDir\light.exe" -nologo -pedantic -ext WixUIExtension -cultures:en-us "$relDir\SqlNotebook.wixobj" | Write-Output
if (-not (Test-Path "$relDir\SqlNotebook.msi")) {
    throw "light failed to produce SqlNotebook.msi"
}

Move-Item -Force "$relDir\SqlNotebook.msi" $msiFilePath

& $signtool sign /n "Brian Luft" /tr http://timestamp.digicert.com /d $msiFilename $msiFilePath | Write-Output

Pop-Location
