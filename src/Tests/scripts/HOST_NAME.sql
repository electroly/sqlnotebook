DECLARE @a = HOST_NAME();
IF LENGTH(@a) > 0
	PRINT 'ok'
--output--
ok
