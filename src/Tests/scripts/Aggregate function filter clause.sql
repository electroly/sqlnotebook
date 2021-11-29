CREATE TABLE foo (a, b, c);
INSERT INTO foo VALUES (111, 222, 333), (444, 555, 666);

SELECT COUNT(*) FILTER (WHERE a = 111) AS count
FROM foo;

--output--
count
1
-
