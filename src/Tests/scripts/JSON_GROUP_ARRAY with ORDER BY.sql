CREATE TABLE test (id INTEGER PRIMARY KEY, value TEXT);

INSERT INTO test (value) VALUES ('apple'), ('banana'), ('cherry');

SELECT json_group_array(value ORDER BY value DESC) AS json_array FROM test;
--output--
json_array
["cherry","banana","apple"]
-
