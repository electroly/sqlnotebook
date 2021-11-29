# Downloads non-NuGet deps
$sqliteCodeUrl = 'https://sqlite.org/2021/sqlite-amalgamation-3370000.zip'
$sqliteCodeHash = 'A443AAF5CF345613492EFA679EF1C9CC31BA109DCDF37EE377F61AB500D042FE'
$sqliteDocUrl = 'https://sqlite.org/2021/sqlite-doc-3370000.zip'
$sqliteDocHash = '8AF0C6A3E0850ECD5003D3318F28F0F15D3386D4E67C09EAE6358A7B159A47D8'
$wapiUrl = 'https://github.com/contre/Windows-API-Code-Pack-1.1/archive/a8377ef8bb6fa95ff8800dd4c79089537087d539.zip'
$wapiHash = '38E59E6AE3BF0FD0CCB05C026F7332D3B56AF81D8C69A62882D04CABAD5EF4AE'

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
& "$PSScriptRoot\Update-Docs.ps1"
