IMPORT XLS '<FILES>\excel-duplicate-header.xlsx' INTO foo;

SELECT * FROM foo;

--output--
a,column2,b,b_2,b_3
1,2,3,4,5
-
