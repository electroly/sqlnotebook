<a href="art/screenshot.png" target="_blank"><img src="art/screenshot-thumb.png" align="right" style="margin-left: 10px; margin-bottom: 10px;"></a>
SQL Notebook is a Windows desktop application for **tabular data exploration and manipulation**.  Fundamentally, it is a frontend for the [SQLite database engine](https://www.sqlite.org/).  A notebook file is an SQLite database and the SQL language is used to interact with data.  SQLite is extended to support the following features:

- **Microsoft SQL Server**, **PostgreSQL**, and **MySQL** servers can be linked into the notebook and queried interchangeably with local tables.  Remote data is not physically copied into the notebook file unless requested; instead, the data source is queried on-the-fly.  Any combination of data sources can be used together in the same SQL query.

- **CSV**, **JSON**, and **Excel** files can be imported into the notebook.

- Familiar **structured programming** constructs are available:
    - Variables ([`DECLARE`](extended-syntax.html#declare), [`SET`](extended-syntax.html#set))
    - Control flow ([`IF`/`ELSE`](extended-syntax.html#if), [`WHILE`](extended-syntax.html#while))
    - Error handling ([`THROW`](extended-syntax.html#throw), [`TRY`/`CATCH`](extended-syntax.html#try))
    - Stored procedures ([`EXECUTE`](extended-syntax.html#execute))

The user queries and processes the data as needed by entering SQL commands using the two available SQL interfaces:

- **Console**: A command prompt at which the user can enter SQL commands interactively and see results inline.  The history of each console is retained in the notebook file for the user's future reference.

- **Script**: A syntax-colored text editor and a result pane, reminiscent of SQL Server Management Studio.  A script can be run directly (by pressing F5), or it can be executed from other scripts/consoles using the [`EXECUTE`](extended-syntax.html#execute) statement.  The script may define input parameters using the [`DECLARE PARAMETER`](extended-syntax.html#declare) statement.

Additionally, the user can create **Notes** to document the contents of the notebook.  These notes are saved in the notebook file and can be referred to at any time.  Basic text formatting (fonts, colors, and horizontal alignment) is supported.

Tables, views, and scripts can be exported to CSV files for use in other applications.  An in-app searchable help system provides quick access to all SQLite documentation as well as SQL Notebook help files.

### Installation Requirements

SQL Notebook requires 64-bit Windows 7 or higher.  .NET Framework 4.6 and the Visual C++ 2015 runtime library are required; the installer will download and install them if necessary.  Click "Download & Install" above to get started.

### Building from Source

Visual Studio 2015 must be installed with support for both C# and C++.  PowerShell 3 or higher is also required.  When building on Windows 7, you must install the latest PowerShell and reboot before building SQL Notebook.  Once the prerequisites are installed, open `src\SqlNotebook.sln` and build the solution.

### License

SQL Notebook is available under the <a href="license.html">MIT license</a>.

Copyright © 2016 Brian Luft
