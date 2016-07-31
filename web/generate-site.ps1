$ErrorActionPreference = "Stop"

# must have pandoc installed.
function ReadFile($filePath) {
    $utf8 = New-Object System.Text.UTF8Encoding($False)
    return [System.IO.File]::ReadAllText($filePath, $utf8)
}

function WriteFile($filePath, $text) {
    $utf8 = New-Object System.Text.UTF8Encoding($False)
    [System.IO.File]::WriteAllText($filePath, $text, $utf8)
}

function MdToHtml($markdownPath) {
    Remove-Item .\temp\md.html -ErrorAction SilentlyContinue
    pandoc -o .\temp\md.html "$markdownPath" | Out-Null
    return (ReadFile .\temp\md.html)
}

function FormatPage($title, $content, $metaDesc) {
    $header = (ReadFile .\header.template.html)
    $tmpl = (ReadFile .\page.template.html)
    if ($title -eq "") {
        $title = "SQL Notebook"
    } else {
        $title = $title + " - SQL Notebook"
    }
    $result = $tmpl.Replace("<!--TITLE-->", $title).Replace("<!--CONTENT-->", $content).Replace("<!--HEADER-->", $header).Replace("<!--METADESC-->", $metaDesc)

    if ($metaDesc -eq "") {
        $result = $result -replace "<meta name=[^<]*>",""
    }
    return $result
}

function FormatMdPage($title, $mdPath, $metaDesc) {
    $html = (MdToHtml $mdPath)
    return (FormatPage $title $html $metaDesc)
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

function GenerateDocMd() {
    Add-Type @"
        using System;
        using System.Collections.Generic;
        using System.IO;
        using System.Text.RegularExpressions;
        using System.Linq;
        public class DocMdGenerator {
            public void WriteTempDocMd(string docPath, string webPath) {
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
                            groupDef.Markdown += string.Format("    - [{0}]({1}){2}", docFile.Title, docFile.Filename, Environment.NewLine);
                            break;
                        }
                    }
                }

                var indexMd = "";
                foreach (var def in groupDefs) {
                    if (def.Book == "SQL Notebook Help") {
                        indexMd += "- **" + def.Group + "**" + Environment.NewLine + def.Markdown + "<br><br>" + Environment.NewLine;
                    }
                }
                var docMd = File.ReadAllText(Path.Combine(webPath, "doc.md"));
                docMd = docMd.Replace("#INSERT-DOC-INDEX-HERE", indexMd);
                File.WriteAllText(Path.Combine(webPath, "temp", "doc.md"), docMd);
            }

            private sealed class DocFile {
                public string Filename;
                public string Title;
            }

            private sealed class GroupDef {
                public string Book;
                public string Group;
                public Regex Pattern;
                public string Markdown = "";
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
    $obj = New-Object DocMdGenerator
    $obj.WriteTempDocMd($docPath, $webPath)
}

New-Item site -Type Directory -ErrorAction SilentlyContinue
New-Item site/art -Type Directory -ErrorAction SilentlyContinue
New-Item temp -Type Directory -ErrorAction SilentlyContinue

copy .\sqlnotebook.css .\site\
copy .\art\*.* .\site\art\
copy ..\doc\art\*.png .\site\art\
copy .\error.html .\site\
copy .\favicon.ico .\site\
copy .\robots.txt .\site\
copy .\sitemap.txt .\site\

# markdown-based pages
WriteFile .\site\index.html (FormatMdPage "" .\index.md "Open source tool for tabular data exploration and manipulation.")
WriteFile .\site\license.html (FormatMdPage "License" ..\license.md "SQL Notebook is available under the MIT license.")
WriteFile .\site\download.html (FormatMdPage "Download & Install" .\download.md "Download and install SQL Notebook on your Windows-based computer.")
WriteFile .\site\video-console.html (FormatMdPage "Example Video: Console" .\video-console.md "Example video of the SQL Notebook console.")
WriteFile .\site\video-script.html (FormatMdPage "Example Video: Script" .\video-script.md "Example video of the SQL Notebook script editor.")
WriteFile .\site\video-note.html (FormatMdPage "Example Video: Note" .\video-note.md "Example video of the SQL Notebook note/documentation tool.")
WriteFile .\site\video-help.html (FormatMdPage "Example Video: Help Viewer" .\video-help.md "Example video of the SQL Notebook integrated help viewer.")

# the doc index page is a special case
GenerateDocMd
WriteFile .\site\doc.html (FormatMdPage "Documentation" .\temp\doc.md "Index of SQL Notebook user documentation.")

# html-based pages
WriteDocFiles
