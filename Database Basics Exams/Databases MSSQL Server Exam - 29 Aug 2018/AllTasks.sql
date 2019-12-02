--2. Insert
INSERT INTO Employees (FirstName, LastName, Phone, Salary) VALUES
  ('Stoyan',	'Petrov',	'888-785-8573',	500.25),
  ('Stamat',	'Nikolov',	'789-613-1122',	999995.25),
  ('Evgeni',	'Petkov',	'645-369-9517',	1234.51),
  ('Krasimir',	'Vidolov',	'321-471-9982',	50.25)

INSERT INTO Items ([Name], Price, CategoryId) VALUES
  ('Tesla battery',154.25	,8),
  ('Chess',	30.25,	8),
  ('Juice',	5.32,1),
  ('Glasses',10,	8),
  ('Bottle of water',	1,	1)

  --03. Update
 UPDATE Items
 SET Price*=1.27
 WHERE CategoryId IN(1,2,3)

 --04. Delete
 DELETE FROm OrderItems
 WHERE OrderId = 48

 --5.Richest People
 SELECT Id,FirstName
 FROM Employees
 WHERE Salary > 6500
 ORDER BY FirstName,Id

 --6.Cool Phone Numbers
 SELECT CONCAT(FirstName,' ',LastName) AS [FullName],Phone AS [Phone Number]
 FROM Employees
 WHERE Phone LIKE '3%'
 ORDER BY FirstName,Phone

 --7.Employee Statistics
 SELECT e.FirstName,e.LastName,COUNT(o.Id) AS [Count]
 FROM Employees AS e
 JOIN Orders AS o ON e.Id = o.EmployeeId
 GROUP BY e.FirstName,e.LastName
 ORDER BY [Count] DESC,e.FirstName

 --8.Hard Workers Club
SELECT 
	e.FirstName,
	e.LastName,
	AVG(DATEDIFF(HOUR,s.CheckIn,s.CheckOut)) AS [Work Hours]
FROM 
	Employees AS e
	JOIN Shifts AS s ON s.EmployeeId = e.Id
	GROUP BY e.FirstName,e.LastName,e.Id
	HAVING AVG(DATEDIFF(HOUR,s.CheckIn,s.CheckOut)) > 7
	ORDER BY [Work Hours] DESC,e.Id

--9.The Most Expensive Order
SELECT TOP 1 oi.OrderId,SUM(i.Price * oi.Quantity) AS [TotalPrice]
FROM OrderItems AS oi
JOIN Items AS i ON i.Id = oi.ItemId
GROUP BY oi.OrderId
ORDER BY [TotalPrice] DESC

--10.Rich Item, Poor Item
SELECT TOP 10 o.Id,MAX(i.Price) AS [ExpensivePrice],MIN(i.Price) AS [CheapPrice]
FROM Orders AS o
JOIn OrderItems AS oi ON oi.OrderId = o.Id
JOIN Items AS i ON i.Id = oi.ItemId
GROUP BY o.Id
ORDER BY ExpensivePrice DESC,o.Id

--11.Cashiers
SELECT e.Id,e.FirstName,e.LastName
FROM Employees AS e
JOIn Orders AS o ON o.EmployeeId = e.Id
GROUP BY e.Id,e.FirstName,e.LastName
ORDER BY e.Id

--12.Lazy Employees
SELECT result.Id,result.[Full Name]
FROM
(SELECT 
	e.Id,
	CONCAT(e.FirstName,' ',e.LastName) AS [Full Name],
	DATEDIFF(HOUR,s.CheckIn,s.CheckOut) AS [Work Hours]
FROM 
	Employees AS e
	JOIN Shifts AS s ON s.EmployeeId = e.Id
) AS result
WHERE result.[Work Hours] < 4
GROUP BY result.Id,result.[Full Name]
ORDER BY result.Id

--13.Sellers
SELECT TOP 10 CONCAT(e.FirstName,' ',e.LastName) AS [Full Name],SUM(oi.Quantity * i.Price) AS [Total Sum],SUM(oi.Quantity) AS Items
FROM Employees AS e
JOIN Orders AS o ON o.EmployeeId = e.Id
JOIn OrderItems AS oi ON oi.OrderId = o.Id
JOIn Items AS i ON i.Id = oi.ItemId
WHERE o.[DateTime] < '2018-06-15'
GROUP BY e.FirstName,e.LastName
ORDER BY [Total Sum] DESC,Items DESC

--14.Tough Days
SELECT 
	CONCAT(e.FirstName,' ',e.LastName) AS [Full Name],
	CASE
	WHEN DATEPART(WEEKDAY,s.CheckIn) = 1 THEN 'Sunday'
	WHEN DATEPART(WEEKDAY,s.CheckIn) = 2 THEN 'Monday'
	WHEN DATEPART(WEEKDAY,s.CheckIn) = 3 THEN 'Tuesday'
	WHEN DATEPART(WEEKDAY,s.CheckIn) = 4 THEN 'Wednesday'
	WHEN DATEPART(WEEKDAY,s.CheckIn) = 5 THEN 'Thursday'
	WHEN DATEPART(WEEKDAY,s.CheckIn) = 6 THEN 'Friday'
	WHEN DATEPART(WEEKDAY,s.CheckIn) = 7 THEN 'Sunday'
	END AS [Day of week]
FROM Employees AS e
JOIN Shifts AS s ON s.EmployeeId = e.Id
LEFT JOIN Orders AS o ON o.EmployeeId = e.Id
WHERE o.Id IS NULL AND DATEDIFF(HOUR,s.CheckIn,s.CheckOut) > 12
ORDER BY e.Id

--15.Top Order per Employee

SELECT CONCAT(e.FirstName,' ',e.LastName) AS [FullName],DATEDIFF(HOUR,s.CheckIn,s.CheckOut) AS [WorkHours],result.TotalPrice AS [TotalPrice]
FROM
(SELECT 
	o.EmployeeId,
	o.[DateTime],
	SUM(i.Price * oi.Quantity) AS [TotalPrice],
	DENSE_RANK () OVER(PARTITION BY o.EmployeeId ORDER BY SUM(i.Price * oi.Quantity) DESC) AS [RANK]
FROM Orders AS o
JOIN OrderItems AS oi ON oi.OrderId = o.Id
JOIN Items AS i ON i.Id = oi.ItemId
GROUP BY o.EmployeeId,o.[DateTime],o.Id ) AS result
JOIN Employees AS e ON e.Id = result.EmployeeId
JOIN Shifts AS s ON s.EmployeeId = result.EmployeeId
WHERE result.[RANK] = 1 AND result.[DateTime] BETWEEN s.CheckIn AND s.CheckOut
ORDER BY FullName, WorkHours DESC, TotalPrice DESC

--16.Average Profit per Day
SELECT DATEPART(DAY,o.[DateTime]) AS [DayOfMonth],CAST(AVG(oi.Quantity * i.Price) AS DECIMAL(15,2)) AS [Total Price]
FROM Orders AS o
JOIN OrderItems AS oi ON oi.OrderId = o.Id
JOIN Items AS i ON i.Id = oi.ItemId
GROUP BY DATEPART(DAY,o.[DateTime])
ORDER BY [DayOfMonth]

--17. Top Products

SELECT i.[Name] AS [Item],c.[Name] AS [Category],SUM(oi.Quantity) AS [Count],SUM(i.Price * oi.Quantity) AS [TotalPrice] 
FROM Items AS i
LEFT JOIN OrderItems AS oi ON oi.ItemId = i.Id
LEFT JOIN Categories AS c ON c.Id = i.CategoryId
GROUP BY i.[Name],c.[Name]
ORDER BY TotalPrice DESC,[Count] DESC


--18. Promotion days
CREATE  FUNCTION udf_GetPromotedProducts(@CurrentDate DATETIME, @StartDate DATETIME, @EndDate DATETIME, @Discount INT, @FirstItemId INT, @SecondItemId INT, @ThirdItemId INT)
RETURNS VARCHAR(200)
AS
BEGIN

DECLARE @firstItemCheck NVARCHAR(20) = (SELECT [Name] FROM Items WHERE Id = @FirstItemId)
DECLARE @secondItemCheck NVARCHAR(20) = (SELECT [Name] FROM Items WHERE Id = @SecondItemId)
DECLARE @thirdItemCheck NVARCHAR(20) = (SELECT [Name] FROM Items WHERE Id = @ThirdItemId)

	IF(@firstItemCheck IS NULL OR @secondItemCheck IS NULL OR @thirdItemCheck IS NULL)
		BEGIN
			RETURN ('One of the items does not exists!')
		END
	IF(@CurrentDate NOT BETWEEN @StartDate AND @EndDate)
		BEGIN
			RETURN('The current date is not within the promotion dates!')
		END

DECLARE @firstItemPrice DECIMAL(15,2) = (SELECT Price FROM Items WHERE Id = @FirstItemId)
DECLARE @firstItemDiscount DECIMAL(15,2) = @firstItemPrice - (@firstItemPrice * @Discount / 100)


DECLARE @secondItemPrice DECIMAL(15,2) = (SELECT Price FROM Items WHERE Id = @SecondItemId)
DECLARE @secondItemDiscount DECIMAL(15,2) = @secondItemPrice - (@secondItemPrice * @Discount / 100)

DECLARE @thirdItemPrice DECIMAL(15,2) = (SELECT Price FROM Items WHERE Id = @ThirdItemId)
DECLARE @thirdItemDiscount DECIMAL(15,2) = @thirdItemPrice - (@thirdItemPrice * @Discount / 100)

RETURN @FirstItemCheck + ' price: ' + CAST(ROUND(@firstItemDiscount,2) AS VARCHAR) + ' <-> ' +
		@SecondItemCheck + ' price: ' + CAST(ROUND(@secondItemDiscount,2) AS VARCHAR) +  ' <-> ' +
		@ThirdItemCheck + ' price: ' + CAST(ROUND(@thirdItemDiscount,2) AS VARCHAR)
 
END

--19. Cancel order
CREATE PROC usp_CancelOrder(@OrderId INT, @CancelDate DATETIME)
AS
BEGIN
DECLARE @orderCheck INT = (SELECT Id FROM Orders WHERE Id = @OrderId)

	IF(@orderCheck IS NULL)
		BEGIN
			RAISERROR('The order does not exist!',16,1)
			RETURN
		END
DECLARE @issueDate DATETIME = (SELECT [DateTime] FROM Orders WHERE Id = @OrderId)

	IF(DATEDIFF(DAY,@issueDate,@CancelDate) > 3)
		BEGIN
			RAISERROR('You cannot cancel the order!',16,1)
			RETURN
		END

		DELETE FROM OrderItems WHERE OrderId = @OrderId
	DELETE FROM Orders WHERE Id = @OrderId
END

--20. Deleted Orders
CREATE TRIGGER tr_DeleteORder ON OrderItems
AFTER DELETE
AS
BEGIN
INSERT INTO DeletedOrders
SELECT 
OrderId,
ItemId,
Quantity
FROM deleted
END











	 






	