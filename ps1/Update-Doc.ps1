# Helper that builds everything doc-related from source.

Set-StrictMode -Version 3
$ErrorActionPreference = "Stop"

$dir = $PSScriptRoot

& "$dir/Update-DocFormatting.ps1"
& "$dir/Update-DocResourceZip.ps1"
& "$dir/Update-DocWebsite.ps1"
