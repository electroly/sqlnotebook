PRINT 'a'
EXECUTE Script2;
PRINT 'c'
--script--
PRINT 'b'
RETURN;
PRINT 'z'; -- should not execute
--output--
a
b
c
