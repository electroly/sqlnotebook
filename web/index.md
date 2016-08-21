<style>
    h2 {
        padding-top: 20px;
        margin-left: -24px;
    }
    h2:before {
        content: "✽ ";
        color: #2ca089;
    }
    table {
        width: 100%;
    }
    td {
        vertical-align: top;
    }
    td.tab {
        font-weight: bold;
        width: 36%
    }
    td.tab ul {
        padding-left: 16px;
    }
    .c-keyword {
        color: #009688;
    }
    .c-number {
        color: #808080;
    }
    .c-string {
        color: #D10015;
    }
    .code-block {
        line-height: 110%;
        display: inline-block;
        vertical-align: top;
    }
    .code-block pre {
        display: inline-block;
        font-size: 13px;
        margin-right: 20px;
    }
</style>
<a href="art/screenshot.png" target="_blank"><img src="art/screenshot-thumb.png" align="right" style="margin-left: 10px; margin-bottom: 10px;"></a>
SQL Notebook is a free Windows app for **exploring and manipulating tabular data**.  It is powered by a supercharged [SQLite](https://www.sqlite.org/) engine, supporting both [standard SQL](https://en.wikipedia.org/wiki/SQL-92) queries and SQL Notebook-specific commands.  Everything you need to answer analysis questions about your data, regardless of its format or origin, is built into SQL Notebook.

## Easily import and export data
- **CSV**, **JSON**, and **Excel** files can be imported into the notebook as local SQLite tables.  A graphical import wizard and `IMPORT` script commands are both available.

- **Microsoft SQL Server**, **PostgreSQL**, and **MySQL** tables can be linked into the notebook and queried interchangeably with local tables.  Remote data is not physically copied into the notebook file unless requested; instead, the data source is queried on-the-fly.

- Tables and scripts can be exported in **CSV** format.

## Run quick queries or write sophisticated scripts
SQL Notebook offers two standard user interfaces for entering SQL queries:

- **Console**: A command prompt that is optimal for quick queries.  Enter SQL commands interactively at the "&gt;" prompt and see results inline.  The command history and output log of each console are retained in the notebook file for the user's future reference.

- **Script**: Develop more complex scripts using a syntax-colored text editor.  Run a script directly by pressing F5, or invoke it from consoles and other scripts using [`EXECUTE`](execute-stmt.html).  The script may define input parameters using [`DECLARE PARAMETER`](declare-stmt.html).

Any combination of data sources can be used together in the same SQL query, including cross-file, cross-database, and cross-server queries.

## Use familiar programming constructs
Users with prior SQL or other programming language experience will feel right at home in SQL Notebook.  All common programming constructs from other programming languages are available, in addition to standard SQLite queries and commands.

<blockquote>
<div class="code-block">
<pre><span class="c-keyword">for</span> <em>:i</em> <span class="c-number">= 1</span> <span class="c-keyword">to</span> <span class="c-number">100</span> <span class="c-keyword">begin</span>
    <span class="c-keyword">if</span> <em>:i</em> <span class="c-keyword">%</span> 3 <span class="c-number">= 0</span> <span class="c-keyword">and</span> <em>:i</em> <span class="c-keyword">%</span> <span class="c-number">5 = 0</span>
        <span class="c-keyword">print</span> <span class="c-string">'FizzBuzz'</span>
    <span class="c-keyword">else if</span> <em>:i</em> <span class="c-keyword">%</span> <span class="c-number">3 = 0</span>
        <span class="c-keyword">print</span> <span class="c-string">'Fizz'</span>
    <span class="c-keyword">else if</span> <em>:i</em> <span class="c-keyword">%</span> <span class="c-number">5 = 0</span>
        <span class="c-keyword">print</span> <span class="c-string">'Buzz'</span>
    <span class="c-keyword">else</span>
        <span class="c-keyword">print</span> <em>:i</em>
<span class="c-keyword">end</span></pre>
</div>

<div class="code-block" style="border-left: 1px solid #F0F0F0; padding-left: 20px;">
<pre><span class="c-keyword">select</span>
    <span class="c-number">(</span><span class="c-keyword">case</span>
        <span class="c-keyword">when</span> number <span class="c-keyword">%</span> <span class="c-number">3 = 0</span> <span class="c-keyword">and</span> number <span class="c-keyword">%</span> <span class="c-number">5 = 0</span>
            <span class="c-keyword">then</span> <span class="c-string">'FizzBuzz'</span>
        <span class="c-keyword">when</span> number <span class="c-keyword">%</span> <span class="c-number">3 = 0</span> <span class="c-keyword">then</span> <span class="c-string">'Fizz'</span>
        <span class="c-keyword">when</span> number <span class="c-keyword">%</span> <span class="c-number">5 = 0</span> <span class="c-keyword">then</span> <span class="c-string">'Buzz'</span>
        <span class="c-keyword">else</span> number
    <span class="c-keyword">end</span><span class="c-number">)</span> <span class="c-keyword">as</span> i
<span class="c-keyword">from</span> range<span class="c-number">(1, 100)</span></pre>
</div>
</blockquote>

Learn more in the [documentation](doc.html):

- Variables ([`DECLARE`](declare-stmt.html), [`SET`](set-stmt.html))
- Control flow ([`IF`/`ELSE`](if-stmt.html), [`FOR`](for-stmt.html), [`WHILE`](while-stmt.html))
- Error handling ([`THROW`](throw-stmt.html), [`TRY`/`CATCH`](try-catch-stmt.html))
- Stored procedures ([`EXECUTE`](execute-stmt.html))

## Access a rich library of built-in functionality
SQL Notebook is a "batteries included" analysis solution.  A wide variety of functionality is immediately available out of the box.

<blockquote>
<div class="code-block">
<pre><span class="c-keyword">select</span> filename <span class="c-keyword">from</span> list_files<span class="c-number">(</span><span class="c-string">'C:\'</span><span class="c-number">)</span> <span class="c-keyword">where</span> extension = <span class="c-string">'.csv'</span><span class="c-number">;</span>
<span class="c-keyword">select</span> <span class="c-number">*</span> <span class="c-keyword">from</span> read_csv<span class="c-number">(</span><span class="c-string">'C:\MyData.csv'</span><span class="c-number">);</span>
<span class="c-keyword">import xls</span> <span class="c-string">'C:\Workbook.xls'</span> <span class="c-keyword">worksheet</span> <span class="c-string">'Sheet2'</span> <span class="c-keyword">into</span> tbl1<span class="c-number">;</span>
<span class="c-keyword">select</span> year<span class="c-number">(</span>date_col<span class="c-number">)</span> <span class="c-keyword">as</span> y<span class="c-number">,</span> month<span class="c-number">(</span>date_col<span class="c-number">)</span> <span class="c-keyword">as</span> m<span class="c-number">,</span> day<span class="c-number">(</span>date_col<span class="c-number">)</span> <span class="c-keyword">as</span> d <span class="c-keyword">from</span> tbl1<span class="c-number">;</span>
<span class="c-keyword">print</span> <span class="c-string">'Current time: '</span> <span class="c-keyword">||</span> getdate<span class="c-number">();</span>
</pre>
</div>
</blockquote>

Learn more in the [documentation](doc.html):

- Full-featured import and export statements ([`IMPORT CSV`](import-csv-stmt.html), [`IMPORT XLS`](import-xls-stmt.html), [`EXPORT TXT`](export-txt-stmt.html))
- Quick functions for reading files ([`LIST_FILES`](list-files-func.html), [`READ_CSV`](read-csv-func.html), [`READ_FILE`](read-file-func.html), [`DOWNLOAD`](download-func.html))
- Date and time handling ([`DATEPART`](date-part-func.html), [`DATEADD`](date-add-func.html), [`DATEDIFF`](date-diff-func.html), [`GETDATE`](get-date-func.html))
- Array values ([`ARRAY`](array-func.html), [`ARRAY_COUNT`](array-count-func.html), [`ARRAY_GET`](array-get-func.html), [`ARRAY_SET`](array-set-func.html))

## Document your analysis findings in the notebook
User-written documents are stored directly in notebook files alongside your SQL code and data.  Standard word processing features are available: fonts, lists, text alignment, and tables.  Console and script output can be copied into a note for annotation.  By keeping your notes with your code, everything you need will be in one place should you need to revisit some work done in SQL Notebook.

## Extensive application help is just an "F1" away
A fully searchable in-application help system is ready to answer your questions.  Press F1 to view the index of help documents, or enter a keyword into the "Search Help" box in the upper-right corner of the SQL Notebook window.  Both SQLite and SQL Notebook documentation is included.  Every available statement and function is documented.  [The documentation is also available online](doc.html).

## It's free!
SQL Notebook is **free and open source** software available under the popular [MIT license](license.html).

[Download and install SQL Notebook now!](download.html)
