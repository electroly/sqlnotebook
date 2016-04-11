<a href="https://raw.githubusercontent.com/electroly/sqlnotebook/master/art/screenshot.png" target="_blank"><img src="https://raw.githubusercontent.com/electroly/sqlnotebook/master/art/screenshot-thumb.png" align="right"></a>
<img src="https://raw.githubusercontent.com/electroly/sqlnotebook/master/art/icon-title.png">
### ➔ [**Download & Install**](http://sqlnotebook.com/install/setup.exe)

SQL Notebook is a Windows desktop application for **ad hoc data wrangling**.  At its core, it is a frontend for SQLite database files.  The SQLite engine is augmented with additional functionality beyond its basic support for SQL queries:

- External data sources can be linked in and queried interchangeably with local tables.  Remote data is not physically copied into the notebook file unless requested; instead, the data source is queried on-the-fly.  Any combination of data sources can be used together in the same SQL query.  Support for **Microsoft SQL Server**, **PostgreSQL** and **MySQL** databases is built-in.

- **CSV** and **Excel** files can be imported into the notebook as local tables.

- The SQL language is extended to support familiar **structured programming** constructs:
    - Variables (`DECLARE`, `SET`)
    - Control flow (`IF`/`ELSE`, `WHILE`)
    - Error handling (`THROW`, `TRY`/`CATCH`)
    - Stored procedures (`EXECUTE`)

SQL Notebook allows the user to create three types of documents stored inside the notebook file:

- **Consoles**: A command prompt at which the user can enter commands interactively and see results inline.  The history of each console is retained in the notebook file for the user's future reference.

- **Scripts**: A syntax-colored text editor and a result pane, reminiscent of SQL Server Management Studio.  A script can be run directly (by pressing F5), or it can be executed from other scripts/consoles using the `EXECUTE` statement.  The script may define input parameters using the `CREATE PARAMETER` statement.

- **Notes**: User-readable documentation in RTF format.  The editor supports basic text formatting.

SQL Notebook can export tables, views, and the output of scripts to CSV files for use in other applications.  An in-app searchable help system provides quick access to all SQLite documentation as well as SQL Notebook help files.
