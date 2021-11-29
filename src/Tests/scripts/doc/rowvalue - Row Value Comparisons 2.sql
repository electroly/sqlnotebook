-- https://sqlite.org/rowvalue.html
CREATE TABLE t1(a,b,c);
INSERT INTO t1(a,b,c) VALUES(1,2,3);
SELECT (1,2,3)=(SELECT * FROM t1); -- 1

--output--
( 1 , 2 , 3 ) = ( SELECT * FROM t1 )
1
-
