DECLARE @a = ARRAY(111, 222, 333);
DECLARE @b = ARRAY_APPEND(@a, 999, 1000);
PRINT @a;  -- "[111, 222, 333]"
PRINT @b;  -- "[111, 222, 333, 999, 1000]"
--output--
[111, 222, 333]
[111, 222, 333, 999, 1000]
