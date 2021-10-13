# Run from WSL with "tidy" installed

Set-StrictMode -Version 3
$ErrorActionPreference = "Stop"

$ps1Dir = $PSScriptRoot
$rootDir = (Resolve-Path (Join-Path $ps1Dir "..\")).Path
$docDir = (Resolve-Path "$rootDir\doc").Path

foreach ($svgFilePath in ([System.IO.Directory]::GetFiles("$docDir/art", "*.svg"))) {
    $svg = [System.IO.File]::ReadAllText($svgFilePath)
    $svg = $svg.Replace("background-color: #f5f5f5;", "")
    [System.IO.File]::WriteAllText($svgFilePath, $svg)
}

$htmlFilenames = Get-Item (Join-Path $docDir "*.html") | % { $_.Name }
foreach ($htmlFilename in $htmlFilenames) {
    echo $htmlFilename
    $htmlFilePath = Join-Path $docDir $htmlFilename
    $tempFilePath = "$htmlFilePath.tmp"
    Remove-Item -Force $tempFilePath -ErrorAction SilentlyContinue

    # Write reformatted HTML to $tempFilePath
    tidy -q -utf8 -i -w 120 -o "$tempFilePath" "$htmlFilePath"
    if (-not (Test-Path $tempFilePath)) {
        throw "Tidy failed on `"$htmlFilePath`"!"
    }

    # Remove the <meta name="generator" ...> tag that HTML Tidy inserts.
    $html = [System.IO.File]::ReadAllText($tempFilePath)
    $indexStart = $html.IndexOf('<meta name="generator"')
    $indexStart = $html.LastIndexOf("`n", $indexStart)
    $indexEnd = $html.IndexOf('>', $indexStart)
    $html = $html.Substring(0, $indexStart) + $html.Substring($indexEnd + 1)
    [System.IO.File]::WriteAllText($htmlFilePath, $html)

    # Fix line endings
    & unix2dos --quiet "$htmlFilePath"
    Remove-Item -Force $tempFilePath
}
