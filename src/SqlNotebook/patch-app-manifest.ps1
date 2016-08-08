# Adding Microsoft.Windows.Common-Controls to app.manifest causes Visual Studio to fail to build, but
# just sneaking it into the manifest afterwards seems to work fine. This is needed for CefSharp to show
# tooltips.

$root = (Resolve-Path ([System.IO.Path]::GetDirectoryName($PSCommandPath) + "\..\..\"))

function PatchManifest($fp) {
    $text = Get-Content $fp
    if (-not $text.Contains("Microsoft.Windows.Common-Controls")) {
        $newText = $text.Replace("<application />", '<dependency><dependentAssembly><assemblyIdentity type="Win32" name="Microsoft.Windows.Common-Controls" version="6.0.0.0" processorArchitecture="*" publicKeyToken="6595b64144ccf1df" language="*"></assemblyIdentity></dependentAssembly></dependency><application />')

        if ($text -eq $newText) {
            throw "Failed to insert the Microsoft.Windows.Common-Controls dependency in the manifest."
        }

        Set-Content $fp $newText
    }
}

$debugFp = [System.IO.Path]::Combine($root, "Debug", "SqlNotebook.exe.manifest")
if (Test-Path $debugFp) {
    PatchManifest $debugFp
}

$releaseFp = [System.IO.Path]::Combine($root, "Release", "SqlNotebook.exe.manifest")
if (Test-Path $releaseFp) {
    PatchManifest $releaseFp
}
