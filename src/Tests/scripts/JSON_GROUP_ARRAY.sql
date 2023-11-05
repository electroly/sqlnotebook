CREATE TABLE test (id INTEGER PRIMARY KEY, value TEXT);

INSERT INTO test (value) VALUES ('apple'), ('banana'), ('cherry');

SELECT json_group_array(value) AS json_array FROM test;
--output--
json_array
["apple","banana","cherry"]
-
