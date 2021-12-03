-- 2016-07-23 22:12:55.652
DECLARE @a = GETDATE();
PRINT LENGTH(@a);
PRINT SUBSTR(@a, 1, 2);  -- First two digits of the year (20)
PRINT SUBSTR(@a, 5, 1);  -- Dash after the year
--output--
23
20
-
