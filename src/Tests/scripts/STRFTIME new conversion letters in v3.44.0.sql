CREATE TABLE test (id INTEGER PRIMARY KEY, timestamp DATETIME);

INSERT INTO test (timestamp) VALUES ('2023-11-05 15:45:30');

SELECT strftime('%e', timestamp) AS day_of_month FROM test;

SELECT strftime('%F', timestamp) AS date FROM test;

SELECT strftime('%I', timestamp) AS hour_12 FROM test;

SELECT strftime('%k', timestamp) AS hour_24 FROM test;

SELECT strftime('%l', timestamp) AS hour_12_blank FROM test;

SELECT strftime('%p', timestamp) AS am_pm FROM test;

SELECT strftime('%P', timestamp) AS am_pm_lower FROM test;

SELECT strftime('%R', timestamp) AS time_24 FROM test;

SELECT strftime('%T', timestamp) AS time FROM test;

SELECT strftime('%u', timestamp) AS day_of_week FROM test;
--output--
day_of_month
 5
-
date
2023-11-05
-
hour_12
03
-
hour_24
15
-
hour_12_blank
 3
-
am_pm
PM
-
am_pm_lower
pm
-
time_24
15:45
-
time
15:45:30
-
day_of_week
7
-
