-- https://sqlite.org/windowfunctions.html
CREATE TABLE t2(a, b);
INSERT INTO t2 VALUES('a', 'one'), 
                     ('a', 'two'), 
                     ('a', 'three'), 
                     ('b', 'four'), 
                     ('c', 'five'), 
                     ('c', 'six');

-- The following SELECT statement returns:
-- 
--   a | b     | ntile_2 | ntile_4
----------------------------------
--   a | one   |       1 |       1
--   a | two   |       1 |       1
--   a | three |       1 |       2
--   b | four  |       2 |       2
--   c | five  |       2 |       3
--   c | six   |       2 |       4
-- 
SELECT a                        AS a,
       b                        AS b,
       ntile(2) OVER win        AS ntile_2,
       ntile(4) OVER win        AS ntile_4
FROM t2
WINDOW win AS (ORDER BY a);

--output--
a,b,ntile_2,ntile_4
a,one,1,1
a,two,1,1
a,three,1,2
b,four,2,2
c,five,2,3
c,six,2,4
-
