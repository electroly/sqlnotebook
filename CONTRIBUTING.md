# Guide for contributors

## How to build from source

- A Windows machine is required.
- Install Visual Studio 2019 with:
    - ".NET desktop development" workload
    - "Desktop development with C++" workload
    - "C++/CLI support for v142 build tools (Latest)" component
- Run `src\SqlNotebook\Resources\generate-sqlite-doc-zip.ps1` to generate `sqlite-doc.zip`.
- Open `src\SqlNotebook.sln` and build.
