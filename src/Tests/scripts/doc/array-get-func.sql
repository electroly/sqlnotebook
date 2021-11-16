DECLARE @a = ARRAY(1, 2, 3);
PRINT ARRAY_GET(@a, 2);  -- "3"
DECLARE @b = ARRAY_GET(@a, 10);  -- @b is NULL
IF @b IS NULL PRINT 'Null!';  -- Prints.
--output--
3
Null!
