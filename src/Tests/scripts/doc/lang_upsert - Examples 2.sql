-- https://sqlite.org/lang_upsert.html
CREATE TABLE phonebook(name TEXT PRIMARY KEY, phonenumber TEXT);
INSERT INTO phonebook(name,phonenumber) VALUES('Alice','704-555-1212')
  ON CONFLICT(name) DO UPDATE SET phonenumber=excluded.phonenumber;

--output--
