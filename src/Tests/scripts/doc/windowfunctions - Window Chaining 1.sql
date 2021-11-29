-- https://sqlite.org/windowfunctions.html
CREATE TABLE t1(a INTEGER PRIMARY KEY, b, c);
INSERT INTO t1 VALUES   (1, 'A', 'one'  ),
                        (2, 'B', 'two'  ),
                        (3, 'C', 'three'),
                        (4, 'D', 'one'  ),
                        (5, 'E', 'two'  ),
                        (6, 'F', 'three'),
                        (7, 'G', 'one'  );

SELECT group_concat(b, '.') OVER (
  win ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
)
FROM t1
WINDOW win AS (PARTITION BY a ORDER BY c)

--output--
group_concat ( b , '.' ) OVER ( win ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW )
A
B
C
D
E
F
G
-
