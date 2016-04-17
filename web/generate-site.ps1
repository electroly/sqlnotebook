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
    return $tmpl.Replace("<!--TITLE-->", $title).Replace("<!--CONTENT-->", $content).Replace("<!--HEADER-->", $header).Replace("<!--METADESC-->", $metaDesc)
}

function FormatMdPage($title, $mdPath, $metaDesc) {
    if ($title -eq "") {
        $title = "SQL Notebook"
    } else {
        $title = "SQL Notebook - " + $title
    }
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

# markdown-based pages
WriteFile .\site\index.html (FormatMdPage "" .\index.md "Open source tool for tabular data exploration and manipulation.")
WriteFile .\site\license.html (FormatMdPage "License" ..\license.md "SQL Notebook is available under the MIT license.")
WriteFile .\site\doc.html (FormatMdPage "Documentation" .\doc.md "Index of SQL Notebook user documentation.")

# html-based pages
WriteFile .\site\error-functions.html (FormatPage "Error Functions" (ReadFile ..\doc\error-functions.html) "Documentation of SQL Notebook's error reporting SQL functions.")
WriteFile .\site\extended-syntax.html (FormatPage "Structured Programming in SQL Notebook" (ReadFile ..\doc\extended-syntax.html) "Documentation of SQL Notebook's structured programing syntax.")
