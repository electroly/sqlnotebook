-- https://sqlite.org/json1.html#jptr
PRINT '{"a":"xyz"}' ->> '$.a';
--output--
xyz
