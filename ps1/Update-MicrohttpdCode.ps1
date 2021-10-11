# This script takes the source code tarball for libmicrohttpd and strips out the parts we don't need.
#
# Instructions:
# - Download the libmicrohttpd-[version].tar.gz file from http://ftp.gnu.org/gnu/libmicrohttpd/
# - Untar into ext/libmicrohttpd/mhd/
# - Run this script
# - git commit

$ErrorActionPreference = "Stop"
Set-StrictMode -Version 3

$ps1Dir = $PSScriptRoot
$rootDir = (Resolve-Path (Join-Path $ps1Dir "..\")).Path
$extDir = Join-Path $rootDir "ext"
$mhdDir = Join-Path $extDir "libmicrohttpd/mhd"

if (-not (Test-Path $mhdDir)) {
    Write-Host "Directory not found: $mhdDir"
    break
}

function DeleteIfExists($path) {
    Write-Host "Deleting: $path"
    Remove-Item $path -Recurse -ErrorAction SilentlyContinue
}

# files in the root directory
Dir $mhdDir | Where {!$_.Mode.StartsWith("d") -and $_.Name -ne "AUTHORS" -and $_.Name -ne "COPYING" -and $_.Name -ne "README" } | ForEach {DeleteIfExists($_.FullName)}

# Makefile.am, Makefile.in
Dir $mhdDir -Recurse | Where {$_.Name.StartsWith("Makefile")} | ForEach {DeleteIfExists($_.FullName)}

# hellobrowser
Dir $mhdDir -Recurse | Where {$_.Name.Contains("hellobrowser")} | ForEach {DeleteIfExists($_.FullName)}

# test_*.*
Dir "$mhdDir\src" -Recurse | Where {$_.Name.StartsWith("test_")} | ForEach {DeleteIfExists($_.FullName)}

# connection_https*.*
Dir "$mhdDir\src" -Recurse | Where {$_.Name.StartsWith("connection_https")} | ForEach {DeleteIfExists($_.FullName)}

# mhd/w32/common/* except MHD_config.h
Dir "$mhdDir\w32\common" | Where {-not $_.Name.StartsWith("MHD")} | ForEach {DeleteIfExists($_.FullName)}

DeleteIfExists "$mhdDir\m4"
DeleteIfExists "$mhdDir\doc"
DeleteIfExists "$mhdDir\contrib"
DeleteIfExists "$mhdDir\src\testcurl"
DeleteIfExists "$mhdDir\src\testzzuf"
DeleteIfExists "$mhdDir\src\examples"
DeleteIfExists "$mhdDir\w32\VS2013"
DeleteIfExists "$mhdDir\w32\VS2015"
DeleteIfExists "$mhdDir\src\microhttpd\microhttpd_dll_res.rc.in"
DeleteIfExists "$mhdDir\src\datadir"
