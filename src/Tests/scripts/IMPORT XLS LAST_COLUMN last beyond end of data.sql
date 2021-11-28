-- Last column is beyond the end of the data
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table2 OPTIONS (LAST_COLUMN: 200);
SELECT * FROM table2;

--output--
first,second,third,fourth
A,1,0,111
B,2,1,222
C,3,10,333
D,4,11,444
E,5,100,555
-
