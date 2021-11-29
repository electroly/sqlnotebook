-- https://sqlite.org/rowvalue.html
CREATE TABLE t2(x,y,z);
INSERT INTO t2(x,y,z) VALUES(1,2,3),(2,3,4),(1,NULL,5);
SELECT
   (1,2,3) IN (SELECT * FROM t2),  -- 1
   (7,8,9) IN (SELECT * FROM t2),  -- 0
   (1,3,5) IN (SELECT * FROM t2);  -- NULL

--output--
( 1 , 2 , 3 ) IN ( SELECT * FROM t2 ),( 7 , 8 , 9 ) IN ( SELECT * FROM t2 ),( 1 , 3 , 5 ) IN ( SELECT * FROM t2 )
1,0,null
-
