DECLARE @a = ARRAY(111, 222, 333);
DECLARE @b = ARRAY_SET(@a, 2, 999);
PRINT @a;  -- "[111, 222, 333]"
PRINT @b;  -- "[111, 222, 999]"
--output--
[111, 222, 333]
[111, 222, 999]
