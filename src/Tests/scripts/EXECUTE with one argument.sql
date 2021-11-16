EXECUTE Script2 @foo = 'bar';
--script--
DECLARE PARAMETER @foo;
PRINT @foo;
--output--
bar
