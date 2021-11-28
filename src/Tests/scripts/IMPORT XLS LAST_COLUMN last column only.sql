-- Last column but no first column
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table1 OPTIONS (LAST_COLUMN: 3);
SELECT * FROM table1;

--output--
first,second,third
A,1,0
B,2,1
C,3,10
D,4,11
E,5,100
-
