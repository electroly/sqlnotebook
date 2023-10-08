# Downloads non-NuGet deps

$sqliteCodeUrl = 'https://sqlite.org/2022/sqlite-amalgamation-3400100.zip'
$sqliteCodeHash = '49112CC7328392AA4E3E5DAE0B2F6736D0153430143D21F69327788FF4EFE734'
$sqliteDocUrl = 'https://sqlite.org/2022/sqlite-doc-3400100.zip'
$sqliteDocHash = '72ABAA381459740CFB41F19646F18A5154F93591D34F7EEB3C4F5DC7AE363A29'
$sqliteSrcUrl = 'https://sqlite.org/2022/sqlite-src-3400100.zip'
$sqliteSrcHash = '5064126AA50DB20C35578B612B56C3129425C0506ED4D1610EFA4A0F01BDF8D0'

$wapiUrl = 'https://github.com/contre/Windows-API-Code-Pack-1.1/archive/a8377ef8bb6fa95ff8800dd4c79089537087d539.zip'
$wapiHash = '38E59E6AE3BF0FD0CCB05C026F7332D3B56AF81D8C69A62882D04CABAD5EF4AE'

$sqleanVersion = '0.17.2'
$sqleanZipUrl = "https://github.com/nalgeon/sqlean/releases/download/$sqleanVersion/sqlean-win-x64.zip"
$sqleanZipHash = '0FEDD20244049C0B89B46EA19D4DB4E29D68158887328BB0EED14283BC46FF9F'

$global:ProgressPreference = "SilentlyContinue"
$ErrorActionPreference = "Stop"
Set-StrictMode -Version 3
Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem

$ps1Dir = $PSScriptRoot
$rootDir = (Resolve-Path (Join-Path $ps1Dir "..\")).Path
$extDir = Join-Path $rootDir "ext"
$downloadsDir = Join-Path $extDir "downloads"
if (-not (Test-Path $downloadsDir)) {
    mkdir $downloadsDir
}

function Update-WindowsApiCodePack {
    $wapiDir = Join-Path $extDir "Windows-API-Code-Pack"
    if (Test-Path $wapiDir) { Remove-Item -Force -Recurse $wapiDir }
    mkdir $wapiDir

    $wapiFilename = [System.IO.Path]::GetFileName($wapiUrl)
    $wapiFilePath = Join-Path $downloadsDir $wapiFilename
    if (-not (Test-Path $wapiFilePath)) {
        Write-Host "Downloading: $wapiUrl"
        Invoke-WebRequest -UseBasicParsing -Uri $wapiUrl -OutFile $wapiFilePath
    }
    VerifyHash $wapiFilePath $wapiHash
    Write-Host "Expanding: $wapiFilePath"
    Expand-Archive -LiteralPath $wapiFilePath -DestinationPath $wapiDir
    Move-Item "$wapiDir\Windows-*\*" "$wapiDir\"
    Remove-Item "$wapiDir\Windows-*"

    # Modify the target frameworks to remove targets we don't want.
    $csprojs = [System.IO.Directory]::GetFiles("$wapiDir", "*.csproj", [System.IO.SearchOption]::AllDirectories)
    foreach ($csproj in $csprojs) {
        $txt = [System.IO.File]::ReadAllText($csproj)
        $txt = $txt.Replace("net452;net462;net472;net48;netcoreapp3.1;", "")
        $txt = $txt.Replace("net5.0-windows", "net6.0-windows")
        [System.IO.File]::WriteAllText($csproj, $txt)
    }

    # No idea why this patch is needed
    $cs = [System.IO.File]::ReadAllText("$wapiDir\source\WindowsAPICodePack\Shell\Resources\LocalizedMessages.Designer.cs")
    $cs = $cs.Replace("Microsoft.WindowsAPICodePack.Resources", "Microsoft.WindowsAPICodePack.Shell.Resources")
    [System.IO.File]::WriteAllText("$wapiDir\source\WindowsAPICodePack\Shell\Resources\LocalizedMessages.Designer.cs", $cs)

    # This patch ignores warnings that we don't intend to fix in the third party code.
    $x = [System.IO.File]::ReadAllText("$wapiDir\source\WindowsAPICodePack\Core\Core.csproj")
    $x = $x.Replace('<AssemblyName>', '<NoWarn>$(NoWarn);CS1591;CS1587;CS8073;SYSLIB0003</NoWarn><AssemblyName>')
    [System.IO.File]::WriteAllText("$wapiDir\source\WindowsAPICodePack\Core\Core.csproj", $x)

    $x = [System.IO.File]::ReadAllText("$wapiDir\source\WindowsAPICodePack\Shell\Shell.csproj")
    $x = $x.Replace('<AssemblyName>', '<NoWarn>$(NoWarn);CS8073;CS1572;CS1591;CS0618;CS0108;CS7023;CS1587;SYSLIB00</NoWarn><AssemblyName>')
    [System.IO.File]::WriteAllText("$wapiDir\source\WindowsAPICodePack\Shell\Shell.csproj", $x)
}

function Update-Sqlean {
    $sqleanDir = Join-Path $extDir "sqlean"
    if (Test-Path $sqleanDir) { Remove-Item -Force -Recurse $sqleanDir }
    mkdir $sqleanDir

    $filename = [System.IO.Path]::GetFileName($sqleanZipUrl)

    $filePath = Join-Path $downloadsDir "sqlean-win-x64-$sqleanVersion.zip"
    if (-not (Test-Path $filePath)) {
        Write-Host "Downloading: $sqleanZipUrl"
        Invoke-WebRequest -UseBasicParsing -Uri $sqleanZipUrl -OutFile $filePath
    }
    VerifyHash $filePath $sqleanZipHash

    Expand-Archive $filePath -DestinationPath $sqleanDir
}

function Update-Sqlite {
    $sqliteDir = Join-Path $extDir "sqlite"
    Remove-Item -Force -Recurse $sqliteDir -ErrorAction SilentlyContinue
    mkdir $sqliteDir

    # code
    $sqliteCodeFilename = [System.IO.Path]::GetFileName($sqliteCodeUrl)
    $sqliteCodeDirName = [System.IO.Path]::GetFileNameWithoutExtension($sqliteCodeUrl)
    $sqliteCodeFilePath = Join-Path $downloadsDir $sqliteCodeFilename
    if (-not (Test-Path $sqliteCodeFilePath)) {
        Write-Host "Downloading: $sqliteCodeUrl"
        Invoke-WebRequest -UseBasicParsing -Uri $sqliteCodeUrl -OutFile $sqliteCodeFilePath
    }
    VerifyHash $sqliteCodeFilePath $sqliteCodeHash
    Remove-Item -Force -Recurse "$sqliteDir\$sqliteCodeDirName" -ErrorAction SilentlyContinue
    Write-Host "Expanding: $sqliteCodeFilePath"
    Expand-Archive -LiteralPath $sqliteCodeFilePath -DestinationPath $sqliteDir
    Move-Item -Force "$sqliteDir\$sqliteCodeDirName\sqlite3.c" "$sqliteDir\sqlite3.c"
    Move-Item -Force "$sqliteDir\$sqliteCodeDirName\sqlite3.h" "$sqliteDir\sqlite3.h"
    Move-Item -Force "$sqliteDir\$sqliteCodeDirName\sqlite3ext.h" "$sqliteDir\sqlite3ext.h"
    Remove-Item -Force -Recurse "$sqliteDir\$sqliteCodeDirName"

    # doc
    $sqliteDocFilename = [System.IO.Path]::GetFileName($sqliteDocUrl)
    $sqliteDocDirName = [System.IO.Path]::GetFileNameWithoutExtension($sqliteDocUrl)
    $sqliteDocFilePath = Join-Path $downloadsDir $sqliteDocFilename
    if (-not (Test-Path $sqliteDocFilePath)) {
        Write-Host "Downloading: $sqliteDocUrl"
        Invoke-WebRequest -UseBasicParsing -Uri $sqliteDocUrl -OutFile $sqliteDocFilePath
    }
    VerifyHash $sqliteDocFilePath $sqliteDocHash
    Remove-Item -Force -Recurse "$sqliteDir\$sqliteDocDirName" -ErrorAction SilentlyContinue
    Write-Host "Expanding: $sqliteDocFilePath"
    Expand-Archive -LiteralPath $sqliteDocFilePath -DestinationPath $sqliteDir
    Remove-Item -Force -Recurse "$sqliteDir\sqlite-doc" -ErrorAction SilentlyContinue
    Rename-Item -LiteralPath "$sqliteDir\$sqliteDocDirName" "sqlite-doc"

    # src
    $sqliteSrcFilename = [System.IO.Path]::GetFileName($sqliteSrcUrl)
    $sqliteSrcDirName = [System.IO.Path]::GetFileNameWithoutExtension($sqliteSrcUrl)
    $sqliteSrcFilePath = Join-Path $downloadsDir $sqliteSrcFilename
    if (-not (Test-Path $sqliteSrcFilePath)) {
        Write-Host "Downloading: $sqliteSrcUrl"
        Invoke-WebRequest -UseBasicParsing -Uri $sqliteSrcUrl -OutFile $sqliteSrcFilePath
    }
    VerifyHash $sqliteSrcFilePath $sqliteSrcHash
    Remove-Item -Force -Recurse "$sqliteDir\$sqliteSrcDirName" -ErrorAction SilentlyContinue
    Write-Host "Expanding: $sqliteSrcFilePath"
    Expand-Archive -LiteralPath $sqliteSrcFilePath -DestinationPath $sqliteDir
    Remove-Item -Force -Recurse "$sqliteDir\sqlite-src" -ErrorAction SilentlyContinue
    Rename-Item -LiteralPath "$sqliteDir\$sqliteSrcDirName" "sqlite-src"

    # update enum TokenType.cs
    $notebookCsFilePath = "$rootDir\src\SqlNotebookScript\TokenType.cs"
    if (-not (Test-Path $notebookCsFilePath)) {
        throw "File not found: $notebookCsFilePath"
    }
    $tokenTypeEnumCode = ""
    $sqliteLines = [System.IO.File]::ReadAllLines("$sqliteDir\sqlite3.c")
    foreach ($sqliteLine in $sqliteLines) {
        if ($sqliteLine.StartsWith('#define TK_')) {
            $reformatted = $sqliteLine.Substring('#define TK_'.Length)
            $index = $reformatted.IndexOf(' ')
            $cName = $reformatted.Substring(0, $index)
            $index = $reformatted.LastIndexOf(' ')
            $number = [int]$reformatted.Substring($index + 1)

            # $cName is like "LIKE_KW" but we want "LikeKw"
            $properName = ""
            $nextCharIsCapital = $true
            foreach ($ch in $cName.ToCharArray()) {
                if ($nextCharIsCapital) {
                    $properName += "$ch".ToUpperInvariant()
                    $nextCharIsCapital = $false
                } elseif ($ch -eq '_') {
                    $nextCharIsCapital = $true
                } else {
                    $properName += "$ch".ToLowerInvariant()
                }
            }

            $tokenTypeEnumCode += "    $properName = $number,`r`n"
        } elseif ($sqliteLine.Contains('End of parse.h')) {
            break
        }
    }
    $notebookCs = [System.IO.File]::ReadAllText($notebookCsFilePath)
    $startIndex = $notebookCs.IndexOf('public enum TokenType')
    if ($startIndex -eq -1) {
        throw "Can't find TokenType in $notebookCsFilePath"
    }
    $startIndex = $notebookCs.IndexOf("{", $startIndex)
    if ($startIndex -eq -1) {
        throw "Can't find TokenType's open brace in $notebookCsFilePath"
    }
    $startIndex = $notebookCs.IndexOf("`n", $startIndex)
    if ($startIndex -eq -1) {
        throw "Can't find TokenType's starting newline in $notebookCsFilePath"
    }
    $startIndex++
    $endIndex = $notebookCs.IndexOf('}', $startIndex)
    if ($endIndex -eq -1) {
        throw "Can't find TokenType's end brace in $notebookCsFilePath"
    }
    $notebookCs = $notebookCs.Substring(0, $startIndex) + $tokenTypeEnumCode + $notebookCs.Substring($endIndex)
    [System.IO.File]::WriteAllText($notebookCsFilePath, $notebookCs)
    Write-Host "Rewrote: $notebookCsFilePath"

    # LF -> CRLF
    $sqliteDocDir = "$sqliteDir/sqlite-doc"
    $filePaths = Dir -Recurse $sqliteDocDir | % { $_.FullName }
    foreach ($filePath in $filePaths) {
        if ($filePath.EndsWith(".html")) {
            $html = [System.IO.File]::ReadAllText($filePath)
            $newHtml = $html
            $newHtml = $newHtml.Replace("`r`n", "`n")
            $newHtml = $newHtml.Replace("`n", "`r`n")
            if ($html -ne $newHtml) {
                [System.IO.File]::WriteAllText($filePath, $newHtml)
            }
        }
    }
}

function DeleteIfExists($path) {
    if (Test-Path $path) {
        Remove-Item $path -Recurse
    }
}

function VerifyHash($filePath, $expectedHash) {
    $actualHash = (Get-FileHash $filePath).Hash
    if ($expectedHash -ne $actualHash) {
        throw "Hash verification failed for $filePath"
    }
}

Update-Sqlite
Update-WindowsApiCodePack
Update-Sqlean
& "$PSScriptRoot\Update-Docs.ps1"
