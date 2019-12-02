CREATE DATABASE Movies

CREATE TABLE Directors(
Id INT PRIMARY KEY IDENTITY(1,1),
DirectorName VARCHAR(30) NOT NULL,
Notes VARCHAR(max)
)

CREATE TABLE Genres(
Id INT PRIMARY KEY IDENTITY(1,1),
GenreName VARCHAR(30) NOT NULL,
Notes VARCHAR(max)
)

CREATE TABLE Categories(
Id INT PRIMARY KEY IDENTITY(1,1),
CategoryName VARCHAR(30) NOT NULL,
Notes VARCHAR(max)
)

CREATE TABLE Movies(
Id INT PRIMARY KEY IDENTITY(1,1),
Title VARCHAR(50) NOT NULL,
DirectorId INT FOREIgN KEY REFERENCES Directors(Id) NOT NULL,
CopyrightYear DATETIME NOT NULL,
[Lenght] INT NOT NULL,
GenreId INT FOREIGN KEY REFERENCES Genres(Id) NOT NULL,
CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
Rating DECIMAL(3,2),
Notes VARCHAR(max)
)

INSERT INTO Directors (DirectorName, Notes)
VALUES ('Ivan', 'koi'), ('Kiro', null),('Penchho', 'ti'),('Gosho', 'toi'),('Stoqn', null)

INSERT INTO Genres (GenreName, Notes)
VALUES ('horor', 'koi'), ('comedy', null),('action', 'ti'),('thrilller', 'toi'),('drama', null)

INSERT INTO Categories (CategoryName, Notes)
VALUES ('bad', 'koi'), ('good', null),('all set', 'ti'),('excelent', 'toi'),('worst', null)

INSERT INTO Movies(Title, DirectorId,CopyRightYear,Lenght,GenreId, CategoryId,Rating, Notes)
VALUES ('Friends', 2, '2015', 60, 4, 2, 9.5, 'nqma'),
		('It', 1, '2017', 122, 2, 2, 8, 'nqma'),
		('The One', 4, '2000', 120, 4, 5, 2, 'me'),
		('Too', 3, '2032', 90, 2, 3, 1, 'nqa'),
		('GOT', 2, '2001', 100, 3, 1, 6.7, 'da')

