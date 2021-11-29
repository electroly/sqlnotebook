CREATE TABLE foo (a, b, c);
INSERT INTO foo VALUES (1, 2, 3) RETURNING *;
--output--
a,b,c
1,2,3
-
