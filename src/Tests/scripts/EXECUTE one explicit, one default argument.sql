EXECUTE Script2 @foo = 'foo';
--script--
DECLARE PARAMETER @foo;
DECLARE PARAMETER @bar = 'bar';
PRINT @foo;
PRINT @bar;
--output--
foo
bar
