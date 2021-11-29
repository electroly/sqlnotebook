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
--   a | b | c | group_concat
-----------------------------
--   1 | A | one   | A.D.G       
--   2 | B | two   | A.D.G.C.F.B.E
--   3 | C | three | A.D.G.C.F   
--   4 | D | one   | A.D.G       
--   5 | E | two   | A.D.G.C.F.B.E
--   6 | F | three | A.D.G.C.F   
--   7 | G | one   | A.D.G       
-- 
SELECT a, b, c, 
       group_concat(b, '.') OVER (ORDER BY c) AS group_concat 
FROM t1 ORDER BY a;

--output--
a,b,c,group_concat
1,A,one,A.D.G
2,B,two,A.D.G.C.F.B.E
3,C,three,A.D.G.C.F
4,D,one,A.D.G
5,E,two,A.D.G.C.F.B.E
6,F,three,A.D.G.C.F
7,G,one,A.D.G
-
