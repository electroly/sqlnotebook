# This script takes the HTML documentation bundle downloaded from sqlite.org and creates a single amalgamated file
# from all of the relevant HTML files.
#
# Instructions:
# - Download the sqlite-doc zip from the SQLite website
# - Unzip into sqlite-doc
# - Run this script
# - git commit
# - Delete sqlite-doc.zip in src/SqlNotebook/Resources

if (-not (Test-Path "sqlite-doc")) {
    Write-Host "sqlite-doc does not exist."
    break
}

function DeleteIfExists($path) {
    if (Test-Path $path) {
        Remove-Item $path -Recurse
        Write-Host ("Deleted " + $path)
    } else {
        Write-Host ("Does not exist: " + $path)
    }
}

# Remove stuff that doesn't apply to SQL Notebook
DeleteIfExists "sqlite-doc/doc_backlink_crossref.html"
DeleteIfExists "sqlite-doc/doc_keyword_crossref.html"
DeleteIfExists "sqlite-doc/doc_pagelink_crossref.html"
DeleteIfExists "sqlite-doc/doc_target_crossref.html"
DeleteIfExists "sqlite-doc/favicon.ico"
DeleteIfExists "sqlite-doc/copyright-release.pdf"
DeleteIfExists "sqlite-doc/cvstrac.css"
DeleteIfExists "sqlite-doc/robots.txt"
DeleteIfExists "sqlite-doc/toc.db"
DeleteIfExists "sqlite-doc/vdbe.html"
DeleteIfExists "sqlite-doc/sqlanalyze.html"
DeleteIfExists "sqlite-doc/requirements.html"
DeleteIfExists "sqlite-doc/oldnews.html"
DeleteIfExists "sqlite-doc/pressrelease-20071212.html"
DeleteIfExists "sqlite-doc/c_interface.html"
DeleteIfExists "sqlite-doc/capi3.html"
DeleteIfExists "sqlite-doc/capi3ref.html"
DeleteIfExists "sqlite-doc/cli.html"
DeleteIfExists "sqlite-doc/compile.html"
DeleteIfExists "sqlite-doc/34to35.html"
DeleteIfExists "sqlite-doc/35to36.html"
DeleteIfExists "sqlite-doc/affcase1.html"
DeleteIfExists "sqlite-doc/amalgamation.html"
DeleteIfExists "sqlite-doc/backup.html"
DeleteIfExists "sqlite-doc/compile.html"
DeleteIfExists "sqlite-doc/changes.html"
DeleteIfExists "sqlite-doc/chronology.html"
DeleteIfExists "sqlite-doc/cintro.html"
DeleteIfExists "sqlite-doc/consortium.html"
DeleteIfExists "sqlite-doc/consortium_agreement-20071201.html"
DeleteIfExists "sqlite-doc/copyright-release.html"
DeleteIfExists "sqlite-doc/crew.html"
DeleteIfExists "sqlite-doc/custombuild.html"
DeleteIfExists "sqlite-doc/datatypes.html"
DeleteIfExists "sqlite-doc/dev.html"
DeleteIfExists "sqlite-doc/doclist.html"
DeleteIfExists "sqlite-doc/docs.html"
DeleteIfExists "sqlite-doc/download.html"
DeleteIfExists "sqlite-doc/fileio.html"
DeleteIfExists "sqlite-doc/footprint.html"
DeleteIfExists "sqlite-doc/howtocompile.html"
DeleteIfExists "sqlite-doc/hp1.html"
DeleteIfExists "sqlite-doc/shortnames.html"
DeleteIfExists "sqlite-doc/sitemap.html"
DeleteIfExists "sqlite-doc/sqldiff.html"
DeleteIfExists "sqlite-doc/support.html"
DeleteIfExists "sqlite-doc/btreemodule.html"
DeleteIfExists "sqlite-doc/keyword_index.html"
DeleteIfExists "sqlite-doc/news.html"
DeleteIfExists "sqlite-doc/opcode.html"
DeleteIfExists "sqlite-doc/famous.html"
DeleteIfExists "sqlite-doc/books.html"
DeleteIfExists "sqlite-doc/fts3.html"

DeleteIfExists "sqlite-doc/unlock_notify.html"
DeleteIfExists "sqlite-doc/index.html"
DeleteIfExists "sqlite-doc/sqlite.html"
DeleteIfExists "sqlite-doc/different.html"
DeleteIfExists "sqlite-doc/pgszchng2016.html"
DeleteIfExists "sqlite-doc/about.html"
DeleteIfExists "sqlite-doc/asyncvfs.html"
DeleteIfExists "sqlite-doc/whentouse.html"
DeleteIfExists "sqlite-doc/arch.html"
DeleteIfExists "sqlite-doc/undoredo.html"
DeleteIfExists "sqlite-doc/aff_short.html"
DeleteIfExists "sqlite-doc/malloc.html"
DeleteIfExists "sqlite-doc/errlog.html"
DeleteIfExists "sqlite-doc/features.html"
DeleteIfExists "sqlite-doc/formatchng.html"
DeleteIfExists "sqlite-doc/fileformat.html"
DeleteIfExists "sqlite-doc/fileformat2.html"
DeleteIfExists "sqlite-doc/testing.html"
DeleteIfExists "sqlite-doc/inmemorydb.html"
DeleteIfExists "sqlite-doc/mmap.html"
DeleteIfExists "sqlite-doc/mostdeployed.html"
DeleteIfExists "sqlite-doc/mingw.html"
DeleteIfExists "sqlite-doc/vfs.html"
DeleteIfExists "sqlite-doc/not-found.html"
DeleteIfExists "sqlite-doc/privatebranch.html"
DeleteIfExists "sqlite-doc/getthecode.html"
DeleteIfExists "sqlite-doc/rbu.html"
DeleteIfExists "sqlite-doc/loadext.html"
DeleteIfExists "sqlite-doc/appfileformat.html"
DeleteIfExists "sqlite-doc/quickstart.html"
DeleteIfExists "sqlite-doc/selfcontained.html"
DeleteIfExists "sqlite-doc/serverless.html"
DeleteIfExists "sqlite-doc/rtree.html"
DeleteIfExists "sqlite-doc/rescode.html"
DeleteIfExists "sqlite-doc/session.html"
DeleteIfExists "sqlite-doc/sharedcache.html"
DeleteIfExists "sqlite-doc/th3.html"
DeleteIfExists "sqlite-doc/version3.html"
DeleteIfExists "sqlite-doc/onefile.html"
DeleteIfExists "sqlite-doc/tclsqlite.html"
DeleteIfExists "sqlite-doc/uri.html"
DeleteIfExists "sqlite-doc/threadsafe.html"
DeleteIfExists "sqlite-doc/versionnumbers.html"
DeleteIfExists "sqlite-doc/vtab.html"
DeleteIfExists "sqlite-doc/wal.html"
DeleteIfExists "sqlite-doc/zeroconf.html"
DeleteIfExists "sqlite-doc/spellfix1.html"
DeleteIfExists "sqlite-doc/releaselog"
DeleteIfExists "sqlite-doc/c3ref"
DeleteIfExists "sqlite-doc/images"
DeleteIfExists "sqlite-doc/session"

# Create sqlite-doc.txt
function FirstLineContaining([String[]]$haystack, [String]$needle) {
Write-Host "FLC: " ($haystack | Measure).Count
    foreach ($line in $haystack) {
        if ($line.Contains($needle)) {
            return $line
        }
    }
    return ""
}

function ReadDocFile($filePath) {
    $lines = [System.IO.File]::ReadAllLines($filePath)
    $data = [System.String]::Join([System.Environment]::NewLine, $lines)

    $contentTypeLine = $lines | Where-Object {$_.Contains("text/html; charset=UTF-8")} | Select-Object -First 1
    $titleLine = $lines | Where-Object {$_.Contains("<title>")} | Select-Object -First 1

    $startOfContent = $data.IndexOf('<div class=startsearch></div>')
    $nl = [System.Environment]::NewLine
    if ($startOfContent -ne -1) {
        $data = $contentTypeLine + [System.Environment]::NewLine + $titleLine + [System.Environment]::NewLine + $data.Substring($startOfContent)
    }
    $relativePath = (Resolve-Path $filePath -Relative)
    return ($relativePath + [System.Environment]::NewLine + $data)
}

$fileSeparator = [System.Environment]::NewLine + "--[file separator]--" + [System.Environment]::NewLine
$files = Get-ChildItem sqlite-doc/*.html -Recurse | Sort-Object FullName | ForEach-Object {ReadDocFile($_.FullName)}
$amalgamation = [System.String]::Join($fileSeparator, $files)
DeleteIfExists "sqlite-doc.txt"
$utf8 = New-Object System.Text.UTF8Encoding($False)
$outPath = [System.IO.Path]::Combine((Resolve-Path .).Path, "sqlite-doc.txt")
[System.IO.File]::WriteAllText($outPath, $amalgamation, $utf8)
