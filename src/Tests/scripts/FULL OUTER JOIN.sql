CREATE TABLE a (c1, c2);
INSERT INTO a VALUES (1, 2);

CREATE TABLE b (c3, c4);
INSERT INTO b VALUES (1, 3);

SELECT a.c1, a.c2, b.c3, b.c4
FROM a
FULL OUTER JOIN b ON a.c1 = b.c3;

--output--
c1,c2,c3,c4
1,2,1,3
-
