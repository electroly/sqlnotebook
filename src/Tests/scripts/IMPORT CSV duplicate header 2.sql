IMPORT CSV '<FILES>\duplicate-header.csv' INTO foo (a, column2, b, b_2, b_3);

SELECT * FROM foo;

--output--
a,column2,b,b_2,b_3
1,2,3,4,5
-
