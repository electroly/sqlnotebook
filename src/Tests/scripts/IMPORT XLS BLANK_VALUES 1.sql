-- Blank values: Import as an empty string.
-- If conversion fails, skip row
IMPORT XLS '<FILES>\excel-blank-values.xlsx' WORKSHEET 1 INTO table1 (a INTEGER, b INTEGER, c INTEGER)
OPTIONS (BLANK_VALUES: 1, IF_CONVERSION_FAILS: 2);

SELECT * FROM table1;

-- Blank values: Import as an empty string.
-- If conversion fails, import as text
IMPORT XLS '<FILES>\excel-blank-values.xlsx' WORKSHEET 1 INTO table2 (a INTEGER, b INTEGER, c INTEGER)
OPTIONS (BLANK_VALUES: 1, IF_CONVERSION_FAILS: 1);

SELECT *, TYPEOF(b) AS type FROM table2;

--output--
a,b,c
444,555,666
-
a,b,c,type
111,,333,text
444,555,666,integer
-
