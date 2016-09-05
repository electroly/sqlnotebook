$ErrorActionPreference = "Stop"

function ReadFile($filePath) {
    $utf8 = New-Object System.Text.UTF8Encoding($False)
    return [System.IO.File]::ReadAllText($filePath, $utf8)
}

function WriteFile($filePath, $text) {
    $utf8 = New-Object System.Text.UTF8Encoding($False)
    [System.IO.File]::WriteAllText($filePath, $text, $utf8)
}

function FormatPage($title, $content, $metaDesc) {
    $tmpl = (ReadFile .\page.template.html)
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
    $html = (ReadFile $htmlPath)
    $start = $html.IndexOf("<body>") + 6
    $end = $html.IndexOf("</body>")
    $body = $html.Substring($start, $end - $start)
    return (FormatPage $title $body $metaDesc)
}

function WriteDocFiles() {
    $docFilenames = ls -file ..\doc | select -expand Name
    $docPath = (Resolve-Path ..\doc)
    $sitePath = (Resolve-Path .\site)
    foreach ($docFilename in $docFilenames) {
        $docFilePath = [System.IO.Path]::Combine($docPath, $docFilename)
        if ($docFilePath.EndsWith("books.txt")) {
            continue
        }
        $html = (ReadFile $docFilePath)

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
        WriteFile $siteFilePath (FormatPage $title $html "")
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

    $docPath = (Resolve-Path ..\doc)
    $webPath = (Resolve-Path ..\web)
    $obj = New-Object DocHtmlGenerator
    $obj.WriteDocHtml($docPath, $webPath)
}

New-Item site -Type Directory -ErrorAction SilentlyContinue
New-Item site/art -Type Directory -ErrorAction SilentlyContinue
New-Item temp -Type Directory -ErrorAction SilentlyContinue

copy .\appversion.txt .\site\
copy .\sqlnotebook.css .\site\
copy .\art\*.* .\site\art\
copy ..\doc\art\*.png .\site\art\
copy .\error.html .\site\
copy .\favicon.ico .\site\
copy .\robots.txt .\site\
copy .\sitemap.txt .\site\

WriteFile .\site\index.html (FormatHtmlPage "" .\index.html "Open source tool for tabular data exploration and manipulation.")
WriteFile .\site\license.html (FormatHtmlPage "License" ..\src\SqlNotebook\Resources\ThirdPartyLicenses.html "SQL Notebook is freely available under the MIT license.")
WriteFile .\site\download.html (FormatHtmlPage "Download & Install" .\download.html "Download and install SQL Notebook on your Windows-based computer.")

# doc index page
GenerateTempDocHtml
WriteFile .\site\doc.html (FormatHtmlPage "Documentation" .\temp\doc.html "Index of SQL Notebook user documentation.")

# individual doc article pages
WriteDocFiles
