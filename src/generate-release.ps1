# Release procedure:
# - Switch to Release build configuration
# - Delete "echo" in post-build event for running signtool
# - Delete src\SqlNotebook\Resources\sqlite-doc.zip
# - Delete publish\
# - Bump AssemblyFileVersion in src\SqlNotebook\Properties\AssemblyInfo.cs
# - Bump publish version in project properties under Publish
# - Rebuild
# - Verify version, signature, and signature timestamp in Release\SqlNotebook.exe
# - Run Release\SqlNotebook.exe to test
# - Publish
# - Run src\generate-release.ps1
# - Update web\download.md with new version and date
# - Delete web\site\
# - Run web\generate-site.ps1
# - Open S3 Management Console in sqlnotebook.com bucket
# - Upload publish\SqlNotebook_X_X_X_X.zip to sqlnotebook.com/install. Make public.
# - Upload publish\Application Files\SqlNotebook_X_X_X_X. Make public.
# - Upload publish\SqlNotebook.application and setup.exe. Make public.
# - Upload web\site. Make public.
# - Replace "echo" in post-build event for running signtool
# - Commit changes using commit message "Version X.X"
# - Push origin master
# - Make release on GitHub

# Before running this script, publish the ClickOnce installer in Visual Studio.
# ..\publish\ will be populated.

# Re-sign setup.exe so it has a timestamp (VS already signed it, but without a timestamp)
& "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe" sign /n "Brian Luft" /tr http://tsa.startssl.com/rfc3161 ..\publish\setup.exe

Remove-Item ..\publish-portable -Recurse -ErrorAction SilentlyContinue
mkdir ..\publish-portable

# folder: 'SqlNotebook_0_4_0_0'
$folder = (dir '..\publish\Application Files' | ForEach {$_.Name} | Sort {$_} | Select -Last 1)
# absFolder: 'C:\Projects\sqlnotebook\publish\Application Files\SqlNotebook_0_4_0_0'
$absFolder = (Resolve-Path ('..\publish\Application Files\' + $folder))

copy "$absFolder\*.*" ..\publish-portable\
dir ..\publish-portable\*.deploy | ForEach {Rename-Item $_.FullName $_.FullName.Replace(".deploy", "")}
del ..\publish-portable\SqlNotebook.application

copy "C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\redist\x64\Microsoft.VC140.CRT\*.dll" ..\publish-portable\

Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem
$inFolder = (Resolve-Path ..\publish-portable)
$outZip = [System.IO.Path]::Combine((Resolve-Path ..\publish), $folder + ".zip")
Remove-Item "..\publish\$folder.zip" -ErrorAction SilentlyContinue
[System.IO.Compression.ZipFile]::CreateFromDirectory($inFolder, $outZip)

Remove-Item ..\publish-portable -Recurse -ErrorAction SilentlyContinue