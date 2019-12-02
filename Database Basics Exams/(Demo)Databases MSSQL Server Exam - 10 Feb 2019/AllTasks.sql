--2.Insert
INSERT INTO Planets
VALUES
('Mars'),
('Earth'),
('Jupiter'),
('Saturn')

INSERT INTO Spaceships
VALUES
('Golf','VW',3),
('WakaWaka','Wakanda',4),
('Falcon9','SpaceX',1),
('Bed','Vidolov',6)

--3.Update
UPDATE Spaceships
SET LightSpeedRate += 1
WHERE Id BETWEEN  8 AND 12

--4.Delete
DELETE FROM TravelCards
WHERE JourneyId IN (1,2,3)

DELETE TOP (3) FROM Journeys

--5.Select all travel cards
SELECT CardNumber,JobDuringJourney
FROM TravelCards
ORDER BY CardNumber

--6.Select All Colonists
SELECT Id,CONCAT(FirstName,' ',LastName) AS [Full Name],Ucn
FROM Colonists
ORDER BY FirstName,LastName,Id

--7.Select All Military Journeys
SELECT Id,FORMAT(JourneyStart, 'dd/MM/yyyy'),FORMAT(JourneyEnd,'dd/MM/yyyy')
FROM Journeys
WHERE Purpose = 'Military'
ORDER BY JourneyStart

--8.Select All Pilots
SELECT c.Id,CONCAT(c.FirstName,' ',c.LastName) AS [Full Name]
FROM
Colonists AS c
JOIN TravelCards AS tc ON tc.ColonistId = c.Id
WHERE tc.JobDuringJourney = 'Pilot'
ORDER BY c.Id

--9.Count Colonists
SELECT COUNT(c.Id) AS [count]
FROM Colonists AS c
JOIN TravelCards AS tc ON tc.ColonistId = c.Id
JOIN Journeys AS j ON j.Id = tc.JourneyId
WHERE j.Purpose = 'Technical'

--10.Select The Fastest Spaceship
SELECT TOP 1 ss.[Name] AS [SpaceshipName],sp.[Name] AS [SpaceportName]
FROM Spaceships AS ss
JOIN Journeys AS j ON j.SpaceshipId = ss.Id
JOIN Spaceports AS sp ON sp.Id = j.DestinationSpaceportId
ORDER BY ss.LightSpeedRate DESC

--11.Select spaceships with pilots younger than 30 years
SELECT ss.[Name],ss.Manufacturer
FROM Colonists AS c
JOIN TravelCards AS tc ON tc.ColonistId = c.Id
JOIN Journeys AS j ON j.Id = tc.JourneyId
JOIN Spaceships AS ss ON ss.Id = j.SpaceshipId
WHERE DATEDIFF(YEAR,c.BirthDate,'01-01-2019') < 30 AND tc.JobDuringJourney = 'Pilot'
ORDER BY ss.[Name]

--12.Select All Educational Mission
SELECT p.[Name],sp.[Name]
FROM Planets AS p
JOIN Spaceports AS sp ON sp.PlanetId = p.Id
JOIN Journeys AS j ON j.DestinationSpaceportId  = sp.Id
WHERE j.Purpose = 'Educational'
ORDER BY sp.[Name]  DESC

--13.Planets And Journeys
SELECT p.[Name] AS [PlanetName],COUNT(j.DestinationSpaceportId) AS [JourneysCount]
FROM Planets AS p
JOIN Spaceports AS sp ON sp.PlanetId = p.Id
JOIN Journeys AS j ON j.DestinationSpaceportId = sp.Id
GROUP BY p.[Name]
ORDER BY [JourneysCount] DESC,p.[Name]

--14.Extract The Shortest Journey
SELECT TOP (1) j.Id,p.[Name] AS [PlanetName],sp.[Name] AS [SpaceportName],j.Purpose AS [JourneyPurpose]
FROM Journeys AS j
JOIN Spaceports AS sp ON sp.Id = j.DestinationSpaceportId
JOIN Planets AS p ON p.Id = sp.PlanetId
ORDER BY (j.JourneyStart - j.JourneyEnd) DESC

--15.Select The Less Popular Job
SELECT TOP(1) tc.JourneyId, tc.JobDuringJourney AS JobName
      FROM TravelCards AS tc
     WHERE tc.JourneyId = (SELECT TOP(1) j.Id FROM Journeys AS j ORDER BY DATEDIFF(MINUTE, j.JourneyStart, j.JourneyEnd) DESC)
  GROUP BY tc.JobDuringJourney, tc.JourneyId
  ORDER BY COUNT(tc.JobDuringJourney) ASC

  --16.Select Second Oldest Important Colonist
  SELECT result.JobDuringJourney,result.FullName,result.[RANK] AS [JobRank]
  FROM
  (SELECT j.Id,tc.JobDuringJourney,CONCAT(c.FirstName,' ', c.LastName) AS [FullName],DENSE_RANK () OVER(PARTITION BY tc.JobDuringJourney ORDER BY c.Birthdate) AS [RANK]
  FROM Colonists AS c
  JOIN TravelCards AS tc ON tc.ColonistId = c.Id
  JOIN Journeys AS j ON j.Id = tc.JourneyId) AS result
  GROUP BY result.JobDuringJourney,result.FullName,result.[RANK]
  HAVING result.[RANK] = 2

  --17. Planets and Spaceports
  SELECT p.[Name],COUNT(sp.Id) AS [Count]
  FROM Planets AS p
  LEFT JOIN Spaceports AS sp ON sp.PlanetId = p.Id
  GROUP BY p.[Name]
  ORDER BY [Count] DESC,p.[Name]

  --18. Get Colonists Count
  CREATE FUNCTION dbo.udf_GetColonistsCount(@PlanetName VARCHAR (30))
  RETURNS INT
  AS
  BEGIN

  RETURN 
  (SELECT COUNT(tc.ColonistId)
  FROM Planets AS p
  JOIN Spaceports AS sp ON sp.PlanetId = p.Id
  JOIN Journeys AS j ON j.DestinationSpaceportId = sp.Id
  JOIn TravelCards AS tc ON tc.JourneyId = j.Id
  WHERE p.[Name] = @PlanetName)

  END 

  --19. Change Journey Purpose
  CREATE  PROCEDURE usp_ChangeJourneyPurpose(@JourneyId INT, @NewPurpose VARCHAR(20))
  AS
  BEGIN
	
	DECLARE @idCheck INT = (SELECT Id FROM Journeys WHERE Id = @JourneyId);

	IF(@idCheck IS NULL)
		BEGIN
			RAISERROR('The journey does not exist!',16,1)
			RETURN
		END

	DECLARE @currentPurpose VARCHAR(20) = (SELECT Purpose FROM Journeys WHERE Id = @JourneyId)

	IF(@currentPurpose = @NewPurpose)
		BEGIN
			RAISERROR('You cannot change the purpose!',16,1)
			RETURN
		END

	UPDATE Journeys
	SET Purpose = @NewPurpose
	WHERE Id = @JourneyId

  END


--20.Deleted Journeys
CREATE TRIGGER tr_DeleteJourneys ON Journeys
AFTER DELETE
AS
BEGIN
INSERT INTO DeletedJourneys(Id,JourneyStart,JourneyEnd, Purpose, DestinationSpaceportId, SpaceshipId)
		SELECT Id,JourneyStart,JourneyEnd, Purpose, DestinationSpaceportId, SpaceshipId FROM deleted
		END
