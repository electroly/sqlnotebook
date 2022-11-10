CREATE TABLE a (c1, c2);
INSERT INTO a VALUES (1, 2);

SELECT MAX(c1) AS max
FROM a
HAVING c2 > 0

--output--
max
1
-
