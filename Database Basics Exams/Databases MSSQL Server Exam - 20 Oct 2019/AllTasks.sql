--2.Insert
INSERT INTO Employees (FirstName,LastName,Birthdate,DepartmentId)
VALUES
('Marlo','O''Malley','1958-9-21',1),
('Niki','Stanaghan','1969-11-26',4),
('Ayrton','Senna','1960-03-21',9),
('Ronnie','Peterson','1944-02-14',9),
('Giovanna','Amati','1959-07-20',5)

INSERT INTO Reports (CategoryId,StatusId,OpenDate,CloseDate,[Description],UserId,EmployeeId)
VALUES
(1,1,'2017-04-13',NULL,'Stuck Road on Str.133',6,2),
(6,3,'2015-09-05','2015-12-06','Charity trail running',3,5),
(14,2,'2015-09-07',NULL,'Falling bricks on Str.58',5,2),
(4,3,'2017-07-03','2017-07-06','Cut off streetlight on Str.11',1,1)


--3.Update
UPDATE Reports
SET [CloseDate] = GETDATE()
WHERE [CloseDate] IS NULL




--4.Delete
ALTER TABLE Reports
DROP CONSTRAINT FK__Reports__StatusI__1FCDBCEB

DELETE FROM Reports
WHERE StatusId = 4

--05. Unassigned Reports
SELECT r.[Description],FORMAT(r.OpenDate,'dd-MM-yyyy') AS [OpenDate]
FROM Reports AS r
WHERE EmployeeId IS NULL
ORDER BY r.OpenDate,r.[Description]

--6.Reports & Categories
SELECT r.[Description],c.[Name]
FROM Reports AS r
JOIN Categories AS c ON c.Id = r.CategoryId
ORDER BY r.[Description],c.[Name]

--07.Most Reported Category
SELECT TOP 5 c.[Name],COUNT(r.CategoryId) AS [ReportsNumber]
FROM Reports AS r
JOIN Categories AS c ON c.Id = r.CategoryId
GROUP BY c.[Name]
ORDER BY ReportsNumber DESC,c.[Name]

--08.Birthday Report
SELECT u.Username,c.[Name] AS CategoryName
FROM Reports AS r
JOIN Users AS u ON r.UserId = u.Id
JOIN Categories AS c ON c.Id = r.CategoryId
WHERE DATEPART(DAY,u.Birthdate) = DATEPART(DAY,r.OpenDate)
ORDER BY u.Username,c.[Name]

--09. User per Employee
SELECT CONCAT(e.FirstName,' ',e.LastName) AS [FullName],COUNT(r.UserId) AS UserCount
FROM Reports AS r
RIGHT JOIN Employees AS e ON e.Id = r.EmployeeId
GROUP BY e.FirstName,e.LastName
ORDER BY UserCount DESC,e.FirstName,e.LastName

--10. Full Info
SELECT 
ISNULL(e.FirstName + ' ' + e.LastName,'None') AS Employee,
ISNULL(d.[Name], 'None') AS Department,
ISNULL(c.[Name], 'None') AS Category,
ISNULL(r.[Description],'None') AS [Description],
ISNULL(FORMAT(r.OpenDate,'dd.MM.yyyy'),'None') AS OpenDate,
ISNULL(s.[Label],'None') AS [Status],
ISNULL(u.[Name],'None') AS [User]
FROM Reports AS r
LEFT JOIN Employees AS e ON e.Id = r.EmployeeId
LEFT JOIN Departments AS d ON d.Id = e.DepartmentId
LEFT JOIN Categories AS c ON c.Id = r.CategoryId
LEFT JOIN Users AS u ON u.Id = r.UserId
LEFT JOIN [Status] AS s ON s.Id = r.StatusId
ORDER BY e.FirstName DESC,e.LastName DESC,d.[Name],c.[Name] ASC,r.[Description],r.OpenDate,s.[Label],u.[Name]


--11. Hours to Complete
CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
AS
BEGIN

DECLARE @hours AS INT = ISNULL(DATEDIFF(HOUR,@StartDate,@EndDate), 0);

RETURN @hours

END

--12. Assign Employee
CREATE PROC usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
BEGIN

DECLARE @employeeDep NVARCHAR(50) = (SELECT d.[Name]
FROM Employees AS e
JOIN Departments AS d ON d.Id = e.DepartmentId
WHERE e.Id = @EmployeeId)

DECLARE @reportDep NVARCHAR(50) = 
(SELECT d.[Name]
FROM Reports AS r
JOIN Categories AS c ON c.Id = r.CategoryId
JOIN Departments AS d ON d.Id = c.DepartmentId
WHERE r.Id = @ReportId)

	IF(@employeeDep != @reportDep)
		BEGIN
		RAISERROR('Employee doesn''t belong to the appropriate department!',16,1)
		RETURN
		END

	UPDATE Reports
	SET EmployeeId = @EmployeeId
	WHERE Id = @ReportId
END
