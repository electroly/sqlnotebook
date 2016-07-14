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
WriteFile .\site\error-functions.html (FormatPage "Error Functions" (ReadFile ..\doc\error-functions.html) "Documentation of SQL Notebook's error reporting SQL functions.")
WriteFile .\site\extended-syntax.html (FormatPage "Structured Programming in SQL Notebook" (ReadFile ..\doc\extended-syntax.html) "Documentation of SQL Notebook's structured programing syntax.")
WriteFile .\site\import-csv-file.html (FormatPage "How to Import a CSV File" (ReadFile ..\doc\import-csv-file.html) "A tutorial for importing comma-separated value (CSV) files into SQL Notebook.")
WriteFile .\site\import-json-file.html (FormatPage "How to Import a JSON File" (ReadFile ..\doc\import-json-file.html) "A tutorial for importing JSON (JavaScript Object Notation) files into SQL Notebook.")


WriteFile .\site\import-csv-stmt.html (FormatPage "IMPORT CSV Statement" (ReadFile ..\doc\import-csv-stmt.html) "The IMPORT CSV statement allows comma-separated data to be imported into notebook tables.")
WriteFile .\site\import-txt-stmt.html (FormatPage "IMPORT TXT Statement" (ReadFile ..\doc\import-txt-stmt.html) "The IMPORT TXT statement allows plain text files to be read line-by-line into a notebook table.")
WriteFile .\site\export-txt-stmt.html (FormatPage "EXPORT TXT Statement" (ReadFile ..\doc\export-txt-stmt.html) "The EXPORT TXT statement allows a query to be written as plain text to a file on disk.")