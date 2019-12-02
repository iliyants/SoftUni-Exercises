CREATE DATABASE Hotel

CREATE TABLE Employees(
Id INT PRIMARY KEY IDENTITY(1,1),
FirstName NVARCHAR(30) NOT NULL,
LastName NVARCHAR(30) NOT NULL,
Title NVARCHAR(80) NOT NULL,
NOTES NVARCHAR(max)
)

CREATE TABLE Customers(
AccountNumber INT PRIMARY KEY IDENTITY(1,1),
FirstName NVARCHAR(30) NOT NULL,
LastName NVARCHAR(30) NOT NULL,
PhoneNumber DECIMAL(10,0) NOT NULL,
EmergencyName NVARCHAR(30) NOT NULL,
EmergencyNumber DECIMAL(10,0) NOT NULL,
Notes NVARCHAR(max)
)

CREATE TABLE RoomStatus(
RoomStatus BIT NOT NULL,
Notes NVARCHAR(max)
)

CREATE TABLE RoomTypes(
RoomType INT PRIMARY KEY IDENTITY(1,1),
Notes VARCHAR(max)
)

CREATE TABLE BedTypes(
BedType INT PRIMARY KEY IDENTITY(1,1),
Notes NVARCHAR(max)
)

CREATE TABLE Rooms(
RoomNumber INT PRIMARY KEY IDENTITY(1,1),
RoomType INT FOREIGN KEY REFERENCES RoomTypes(RoomType),
BedType INT FOREIGN KEy REFERENCES BedTypes(BedType),
Rate INT,
RoomStatus BIT NOT NULL,
Notes VARCHAR(max)
)

CREATE TABLE Payments(
Id INT PRIMARY KEY IDENTITY(1,1),
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
PaymentDate DATE NOT NULL,
AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber),
FirstDateOccupied DATE,
LastDateOccupied DATE,
TotalDays INT,
AmountCharged DECIMAL(15,2),
TaxRate DECIMAL(15,2),
TaxAmount DECIMAL(15,2),
PaymentTotal DECIMAL(15,2),
Notes NVARCHAR(max)
)

CREATE TABLE Occupancies(
Id INT PRIMARY KEY IDENTITY(1,1),
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
DateOccupied DATE,
AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber),
RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber),
RateApplied DECIMAL(15,2) NOT NULL,
PhoneCharge DECIMAL(15,2) NOT NULL,
Notes NVARCHAR(max)
)

INSERT INTO Employees
VALUES
('Ceko','Chonov','Nobody','I am the gypsy king!'),
('Gosho','Ribov','Fisherman','sharani palamydi'),
('Maria','Dashnata','Whore','come and ge it')

INSERT INTO Customers
VALUES
('Penka','Penkova',0898765432,'Hyliganka',911,'don`t fuck with me'),
('Penko','Penkov',0898765431,'Hyligan',9111,'don`t fuck with me either'),
('Dimo','Cireq',0898765435,'Ashlaka',7746352,'i don`t care')

INSERT INTO RoomStatus
VALUES
(1,NULL),
(0,NULL),
(1,NULL)

INSERT INTO RoomTypes
VALUES
('Two beds'),
('single bed'),
('no bed')

INSERT INTO BedTypes
VALUES
('big'),
('medium'),
('small')

INSERT INTO Rooms
VALUES
(1,1,30,0,NULL),
(2,2,40,1,NULL),
(3,3,50,0,NULL)

INSERT INTO Payments
VALUES
(1,'2018-12-12',1,'2017-10-10','2017-11-11',15,200,15,30,230,null),
(2,'2019-05-02',2,'2017-12-12','2017-12-12',12,300,45,60,530,null),
(3,'2019-06-10',2,'2017-07-07','2017-11-10',11,800,85,770,1230,null)

INSERT INTO Occupancies
VALUES
(3,'2017-06-05',2,2,15,26867846,null),
(2,'2017-07-03',1,3,45,26867846,null),
(1,'2017-11-02',3,1,55,26867846,null)


DROP TABLE Payments
SELECT * FROM Occupancies

















