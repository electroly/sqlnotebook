-- 2016-07-23 22:12:55.652 -04:00
DECLARE @a = GETUTCDATE();
PRINT LENGTH(@a);
PRINT SUBSTR(@a, 1, 2);  -- First two digits of the year (20)
PRINT SUBSTR(@a, 5, 1);  -- Dash after the year
PRINT SUBSTR(@a, 25, 6);  -- UTC offset (+00:00)
--output--
30
20
-
+00:00
