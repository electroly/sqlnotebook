# Helper that builds everything doc-related from source.

Set-StrictMode -Version 3
$ErrorActionPreference = "Stop"

$ps1Dir = $PSScriptRoot
$rootDir = (Resolve-Path (Join-Path $ps1Dir "..\")).Path
$docDir = (Resolve-Path "$rootDir\doc").Path
$sqliteDir = (Resolve-Path "$rootDir\ext\sqlite\sqlite-doc").Path
$webDir = (Resolve-Path "$rootDir\web").Path
[System.IO.Directory]::CreateDirectory("$webDir\site")
[System.IO.Directory]::CreateDirectory("$webDir\site\sqlite")
$resDir = (Resolve-Path "$rootDir\src\SqlNotebook\doc").Path

function DeleteIfExists($path) {
    if (Test-Path $path) {
        Remove-Item $path -Recurse
        Write-Host ("Deleted " + $path)
    } else {
        Write-Host ("Does not exist: " + $path)
    }
}

function Update-SqliteDocFiles {
    # Remove stuff that doesn't apply to SQL Notebook
    DeleteIfExists "$sqliteDir/doc_backlink_crossref.html"
    DeleteIfExists "$sqliteDir/doc_keyword_crossref.html"
    DeleteIfExists "$sqliteDir/doc_pagelink_crossref.html"
    DeleteIfExists "$sqliteDir/doc_target_crossref.html"
    DeleteIfExists "$sqliteDir/favicon.ico"
    DeleteIfExists "$sqliteDir/copyright-release.pdf"
    DeleteIfExists "$sqliteDir/cvstrac.css"
    DeleteIfExists "$sqliteDir/robots.txt"
    DeleteIfExists "$sqliteDir/toc.db"
    DeleteIfExists "$sqliteDir/vdbe.html"
    DeleteIfExists "$sqliteDir/sqlanalyze.html"
    DeleteIfExists "$sqliteDir/requirements.html"
    DeleteIfExists "$sqliteDir/oldnews.html"
    DeleteIfExists "$sqliteDir/pressrelease-20071212.html"
    DeleteIfExists "$sqliteDir/c_interface.html"
    DeleteIfExists "$sqliteDir/capi3.html"
    DeleteIfExists "$sqliteDir/capi3ref.html"
    DeleteIfExists "$sqliteDir/cli.html"
    DeleteIfExists "$sqliteDir/compile.html"
    DeleteIfExists "$sqliteDir/34to35.html"
    DeleteIfExists "$sqliteDir/35to36.html"
    DeleteIfExists "$sqliteDir/affcase1.html"
    DeleteIfExists "$sqliteDir/amalgamation.html"
    DeleteIfExists "$sqliteDir/backup.html"
    DeleteIfExists "$sqliteDir/compile.html"
    DeleteIfExists "$sqliteDir/changes.html"
    DeleteIfExists "$sqliteDir/chronology.html"
    DeleteIfExists "$sqliteDir/cintro.html"
    DeleteIfExists "$sqliteDir/consortium.html"
    DeleteIfExists "$sqliteDir/consortium_agreement-20071201.html"
    DeleteIfExists "$sqliteDir/copyright-release.html"
    DeleteIfExists "$sqliteDir/crew.html"
    DeleteIfExists "$sqliteDir/custombuild.html"
    DeleteIfExists "$sqliteDir/datatypes.html"
    DeleteIfExists "$sqliteDir/dev.html"
    DeleteIfExists "$sqliteDir/doclist.html"
    DeleteIfExists "$sqliteDir/docs.html"
    DeleteIfExists "$sqliteDir/download.html"
    DeleteIfExists "$sqliteDir/fileio.html"
    DeleteIfExists "$sqliteDir/footprint.html"
    DeleteIfExists "$sqliteDir/howtocompile.html"
    DeleteIfExists "$sqliteDir/hp1.html"
    DeleteIfExists "$sqliteDir/shortnames.html"
    DeleteIfExists "$sqliteDir/sitemap.html"
    DeleteIfExists "$sqliteDir/sqldiff.html"
    DeleteIfExists "$sqliteDir/support.html"
    DeleteIfExists "$sqliteDir/btreemodule.html"
    DeleteIfExists "$sqliteDir/keyword_index.html"
    DeleteIfExists "$sqliteDir/news.html"
    DeleteIfExists "$sqliteDir/opcode.html"
    DeleteIfExists "$sqliteDir/famous.html"
    DeleteIfExists "$sqliteDir/books.html"
    DeleteIfExists "$sqliteDir/fts3.html"

    DeleteIfExists "$sqliteDir/unlock_notify.html"
    DeleteIfExists "$sqliteDir/index.html"
    DeleteIfExists "$sqliteDir/sqlite.html"
    DeleteIfExists "$sqliteDir/different.html"
    DeleteIfExists "$sqliteDir/pgszchng2016.html"
    DeleteIfExists "$sqliteDir/about.html"
    DeleteIfExists "$sqliteDir/asyncvfs.html"
    DeleteIfExists "$sqliteDir/whentouse.html"
    DeleteIfExists "$sqliteDir/arch.html"
    DeleteIfExists "$sqliteDir/undoredo.html"
    DeleteIfExists "$sqliteDir/aff_short.html"
    DeleteIfExists "$sqliteDir/malloc.html"
    DeleteIfExists "$sqliteDir/errlog.html"
    DeleteIfExists "$sqliteDir/features.html"
    DeleteIfExists "$sqliteDir/formatchng.html"
    DeleteIfExists "$sqliteDir/fileformat.html"
    DeleteIfExists "$sqliteDir/fileformat2.html"
    DeleteIfExists "$sqliteDir/testing.html"
    DeleteIfExists "$sqliteDir/inmemorydb.html"
    DeleteIfExists "$sqliteDir/mmap.html"
    DeleteIfExists "$sqliteDir/mostdeployed.html"
    DeleteIfExists "$sqliteDir/mingw.html"
    DeleteIfExists "$sqliteDir/vfs.html"
    DeleteIfExists "$sqliteDir/not-found.html"
    DeleteIfExists "$sqliteDir/privatebranch.html"
    DeleteIfExists "$sqliteDir/getthecode.html"
    DeleteIfExists "$sqliteDir/rbu.html"
    DeleteIfExists "$sqliteDir/loadext.html"
    DeleteIfExists "$sqliteDir/appfileformat.html"
    DeleteIfExists "$sqliteDir/quickstart.html"
    DeleteIfExists "$sqliteDir/selfcontained.html"
    DeleteIfExists "$sqliteDir/serverless.html"
    DeleteIfExists "$sqliteDir/rtree.html"
    DeleteIfExists "$sqliteDir/rescode.html"
    DeleteIfExists "$sqliteDir/session.html"
    DeleteIfExists "$sqliteDir/sharedcache.html"
    DeleteIfExists "$sqliteDir/th3.html"
    DeleteIfExists "$sqliteDir/version3.html"
    DeleteIfExists "$sqliteDir/onefile.html"
    DeleteIfExists "$sqliteDir/tclsqlite.html"
    DeleteIfExists "$sqliteDir/uri.html"
    DeleteIfExists "$sqliteDir/threadsafe.html"
    DeleteIfExists "$sqliteDir/versionnumbers.html"
    DeleteIfExists "$sqliteDir/vtab.html"
    DeleteIfExists "$sqliteDir/wal.html"
    DeleteIfExists "$sqliteDir/zeroconf.html"
    DeleteIfExists "$sqliteDir/spellfix1.html"
    DeleteIfExists "$sqliteDir/releaselog"
    DeleteIfExists "$sqliteDir/c3ref"
    DeleteIfExists "$sqliteDir/session"
    DeleteIfExists "$sqliteDir/carray.html"
    DeleteIfExists "$sqliteDir/csv.html"
    DeleteIfExists "$sqliteDir/dbhash.html"

    # LF -> CRLF
    $filePaths = Dir -Recurse $sqliteDir | % { $_.FullName }
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

function ReadFileAbsolute($filePath) {
    $utf8 = New-Object System.Text.UTF8Encoding($False)
    return [System.IO.File]::ReadAllText($filePath, $utf8)
}

function ReadFile($filePath) {
    $filePath = Join-Path $webDir $filePath
    return ReadFileAbsolute($filePath)
}

function WriteFileAbsolute($filePath, $text) {
    $utf8 = New-Object System.Text.UTF8Encoding($False)
    [System.IO.File]::WriteAllText($filePath, $text, $utf8)
}

function WriteFile($filePath, $text) {
    $filePath = Join-Path $webDir $filePath
    WriteFileAbsolute $filePath $text
}

function FormatPage($title, $content, $metaDesc) {
    Write-Host "FormatPage($title)"

    $tmpl = (ReadFile page.template.html)
    if ($title -eq "") {
        $title = "SQL Notebook"
    } else {
        $title = $title + " - SQL Notebook"
    }
    $result = $tmpl.Replace("<!--TITLE-->", $title).Replace("<!--CONTENT-->", $content.Trim()).Replace("<!--METADESC-->", $metaDesc)

    if ($metaDesc -eq "") {
        $result = $result -replace "<meta name=[^<]*>",""
    }
    return $result
}

function FormatHtmlPage($title, $htmlPath, $metaDesc) {
    Write-Host "FormatHtmlPage($htmlPath)"
    $html = (ReadFile $htmlPath)
    $start = $html.IndexOf("<body>") + 6
    $end = $html.IndexOf("</body>")
    $body = $html.Substring($start, $end - $start)
    return (FormatPage $title $body $metaDesc)
}

function WriteDocFiles() {
    $docFilenames = Dir -File $docDir | select -expand Name
    $sitePath = (Resolve-Path "$webDir\site")
    foreach ($docFilename in $docFilenames) {
        $docFilePath = [System.IO.Path]::Combine($docDir, $docFilename)
        if ($docFilePath.EndsWith("books.txt")) {
            continue
        }
        $html = (ReadFileAbsolute $docFilePath)

        # parse out the title
        $startIndex = $html.IndexOf("<title>") + 7
        $endIndex = $html.IndexOf("</title>")
        $title = $html.Substring($startIndex, $endIndex - $startIndex)

        # strip stuff from article body that we don't want embedded in the resulting html body
        $html = $html -replace "<!doctype[^<]*>",""
        $html = $html -replace "<!DOCTYPE[^<]*>",""
        $html = $html -replace "<meta[^<]*>",""
        $html = $html -replace "<title>[^<]*</title>",""
        $html = $html -replace "<html>",""
        $html = $html -replace "</html>",""
        $html = $html -replace "<head>",""
        $html = $html -replace "</head>",""
        $html = $html -replace "<body>",""
        $html = $html -replace "</body>",""

        $siteFilePath = [System.IO.Path]::Combine($sitePath, $docFilename)
        WriteFileAbsolute $siteFilePath (FormatPage $title $html "")
    }
}

function GenerateTempDocHtml() {
    Add-Type @"
        using System;
        using System.Collections.Generic;
        using System.IO;
        using System.Text.RegularExpressions;
        using System.Linq;
        public class DocHtmlGenerator {
            public void WriteDocHtml(string docPath, string webPath) {
                var booksTxt = File.ReadAllLines(Path.Combine(docPath, "books.txt"));
                var groupDefs = ReadGroupDefs(booksTxt);
                var docFilePaths = Directory.GetFiles(docPath);
                var docFiles = new List<DocFile>();
                foreach (var docFilePath in docFilePaths) {
                    string filename = Path.GetFileName(docFilePath);
                    if (Path.GetExtension(filename) == ".html") {
                        var html = File.ReadAllText(docFilePath);
                        var startIndex = html.IndexOf("<title>") + 7;
                        var endIndex = html.IndexOf("</title>");
                        var title = html.Substring(startIndex, endIndex - startIndex);
                        docFiles.Add(new DocFile {
                            Filename = filename,
                            Title = title,
                        });
                    }
                }

                foreach (var docFile in docFiles.OrderBy(x => x.Title)) {
                    foreach (var groupDef in groupDefs) {
                        if (groupDef.Book == "SQL Notebook Help" && groupDef.Pattern.IsMatch(docFile.Title)) {
                            groupDef.Html += string.Format("<li><a href=\"{0}\">{1}</a></li>\r\n", docFile.Filename, docFile.Title);
                            break;
                        }
                    }
                }

                var indexHtml = "";
                foreach (var def in groupDefs) {
                    if (def.Book == "SQL Notebook Help") {
                        indexHtml += "<h2>" + def.Group + "</h2>\r\n<ul class=\"doc-list\">\r\n" + def.Html + "</ul>\r\n";
                    }
                }

                indexHtml += "<h2>SQLite Documentation</h2>\r\n<ul class=\"doc-list\">\r\n";
                var sqliteFilePaths = Directory.GetFiles(Path.Combine(webPath, "site", "sqlite"), "*.html", SearchOption.AllDirectories);
                var sqliteFiles = new List<Tuple<string, string>>();
                foreach (var sqliteFilePath in sqliteFilePaths) {
                    var html = File.ReadAllText(sqliteFilePath);
                    var title = Regex.Match(html, "<title>([^<]+)</title>").Groups[1].Value;
                    sqliteFiles.Add(Tuple.Create(title, sqliteFilePath.Substring(Path.Combine(webPath, "site").Length + 1)));
                }
                foreach (var x in sqliteFiles.OrderBy(x => x.Item1)) {
                    indexHtml += string.Format("<li><a href=\"{0}\">{1}</a></li>\r\n", x.Item2, x.Item1);
                }
                indexHtml += "</ul>\r\n";

                var docHtml = File.ReadAllText(Path.Combine(webPath, "doc.html"));
                docHtml = docHtml.Replace("<!--INDEX-->", indexHtml);
                File.WriteAllText(Path.Combine(webPath, "temp", "doc.html"), docHtml);
            }

            private sealed class DocFile {
                public string Filename;
                public string Title;
            }

            private sealed class GroupDef {
                public string Book;
                public string Group;
                public Regex Pattern;
                public string Html = "";
            }

            private List<GroupDef> ReadGroupDefs(string[] lines) {
                var list = new List<GroupDef>();
                foreach (var line in lines) {
                    if (line.StartsWith("Group#")) {
                        var cells = line.Split(new[] { '#' }, 4);
                        if (cells.Length != 4) {
                            throw new Exception("Internal error. Invalid group line: " + line);
                        }
                        list.Add(new GroupDef {
                            Book = cells[1],
                            Group = cells[2],
                            Pattern = new Regex(cells[3].Trim())
                        });
                    }
                }
                return list;
            }
        }
"@

    $obj = New-Object DocHtmlGenerator
    $obj.WriteDocHtml($docDir, $webDir)
}

function Update-DocWebsite {
    Write-Host "Dir: $webDir"
    Push-Location $webDir

    New-Item site -Type Directory -ErrorAction SilentlyContinue
    New-Item site/art -Type Directory -ErrorAction SilentlyContinue
    New-Item temp -Type Directory -ErrorAction SilentlyContinue

    copy .\appversion.txt .\site\
    copy .\sqlnotebook.css .\site\
    copy .\art\*.* .\site\art\
    copy "$docDir\art\*.png" .\site\art\
    copy "$docDir\art\*.svg" .\site\art\
    copy .\error.html .\site\
    copy .\favicon.ico .\site\
    copy .\robots.txt .\site\
    copy .\sitemap.txt .\site\

    WriteFile .\site\index.html (FormatHtmlPage "" .\index.html "Open source tool for tabular data exploration and manipulation.")
    WriteFile .\site\license.html (FormatHtmlPage "License" ..\src\SqlNotebook\ThirdPartyLicenses.html "SQL Notebook is freely available under the MIT license.")

    # sqlite docs
    Remove-Item -Force -Recurse ".\site\sqlite" -ErrorAction SilentlyContinue
    Copy-Item -Recurse $sqliteDir ".\site\sqlite"
    $sqliteFilePaths = Dir -Recurse ".\site\sqlite" | % { $_.FullName }
    foreach ($sqliteFilePath in $sqliteFilePaths) {
        if ($sqliteFilePath.EndsWith(".html")) {
            $html = [System.IO.File]::ReadAllText($sqliteFilePath)
            $html = $html.Replace('</style>', 'img.logo, div.tagline, table.menubar { display: none; }</style>')
            [System.IO.File]::WriteAllText($sqliteFilePath, $html)
        }
    }

    # doc index page
    GenerateTempDocHtml
    WriteFile .\site\doc.html (FormatHtmlPage "Documentation" .\temp\doc.html "Index of SQL Notebook user documentation.")
    
    # individual doc article pages
    WriteDocFiles
    Pop-Location

    # distribute a copy of the website with the app
    Remove-Item -Force -Recurse "$rootDir\src\SqlNotebook\doc" -ErrorAction SilentlyContinue
    Copy-Item -Recurse "$webDir\site" "$rootDir\src\SqlNotebook\doc"
    Remove-Item -Force "$rootDir\src\SqlNotebook\doc\*.txt"
    Remove-Item -Force "$rootDir\src\SqlNotebook\doc\art\*.txt"
    Remove-Item -Force "$rootDir\src\SqlNotebook\doc\favicon.ico"
}

function Update-Csproj {
    $relativeFilePaths = Dir -Recurse $resDir | % { $_.FullName.Substring($resDir.Length + 1) }

    $crlf = "`r`n"
    $xml = $crlf
    foreach ($relativeFilePath in $relativeFilePaths) {
        $relativeFilePath = $relativeFilePath.Replace('/', '\')
        $xml += '    <None Update="doc\' + $relativeFilePath + '">' + $crlf
        $xml += "      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>$crlf"
        $xml += "    </None>$crlf"
    }

    $csprojFilePath = "$rootDir/src/SqlNotebook/SqlNotebook.csproj"
    $csproj = [System.IO.File]::ReadAllText($csprojFilePath)
    $startIndex = $csproj.IndexOf('<!--doc start-->')
    $endIndex = $csproj.IndexOf('<!--doc end-->')
    $newCsproj = $csproj.Substring(0, $startIndex) + '<!--doc start-->' + $xml + $csproj.Substring($endIndex)
    [System.IO.File]::WriteAllText($csprojFilePath, $newCsproj)
}

Update-SqliteDocFiles
Update-DocWebsite
Update-Csproj
