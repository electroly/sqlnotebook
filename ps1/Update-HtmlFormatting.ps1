#!/usr/bin/env pwsh
# Formats the HTML files under doc/. Must have HTML Tidy installed (see CONTRIBUTING.md).
$ErrorActionPreference = "Stop"
Set-StrictMode -Version 3

$ps1Dir = $PSScriptRoot
$docDir = Resolve-Path (Join-Path $ps1Dir "..\doc")

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
    $html = [System.IO.File]::ReadAllText($tempFilePath)

    # Remove the <meta name="generator" ...> tag that HTML Tidy inserts.
    $indexStart = $html.IndexOf('<meta name="generator"')
    $indexStart = $html.LastIndexOf("`n", $indexStart)
    $indexEnd = $html.IndexOf('>', $indexStart)
    $html = $html.Substring(0, $indexStart) + $html.Substring($indexEnd + 1)

    # Write back to $htmlFilePath
    [System.IO.File]::WriteAllText($htmlFilePath, $html)
    & unix2dos --quiet "$htmlFilePath"
    Remove-Item -Force $tempFilePath
}
