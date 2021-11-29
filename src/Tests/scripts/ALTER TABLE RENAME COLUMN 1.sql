CREATE TABLE foo (a, b, c);
INSERT INTO foo VALUES (1, 2, 3);
ALTER TABLE foo RENAME COLUMN a TO aaa;
SELECT * FROM foo;
--output--
aaa,b,c
1,2,3
-
