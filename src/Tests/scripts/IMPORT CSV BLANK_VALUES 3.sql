-- Blank values: Import as empty string or NULL.
-- If conversion fails, skip row
IMPORT CSV '<FILES>\blank-values.csv' INTO table1 (a INTEGER, b INTEGER, c INTEGER)
OPTIONS (BLANK_VALUES: 3, IF_CONVERSION_FAILS: 2);

SELECT *, TYPEOF(b) AS type FROM table1;

-- Blank values: Import as empty string or NULL.
-- If conversion fails, skip row
IMPORT CSV '<FILES>\blank-values.csv' INTO table2 (a INTEGER, b TEXT, c INTEGER)
OPTIONS (BLANK_VALUES: 3, IF_CONVERSION_FAILS: 2);

SELECT *, TYPEOF(b) AS type FROM table2;

--output--
a,b,c,type
111,null,333,null
444,555,666,integer
-
a,b,c,type
111,,333,text
444,555,666,text
-
