-- Returns a 10-row table containing the numbers
-- 1 through 10 in ascending order.
SELECT number FROM INTEGER_RANGE(1, 10);

-- Returns a 10-row table containing the numbers
-- 10 through 1 in descending order.
SELECT number FROM INTEGER_RANGE(10, 10, -1);

-- Another way to generate a 10-row table containing
-- the numbers 10 through 1 in descending order.
SELECT number FROM INTEGER_RANGE(1, 10) ORDER BY number DESC;

-- Returns 5 rows: 1, 3, 5, 7, 9.
SELECT number FROM INTEGER_RANGE(1, 5, 2);

--output--
number
1
2
3
4
5
6
7
8
9
10
-
number
10
9
8
7
6
5
4
3
2
1
-
number
10
9
8
7
6
5
4
3
2
1
-
number
1
3
5
7
9
-
