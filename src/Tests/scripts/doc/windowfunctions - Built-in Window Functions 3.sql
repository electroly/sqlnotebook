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
--   b | lead | lag  | first_value | last_value | nth_value_3
-------------------------------------------------------------
--   A | C    | NULL | A           | A          | NULL       
--   B | D    | A    | A           | B          | NULL       
--   C | E    | B    | A           | C          | C          
--   D | F    | C    | A           | D          | C          
--   E | G    | D    | A           | E          | C          
--   F | n/a  | E    | A           | F          | C          
--   G | n/a  | F    | A           | G          | C          
-- 
SELECT b                          AS b,
       lead(b, 2, 'n/a') OVER win AS lead,
       lag(b) OVER win            AS lag,
       first_value(b) OVER win    AS first_value,
       last_value(b) OVER win     AS last_value,
       nth_value(b, 3) OVER win   AS nth_value_3
FROM t1
WINDOW win AS (ORDER BY b ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW);

--output--
b,lead,lag,first_value,last_value,nth_value_3
A,C,null,A,A,null
B,D,A,A,B,null
C,E,B,A,C,C
D,F,C,A,D,C
E,G,D,A,E,C
F,n/a,E,A,F,C
G,n/a,F,A,G,C
-
