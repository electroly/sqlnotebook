# Release procedure:
# - Delete src\SqlNotebook\bin\publish
# - Bump AssemblyFileVersion in src\SqlNotebook\Properties\AssemblyInfo.cs
# - Bump ProductVersion in SqlNotebook.wxs
# - Package
# - Run ps1\New-Release.ps1
# - Verify version in SqlNotebook.exe and .msi
# - Update web\appversion.txt with new version and MSI URL
# - Delete web\site\
# - Run ps1\Update-Website.ps1
# - Create release on GitHub, upload zip and msi.
# - Upload web\site to sqlnotebook.com/. Make public.
# - Update src\chocolatey\sqlnotebook.nuspec with version
# - Update src\chocolatey\tools\chocolateyInstall.ps1 with MSI URL
# - Run choco pack
# - Run choco apikey -k <chocolatey api key> -source https://chocolatey.org/
# - Run choco push .\sqlnotebook.X.X.X.nupkg -s https://chocolatey.org/
# - Commit changes using commit message "Version X.X.X"
# - Push origin master
# - Make release on GitHub

$ErrorActionPreference = "Stop"
Set-StrictMode -Version 3

$wixDir = "C:\Program Files (x86)\WiX Toolset v3.11\bin"
if (-not (Test-Path $wixDir)) {
    throw "WiX not found!"
}

$version = Read-Host -prompt "Version (like 0_6_0)"

$ps1Dir = $PSScriptRoot
$rootDir = (Resolve-Path (Join-Path $ps1Dir "..\")).Path
$srcDir = Join-Path $rootDir "src"
$relDir = Join-Path $srcDir "SqlNotebook\bin\publish"

Remove-Item "$relDir\portable" -Recurse -ErrorAction SilentlyContinue
Remove-Item "$relDir\*.pdb" -ErrorAction SilentlyContinue
Remove-Item "$relDir\*.wixpdb" -ErrorAction SilentlyContinue
Remove-Item "$relDir\*.wixobj" -ErrorAction SilentlyContinue
Remove-Item "$relDir\*.wxs" -ErrorAction SilentlyContinue
Remove-Item "$srcDir\SqlNotebook\Resources\sqlite-doc.zip" -ErrorAction SilentlyContinue

.\Update-DocResourceZip.ps1

Push-Location $relDir

#
# Part 1: Generate MSI
#

$msiFilename = "SQLNotebook_$version.msi"
$zipFilePath = "$relDir\SQLNotebook_$version.zip"

rm "$relDir\$msiFilename" -ErrorAction SilentlyContinue
rm $zipFilePath -ErrorAction SilentlyContinue
rm "$relDir\SqlNotebook.wixobj" -ErrorAction SilentlyContinue
rm "$relDir\SqlNotebook.wxs" -ErrorAction SilentlyContinue

# Replace <!--FILES--> in the wxs file with <File> entries
$filesXml = ""
$fileId = 1

$filenames = ls "$relDir\*" | select -expandprop Name
foreach ($filename in $filenames) {
    $filesXml += "<File Id=""$filename"" Source=""$relDir/$filename""/>`r`n"
    $fileId++
}

$wxs = (Get-Content "$srcdir\SqlNotebook.wxs").Replace("<!--FILES-->", $filesXml)
Set-Content "$relDir\SqlNotebook.wxs" $wxs

copy -force "$srcdir\SqlNotebook\SqlNotebookIcon.ico" "$relDir\SqlNotebookIcon.ico"

#& "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe" sign /n "Brian Luft" /tr http://tsa.startssl.com/rfc3161 "$relDir\SqlNotebook.exe" | Write-Output
#& "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe" sign /n "Brian Luft" /tr http://tsa.startssl.com/rfc3161 "$relDir\SqlNotebookUpdater.exe" | Write-Output

& "$wixDir\candle.exe" -nologo -pedantic "$relDir\SqlNotebook.wxs" | Write-Output
if (-not (Test-Path "$relDir\SqlNotebook.wixobj")) {
    throw "candle failed to produce SqlNotebook.wixobj"
}

& "$wixDir\light.exe" -nologo -pedantic -ext WixUIExtension -cultures:en-us "$relDir\SqlNotebook.wixobj" | Write-Output
if (-not (Test-Path "$relDir\SqlNotebook.msi")) {
    throw "light failed to produce SqlNotebook.msi"
}

$msiFilePath = "$relDir\$msiFilename"
mv "$relDir\SqlNotebook.msi" $msiFilePath

#& "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe" sign /n "Brian Luft" /tr http://tsa.startssl.com/rfc3161 /d $msiFilename $msiFilePath | Write-Output

#
# Part 2: Generate ZIP
#

Remove-Item "$relDir\portable" -Recurse -ErrorAction SilentlyContinue
mkdir "$relDir\portable" -ErrorAction SilentlyContinue

# get the list of necessary files from the .wxs file
$fileLines = [System.IO.File]::ReadAllLines("$relDir\SqlNotebook.wxs")
foreach ($line in $fileLines) {
    if ($line.Contains("<File")) {
        $line = $line.Replace('<File Id="', "").Trim()
        $filename = $line.Substring(0, $line.IndexOf('"'))
        copy "$relDir\$filename" "$relDir\portable\$filename"
    }
}

Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory("$relDir\portable", $zipFilePath)

Pop-Location
