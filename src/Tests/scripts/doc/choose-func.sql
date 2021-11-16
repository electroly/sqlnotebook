PRINT CHOOSE(1, 'A', 'B');  -- "A"
DECLARE @x = CHOOSE(5, 111, 222);
IF @x IS NULL
    PRINT 'Out of range!'  -- Prints.
--output--
A
Out of range!
