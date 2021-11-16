EXECUTE @x = Script2;
PRINT @x;
--script--
RETURN 'foo';
--output--
foo
