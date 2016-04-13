# This script takes the source code tarball for libmicrohttpd and strips out the parts we don't need.
#
# Instructions:
# - Download the libmicrohttpd-[version].tar.gz file from http://ftp.gnu.org/gnu/libmicrohttpd/
# - Untar into mhd/
# - Run this script
# - git commit

if (-not (Test-Path "mhd")) {
    Write-Host "mhd does not exist."
    break
}

function DeleteIfExists($path) {
    Remove-Item $path -Recurse -ErrorAction SilentlyContinue
}

# files in the root directory
Dir .\mhd | Where {!$_.Mode.StartsWith("d") -and $_.Name -ne "AUTHORS" -and $_.Name -ne "COPYING" -and $_.Name -ne "README" } | ForEach {DeleteIfExists($_.FullName)}

# Makefile.am, Makefile.in
Dir .\mhd -Recurse | Where {$_.Name.StartsWith("Makefile")} | ForEach {DeleteIfExists($_.FullName)}

# hellobrowser
Dir .\mhd -Recurse | Where {$_.Name.Contains("hellobrowser")} | ForEach {DeleteIfExists($_.FullName)}

# test_*.*
Dir .\mhd\src -Recurse | Where {$_.Name.StartsWith("test_")} | ForEach {DeleteIfExists($_.FullName)}

# connection_https*.*
Dir .\mhd\src -Recurse | Where {$_.Name.StartsWith("connection_https")} | ForEach {DeleteIfExists($_.FullName)}

# mhd/w32/common/* except MHD_config.h
Dir .\mhd\w32\common | Where {-not $_.Name.StartsWith("MHD")} | ForEach {DeleteIfExists($_.FullName)}

DeleteIfExists .\mhd\m4
DeleteIfExists .\mhd\doc
DeleteIfExists .\mhd\contrib
DeleteIfExists .\mhd\src\testcurl
DeleteIfExists .\mhd\src\testzzuf
DeleteIfExists .\mhd\src\examples
DeleteIfExists .\mhd\w32\VS2013
DeleteIfExists .\mhd\w32\VS2015
DeleteIfExists .\mhd\src\microhttpd\microhttpd_dll_res.rc.in
DeleteIfExists .\mhd\src\datadir
