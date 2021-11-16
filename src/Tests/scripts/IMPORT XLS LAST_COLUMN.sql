-- Last column but no first column
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table1 OPTIONS (LAST_COLUMN: 3);
SELECT * FROM table1;

-- Last column is beyond the end of the data
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table2 OPTIONS (LAST_COLUMN: 200);
SELECT * FROM table2;

-- 0 = all columns
IMPORT XLS '<FILES>\excel.xlsx' WORKSHEET 3 INTO table3 OPTIONS (LAST_COLUMN: 0);
SELECT * FROM table3;

--output--
first,second,third
A,1,0
B,2,1
C,3,10
D,4,11
E,5,100
-
first,second,third,fourth
A,1,0,111
B,2,1,222
C,3,10,333
D,4,11,444
E,5,100,555
-
first,second,third,fourth
A,1,0,111
B,2,1,222
C,3,10,333
D,4,11,444
E,5,100,555
-
