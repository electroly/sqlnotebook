DECLARE @a = USER_NAME();
IF LENGTH(@a) > 0
	PRINT 'ok'
--output--
ok
