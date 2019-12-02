--Problem 1. Employees with Salary Above 35000
CREATE PROC usp_GetEmployeesSalaryAbove35000
AS
BEGIN
	SELECT 
		FirstName,
		LastName
	FROM
		Employees
	WHERE Salary > 35000
END;
--02. Employees with Salary Above Number
CREATE PROC usp_GetEmployeesSalaryAboveNumber (@aboveOrEqual DECIMAl(18,4))
AS
BEGIN
	SELECT
		FirstName,
		LastName
	FROM
		Employees
	WHERE Salary >= @aboveOrEqual
END;
--03. Town Names Starting With
CREATE PROC usp_GetTownsStartingWith (@startsWith NVARCHAR(5))
AS
BEGIN
	SELECT
		[Name]
	FROM
		Towns
	WHERE [Name] LIKE CONCAT(@startsWith,'%')
END;
--04. Employees from Town
CREATE PROC usp_GetEmployeesFromTown (@townName NVARCHAR(50))
AS
BEGIN
	SELECT
		e.FirstName,
		e.LastName
	FROM
		Employees AS e
	JOIN Addresses AS a ON e.AddressID = a.AddressID
	JOIN Towns AS t ON a.TownID = t.TownID
	WHERE t.Name = @townName
END;
--05. Salary Level Function
CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4))
RETURNS NVARCHAR(10)
AS
BEGIN
	DECLARE @result NVARCHAR(10)

	IF(@salary < 30000)
		SET @result = 'Low'
	ELSE IF(@salary < 50000)
		SET @result = 'Average'
	ELSE
		SET @result = 'High'

	RETURN @result
END; 
--06. Employees by Salary Level
CREATE PROC usp_EmployeesBySalaryLevel(@lvlOfSalary NVARCHAR(10))
AS
BEGIN
	SELECT
		FirstName,
		LastName
	FROM Employees
	WHERE dbo.ufn_GetSalaryLevel(Salary) = @lvlOfSalary		
END;
--07. Define Function
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(10), @word NVARCHAR(10))
RETURNS BIT
AS
BEGIN
DECLARE @count INT = 1
DECLARE @check BIT = 1
WHILE (@count <= LEN(@word))
BEGIN

DECLARE @currentLetter CHAR(1) = LOWER(SUBSTRING(@word,@count,1));

IF(CHARINDEX(@currentLetter,LOWER(@setOfLetters)) = 0)
SET @check = 0 
SET @count += 1
END
RETURN @check;
END
--08. Delete Employees and Departments
CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT)
AS
BEGIN
ALTER TABLE EmployeesProjects
NOCHECK CONSTRAINT ALL
ALTER TABLE Departments
NOCHECK CONSTRAINT ALL
ALTER TABLE Employees
NOCHECK CONSTRAINT ALL

DELETE FROM Employees
WHERE DepartmentID = @departmentId
DELETE FROM Departments
WHERE DepartmentID = @departmentId

SELECT COUNT(EmployeeID)
FROM Employees
WHERE DepartmentID = @departmentId


ALTER TABLE EmployeesProjects
CHECK CONSTRAINT ALL
ALTER TABLE Departments
CHECK CONSTRAINT ALL
ALTER TABLE Employees
CHECK CONSTRAINT ALL

END;
--09. Find Full Name
CREATE PROC usp_GetHoldersFullName 
AS
BEGIN
SELECT CONCAT(FirstName,' ',LastName) AS [Full Name]
FROM AccountHolders
END;
--10. People with Balance Higher Than
CREATE PROC usp_GetHoldersWithBalanceHigherThan (@parameter MONEY)
AS
BEGIN

WITH CTE_Accounts
AS
(
SELECT ah.FirstName,ah.LastName,acc.Balance,acc.AccountHolderId
FROM AccountHolders AS ah
JOIN Accounts AS acc
ON ah.Id = acc.AccountHolderId
GROUP BY acc.AccountHolderId,ah.FirstName,ah.LastName,acc.Balance
)

SELECT final.FirstName,final.LastName
FROM
(
SELECT FirstName,LastName,SUM(Balance) AS [Total]
FROM CTE_Accounts
GROUP BY FirstName,LastName
) AS final
WHERE final.Total > @parameter
ORDER BY FirstName,LastName
END;
--Problem 11. Future Value Function
CREATE FUNCTION ufn_CalculateFutureValue (@initialSum DECIMAL(15,4), @yearInterestRate FLOAT(25),@numberOfYears INT)
RETURNS DECIMAL(15,4)
AS
BEGIN

DECLARE @futureValue DECIMAL(15,4) = 0;

SET @futureValue = @initialSum * (POWER((1 + @yearInterestRate),@numberOfYears));
RETURN @futureValue;
END;
--Problem 12. Calculating Interest
CREATE PROC usp_CalculateFutureValueForAccount (@accountID INT, @interestRate FLOAT(25))
AS
BEGIN
	  SELECT 
		acch.Id,
		acch.FirstName,
		acch.LastName,
		acc.Balance AS [Current Balance],
		dbo.ufn_CalculateFutureValue(acc.Balance,@interestRate,5) AS [Balance in 5 years]
	  FROM AccountHolders AS acch
	  JOIN Accounts AS acc
	  ON acch.Id = acc.Id
	  WHERE acch.Id = @accountID
END;
--13. *Cash in User Games Odd Rows
CREATE FUNCTION ufn_CashInUsersGames (@gameName NVARCHAR(20))
RETURNS TABLE
AS
 RETURN WITH CTE_RowsTable
AS
(
SELECT g.[Name],ug.Cash,ROW_NUMBER() OVER (PARTITION BY g.[Name] ORDER BY ug.Cash DESC) AS [RowNumber]
FROM Games AS g
JOIN UsersGames AS ug
ON g.Id = ug.GameId
WHERE g.[Name] = @gameName
)
SELECT SUM(c.Cash) AS [SumCash]
FROM(
SELECT [Name],Cash
FROM CTE_RowsTable
WHERE [RowNumber] % 2 = 1
) AS c
--14. Create Table Logs
CREATE TABLE Logs
(	LogId INT PRIMARY KEY IDENTITY (1,1),
	AccountId INT NOT NULL,
	OldSum DECIMAL(15,2) NOT NULL,
	NewSum DECIMAL(15,2) NOT NULL
)

CREATE TRIGGER tr_SumChangeLog ON Accounts FOR UPDATE
AS
BEGIN
   INSERT INTO Logs (AccountId, OldSum, NewSum)
	   SELECT i.AccountHolderId,
                  d.Balance,
                  i.Balance
	     FROM inserted i
       INNER JOIN deleted d
               ON d.AccountHolderId = i.AccountHolderId
END
--15. Create Table Emails
CREATE TABLE NotificationEmals (
Id INT PRIMARY KEY IDENTITY (1,1),
Recipent INT NOT NULL,
[Subject] NVARCHAR(50) NOT NULL,
Body NVARCHAR(100) NOT NULL
)

CREATE TRIGGER tr_EmailToChange ON Logs AFTER INSERT
AS
BEGIN

DECLARE @recipient INT =  (SELECT AccountId FROM inserted);
DECLARE @subject NVARCHAR(50) = 'Balance change for account: ' + CAST(@recipient AS VARCHAR)
DECLARE @currentDate DATETIME = GETDATE();
DECLARE @oldSum DECIMAL(15,2) = (SELECT OldSum FROM inserted);
DECLARE @newSum DECIMAL(15,2) = (SELECT NewSum FROM inserted); 
DECLARE @body NVARCHAR(100) = 'On ' +  CAST(@currentDate AS VARCHAR(18)) 
+  ' your balance was changed from ' + CAST(@oldSum AS VARCHAR(18)) + ' to ' +  CAST(@newSum AS VARCHAR(18)) + '.';

INSERT INTO NotificationEmails
VALUES (@recipient,@subject,@body)

END;
--Problem 16. Deposit Money
CREATE PROC usp_DepositMoney (@AccountId INT,@MoneyAmmount DECIMAL(15,4))
AS
BEGIN
BEGIN TRANSACTION
DECLARE @idCheck INT = (SELECT Id FROM Accounts WHERE Id = @accountId)
IF(@idCheck IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('Account does not exist',16,1)
		RETURN
	END
IF(@moneyAmmount < 0)
	BEGIN
		ROLLBACK
		RAISERROR('Money should be a possitive number!',16,2)
		RETURN
	END

	UPDATE Accounts
	SET Balance += @MoneyAmmount
	WHERE Id = @accountId

	COMMIT
END;
--Problem 17. Withdraw Money
CREATE PROC usp_WithdrawMoney (@AccountId INT,@MoneyAmmount DECIMAL(15,4))
AS
BEGIN
BEGIN TRANSACTION
DECLARE @idCheck INT = (SELECT Id FROM Accounts WHERE Id = @accountId)
IF(@idCheck IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('Account does not exist',16,1)
		RETURN
	END
DECLARE @ammountCheck DECIMAL(15,4) = (SELECT Balance FROM Accounts WHERE Id = @AccountId)
IF(@moneyAmmount > @ammountCheck)
	BEGIN
		ROLLBACK
		RAISERROR('Insufficient funds!',16,2)
		RETURN
	END

	UPDATE Accounts
	SET Balance -= @MoneyAmmount
	WHERE Id = @accountId

	COMMIT
END;
--18. Money Transfer
CREATE PROC usp_TransferMoney(@SenderId INT, @ReceiverId INT, @Amount DECIMAL(15,2))
AS
BEGIN

EXEC dbo.usp_WithdrawMoney @SenderId,@Amount
EXEC dbo.usp_DepositMoney @ReceiverId, @Amount

END;
--Problem 20. *Massive Shopping


SELECT g.[Name],i.[Name] AS [Item Name],i.MinLevel,it.[Name] AS [Item Type]
FROM Games AS g
JOIN GameTypes AS gt ON g.GameTypeId = gt.Id
JOIN GameTypeForbiddenItems AS gtfi ON gtfi.GameTypeId = gt.Id
JOIN Items AS i ON i.Id = gtfi.ItemId
JOIN ItemTypes AS it ON it.Id = i.ItemTypeId
WHERE g.[Name] = 'Safflower'
ORDER BY g.[Name]




SELECT u.[FirstName],c.[Name] AS [Class],ug.[Level],g.[Name] AS [GameName],ug.Cash,i.[Name] AS [ItemName],i.MinLevel
FROM UsersGames AS ug
JOIN Characters AS c ON ug.CharacterId = c.Id
JOIN Users AS u ON u.Id = ug.UserId
JOIN Games AS g ON ug.GameId = g.Id
JOIN UserGameItems AS ugi ON ug.GameId = ugi.UserGameId
JOIN Items AS i ON i.Id = ugi.ItemId
WHERE u.FirstName = 'Stamat' AND g.[Name] = 'Safflower'
--Problem 21. Employees with Three Projects
  CREATE PROC dbo.usp_AssignProject(@emloyeeId INT, @projectID INT)
AS
BEGIN
	BEGIN TRANSACTION
	
	DECLARE @IdCheck INT = (SELECT EmployeeId FROM Employees WHERE EmployeeID = @emloyeeId)

	IF(@IdCheck IS NULL)
		BEGIN
			ROLLBACK
			RAISERROR('Employee does not exist',16,1)
			RETURN
		END;

	DECLARE @projectCheck INT = (SELECT ProjectId FROM Projects WHERE ProjectID = @projectID)

	IF(@projectCheck IS NULL)
		BEGIN
			ROLLBACK
			RAISERROR('Project does not exist',16,1)
			RETURN
		END;

	DECLARE @test INT =  (SELECT a.[Number of Projects] FROM(
		SELECT e.EmployeeID, FirstName,COUNT(p.[Name]) AS [Number of Projects]
		FROM Employees AS e
		JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
		JOIN Projects AS p ON ep.ProjectID = p.ProjectID
		GROUP BY e.FirstName,E.EmployeeID
		) AS a
		WHERE a.EmployeeID = @emloyeeId)


	IF(@test > 3 OR @test = 3)
		BEGIN
			ROLLBACK
			RAISERROR('The employee has too many projects!',16,1)
			RETURN
		END;

	INSERT INTO EmployeesProjects
	VALUES(@emloyeeId,@projectID)

COMMIT

END; 
--Problem 22. Delete Employees
CREATE TABLE Deleted_Employees (
EmployeeId INT NOT NULL,
FirstName NVARCHAR(50) NOT NULL,
LastName NVARCHAR(50) NOT NULL,
MiddleName NVARCHAR(50) NOT NULL,
JobTitle NVARCHAR(50) NOT NULL,
DepartmentId INT NOT NULL,
Salary MONEY NOT NULL
)

CREATE TRIGGER tr_DeleteEmployee ON Employees
AFTER DELETE
AS
INSERT INTO Deleted_Employees
SELECT
FirstName,
LastName,
MiddleName,
JobTitle,
DepartmentId,
Salary
FROM deleted

