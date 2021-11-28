-- First column but no last column
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table2 OPTIONS (FIRST_COLUMN: 'B');
SELECT * FROM table2;

--output--
second,third,fourth
1,0,111
2,1,222
3,10,333
4,11,444
5,100,555
-
