CREATE TABLE foo (a, b, c);
INSERT INTO foo VALUES (1, 2, 3);
ALTER TABLE foo DROP a;
SELECT * FROM foo;
--output--
b,c
2,3
-
