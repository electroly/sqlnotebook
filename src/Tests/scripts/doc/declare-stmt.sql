-- @a, @b, and @c are local variables.
DECLARE @a = 1 + 2;
PRINT @a;  -- "3"

DECLARE @b = (SELECT COUNT(*) FROM sqlite_master);
PRINT @b;  -- "0"

DECLARE @c;
IF @c IS NULL
    PRINT 'c is null';  -- Prints.

-- Creates a parameter variable called @optionalParam. Because
-- an initial value of 5 is specified, the caller is not required
-- to provide a value for this parameter, but may do so if it wants
-- to override the default value.
DECLARE PARAMETER @optionalParam = 5;
PRINT @optionalParam;

--output--
3
0
c is null
5
