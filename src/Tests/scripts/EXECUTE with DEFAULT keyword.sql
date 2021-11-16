EXECUTE Script2 @foo = DEFAULT;
--script--
DECLARE PARAMETER @foo = 'foo'
PRINT @foo;
--output--
foo
