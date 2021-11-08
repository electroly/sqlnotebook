# Contributors Guide

Use a Windows machine with at least 8GB RAM.

- Install Visual Studio Community 2019.
    - Include ".NET desktop development" workload
    - Include "Desktop development with C++" workload
    - Include "C++/CLI support for v142 build tools (Latest)" component
- Install WSL with Ubuntu 20.04 with `tidy`, `unix2dos`, and `pwsh`.
    ```
    sudo apt-get update
    sudo apt-get install tidy dos2unix
    sudo apt-get install -y wget apt-transport-https software-properties-common
    wget -q https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    sudo apt-get update
    sudo apt-get install -y powershell
    ```
- Install SeaMonkey.
- Install WiX Toolset.

## How to build from source

- In PowerShell on Windows, run `ps1/Update-Deps.ps1` to download non-NuGet deps and generate the doc files.
- Open `src\SqlNotebook.sln` in Visual Studio and build.

## How to edit documentation

- Use SeaMonkey Composer to edit the files in `doc/`.
- In WSL, run `ps1/Update-DocFormatting.ps1` to reformat the HTML.
- In Windows, run `ps1/Update-Docs.ps1` to rebuild the website and integrated help.

## How to generate railroad diagram files

- https://railroad.omegatower.net/generator.html
- Copy-paste the .txt file from `doc/art` into the page.
- Click "Save" to download the .svg.
- Copy to `doc/art`.
- Use SeaMonkey Composer to delete the image in the corresponding HTML page, then re-add it. Set its `class="railroad"` attribute.
- Run `ps1/Update-Docs.ps1` to rebuild the website and integrated help.

## How to update SQLite

- Download the amalgamation and documentation zips from the SQLite website.
- Update `ps1/Update-Deps.ps1` with the URLs. Use `Get-FileHash` to produce the SHA-256 hashes.
- Run `ps1/Update-Deps.ps1`.
- Build the app and fix any errors.

## How to update SQL client packages

- Update the `PackageReference` in `SqlNotebook.csproj`.
- Restore or build to download the package.
- Open Explorer to the `src\packages\` folder and locate the DLL.
- Update the `Reference` in `SqlNotebookCore.vcxproj` with the relative path to the DLL.
