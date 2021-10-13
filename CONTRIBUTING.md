# Contributors Guide

Use a Windows machine with at least 8GB RAM.

- Install Visual Studio Community 2019.
    - ".NET desktop development" workload
    - "Desktop development with C++" workload
    - "C++/CLI support for v142 build tools (Latest)" component
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

- In PowerShell (Windows or WSL), run `ps1/Update-Docs.ps1` to generate the help files.
- Open `src\SqlNotebook.sln` and build.

## How to edit documentation

- Use SeaMonkey Composer to edit the files in `doc/`.
- In WSL, run `ps1/Update-DocFormatting.ps1` to reformat the HTML.
- In Windows or WSL, run `ps1/Update-Docs.ps1` to rebuild the website and integrated help.

# How to generate railroad diagram files

- https://railroad.omegatower.net/generator.html
- Copy-paste the .txt file from `doc/art` into the page.
- Click "Save" to download the .svg.
- Copy to `doc/art`.
- Run `ps1/Update-Doc.ps1` to rebuild the website and integrated help.
