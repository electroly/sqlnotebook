CREATE TABLE foo (a, b, c);
UPDATE foo SET a = 0 RETURNING *;
--output--
a,b,c
-
