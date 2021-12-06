IMPORT CSV '<FILES>\missing-header.csv' INTO foo (C, B, A);

SELECT * FROM foo;

--output--
C,B,A
555,333,111
0,888,666
-
