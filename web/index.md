<a href="art/screenshot.png" target="_blank"><img src="art/screenshot-thumb.png" align="right" style="margin-left: 10px; margin-bottom: 10px;"></a>
SQL Notebook is a Windows desktop application for **tabular data exploration and manipulation**.  Fundamentally, it is a frontend for the [SQLite database engine](https://www.sqlite.org/).  A notebook file is an SQLite database and the SQL language is used to interact with data.  SQLite is extended to support the following features:

- **Microsoft SQL Server**, **PostgreSQL**, and **MySQL** servers can be linked into the notebook and queried interchangeably with local tables.  Remote data is not physically copied into the notebook file unless requested; instead, the data source is queried on-the-fly.  Any combination of data sources can be used together in the same SQL query.

- **CSV**, **JSON**, and **Excel** files can be imported into the notebook.

- Familiar **structured programming** constructs are available:
    - Variables ([`DECLARE`](extended-syntax.html#declare), [`SET`](extended-syntax.html#set))
    - Control flow ([`IF`/`ELSE`](extended-syntax.html#if), [`WHILE`](extended-syntax.html#while))
    - Error handling ([`THROW`](extended-syntax.html#throw), [`TRY`/`CATCH`](extended-syntax.html#try))
    - Stored procedures ([`EXECUTE`](extended-syntax.html#execute))

SQL Notebook offers two user interfaces for entering SQL queries:

- **Console**: A command prompt at which the user can enter SQL commands interactively and see results inline.  The history of each console is retained in the notebook file for the user's future reference.

- **Script**: A syntax-colored text editor and a result pane, reminiscent of SQL Server Management Studio.  The user can run a script directly by pressing F5, or the script can be invoked from consoles and other scripts using [`EXECUTE`](extended-syntax.html#execute).  The script may define input parameters using [`DECLARE PARAMETER`](extended-syntax.html#declare).

Additionally, the user can create **Notes** to document the contents of the notebook.  These notes are saved in the notebook file and can be referred to at any time.  Fonts, colors, and paragraph alignment are supported.

Tables, views, and scripts can be exported to CSV files for use in other applications.  An in-app searchable help system provides quick access to all SQLite documentation as well as SQL Notebook help files.
<br><br>

### Installation Requirements
SQL Notebook requires a 64-bit system running Windows 7 or higher.  The installer will download .NET Framework 4.6 and the Visual C++ 2015 runtime library if they aren't already installed.  Click [Download & Install](download.html) to get started.
<br><br>

### License

SQL Notebook is available under the <a href="license.html">MIT license</a>.

Copyright © 2016 Brian Luft
