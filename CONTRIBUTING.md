# Contributors Guide

A Windows build machine is assumed.

- Install Visual Studio 2019.
    - ".NET desktop development" workload
    - "Desktop development with C++" workload
    - "C++/CLI support for v142 build tools (Latest)" component
- Install WSL with Ubuntu 20.04.
    - Install some tools: `sudo apt-get install tidy`
    - Install PowerShell:
        ```
        sudo apt-get update
        sudo apt-get install -y wget apt-transport-https software-properties-common
        wget -q https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb
        sudo dpkg -i packages-microsoft-prod.deb
        sudo apt-get update
        sudo apt-get install -y powershell
        ```
- Install SeaMonkey from https://www.seamonkey-project.org/releases/

## How to build from source

- Run `src\SqlNotebook\Resources\generate-sqlite-doc-zip.ps1` to generate `sqlite-doc.zip`.
- Open `src\SqlNotebook.sln` and build.

## How to edit HTML files

- Use SeaMonkey Composer to edit.
- In WSL, run `ps1/Update-HtmlFormatting.ps1` to fix up the HTML formatting after saving in SeaMonkey.
