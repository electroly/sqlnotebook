DECLARE @data = ARRAY(111, 222, 333);
PRINT ARRAY_GET(@data, 0);  -- "111"
PRINT ARRAY_GET(@data, 2);  -- "333"
--output--
111
333
