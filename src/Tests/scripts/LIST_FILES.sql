SELECT filename, extension
FROM LIST_FILES('<FILES>')
WHERE filename = 'excel.xls';

SELECT filename, extension
FROM LIST_FILES('<FILES>', 0)
WHERE filename = 'subdir-file.txt';

SELECT filename, extension
FROM LIST_FILES('<FILES>', 1)
WHERE filename = 'subdir-file.txt';

--output--
filename,extension
excel.xls,.xls
-
filename,extension
-
filename,extension
subdir-file.txt,.txt
-
