--2.Insert
INSERT INTO Files
VALUES
('Trade.idk',2598.0,1,1),
('menu.net',9238.21,2,2),
('Administrate.soshy',1246.93,3,3),
('Controller.php',7353.15,4,4),
('Find.java',9957.86,5,5),
('Controller.json',14034.87,3,6),
('Operate.xix',7662.92,7,7)

INSERT INTO Issues
VALUES
('Critical Problem with HomeController.cs file','open',1,4),
('Typo fix in Judge.html','open',4,3),
('Implement documentation for UsersService.cs','closed',8,2),
('Unreachable code in Index.cs','open',9,8)

--03.Update
UPDATE Issues
SET IssueStatus = 'closed'
WHERE AssigneeId = 6

--04. Delete
DELETE FROM Issues
WHERE RepositoryId = 3

DELETE FROM RepositoriesContributors
WHERE RepositoryId = 3

--5.Commits
SELECT Id,[Message],RepositoryId,ContributorId
FROM Commits
ORDER BY Id,[Message],RepositoryId,ContributorId

--6.Heavy HTML
SELECT Id,[Name],Size
FROM Files
WHERE Size > 1000 AND [Name] LIKE '%html%'
ORDER BY Size DESC,Id,[Name]

--7.Issues and Users
SELECT i.Id,CONCAT(u.Username,' : ',i.Title)
FROM Issues AS i
JOIN Users AS u ON u.Id = i.AssigneeId
ORDER BY i.Id DESC,u.Username

--8.Non-Directory Files
SELECT f.Id,f.[Name],CONCAT(f.Size,'KB') AS [Size]
FROM Files AS f
LEFT JOIN Files AS p ON p.ParentId = f.Id
WHERE p.ParentId IS NULL
ORDER BY f.Id,f.[Name],f.Size DESC

--9.Most Contributed Repositories
SELECT TOP 5 r.Id,r.[Name],COUNT(rc.ContributorId) AS [Commits]
FROM Commits AS c
JOIN Repositories AS r ON r.Id = c.RepositoryId
JOIN RepositoriesContributors AS rc ON rc.RepositoryId = c.RepositoryId
GROUP BY r.Id,r.Name
ORDER BY [Commits] DESC,r.Id,r.[Name]

--10.User and Files
SELECT u.Username,AVG(f.Size) AS Size
FROm Users AS u
JOIN Commits AS c ON c.ContributorId = u.Id
JOIN Files AS f ON f.CommitId = c.Id
GROUP BY u.Username
ORDER BY Size DESC,u.Username

--11. User Total Commits
CREATE FUNCTION udf_UserTotalCommits(@username NVARCHAR(50))
RETURNS INT
AS
BEGIN

RETURN 
(SELECT COUNT(c.RepositoryId)
FROM Users AS u
JOIN Commits AS c ON c.ContributorId = u.Id
WHERE u.Username = @username
 )

END 

--12.Find by Extensions
CREATE PROC usp_FindByExtension(@extension NVARCHAR(20))
AS
BEGIN

SELECT Id,[Name],CONCAT(Size,'KB')
FROM Files
WHERE [Name] LIKE '%' + @extension

END


EXEC dbo.usp_FindByExtension 'txt'