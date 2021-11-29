CREATE TABLE foo (a, b, c);
SELECT * FROM foo ORDER BY a NULLS FIRST;
--output--
a,b,c
-
