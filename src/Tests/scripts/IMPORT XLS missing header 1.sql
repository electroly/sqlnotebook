IMPORT XLS '<FILES>\excel-missing-header.xlsx' INTO foo (A, B, C);

SELECT * FROM foo;

--output--
A,B,C
111,333,555
666,888,0
-
