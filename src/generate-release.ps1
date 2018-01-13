$ErrorActionPreference = "Stop"

# Release procedure:
# - Switch to Release build configuration
# - Delete src\SqlNotebook\Resources\sqlite-doc.zip
# - Bump AssemblyFileVersion in src\SqlNotebook\Properties\AssemblyInfo.cs
# - Bump ProductVersion in SqlNotebook.wxs
# - Delete all files in Release\
# - Rebuild
# - Run src\generate-release.ps1
# - Verify version, signature, and timestamp in Release\SqlNotebook.exe and .msi
# - Update web\download.md with new version and date, also update two download links
# - Update web\appversion.txt with new version and MSI URL
# - Delete web\site\
# - Run web\generate-site.ps1
# - Open S3 Management Console in sqlnotebook.com bucket
# - Upload Release\SqlNotebook_X_X_X.zip and .msi to sqlnotebook.com/install.  Make public.
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
$reldir = (Resolve-Path "$srcdir\..\Release")
$wixdir = "C:\Program Files (x86)\WiX Toolset v3.10\bin"

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

# Replace <!--UCRT_FILES--> in the wxs file with <File> entries for the C runtime
$crtFilesXml = ""
$crtFileId = 1

$ucrtDllPath = "C:\Program Files (x86)\Windows Kits\10\Redist\ucrt\DLLs\x64"
$ucrtFilenames = ls "$ucrtDllPath\*.dll" | select -expandprop Name
foreach ($filename in $ucrtFilenames) {
    $crtFilesXml += "<File Id=""_Crt$crtFileId"" Source=""$ucrtDllPath\$filename""/>"
    $crtFileId++
}

$vcrDllPath = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\redist\x64\Microsoft.VC140.CRT"
$vcrFilesXml = ""
$vcrFilenames = ls "$vcrDllPath\*.dll" | select -expandprop Name
foreach ($filename in $vcrFilenames) {
    $crtFilesXml += "<File Id=""_Crt$crtFileId"" Source=""$vcrDllPath\$filename""/>"
    $crtFileId++
}

$wxs = (Get-Content "$srcdir\SqlNotebook.wxs").Replace("<!--UCRT_FILES-->", $crtFilesXml)
Set-Content "$reldir\SqlNotebook.wxs" $wxs

copy -force "$srcdir\SqlNotebook\SqlNotebookIcon.ico" "$reldir\SqlNotebookIcon.ico"

& "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe" sign /n "Brian Luft" /tr http://tsa.startssl.com/rfc3161 "$reldir\SqlNotebook.exe" | Write-Output

& "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe" sign /n "Brian Luft" /tr http://tsa.startssl.com/rfc3161 "$reldir\SqlNotebookUpdater.exe" | Write-Output

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

& "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe" sign /n "Brian Luft" /tr http://tsa.startssl.com/rfc3161 /d $msiFilename $msiFilePath | Write-Output

#
# Part 2: Generate ZIP
#

Remove-Item "$reldir\portable" -Recurse -ErrorAction SilentlyContinue
mkdir "$reldir\portable" -ErrorAction SilentlyContinue

# get the list of necessary files from the .wxs file
$fileLines = [System.IO.File]::ReadAllLines("$srcdir\SqlNotebook.wxs")
foreach ($line in $fileLines) {
    if ($line.Contains("<File")) {
        $line = $line.Replace('<File Id="', "").Trim()
        $filename = $line.Substring(0, $line.IndexOf('"'))
        copy "$reldir\$filename" "$reldir\portable\$filename"
    }
}

copy "$ucrtDllPath\*.dll" "$reldir\portable\"
copy "$vcrDllPath\*.dll" "$reldir\portable\"

Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory("$reldir\portable", $zipFilePath)

Pop-Location
