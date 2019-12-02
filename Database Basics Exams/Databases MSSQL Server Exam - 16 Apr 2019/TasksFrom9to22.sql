--9.Most Used Luggage's
SELECT result.[Type], COUNT(result.[Type]) AS [MostUsedLuggage] FROM (
SELECT lt.[Type]
FROM Luggages AS l
JOIN LuggageTypes AS lt
ON lt.Id = l.LuggageTypeId
) AS result
GROUP BY result.[Type]
ORDER BY MostUsedLuggage DESC,[Type]
--10.Passenger Trips
SELECT 
	CONCAT(p.FirstName,' ',p.LastName) AS [Full Name],
	f.Origin,
	f.Destination
FROM Passengers AS p
JOIN Tickets AS t ON t.PassengerId = p.Id
JOIN Flights AS f ON t.FlightId = f.Id
ORDER BY [Full Name],f.Origin,f.Destination
--11.Non Adventures People
SELECT p.FirstName,p.LastName,p.Age
FROM Passengers AS p
LEFT JOIN Tickets AS t ON p.Id = t.PassengerId
WHERE FlightId IS NULL
ORDER BY p.Age DESC,p.FirstName,p.LastName
--12.Lost Luggages
SELECT p.PassportId,p.[Address]
FROM Passengers AS p
LEFT JOIN Luggages AS l ON p.Id = l.PassengerId 
WHERE l.LuggageTypeId IS NULL
ORDER BY p.PassportId,p.[Address]
--13.Count of Trips
SELECT result.FirstName AS [FirstName],result.LastName AS [Last Name],COUNT(result.FlightId) AS [Total Trips]
FROM(
SELECT p.FirstName,p.LastName,t.FlightId
FROM Passengers AS p
LEFT JOIN Tickets AS t ON t.PassengerId = p.Id
) AS result
GROUP BY result.FirstName,result.LastName
ORDER BY [Total Trips] DESC,FirstName ASC,LastName ASC
--14.Full Info	
SELECT
CONCAT(p.FirstName,' ',p.LastName) AS [Full Name],
pl.[Name] AS [Plane Name],
CONCAT(fl.Origin, ' - ', fl.Destination) AS [Trip],
lggt.[Type] AS [Luggage Type]
FROM Passengers AS p
JOIN Tickets AS t ON p.Id = t.PassengerId
JOIN Flights AS fl ON fl.Id = t.FlightId
JOIN Planes AS pl ON pl.Id = fl.PlaneId
JOIN Luggages AS lggs ON lggs.Id = t.LuggageId
JOIN LuggageTypes AS lggt ON lggt.Id = lggs.LuggageTypeId
ORDER BY [Full Name],pl.[Name],Origin,fl.Destination,lggt.[Type]
--15.Most Expesnive Trips
SELECT result.FirstName,result.LastName,result.Destination,result.Price 
FROM
(SELECT 
	p.FirstName,
	p.LastName,
	fl.Destination,
	t.Price,
	DENSE_RANK () OVER(PARTITION BY p.FirstName ORDER BY t.Price DESC) AS [RANK]
FROM Passengers AS p
JOIN Tickets AS t ON t.PassengerId = p.Id
JOIN Flights AS fl ON fl.Id = t.FlightId
GROUP BY p.FirstName,p.LastName,fl.Destination,t.Price) AS result
WHERE result.[RANK] = 1
ORDER BY result.Price DESC,result.FirstName,result.LastName,result.Destination
--16.Destinations Info
SELECT f.Destination, COUNT(t.Id) AS [Count]
     FROM Flights AS f
LEFT JOIN Tickets AS t ON t.FlightId = f.Id
 GROUP BY f.Destination
 ORDER BY [Count] DESC, f.Destination
 --17. PSP
 SELECT result.[Name],result.Seats,COUNT(result.PassengerId) AS [Passengers Count]
 FROM
 (SELECT p.[Name],p.Seats,t.PassengerId
 FROM Planes AS p
 LEFT JOIN Flights AS f ON f.PlaneId = p.Id
 LEFT JOIN Tickets AS t ON t.FlightId = f.Id
) AS result
 GROUP BY result.[Name],result.Seats
 ORDER BY [Passengers Count] DESC,result.[Name],result.Seats
 --18.Vacation
 CREATE FUNCTION udf_CalculateTickets(@origin NVARCHAR(50), @destination NVARCHAR(50), @peopleCount INT) 
 RETURNS NVARCHAR(100)
 BEGIN

 DECLARE @route NVARCHAR(50) = CONCAT(@origin, ' ',@destination)
 DECLARE @routeCheck NVARCHAR(100) = (SELECT r.FullRoute
FROM(
SELECT CONCAT(f.Origin,' ',f.Destination) AS FullRoute,t.Price
FROM Flights AS f
JOIN Tickets AS t ON f.Id = t.FlightId) AS r
WHERE r.FullRoute = @route)

IF(@routeCheck IS NULL )
	BEGIN
		RETURN('Invalid flight!')
	END;
IF(@peopleCount = 0 OR @peopleCount < 0)
	BEGIN
		RETURN('Invalid people count!')
	END;

RETURN 'Total price ' + CAST (@peopleCount *  (SELECT r.Price
FROM(
SELECT CONCAT(f.Origin,' ',f.Destination) AS FullRoute,t.Price
FROM Flights AS f
JOIN Tickets AS t ON f.Id = t.FlightId) AS r
WHERE r.FullRoute = @route) AS NVARCHAR(100) )

 END;
--19. Wrong Data
CREATE PROC usp_CancelFlights
AS
BEGIN

DECLARE @currentRow INT = 1;
DECLARE @totalRows INT = (SELECT COUNT(r.[ROWS])
FROM
(
SELECT DepartureTime,ArrivalTime,ROW_NUMBER() OVER (ORDER BY PlaneId) AS [ROWS] FROM Flights
WHERE DepartureTime < ArrivalTime
) AS r);

WHILE(@currentRow <= @totalRows)
BEGIN
		DECLARE @IdTodelete INT = (SELECT TOP 1 Id
		FROM Flights
		WHERE DepartureTime < ArrivalTime)

			UPDATE Flights
			SET DepartureTime = NULL
			WHERE Id = @IdTodelete

			UPDATE Flights
			SET ArrivalTime = NULL
			WHERE Id = @IdTodelete

			SET @currentRow += 1
END
END
--20.Deleted Planes
CREATE TABLE DeletedPlanes (
Id INT PRIMARY KEY IDENTITY(1,1),
[Name] NVARCHAR(50) NOT NULL,
Seats INT NOT NULL,
[Range] INT NOT NULL
)

CREATE TRIGGER tr_DeletePlane ON Planes
AFTER DELETE
AS
INSERT INTO DeletedPlanes
SELECT
Id,
[Name],
Seats,
[Range]
FROM deleted



