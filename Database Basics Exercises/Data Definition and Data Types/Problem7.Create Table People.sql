CREATE DATABASE People

CREATE TABLE People (
Id INT PRIMARY KEY IDENTITY(1,1),
[Name] VARCHAR(200) NOT NULL,
Picture VARBINARY(max)
CHECK(DATALENGTH (Picture) <= 2097152),
Height DECIMAL(3,2),
[Weight] DECIMAL(5,2),
Gender CHAR(1) NOT NULL
CHECK (Gender in ('m','f')),
Birthdate DATE NOT NULL,
Biography VARCHAR(max)
)


INSERT INTO People ([Name], Picture, Height, [Weight], Gender, Birthdate, Biography)
       VALUES ('Ivan', NULL, 1.7999999, 100.0001, 'm', '1984-02-29', 'I am Ivan'),
              ('Georgi', NULL, 1.93, 98.44, 'm', '1988-04-16', 'I am Georgi'),
              ('Maria', NULL, 1.666666, 66.6666, 'f', '1990-06-25', 'I am Maria'),
              ('Dimitar', NULL, 1.854, 78, 'm', '1985-05-05', 'I am Dimitar'),
              ('Irina', NULL, 1.76, 88.2344, 'f', '1993-07-30', 'I am Irina')


SELECT * FROM People

DROP TABLE People




