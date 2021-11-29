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
--   c    | a | b | no_others     | current_row | grp       | ties
--  one   | 1 | A | A.D.G         | D.G         |           | A
--  one   | 4 | D | A.D.G         | A.G         |           | D
--  one   | 7 | G | A.D.G         | A.D         |           | G
--  three | 3 | C | A.D.G.C.F     | A.D.G.F     | A.D.G     | A.D.G.C
--  three | 6 | F | A.D.G.C.F     | A.D.G.C     | A.D.G     | A.D.G.F
--  two   | 2 | B | A.D.G.C.F.B.E | A.D.G.C.F.E | A.D.G.C.F | A.D.G.C.F.B
--  two   | 5 | E | A.D.G.C.F.B.E | A.D.G.C.F.B | A.D.G.C.F | A.D.G.C.F.E
-- 
SELECT c, a, b,
  group_concat(b, '.') OVER (
    ORDER BY c GROUPS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW EXCLUDE NO OTHERS
  ) AS no_others,
  group_concat(b, '.') OVER (
    ORDER BY c GROUPS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW EXCLUDE CURRENT ROW
  ) AS current_row,
  group_concat(b, '.') OVER (
    ORDER BY c GROUPS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW EXCLUDE GROUP
  ) AS grp,
  group_concat(b, '.') OVER (
    ORDER BY c GROUPS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW EXCLUDE TIES
  ) AS ties
FROM t1 ORDER BY c, a;

--output--
c,a,b,no_others,current_row,grp,ties
one,1,A,A.D.G,D.G,null,A
one,4,D,A.D.G,A.G,null,D
one,7,G,A.D.G,A.D,null,G
three,3,C,A.D.G.C.F,A.D.G.F,A.D.G,A.D.G.C
three,6,F,A.D.G.C.F,A.D.G.C,A.D.G,A.D.G.F
two,2,B,A.D.G.C.F.B.E,A.D.G.C.F.E,A.D.G.C.F,A.D.G.C.F.B
two,5,E,A.D.G.C.F.B.E,A.D.G.C.F.B,A.D.G.C.F,A.D.G.C.F.E
-
