CREATE TABLE Towns(
Id INT PRIMARY KEY IDENTITY(1,1),
[Name] NVARCHAR(30) NOT NULL
)

CREATE TABLE Addresses(
Id INT PRIMARY KEY IDENTITY(1,1),
AddressText NVARCHAR(50) NOT NULL,
TownId INT FOREIGN KEY REFERENCES Towns(Id)
)

CREATE TABLE Departments(
Id INT PRIMARY KEY IDENTITY(1,1),
[Name] NVARCHAR(30) NOT NULL
)

CREATE TABLE Employees(
Id INT PRIMARY KEY IDENTITY(1,1),
FirstName NVARCHAR(30) NOT NULL,
MiddleName NVARCHAR(30),
LastName NVARCHAR(30) NOT NULL,
JobTitle NVARCHAR(30) NOT NULL,
DepartmentId INT FOREIGN KEY REFERENCES Departments(Id),
HireDate DATE NOT NULL,
Salary DECIMAL(8,2) NOT NULL,
AdressId INT FOREIGN KEY REFERENCES Addresses(Id)
)

INSERT INTO Departments
VALUES
('Software Development'),
('Engineering'),
('Quality Assurance'),
('Sales'),
('Marketing')

INSERT INTO Towns
VALUES
('Sofia'),
('Plovdiv'),
('Varna'),
('Burgas')

INSERT INTO Addresses
VALUES
('Nice town',1),
('Not a nice town',2),
('Perhaps a nice town',3),
('I don`t know',4)


INSERT INTO Employees
VALUES ('Ivan', 'Ivanov', 'Ivanov', '.NET Developer', 4, '02-01-2013', 3500.00, 1),
		('Petar', 'Petrov', 'Petrov',	'Senior Engineer',	1,	'03-02-2004', 4000.00, 3),
		('Maria', 'Petrova', 'Ivanova',	'Intern',	5,	'08-28-2016',	525.25, 4),
		('Georgi', 'Teziev', 'Ivanov',	'CEO', 2,	'12-09-2007',	3000.00, 2),
		('Peter', 'Pan', 'Pan',	'Intern', 3,	'08-28-2016',	599.88, 3)


SELECT * FROM Towns
ORDER BY [Name] ASC
SELECT * FROM Departments
ORDER BY [Name] ASC
SELECT * FROM Employees
ORDER BY Salary DESC

SELECT ([Name]) FROM Towns
ORDER BY Name
SELECT ([Name]) FROM Departments
ORDER BY Name
SELECT(FirstName),(LastName),(JobTitle),(Salary) FROM Employees
ORDER BY Salary DESC 


UPDATE Employees
SET Salary = Salary * 1.1
SELECT(Salary) FROM Employees

USE Hotel

UPDATE Payments
SET TaxRate *= 0.97
SELECT TaxRate FROM Payments


SELECT * FROM Occupancies
TRUNCATE TABLE Occupancies












