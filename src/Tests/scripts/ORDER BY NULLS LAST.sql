CREATE TABLE foo (a, b, c);
SELECT * FROM foo ORDER BY a NULLS LAST;
--output--
a,b,c
-
