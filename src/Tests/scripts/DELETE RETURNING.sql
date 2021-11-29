CREATE TABLE foo (a, b, c);
DELETE FROM foo RETURNING *;
--output--
a,b,c
-
