# SQL Notebook

SQL Notebook is a Windows desktop application for ad hoc data wrangling.  At its core, it is a frontend for SQLite database files.  The SQLite engine is augmented with additional functionality beyond its basic support for SQL queries:

- External data sources can be linked in and queried interchangeably with local tables.  Remote data is not physically copied into the notebook file unless requested; instead, the data source is queried on-the-fly.  Any combination of data sources can be used together in the same SQL query.  Supported data sources: Microsoft SQL Server, PostgreSQL, MySQL.

- Common file types can be imported into the notebook as local tables.  Supported file types: CSV, Excel (`.xls` and `.xlsx`).

- The language is extended to support familiar structured programming constructs:
    - Variables (`DECLARE`, `SET`)
    - Control flow (`IF`/`ELSE`, `WHILE`)
    - Error handling (`THROW`, `BEGIN TRY`/`BEGIN CATCH`)
    - Stored procedures (`EXECUTE`)

SQL Notebook allows the user to create three types of documents stored inside the notebook file:

- **Consoles**: A command prompt at which the user can enter commands interactively and see results inline.  The history of each console is retained in the notebook file for the user's future reference.

- **Scripts**: A syntax-colored text editor and a result pane, reminiscent of SQL Server Management Studio.  A script can be run directly (by pressing F5), or it can be executed from other scripts/consoles using the `EXECUTE` statement.  The script may define input parameters using the `CREATE PARAMETER` statement.

- **Notes**: User-readable documentation in RTF format.  The editor supports basic text formatting.

When the user is satisfied with the results, SQL Notebook can export the script output to a CSV file.
