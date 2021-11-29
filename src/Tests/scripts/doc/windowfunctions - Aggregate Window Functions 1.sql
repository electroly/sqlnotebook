-- https://sqlite.org/windowfunctions.html
CREATE TABLE t1(a INTEGER PRIMARY KEY, b, c);
INSERT INTO t1 VALUES   (1, 'A', 'one'  ),
                        (2, 'B', 'two'  ),
                        (3, 'C', 'three'),
                        (4, 'D', 'one'  ),
                        (5, 'E', 'two'  ),
                        (6, 'F', 'three'),
                        (7, 'G', 'one'  );

-- The following SELECT statement returns:
-- 
--   a | b | group_concat
-------------------------
--   1 | A | A.B         
--   2 | B | A.B.C       
--   3 | C | B.C.D       
--   4 | D | C.D.E       
--   5 | E | D.E.F       
--   6 | F | E.F.G       
--   7 | G | F.G         
-- 
SELECT a, b, group_concat(b, '.') OVER (
  ORDER BY a ROWS BETWEEN 1 PRECEDING AND 1 FOLLOWING
) AS group_concat FROM t1;

--output--
a,b,group_concat
1,A,A.B
2,B,A.B.C
3,C,B.C.D
4,D,C.D.E
5,E,D.E.F
6,F,E.F.G
7,G,F.G
-
