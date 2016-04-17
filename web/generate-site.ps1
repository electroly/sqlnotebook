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

function FormatPage($title, $content) {
    $header = (ReadFile .\header.template.html)
    $tmpl = (ReadFile .\generic.template.html)
    return $tmpl.Replace("<!--TITLE-->", $title).Replace("<!--CONTENT-->", $content).Replace("<!--HEADER-->", $header)
}

function FormatMdPage($title, $mdPath) {
    $html = (MdToHtml $mdPath)
    return (FormatPage $title $html)
}

New-Item site -Type Directory -ErrorAction SilentlyContinue
New-Item site/art -Type Directory -ErrorAction SilentlyContinue
New-Item temp -Type Directory -ErrorAction SilentlyContinue

copy .\sqlnotebook.css .\site\
copy .\art\*.* .\site\art\
copy .\error.html .\site\
copy .\favicon.ico .\site\

# index.html
$readme = (MdToHtml ..\readme.md)
$indexTmpl = (ReadFile .\index.template.html)
WriteFile .\site\index.html $indexTmpl.Replace("<!--CONTENT-->", $readme)

# markdown-based pages
WriteFile .\site\license.html (FormatMdPage "License" ..\license.md)
WriteFile .\site\doc.html (FormatMdPage "Documentation" .\doc.md)

# html-based pages
WriteFile .\site\error-functions.html (FormatPage "Error Functions" (ReadFile ..\doc\error-functions.html))
WriteFile .\site\extended-syntax.html (FormatPage "Structured Programming in SQL Notebook" (ReadFile ..\doc\extended-syntax.html))
