$ErrorActionPreference = "Stop"

# Build machine setup:
# - Install Wix toolset

# Release procedure:
# - Delete src\SqlNotebook\Resources\sqlite-doc.zip
# - Delete src\SqlNotebook\bin\publish
# - Bump AssemblyFileVersion in src\SqlNotebook\Properties\AssemblyInfo.cs
# - Bump ProductVersion in SqlNotebook.wxs
# - Package
# - PowerShell in src\:
#   .\generate-release.ps1
# - Verify version in SqlNotebook.exe and .msi
# - Update web\appversion.txt with new version and MSI URL
# - Delete web\site\
# - Run web\generate-site.ps1
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

# ---

$version = Read-Host -prompt "Version (like 0_6_0)"

$srcdir = [System.IO.Path]::GetDirectoryName($PSCommandPath)
$reldir = (Resolve-Path "$srcdir\SqlNotebook\bin\publish")
$wixdir = "C:\Program Files (x86)\WiX Toolset v3.11\bin"

Remove-Item "$reldir\portable" -Recurse -ErrorAction SilentlyContinue
Remove-Item "$reldir\*.pdb" -ErrorAction SilentlyContinue
Remove-Item "$reldir\*.wixpdb" -ErrorAction SilentlyContinue
Remove-Item "$reldir\*.wixobj" -ErrorAction SilentlyContinue
Remove-Item "$reldir\*.wxs" -ErrorAction SilentlyContinue

Push-Location $reldir

#
# Part 1: Generate MSI
#

$msiFilename = "SQLNotebook_$version.msi"
$zipFilePath = "$reldir\SQLNotebook_$version.zip"

rm "$reldir\$msiFilename" -ErrorAction SilentlyContinue
rm $zipFilePath -ErrorAction SilentlyContinue
rm "$reldir\SqlNotebook.wixobj" -ErrorAction SilentlyContinue
rm "$reldir\SqlNotebook.wxs" -ErrorAction SilentlyContinue

# Replace <!--FILES--> in the wxs file with <File> entries
$filesXml = ""
$fileId = 1

$filenames = ls "$reldir\*" | select -expandprop Name
foreach ($filename in $filenames) {
    $filesXml += "<File Id=""$filename"" Source=""$reldir/$filename""/>`r`n"
    $fileId++
}

$wxs = (Get-Content "$srcdir\SqlNotebook.wxs").Replace("<!--FILES-->", $filesXml)
Set-Content "$reldir\SqlNotebook.wxs" $wxs

copy -force "$srcdir\SqlNotebook\SqlNotebookIcon.ico" "$reldir\SqlNotebookIcon.ico"

#& "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe" sign /n "Brian Luft" /tr http://tsa.startssl.com/rfc3161 "$reldir\SqlNotebook.exe" | Write-Output
#& "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe" sign /n "Brian Luft" /tr http://tsa.startssl.com/rfc3161 "$reldir\SqlNotebookUpdater.exe" | Write-Output

& "$wixdir\candle.exe" -nologo -pedantic "$reldir\SqlNotebook.wxs" | Write-Output

if (-not (Test-Path "$reldir\SqlNotebook.wixobj")) {
    throw "candle failed to produce SqlNotebook.wixobj"
}

& "$wixdir\light.exe" -nologo -pedantic -ext WixUIExtension -cultures:en-us "$reldir\SqlNotebook.wixobj" | Write-Output

if (-not (Test-Path "$reldir\SqlNotebook.msi")) {
    throw "light failed to produce SqlNotebook.msi"
}

$msiFilePath = "$reldir\$msiFilename"
mv "$reldir\SqlNotebook.msi" $msiFilePath

#& "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe" sign /n "Brian Luft" /tr http://tsa.startssl.com/rfc3161 /d $msiFilename $msiFilePath | Write-Output

#
# Part 2: Generate ZIP
#

Remove-Item "$reldir\portable" -Recurse -ErrorAction SilentlyContinue
mkdir "$reldir\portable" -ErrorAction SilentlyContinue

# get the list of necessary files from the .wxs file
$fileLines = [System.IO.File]::ReadAllLines("$reldir\SqlNotebook.wxs")
foreach ($line in $fileLines) {
    if ($line.Contains("<File")) {
        $line = $line.Replace('<File Id="', "").Trim()
        $filename = $line.Substring(0, $line.IndexOf('"'))
        copy "$reldir\$filename" "$reldir\portable\$filename"
    }
}

Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory("$reldir\portable", $zipFilePath)

Pop-Location
