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
--   c     | a | b | group_concat
---------------------------------
--   one   | 1 | A | A           
--   two   | 2 | B | A           
--   three | 3 | C | A.C         
--   one   | 4 | D | A.C.D       
--   two   | 5 | E | A.C.D       
--   three | 6 | F | A.C.D.F     
--   one   | 7 | G | A.C.D.F.G   
-- 
SELECT c, a, b, group_concat(b, '.') FILTER (WHERE c!='two') OVER (
  ORDER BY a
) AS group_concat
FROM t1 ORDER BY a;

--output--
c,a,b,group_concat
one,1,A,A
two,2,B,A
three,3,C,A.C
one,4,D,A.C.D
two,5,E,A.C.D
three,6,F,A.C.D.F
one,7,G,A.C.D.F.G
-
