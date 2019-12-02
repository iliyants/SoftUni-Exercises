--One-To-One Relationship
CREATE TABLE Passports(
PassportID INT IDENTITY(101,1) NOT NULL,
PassportNumber CHAR(10) NOT NULL

CONSTRAINT Pk_Passports PRIMARY KEY (PassportID)
)

CREATE TABLE Persons(
PersonId INT IDENTITY(1,1)NOT NULL,
FirstName NVARCHAR(30) NOT NULL,
Salary DECIMAL(8,2)NOT NULL,
PassportID INT NOT NULL
CONSTRAINT Pk_Persons_PersonId PRIMARY KEY (PersonId),
CONSTRAINT Fk_Persons_PassportID FOREIGN KEY(PassportID) REFERENCES Passports(PassportID)
)

INSERT INTO Passports
VALUES
('N34FG21B'),
('K65LO4R7'),
('ZE657QP2')

INSERT INTO Persons
VALUES
('Roberto',43300.00,102),
('Tom',56100.00,103),
('Yana',60200.00,101)

--One-To-Many Relationship
CREATE TABLE Manufacturers(
ManufacturerID INT PRIMARY KEY IDENTITY(1,1),
[Name] NVARCHAR(50)NOT NULL,
EstablishedOn DATE NOT NULL
)

CREATE TABLE Models(
ModelID INT PRIMARY KEY IDENTITY(101,1),
[Name] NVARCHAR(30)NOT NULL,
ManufacturerID INT NOT NULL,
CONSTRAINT FK_Models_ManufacturerID FOREIGN KEY(ManufacturerID) REFERENCES Manufacturers(ManufacturerID)
)

INSERT INTO Manufacturers
VALUES
('BMW','07-03-1916'),
('Tesla','01-01-2003'),
('BMW','01-05-1966')

INSERT INTO Models
VALUES
('X1',1),
('i6',1),
('Model S',2),
('Model X',2),
('Model 3',2),
('Nova',3)

SELECT * FROm Manufacturers
SELECT * FROM Models
--Many-To-Many Relationship
CREATE TABLE Students(
StudentID INT PRIMARY KEY IDENTITY(1,1),
[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE Exams(
ExamID INT PRIMARY KEY IDENTITY(101,1),
[Name] NVARCHAR(30) NOT NULL
)

INSERT INTO Students
VALUES
('Mila'),
('Toni'),
('Ron')

INSERT INTO Exams
VALUES
('SpringMVC'),
('Neo4j'),
('Oracle 11g')

CREATE TABLE StudentsExams(
StudentID INT NOT NULL,
ExamID INT NOT NULL

CONSTRAINT Pk_Composite PRIMARY KEY (StudentID,ExamID)
CONSTRAINT Fk_StudentID FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
CONSTRAINT Fk_ExamID FOREIGN KEY (ExamID) REFERENCES Exams(ExamID)
)

INSERT INTO StudentsExams
VALUES
(1,101),
(1,102),
(2,101),
(3,103),
(2,102),
(2,103)

--Self-Referencing with ALTER
CREATE TABLE Teachers(
TeacherID INT PRIMARY KEY IDENTITY(1,1),
[Name] NVARCHAR(30) NOT NULL,
ManagerID INT
)

ALTER TABLE Teachers
ADD CONSTRAINT Fk_TeacherManagerRF
FOREIGN KEY (ManagerId) 
REFERENCES Teachers(TeacherID)

--Self-Referencing without ALTER
CREATE TABLE Teachers(
TeacherID INT PRIMARY KEY IDENTITY(1,1),
[Name] NVARCHAR(30) NOT NULL,
ManagerID INT

CONSTRAINT FK_Teachers FOREIGN KEY (ManagerID) REFERENCES Teachers(TeacherID)
)
--Online Store Database
CREATE DATABASE OnlineStore

CREATE TABLE Cities(
CityID INT PRIMARY KEY NOT NULL,
[Name] NVARCHAR(50)
)

CREATE TABLE Customers(
CustomerID INT PRIMARY KEY NOT NULL,
[Name] NVARCHAR(50) NOT NULL,
Birthdat DATE,
CityID INT NOT NULL,

CONSTRAINT Fk_CityID FOREIGN KEY (CityID) REFERENCES Cities(CityID)
)

CREATE TABLE Orders(
OrderID INT PRIMARY KEY NOT NULL,
CustomerID INT NOT NULL,

CONSTRAINT Fk_CustomerID FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
)

CREATE TABLE ItemTypes(
ItemTypeID INT PRIMARY KEY NOT NULL,
[Name] NVARCHAR(50)
)

CREATE TABLE Items(
ItemID INT PRIMARY KEY NOT NULL,
[Name] NVARCHAR(50) NOT NULL,
ItemTypeID INT NOT NULL,

CONSTRAINT Fk_ItemTypeID FOREIGN KEY (ItemTypeID) REFERENCES ItemTypes(ItemTypeID)
)

CREATE TABLE OrderItems(
OrderID INT NOT NULL,
ItemID INT NOT NULL,

CONSTRAINT Pk_OrderAndItemID PRIMARY KEY (OrderID,ItemID),
CONSTRAINT Fk_OrderId FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
CONSTRAINT Fk_ItemID FOREIGN KEY (ItemID) REFERENCES Items(ItemID)
)
--University Database
CREATE DATABASE University

CREATE TABLE Majors(
MajorID INT PRIMARY KEY NOT NULL,
[Name] NVARCHAR(50) NOT NULL
)

CREATE TABLE Students(
StudentID INT PRIMARY KEY NOT NULL,
StudentNumber CHAR(10) NOT NULL,
StudentName NVARCHAR(50) NOT NULL,
MajorID INT NOT NULL,

CONSTRAINT Fk_MajorID FOREIGN KEY (MajorID) REFERENCES Majors(MajorID)
)

CREATE TABLE Payments(
PaymentID INT PRIMARY KEY NOT NULL,
PaymentDate DATE NOT NULL,
PaymentAmount DECIMAL(8,1) NOT NULL,
StudentID INT NOT NULL,

CONSTRAINT Fk_StudentID FOREIGN KEY (StudentID) REFERENCES Students(StudentID)
)

CREATE TABLE Subjects(
SubjectID INT PRIMARY KEY NOT NULL,
[SubjectName] NVARCHAR(50) NOT NULL
)

CREATE TABLE Agenda(
StudentID INT NOT NULL,
SubjectID INT NOT NULL,

CONSTRAINT Pk_StudentAndSubjectIds PRIMARY KEY (StudentID,SubjectID),
CONSTRAINT Fk_StudentIDAgenda FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
CONSTRAINT Fk_SubjectIDAgenda FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID)
)
--Peaks in Rila
SELECT Mountains.MountainRange,Peaks.PeakName,Peaks.Elevation
FROM Mountains
INNER JOIN Peaks
ON Mountains.Id = Peaks.MountainId
WHERE Mountains.MountainRange = 'Rila'
ORDER BY Elevation DESC




 













































