# Contributors Guide

Use a Windows machine with at least 8GB RAM and 100GB disk.
In AWS, a `c5a.xlarge` instance running Windows Server 2022 will do.

- Install [Visual Studio Community 2022](https://visualstudio.microsoft.com/vs/).
    - Include ".NET desktop development" workload.
    - Include "Desktop development with C++" workload.
- Install [Windows Subsystem for Linux](https://docs.microsoft.com/en-us/windows/wsl/) with Ubuntu 20.04, and install `tidy`, `unix2dos`, and `pwsh` inside.
    ```
    sudo apt-get update
    sudo apt-get install -y wget apt-transport-https software-properties-common tidy dos2unix
    wget -q https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    sudo apt-get update
    sudo apt-get install -y powershell
    ```
- Install [SeaMonkey](https://www.seamonkey-project.org/releases/). Open it, Edit > Preferences > Appearance. Set "When SeaMonkey starts up, open..." to Composer.
- Install [WiX Toolset](https://wixtoolset.org/releases/).
- Install [SQL Server 2019 Express](https://www.microsoft.com/en-us/Download/details.aspx?id=101064).
- Install [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).
- Install [PostgreSQL](https://www.postgresql.org/download/windows/).
- Install [MySQL](https://dev.mysql.com/downloads/mysql/).
- In `services.msc`, set SQL Server, PostgresQL, and MySQL servivces to manual startup.

## How to build from source

- In PowerShell on Windows, run `ps1/Update-Deps.ps1` to download non-NuGet deps and generate the doc files.
- Open `src\SqlNotebook.sln` in Visual Studio and build.

## How to edit documentation

- Use SeaMonkey Composer to edit the files in `doc/`.
- In WSL, run `ps1/Update-DocFormatting.ps1` to reformat the HTML.
- In Windows, run `ps1/Update-Docs.ps1` to rebuild the website and integrated help.

## How to generate railroad diagram files

- https://pikchr.org/home/pikchrshow
- Save the Pikchr source to a .pikchr file in `doc/art`.
- Click the copy SVG button on the website, paste into a .svg file.
- Get dimensions from the first line of the SVG file.
- Use SeaMonkey Composer to delete the image in the corresponding HTML page, then re-add it.
    - Set the image dimensions.
    - Set its `class="railroad"` attribute.
- Run `ps1/Update-Docs.ps1` to rebuild the website and integrated help.

## How to update SQLite

- Download the amalgamation and documentation zips from the SQLite website.
- Update `ps1/Update-Deps.ps1` with the URLs. Use `Get-FileHash` to produce the SHA-256 hashes.
- Run `ps1/Update-Deps.ps1`.
- Build the app and fix any errors.

## How to release a new version

- Bump `AssemblyFileVersion` and `AssemblyCopyright` in `src\SqlNotebook\Properties\AssemblyInfo.cs`.
- Bump `ProductVersion` in `src\SqlNotebook.wxs`.
- Close Visual Studio.
- In PowerShell from the repo root:
    ```
    $p = "src\SqlNotebook\bin"; if (Test-Path $p) { rm -Force -Recurse $p }
    $p = "src\SqlNotebook\obj"; if (Test-Path $p) { rm -Force -Recurse $p }
    $p = "src\SqlNotebookScript\bin"; if (Test-Path $p) { rm -Force -Recurse $p }
    $p = "src\SqlNotebookScript\obj"; if (Test-Path $p) { rm -Force -Recurse $p }
    $p = "src\SqlNotebookDb\bin"; if (Test-Path $p) { rm -Force -Recurse $p }
    $p = "src\SqlNotebookDb\obj"; if (Test-Path $p) { rm -Force -Recurse $p }
    $p = "src\packages"; if (Test-Path $p) { rm -Force -Recurse $p }
    $p = "web\site"; if (Test-Path $p) { rm -Force -Recurse $p }
    ps1\Update-Deps.ps1
    ps1\Update-Docs.ps1
    ```
- In Dev Command Prompt from `src\SqlNotebook`:
    ```
    msbuild /t:restore /p:Configuration=Release /p:Platform=x64 SqlNotebook.csproj
    msbuild /t:build /p:Configuration=Release /p:Platform=x64 ..\SqlNotebookDb\SqlNotebookDb.vcxproj
    msbuild /t:publish /p:Configuration=Release /p:Platform=x64 /p:PublishProfile=FolderProfile SqlNotebook.csproj
    ```
- In PowerShell from the repo root: `ps1\New-Release.ps1`
- Test the zip and MSI in `src\SqlNotebook\bin`.
- Commit changes using commit message "Version X.X.X", and push.
- Create release on GitHub, upload zip and msi.
- Update `web\appversion.txt` with new version and MSI URL.
- Upload `web\site` to sqlnotebook.com. Make public.
- Update `src\chocolatey\sqlnotebook.nuspec` with version.
- Update `src\chocolatey\tools\chocolateyInstall.ps1` with MSI URL.
- In PowerShell:
    ```
    choco pack
    choco apikey -k <chocolatey api key> -source https://chocolatey.org/
    choco push .\sqlnotebook.X.X.X.nupkg -s https://chocolatey.org/
    ```
- Commit changes using commit message "Update website and Chocolatey to version X.X.X", and push.
