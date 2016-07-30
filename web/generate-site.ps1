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
    return $tmpl.Replace("<!--TITLE-->", $title).Replace("<!--CONTENT-->", $content).Replace("<!--HEADER-->", $header).Replace("<!--METADESC-->", $metaDesc)
}

function FormatMdPage($title, $mdPath, $metaDesc) {
    $html = (MdToHtml $mdPath)
    return (FormatPage $title $html $metaDesc)
}

function WriteDocFiles() {
    $docFilenames = ls ..\doc | select -expand Name
    $docPath = (Resolve-Path ..\doc)
    $sitePath = (Resolve-Path .\site)
    foreach ($docFilename in $docFilenames) {
        $docFilePath = [System.IO.Path]::Combine($docPath, $docFilename)
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
        WriteFile $siteFilePath (FormatPage $title $html "SQL Notebook online documentation.")
    }
}

New-Item site -Type Directory -ErrorAction SilentlyContinue
New-Item site/art -Type Directory -ErrorAction SilentlyContinue
New-Item temp -Type Directory -ErrorAction SilentlyContinue

copy .\sqlnotebook.css .\site\
copy .\art\*.* .\site\art\
copy .\error.html .\site\
copy .\favicon.ico .\site\
copy .\robots.txt .\site\
copy .\sitemap.txt .\site\

# markdown-based pages
WriteFile .\site\index.html (FormatMdPage "" .\index.md "Open source tool for tabular data exploration and manipulation.")
WriteFile .\site\license.html (FormatMdPage "License" ..\license.md "SQL Notebook is available under the MIT license.")
WriteFile .\site\doc.html (FormatMdPage "Documentation" .\doc.md "Index of SQL Notebook user documentation.")
WriteFile .\site\download.html (FormatMdPage "Download & Install" .\download.md "Download and install SQL Notebook on your Windows-based computer.")
WriteFile .\site\video-console.html (FormatMdPage "Example Video: Console" .\video-console.md "Example video of the SQL Notebook console.")
WriteFile .\site\video-script.html (FormatMdPage "Example Video: Script" .\video-script.md "Example video of the SQL Notebook script editor.")
WriteFile .\site\video-note.html (FormatMdPage "Example Video: Note" .\video-note.md "Example video of the SQL Notebook note/documentation tool.")
WriteFile .\site\video-help.html (FormatMdPage "Example Video: Help Viewer" .\video-help.md "Example video of the SQL Notebook integrated help viewer.")

# html-based pages
WriteDocFiles
