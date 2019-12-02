--PART I Queries for SoftUni Database
--Find Names of All Employees by First Name
SELECT FirstName, LastName
FROM Employees
WHERE FirstName LIKE 'Sa%'
--Find Names of All Employees by Last Name
SELECT FirstName, LastName
FROM Employees
WHERE LastName LIKE '%ei%'
--Find First Name of All Employees
SELECT FirstName 
FROM Employees
WHERE DepartmentId = 3 OR DepartmentId = 10 AND YEAR(HireDate) BETWEEN 1995 AND 2015
--Find All Employees Except Engineers
SELECT FirstName, LastName
FROM Employees
WHERE JobTitle NOT LIKE '%engineer%'
--Find Towns With Name Length
SELECT [Name]
FROM Towns
WHERE LEN([Name]) = 5 OR LEN([Name]) = 6
ORDER BY [Name]
--Find Towns Starting With
SELECT TownId, [Name]
FROM Towns
WHERE [Name] LIKE 'M%' OR [Name] LIKE 'K%' OR [Name] LIKE 'B%' OR [NAME] LIKE 'E%'
ORDER BY [Name]
--Find Towns Not Starting With
SELECT TownId, [Name]
FROM Towns
WHERE [Name] NOT LIKE 'R%' AND [Name] NOT LIKE 'B%' AND [Name] NOT LIKE 'D%'
ORDER BY [Name]
--Create View Employees Hired After 2000 Year
CREATE VIEW V_EmployeesHiredAfter2000 AS
SELECT FirstName, LastName
FROM Employees
WHERE YEAR(HireDate) > 2000
--Length of Last Name
SELECT FirstName, LastName
FROM Employees
WHERE LEN(LastName) = 5
--Rank Employees by Salary
SELECT EmployeeId,FirstName,LastName,Salary,
DENSE_RANK() OVER (PARTITION by Salary ORDER BY EmployeeId) AS [Rank]
FROM Employees
WHERE Salary BETWEEN 10000 AND 50000
ORDER BY Salary DESC
--Find All Employees with Rank 2
SELECT * FROM(SELECT EmployeeId,FirstName,LastName,Salary,
DENSE_RANK() OVER (PARTITION by Salary ORDER BY EmployeeId) AS [Rank]
FROM Employees
WHERE Salary BETWEEN 10000 AND 50000) AS RankedTable
WHERE RankedTable.Rank = 2
ORDER BY Salary DESC
--Part II Queries for Geography Database
--Countries Holding 'A' 3 or More Times
SELECT CountryName,IsoCode
FROM Countries
WHERE CountryName LIKE '%a%a%a%'
ORDER BY IsoCode
--Mix of Peak and River Names
SELECT PeakName,RiverName,CONCAT(LOWER(PeakName),LOWER(SUBSTRING(RiverName,2,LEN(RiverName)))) AS Mix
FROM Peaks,Rivers
WHERE RIGHT(PeakName,1) = LEFT(RiverName,1)
ORDER BY Mix
--Part III Queries for Diablo Database
--Games from 2011 and 2012 Year
SELECT TOP 50 [Name],FORMAT([Start],'yyy-MM-dd') AS [Start]
FROM Games
WHERE YEAR([Start]) = 2011 OR YEAR([Start]) = 2012
ORDER BY [Start]
--User Email Providers
SELECT Username,SUBSTRING(Email,CHARINDEX('@',Email) + 1,LEN(Email)) AS [Email Provider]
FROM Users
ORDER BY [Email Provider],UserName 
--Get Users with IPAdress Like Pattern
SELECT UserName,IpAddress AS [IP Address]
FROM Users
WHERE IpAddress LIKE '___.1%.%.___'
ORDER BY UserName
--Show All Games With Duration and Part of the Day
  SELECT [Name] AS [Game],
         CASE 
             WHEN DATEPART(HOUR, [Start]) >= 0 AND DATEPART(HOUR, [Start]) < 12 THEN 'Morning'
             WHEN DATEPART(HOUR, [Start]) >= 12 AND DATEPART(HOUR, [Start]) < 18 THEN 'Afternoon'
             WHEN DATEPART(HOUR, [Start]) >= 18 AND DATEPART(HOUR, [Start]) < 24 THEN 'Evening'
         END AS [Part of the Day],
         CASE
             WHEN Duration <= 3 THEN 'Extra Short'
             WHEN Duration BETWEEN 4 AND 6 THEN 'Short'
             WHEN Duration > 6 THEN 'Long'
             ELSE 'Extra Long'
         END AS [Duration]
    FROM Games AS g
ORDER BY [Name],
         [Duration]
--Orders Table
SELECT ProductName,OrderDate,DATEADD(DAY,3,OrderDate) AS [Pay Due],DATEADD(MONTH,1,OrderDate) AS [Deliver Due]
FROM Orders




