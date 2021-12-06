IMPORT CSV '<FILES>\missing-header.csv' INTO foo (A, B, C);

SELECT * FROM foo;

--output--
A,B,C
111,333,555
666,888,0
-
