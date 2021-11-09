# Downloads non-NuGet deps
$sqliteCodeUrl = 'https://sqlite.org/2021/sqlite-amalgamation-3360000.zip'
$sqliteCodeHash = '999826FE4C871F18919FDB8ED7EC9DD8217180854DD1FE21EEA96AED36186729'
$sqliteDocUrl = 'https://sqlite.org/2021/sqlite-doc-3360000.zip'
$sqliteDocHash = '79F03BF2B4AFD845B66E7D07C7FA08C46A30C2484A4A5F8508DB653C9DA5D32D'
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
    if (-not (Test-Path $wapiDir)) {
        mkdir $wapiDir
    }

    $wapiFilename = [System.IO.Path]::GetFileName($wapiUrl)
    $wapiFilePath = Join-Path $downloadsDir $wapiFilename
    if (-not (Test-Path $wapiFilePath)) {
        Write-Host "Downloading: $wapiUrl"
        Invoke-WebRequest -UseBasicParsing -Uri $wapiUrl -OutFile $wapiFilePath
    }
    VerifyHash $wapiFilePath $wapiHash
    Write-Host "Expanding: $wapiFilePath"
    Remove-Item -Force -Recurse "$wapiDir\*" -ErrorAction SilentlyContinue
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
}

function Update-Sqlite {
    $sqliteDir = Join-Path $extDir "sqlite"
    if (-not (Test-Path $sqliteDir)) {
        mkdir $sqliteDir
    }

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

    # update enum TokenType in INotebook.cs
    $notebookCsFilePath = "$rootDir\src\SqlNotebookScript\INotebook.cs"
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

            $tokenTypeEnumCode += "        $properName = $number,`r`n"
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
    $endIndex = $notebookCs.IndexOf('    }', $startIndex)
    if ($endIndex -eq -1) {
        throw "Can't find TokenType's end brace in $notebookCsFilePath"
    }
    $notebookCs = $notebookCs.Substring(0, $startIndex) + $tokenTypeEnumCode + $notebookCs.Substring($endIndex)
    [System.IO.File]::WriteAllText($notebookCsFilePath, $notebookCs)
    Write-Host "Rewrote: $notebookCsFilePath"

    # Remove stuff that doesn't apply to SQL Notebook
    $sqliteDocDir = "$sqliteDir/sqlite-doc"
    DeleteIfExists "$sqliteDocDir/doc_backlink_crossref.html"
    DeleteIfExists "$sqliteDocDir/doc_keyword_crossref.html"
    DeleteIfExists "$sqliteDocDir/doc_pagelink_crossref.html"
    DeleteIfExists "$sqliteDocDir/doc_target_crossref.html"
    DeleteIfExists "$sqliteDocDir/favicon.ico"
    DeleteIfExists "$sqliteDocDir/copyright-release.pdf"
    DeleteIfExists "$sqliteDocDir/cvstrac.css"
    DeleteIfExists "$sqliteDocDir/robots.txt"
    DeleteIfExists "$sqliteDocDir/toc.db"
    DeleteIfExists "$sqliteDocDir/vdbe.html"
    DeleteIfExists "$sqliteDocDir/sqlanalyze.html"
    DeleteIfExists "$sqliteDocDir/requirements.html"
    DeleteIfExists "$sqliteDocDir/oldnews.html"
    DeleteIfExists "$sqliteDocDir/pressrelease-20071212.html"
    DeleteIfExists "$sqliteDocDir/c_interface.html"
    DeleteIfExists "$sqliteDocDir/capi3.html"
    DeleteIfExists "$sqliteDocDir/capi3ref.html"
    DeleteIfExists "$sqliteDocDir/cli.html"
    DeleteIfExists "$sqliteDocDir/compile.html"
    DeleteIfExists "$sqliteDocDir/34to35.html"
    DeleteIfExists "$sqliteDocDir/35to36.html"
    DeleteIfExists "$sqliteDocDir/affcase1.html"
    DeleteIfExists "$sqliteDocDir/amalgamation.html"
    DeleteIfExists "$sqliteDocDir/backup.html"
    DeleteIfExists "$sqliteDocDir/compile.html"
    DeleteIfExists "$sqliteDocDir/changes.html"
    DeleteIfExists "$sqliteDocDir/chronology.html"
    DeleteIfExists "$sqliteDocDir/cintro.html"
    DeleteIfExists "$sqliteDocDir/consortium.html"
    DeleteIfExists "$sqliteDocDir/consortium_agreement-20071201.html"
    DeleteIfExists "$sqliteDocDir/copyright-release.html"
    DeleteIfExists "$sqliteDocDir/crew.html"
    DeleteIfExists "$sqliteDocDir/custombuild.html"
    DeleteIfExists "$sqliteDocDir/datatypes.html"
    DeleteIfExists "$sqliteDocDir/dev.html"
    DeleteIfExists "$sqliteDocDir/doclist.html"
    DeleteIfExists "$sqliteDocDir/docs.html"
    DeleteIfExists "$sqliteDocDir/download.html"
    DeleteIfExists "$sqliteDocDir/fileio.html"
    DeleteIfExists "$sqliteDocDir/footprint.html"
    DeleteIfExists "$sqliteDocDir/howtocompile.html"
    DeleteIfExists "$sqliteDocDir/hp1.html"
    DeleteIfExists "$sqliteDocDir/shortnames.html"
    DeleteIfExists "$sqliteDocDir/sitemap.html"
    DeleteIfExists "$sqliteDocDir/sqldiff.html"
    DeleteIfExists "$sqliteDocDir/support.html"
    DeleteIfExists "$sqliteDocDir/btreemodule.html"
    DeleteIfExists "$sqliteDocDir/keyword_index.html"
    DeleteIfExists "$sqliteDocDir/news.html"
    DeleteIfExists "$sqliteDocDir/opcode.html"
    DeleteIfExists "$sqliteDocDir/famous.html"
    DeleteIfExists "$sqliteDocDir/books.html"
    DeleteIfExists "$sqliteDocDir/fts3.html"
    DeleteIfExists "$sqliteDocDir/unlock_notify.html"
    DeleteIfExists "$sqliteDocDir/index.html"
    DeleteIfExists "$sqliteDocDir/sqlite.html"
    DeleteIfExists "$sqliteDocDir/different.html"
    DeleteIfExists "$sqliteDocDir/pgszchng2016.html"
    DeleteIfExists "$sqliteDocDir/about.html"
    DeleteIfExists "$sqliteDocDir/asyncvfs.html"
    DeleteIfExists "$sqliteDocDir/whentouse.html"
    DeleteIfExists "$sqliteDocDir/arch.html"
    DeleteIfExists "$sqliteDocDir/undoredo.html"
    DeleteIfExists "$sqliteDocDir/aff_short.html"
    DeleteIfExists "$sqliteDocDir/malloc.html"
    DeleteIfExists "$sqliteDocDir/errlog.html"
    DeleteIfExists "$sqliteDocDir/features.html"
    DeleteIfExists "$sqliteDocDir/formatchng.html"
    DeleteIfExists "$sqliteDocDir/fileformat.html"
    DeleteIfExists "$sqliteDocDir/fileformat2.html"
    DeleteIfExists "$sqliteDocDir/testing.html"
    DeleteIfExists "$sqliteDocDir/inmemorydb.html"
    DeleteIfExists "$sqliteDocDir/mmap.html"
    DeleteIfExists "$sqliteDocDir/mostdeployed.html"
    DeleteIfExists "$sqliteDocDir/mingw.html"
    DeleteIfExists "$sqliteDocDir/vfs.html"
    DeleteIfExists "$sqliteDocDir/not-found.html"
    DeleteIfExists "$sqliteDocDir/privatebranch.html"
    DeleteIfExists "$sqliteDocDir/getthecode.html"
    DeleteIfExists "$sqliteDocDir/rbu.html"
    DeleteIfExists "$sqliteDocDir/loadext.html"
    DeleteIfExists "$sqliteDocDir/appfileformat.html"
    DeleteIfExists "$sqliteDocDir/quickstart.html"
    DeleteIfExists "$sqliteDocDir/selfcontained.html"
    DeleteIfExists "$sqliteDocDir/serverless.html"
    DeleteIfExists "$sqliteDocDir/rtree.html"
    DeleteIfExists "$sqliteDocDir/rescode.html"
    DeleteIfExists "$sqliteDocDir/session.html"
    DeleteIfExists "$sqliteDocDir/sharedcache.html"
    DeleteIfExists "$sqliteDocDir/th3.html"
    DeleteIfExists "$sqliteDocDir/version3.html"
    DeleteIfExists "$sqliteDocDir/onefile.html"
    DeleteIfExists "$sqliteDocDir/tclsqlite.html"
    DeleteIfExists "$sqliteDocDir/uri.html"
    DeleteIfExists "$sqliteDocDir/threadsafe.html"
    DeleteIfExists "$sqliteDocDir/versionnumbers.html"
    DeleteIfExists "$sqliteDocDir/vtab.html"
    DeleteIfExists "$sqliteDocDir/wal.html"
    DeleteIfExists "$sqliteDocDir/zeroconf.html"
    DeleteIfExists "$sqliteDocDir/spellfix1.html"
    DeleteIfExists "$sqliteDocDir/releaselog"
    DeleteIfExists "$sqliteDocDir/c3ref"
    DeleteIfExists "$sqliteDocDir/session"
    DeleteIfExists "$sqliteDocDir/carray.html"
    DeleteIfExists "$sqliteDocDir/csv.html"
    DeleteIfExists "$sqliteDocDir/dbhash.html"

    # LF -> CRLF
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
