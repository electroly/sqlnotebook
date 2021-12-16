# Contributors Guide

Use a Windows machine with at least 8GB RAM and 100GB disk.
In AWS, a `c5a.xlarge` instance running Windows Server 2022 will do.

- Install [7-Zip](https://www.7-zip.org/).
- Install [Visual Studio Community 2022](https://visualstudio.microsoft.com/vs/).
    - Include ".NET desktop development" workload.
    - Include "Desktop development with C++" workload.
    - Include individual component: Windows Universal CRT SDK
    - Include individual component: Windows Universal C Runtime
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
- Install [SQL Server 2019 Express](https://www.microsoft.com/en-us/Download/details.aspx?id=101064). Set the instance name to `SQLEXPRESS`.
- Install [SQL Server 2008 R2 Express](https://www.microsoft.com/en-us/download/details.aspx?id=30438). Set the instance name to `SQL2008R2`.
- Install [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).
- Install [PostgreSQL](https://www.postgresql.org/download/windows/). Set the `postgres` password to `password`.
- Install [MySQL](https://dev.mysql.com/downloads/mysql/). Set the `root` password to `password`.
- Install [Chocolatey](https://chocolatey.org/install).
- Install [NuGet](https://www.nuget.org/downloads) and put it in your `PATH`.

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
- Update `ps1/Update-Deps.ps1` with the URLs. Right-click the two zips in Explorer and use CRC SHA > SHA-256 to produce the SHA-256 hashes.
- Run `ps1/Update-Deps.ps1`.
- Build the app and fix any errors.
- Read the release notes. The grammar may have changed, which needs to be reflected in `SqliteGrammar.cs`.

## How to release a new version

- Update Visual Studio.
    - In the Visual Studio Installer, check to see if there's a newer Windows 10 SDK under Invididual Components. If there is, then:
        - Install the SDK update.
        - SqlNotebookDb > Properties > All Configurations > General > Set "Windows SDK Version" to the new version.
        - Update `New-Release.ps1` with the new SDK version.
    - Look in `C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Redist\MSVC` and update `New-Release.ps1` with the new version if there is one.
- Check for new updates to NuGet packages.
- Bump `AssemblyFileVersion` and `AssemblyCopyright` in `src\SqlNotebook\Properties\AssemblyInfo.cs`.
- Bump `ProductVersion` in `src\SqlNotebook.wxs`.
- Add a news entry in `web\index.html`.
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
    ```
- In Dev Command Prompt from `src\SqlNotebook`:
    ```
    msbuild /t:restore /p:Configuration=Release /p:Platform=x64 /p:PublishReadyToRun=true SqlNotebook.csproj
    msbuild /t:build /p:Configuration=Release /p:Platform=x64 ..\SqlNotebookDb\SqlNotebookDb.vcxproj
    msbuild /t:publish /p:Configuration=Release /p:Platform=x64 /p:PublishProfile=FolderProfile SqlNotebook.csproj
    ```
- In PowerShell from the repo root: `ps1\New-Release.ps1`
- Test the zip and MSI in `src\SqlNotebook\bin`. Rename them to `SQLNotebook-X.X.X.*`.
- Commit changes using commit message "Version X.X.X", and push.
- Create release on GitHub, upload zip and msi.
    - Let GitHub create a new tag, name it `vX.X.X`.
    - Set release title to `vX.X.X`.
    - Copy the release verbiage from the previous release, and edit in the new release notes.
    - Edit the previous release and remove the first two lines, the download links. We don't want to confuse users who visit the releases page.
- Update `web\appversion.txt` with new version and MSI URL.
- Run `ps1\Update-GitHubPages.ps1` and force push the `sqlnotebook-gh-pages` repo.
- Update `src\chocolatey\sqlnotebook.nuspec` with copyright and version.
- Update `src\chocolatey\tools\chocolateyInstall.ps1` with MSI URL.
- Put a copy of the code signing certificate into `C:\Tools\Brian Luft.pfx`.
- Get your [Chocolatey API key](https://community.chocolatey.org/account).
- In PowerShell from `src\chocolatey`:
    ```
    choco pack
    nuget sign sqlnotebook.X.X.X.nupkg -CertificatePath "C:\Tools\Brian Luft.pfx" -Timestamper http://timestamp.digicert.com
    choco install sqlnotebook -s .
    (test that it worked)
    $api = '<chocolatey api key>'
    choco apikey -k "$api" -source https://chocolatey.org/
    choco push .\sqlnotebook.X.X.X.nupkg -s https://chocolatey.org/
    ```
- Commit changes using commit message "Update website and Chocolatey to version X.X.X", and push.
