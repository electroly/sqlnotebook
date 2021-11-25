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
                foreach (var sqliteFilePath in sqliteFilePaths.Where(x => !x.Contains("syntax/") && !x.Contains("syntax\\"))) {
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
    Push-Location $webDir

    New-Item site -Type Directory -ErrorAction SilentlyContinue
    New-Item site/art -Type Directory -ErrorAction SilentlyContinue
    New-Item temp -Type Directory -ErrorAction SilentlyContinue

    copy .\appversion.txt .\site\
    copy .\sqlnotebook.css .\site\
    copy .\art\*.* .\site\art\
    copy .\error.html .\site\
    copy .\favicon.ico .\site\
    copy .\robots.txt .\site\
    copy .\sitemap.txt .\site\
    copy .\github-btn.html .\site\

    copy -Force "$docDir\art\*.svg" .\site\art\
    foreach ($filePath in [System.IO.Directory]::GetFiles("$webDir\site\art", "*.svg")) {
        $svg = [System.IO.File]::ReadAllText($filePath);
        $tagIndex = $svg.IndexOf('</svg>', 1)
        $svg = $svg.Substring(0, $tagIndex) + '<style>text { font-family: "Segoe UI", sans-serif; }</style>' + "`r`n" + $svg.Substring($tagIndex)
        [System.IO.File]::WriteAllText($filePath, $svg)
    }

    WriteFile .\site\index.html (FormatHtmlPage "" .\index.html "Open source tool for casual data exploration in SQL.")

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
    $relativeFilePaths = Dir -File -Recurse $resDir | % { $_.FullName.Substring($resDir.Length + 1) }

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

Update-DocWebsite
Update-Csproj
