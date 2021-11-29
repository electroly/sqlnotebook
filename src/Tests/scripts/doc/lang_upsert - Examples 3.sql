-- https://sqlite.org/lang_upsert.html
CREATE TABLE phonebook2(
  name TEXT PRIMARY KEY,
  phonenumber TEXT,
  validDate DATE
);
INSERT INTO phonebook2(name,phonenumber,validDate)
  VALUES('Alice','704-555-1212','2018-05-08')
  ON CONFLICT(name) DO UPDATE SET
    phonenumber=excluded.phonenumber,
    validDate=excluded.validDate
  WHERE excluded.validDate>phonebook2.validDate;

--output--
