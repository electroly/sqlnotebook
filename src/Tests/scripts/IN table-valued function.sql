-- https://sqlite.org/releaselog/3_14.html
-- "Allow table-valued functions to appear on the right-hand side of an IN operator."
SELECT value FROM generate_series(1, 5) WHERE value NOT IN generate_series(1, 2);

--output--
value
3
4
5
-
