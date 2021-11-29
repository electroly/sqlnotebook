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
--   a | row_number | rank | dense_rank | percent_rank | cume_dist
------------------------------------------------------------------
--   a |          1 |    1 |          1 |          0.0 |       0.5
--   a |          2 |    1 |          1 |          0.0 |       0.5
--   a |          3 |    1 |          1 |          0.0 |       0.5
--   b |          4 |    4 |          2 |          0.6 |       0.66
--   c |          5 |    5 |          3 |          0.8 |       1.0
--   c |          6 |    5 |          3 |          0.8 |       1.0
-- 
SELECT a                        AS a,
       row_number() OVER win    AS row_number,
       rank() OVER win          AS rank,
       dense_rank() OVER win    AS dense_rank,
       percent_rank() OVER win  AS percent_rank,
       cume_dist() OVER win     AS cume_dist
FROM t2
WINDOW win AS (ORDER BY a);

--output--
a,row_number,rank,dense_rank,percent_rank,cume_dist
a,1,1,1,0,0.5
a,2,1,1,0,0.5
a,3,1,1,0,0.5
b,4,4,2,0.6,0.6667
c,5,5,3,0.8,1
c,6,5,3,0.8,1
-
