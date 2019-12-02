CREATE DATABASE CarRental

CREATE TABLE Categories(
Id INT PRIMARY KEY IDENTITY(1,1),
CategoryName NVARCHAR(50) NOT NULL,
DailyRate DECIMAL(3,1) NOT NULL
CHECK (DailyRate <= 100),
WeeklyRate DECIMAL(3,1),
MonthlyRate DECIMAL(3,1),
WeekendRate DECIMAL(3,1)
)

CREATE TABLE Cars(
Id INT PRIMARY KEY IDENTITY(1,1),
PlateNumber NVARCHAR(15)NOT NULL,
Manufacturer NVARCHAR(80)NOT NULL,
Model NVARCHAR(80)NOT NULL,
CarYear DATE NOT NULL,
CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
Doors TINYINT NOT NULL
CHECK (Doors <= 8),
Picture VARBINARY(max),
Condition NVARCHAR(max),
Available BIT NOT NULL
)

CREATE TABLE Employees(
Id INT PRIMARY KEY IDENTITY(1,1),
FirstName NVARCHAR(30) NOT NULL,
LastName NVARCHAR(30) NOT NULL,
Title NVARCHAR(80) NOT NULL,
Notes NVARCHAR(max)
)

CREATE TABLE Customers(
Id INT PRIMARY KEY IDENTITY(1,1),
DriverLicenceNumber DECIMAL(10,0)NOT NULL,
FullName NVARCHAR(100)NOT NULL,
Adress NVARCHAR(100)NOT NULL,
City NVARCHAR(30)NOT NULL,
ZIPCode VARCHAR(10)NOT NULL,
Notes NVARCHAR(max)
)

CREATE TABLE RentalOrders(
Id INT PRIMARY KEY IDENTITY(1,1),
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
CustomerId INT FOREIGN KEY REFERENCES Customers(Id),
CarId INT FOREIGN KEY REFERENCES Cars(Id),
TankLevel TINYINT NOT NULL
CHECK (TankLevel > 0),
KilometrageStart INT NOT NULL
CHECK (KilometrageStart > 0),
KilometrageEnd INT NOT NULL
CHECK (KilometrageEnd > 0),
TotalKilometrage INT NOT NULL
CHECK (TotalKilometrage < 800),
StartDate DATE NOT NULL,
EndDate DATE NOT NULL,
TotalDays INT,
RateApplied INT NOT NULL
CHECK (RateApplied <= 100),
TaxRate INT NOT NULL,
OrderStatus BIT NOT NULL,
Notes NVARCHAR(max)
)

INSERT INTO Categories
VALUES
('cars',99.9,8.8,1.1,2.2),
('truck',9.9,8.8,1.1,2.2),
('jeep',9.9,8.8,1.1,2.2)

INSERT INTO Cars
VALUES
('CA-255-KM','Opel','Corsa','1995',1,5,NULL,'stable',0),
('PA-67854-PK','Bugatti','Veiron','2019',2,5,NULL,'brand new',0),
('GA-000-GA','Jigyli','Strashnakola','1956',3,5,NULL,'perfect',0)

INSERT INTO Employees
VALUES
('Stamo','Petkov','Bosa na kokosa',NULL),
('Ivailo','Kenov','Shefa na relefa',NULL),
('Svetlin','Nakov','Picha na naroda',NULL)

INSERT INTO Customers
VALUES
(12345678,'Stamo Petkov Stamov','Somewhere','Code','123-123',NULL),
(33222,'Jeko Jekov Jekov','Overthere','bum','432-53-2',NULL),
(54333,'KOlio Koliov Ficheto','Here','paw','32-22-1',NULL)

INSERT INTO RentalOrders
VALUES
(1,1,1,5,1,2,4,'2017','2018',12,23,12,1,NULL),
(2,2,2,5,1,2,4,'2017','2018',12,23,12,1,NULL),
(3,3,3,5,1,2,4,'2017','2018',12,23,12,1,NULL)





