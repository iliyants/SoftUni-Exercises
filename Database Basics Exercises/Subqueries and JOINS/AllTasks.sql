SELECT * FROM Departments
SELECT * FROM Addresses
SELECT * FROm Towns
SELECT * FROm Employees
SELECT * FROM Projects
SELECT * FROm EmployeesProjects

--Employee Address
SELECT TOP 5 e.EmployeeId,e.JobTitle,e.AddressId,a.AddressText
FROM Employees AS e
JOIN Addresses AS a
ON e.AddressID = a.AddressID
ORDER BY a.AddressID
--Addresses with Towns
SELECT TOP 50 e.FirstName,e.LastName,t.[Name],a.AddressText
FROM Employees AS e
JOIN Addresses AS a
ON e.AddressID = a.AddressID
JOIN Towns AS t
ON a.TownID = t.TownID
ORDER BY e.FirstName,e.LastName
--Sales Employees
SELECT e.EmployeeID,e.FirstName,e.LastName,d.[Name]
FROM Employees AS e
JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
WHERE d.[Name] = 'Sales'
ORDER BY e.EmployeeID
--Employee Departments
SELECT TOP 5 e.EmployeeID,e.FirstName,e.Salary,d.[Name]
FROM Employees AS E
JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
WHERE e.Salary > 15000
ORDER BY e.DepartmentID
--Employees Without Project
SELECT TOP 3 e.EmployeeID,e.FirstName
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep
ON e.EmployeeID = ep.EmployeeID
WHERE ep.EmployeeID IS NULL
ORDER BY e.EmployeeID
--Employees Hired After
SELECT e.FirstName,e.LastName,e.HireDate,d.[Name]
FROM Employees AS e
JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
WHERE e.HireDate > '1.1.1999' AND (d.[Name] = 'Sales' OR d.[Name] = 'Finance')
ORDER BY e.HireDate
--Employees With Project
SELECT TOP 5
			e.EmployeeID,
			e.FirstName,
			p.[Name]
	FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep
	ON e.EmployeeID = ep.EmployeeID
LEFT JOIN Projects AS p
	ON ep.ProjectID = p.ProjectID
	WHERE p.StartDate > '08-13-2002'
	AND p.EndDate IS NULL
ORDER BY e.EmployeeID
--Employee 24
SELECT e.EmployeeID,e.FirstName,
CASE
WHEN p.StartDate > '01-01-2005'
THEN NULL
ELSE p.[Name]
END
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep
ON e.EmployeeID = ep.EmployeeID
LEFT JOIN Projects AS p
ON p.ProjectID = ep.ProjectID
WHERE e.EmployeeID = 24
--Employee Manager
SELECT e.EmployeeID,e.FirstName,e.ManagerID,m.FirstName AS [ManagerName]
FROM Employees AS e
JOIN Employees as m
ON m.EmployeeID = e.ManagerID
WHERE e.ManagerID IN (3,7)
ORDER BY e.EmployeeID
--Employee Summary
SELECT TOP 50 e.EmployeeID,(e.FirstName + ' ' + e.LastName) AS [EmployeeName],(m.FirstName + ' ' + m.LastName) AS [ManagerName], d.[Name]
FROM Employees AS e
JOIN Employees AS m
ON m.EmployeeID = e.ManagerID
JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
ORDER BY e.EmployeeID
--Min Average Salary
SELECT MIN(dt.avgSalary) FROM
(SELECT AVG(Salary) AS avgSalary FROM Employees
GROUP BY DepartmentID) AS dt
--Highest Peaks in Bulgaria
SELECT mc.CountryCode,m.MountainRange
FROM MountainsCountries AS mc
JOIN Mountains AS m
ON mc.MountainId = m.Id 
--Count Mountain Ranges
SELECT CountryCode,COUNT(*) MountainID
FROM MountainsCountries
GROUP BY CountryCode
HAVING CountryCode IN ('BG','RU','US')
--Countries With or Without Rivers
SELECT TOP 5 c.CountryName,r.RiverName
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr 
ON c.CountryCode = cr.CountryCode 
LEFT JOIN Rivers AS r 
ON cr.RiverId = r.Id
WHERE c.ContinentCode = 'AF'
ORDER BY c.CountryName
--Continents and Currencies
SELECT ContinentCode,CurrencyCode,CurrencyUsage FROM(
SELECT ContinentCode,CurrencyCode,COUNT(CurrencyCode) AS [CurrencyUsage],
DENSE_RANK () OVER (PARTITION BY ContinentCode ORDER BY COUNT(CurrencyCode) DESC) AS [Rank]
FROM Countries
GROUP BY CurrencyCode,ContinentCode
HAVING COUNT(CurrencyCode) > 1) AS a
WHERE [Rank] = 1
ORDER BY ContinentCode
  --Countries Without any Mountains
  SELECT COUNT(c.CountryName) AS [CountryCode]FROM
(SELECT c.CountryName,m.MountainRange
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc
ON c.CountryCode = mc.CountryCode
LEFT JOIN Mountains AS m
ON m.Id = mc.MountainId
WHERE m.MountainRange IS NULL) AS c
--Highest Peak and Longest River by Country
SELECT TOP 5 c.CountryName,MAX(p.Elevation) AS [HighestPeakElevation],MAX(r.[Length]) AS [LongestRiverLength]
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc
ON c.CountryCode = mc.CountryCode
LEFT JOIN Mountains AS m
ON mc.MountainId = m.Id
LEFT JOIN Peaks AS p
ON m.Id = p.MountainId
LEFT JOIN CountriesRivers AS cr
ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r
ON cr.RiverId = r.Id
GROUP BY c.CountryName
ORDER BY [HighestPeakElevation] DESC,[LongestRiverLength] DESC,c.CountryName
--Highest Peak Name and Elevation by Country
SELECT TOP 5 nested.CountryName, nested.[Highest Peak Name],nested.[Highest Peak Elevation],nested.Mountain FROM(
SELECT  c.CountryName,
ISNULL(p.PeakName,'(no highest peak)') AS [Highest Peak Name],
ISNULL(p.Elevation, 0)AS [Highest Peak Elevation],
ISNULL(m.MountainRange, '(no mountain)') AS [Mountain],
DENSE_RANK() OVER(PARTITION BY CountryName ORDER BY p.Elevation DESC) AS [elev] 
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc
ON c.CountryCode = mc.CountryCode
LEFT JOIN Mountains AS m
ON m.Id = mc.MountainId
LEFT JOIN Peaks AS p
ON p.MountainId = mc.MountainId
) AS nested
WHERE nested.elev = 1
ORDER BY nested.CountryName ,nested.[Highest Peak Name] 


















