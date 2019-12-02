--02. Insert
INSERT INTO Teachers (FirstName,LastName,[Address],Phone,SubjectId)
VALUES
('Ruthanne','Bamb','84948 Mesta Junction','3105500146',6),
('Gerrard','Lowin','370 Talisman Plaza','3324874824',2),
('Merrile','Lambdin','81 Dahle Plaza','4373065154',5),
('Bert','Ivie','2 Gateway Circle','4409584510',4)

INSERT INTO Subjects ([Name],Lessons)
VALUES
('Geometry',12),
('Health',10),
('Drama',7),
('Sports',9)
--03.Update
UPDATE StudentsSubjects
SET Grade = 6.00
WHERE (Grade > 5.50 OR Grade = 5.50) AND SubjectId IN (1, 2)
--04.Delete
DELETE FROM StudentsTeachers
WHERE TeacherId IN (SELECT Id FROM Teachers WHERE Phone LIKE '%72%')

DELETE FROM Teachers
WHERE Phone LIKE '%72%'
--5. Teen Students
SELECT FirstName,LastName,Age 
FROM Students
WHERE AGE >= 12
ORDER BY FirstName,LastName
--06. Cool Addresses
SELECT CONCAT(FirstName,' ',MiddleName,' ',LastName) AS [Full Name],[Address]
FROM Students
WHERE [Address] LIKE '%Road%'
ORDER BY FirstName,LastName
--07. 42 Phones
SELECT FirstName,[Address],Phone
FROM Students
WHERE MiddleName IS NOT NULL AND Phone LIKE '42%'
ORDER BY FirstName
--08. Students Teachers
SELECT r.FirstName,r.LastName,COUNT(r.Id) AS [Teachers Count]
FROM(
SELECT s.FirstName,s.LastName,t.Id
FROM StudentsTeachers AS st
JOIN Students AS s ON s.Id = st.StudentId
JOIN Teachers AS t ON t.Id = st.TeacherId ) AS r
GROUP BY r.FirstName,r.LastName
--09. Subjects with Students
WITH CTE_TeachersStudents
AS(
SELECT r.Id,r.FirstName,r.LastName,COUNT(r.StudentId) AS [Students]
FROM
(SELECT t.Id,t.FirstName,t.LastName,st.StudentId
FROM Teachers AS t
JOIN StudentsTeachers AS st ON t.Id = st.TeacherId) AS r
GROUP BY r.FirstName,r.LastName,r.Id)

SELECT CONCAT(t.FirstName,' ',t.LastName) AS [Full Name],CONCAT(s.[Name],'-',s.Lessons) AS [Subjects],cte.Students
FROM Teachers AS t
JOIN Subjects AS s ON s.Id = t.SubjectId
JOIN CTE_TeachersStudents AS cte ON cte.Id = t.Id
ORDER BY cte.Students DESC,[Full Name] ASC,Subjects ASC 
--10. Students to Go
SELECT CONCAT(s.FirstName,' ',s.LastName) AS [Full Name]
FROM Students AS s
LEFT JOIN StudentsExams AS se ON se.StudentId = s.Id
WHERE se.Grade IS NULL
ORDER By [Full Name]
--11. Busiest Teachers
SELECT TOP 10 r.FirstName,r.LastName,COUNT(r.StudentId) AS [Students]
FROM
(SELECT t.FirstName,t.LastName,st.StudentId
FROM Teachers AS t
JOIN StudentsTeachers AS st ON t.Id = st.TeacherId) AS r
GROUP BY r.FirstName,r.LastName
ORDER BY Students DESC,r.FirstName,r.LastName
--12. Top Students
SELECT TOP 10 r.FirstName,r.LastName,CAST(AVG(r.Grade) AS DECIMAL(3,2)) AS [Grade]
FROM(
SELECT s.FirstName,s.LastName,se.Grade
FROM Students AS s
JOIN StudentsExams AS se ON s.Id = se.StudentId ) AS r
GROUP BY r.FirstName,r.LastName
ORDER BY Grade DESC,r.FirstName,r.LastName
--13. Second Highest Grade
SELECT r.FirstName,r.LastName,r.Grade
FROM(
SELECT 
	s.FirstName,
	s.LastName,
	se.Grade,
	ROW_NUMBER () OVER(PARTITION BY s.FirstName,s.LastName ORDER BY se.Grade DESC) AS [GradeRank]
FROM Students AS s
JOIN StudentsSubjects AS se ON se.StudentId = s.Id ) AS r
WHERE r.GradeRank = 2
ORDER BY r.FirstName,r.LastName
--14. Not So In The Studying
SELECT s.FirstName,s.LastName--,sub.[Name]
 FROM Students AS s
LEFT JOIn StudentsSubjects AS ss ON ss.StudentId = s.Id
LEFT JOIN Subjects AS sub ON sub.Id = ss.SubjectId

SELECT s.FirstName + ISNULL(' ' + s.MiddleName + ' ',' ') + s.LastName AS [Full Name] 
FROM Students AS s
FULL JOIN StudentsSubjects AS ss ON ss.StudentId = s.Id
WHERE ss.SubjectId IS NULL
ORDER BY [Full Name]
--15. Top Student per Teacher
WITH CTE_cte
AS (
SELECT r.[Teacher Full Name],r.[Subject],r.[Student Full Name],r.Grade
FROM
(SELECT 
	CONCAT(t.FirstName,' ',t.LastName) AS [Teacher Full Name],
	CONCAT(stu.FirstName,' ',stu.LastName) AS [Student Full Name],
	CAST(AVG(stusub.Grade) AS DECIMAL(3,2)) AS [Grade] ,subj.[Name]  AS [Subject]
FROM Teachers AS t
JOIN StudentsTeachers AS stut ON stut.TeacherId = t.Id
JOIN Students AS stu ON stu.Id = stut.StudentId
JOIN StudentsSubjects AS stusub ON stusub.StudentId = stu.Id
JOIN Subjects AS subj ON subj.Id = stusub.SubjectId AND t.SubjectId = subj.Id
 GROUP BY t.FirstName,t.LastName,stu.FirstName,stu.LastName,subj.[Name]) AS r )

 SELECT result.[Teacher Full Name],result.[Subject],result.[Student Full Name],result.Grade
 FROM (
 SELECT
	[Teacher Full Name],
	[Subject],
	[Student Full Name],
	Grade,
	DENSE_RANK () OVER (PARTITION BY [Teacher Full Name] ORDER BY Grade DESC ) AS [Ranking]
	FROM CTE_cte  
	GROUP BY [Teacher Full Name],[Subject],[Student Full Name],Grade ) AS result
	WHERE result.Ranking = 1
	ORDER BY result.[Subject],result.[Teacher Full Name],result.Grade DESC
--16. Average Grade per Subject
SELECT result.[Name],result.[Grade]
FROM
(SELECT subj.Id,[Name],AVG(subjstud.Grade) AS [Grade]
FROM Subjects AS subj
JOIN StudentsSubjects AS subjstud ON subjstud.SubjectId = subj.Id
GROUP BY subj.[Name],subj.Id ) AS result
ORDER BY result.Id
--17. Exams Information

SELECT result.Quarter,result.[Name],COUNT(result.Grade) AS [Students Count]
	FROM(
SELECT CASE
        WHEN MONTH([Date]) >= 1 AND MONTH([Date]) <=3
        THEN
            'Q1'
        WHEN MONTH([Date]) >= 4 AND MONTH([Date]) <=6
        THEN
            'Q2'
        WHEN MONTH([Date]) >= 7 AND MONTH([Date]) <=9
        THEN
            'Q3'
        WHEN MONTH([Date]) >= 10 AND MONTH([Date]) <=12
        THEN
            'Q4'
		WHEN [DATE] IS NULL
		THEN
			'TBA'
    END 'Quarter'
	,subj.[Name],
	stue.Grade
FROM Exams AS e
	JOIN StudentsExams AS stue ON stue.ExamId = e.Id
	JOIN Subjects AS subj ON subj.Id = e.SubjectId
WHERE stue.Grade >= 4) AS result
GROUP BY result.Quarter,result.[Name]
ORDER BY result.Quarter
--18. Exam Grades
CREATE FUNCTION udf_ExamGradesToUpdate(@studentId INT, @grade DECIMAL(3,2))
RETURNS VARCHAR(100)
AS
BEGIN

	DECLARE @idCheck INT = (SELECT Id FROM Students WHERE Id = @studentId);

		IF(@idCheck IS NULL)
			BEGIN
				RETURN ('The student with provided id does not exist in the school!')
			END

		IF(@grade + 0.5 > 6)
			BEGIN
				RETURN ('Grade cannot be above 6.00!')
			END

	DECLARE @upperGrade DECIMAL(3,2) = @grade + 0.5;

	DECLARE @gradesCount INT = (SELECT COUNT(Grade)
										FROM StudentsExams
										WHERE StudentId = @studentId AND (Grade >= @grade AND Grade <= @upperGrade))

	DECLARE @studentName NVARCHAR(50) = (SELECT FirstName FROM Students WHERE Id = @studentId)

	RETURN 'You have to update ' + CAST(@gradesCount AS VARCHAR(100)) + ' grades for the student ' + @studentName;
	
END 
--19. Exclude From School
CREATE PROC usp_ExcludeFromSchool(@StudentId INT)
AS
BEGIN
	DECLARE @idCheck INT = (SELECT Id FROM Students WHERE Id = @StudentId);
	IF(@idCheck IS NULL)
		BEGIN
			RAISERROR('This school has no student with the provided id!',16,1)
			RETURN
		END

	
DELETE FROM StudentsExams
WHERE StudentId = @StudentId

DELETE FROM StudentsSubjects
WHERE StudentId = @StudentId

DELETE FROM StudentsTeachers
WHERE StudentId = @StudentId

DELETE FROM Students
WHERE Id = @StudentId

END
--20. Deleted Students
CREATE TABLE ExcludedStudents(
StudentId INT NOT NULL,
StudentName NVARCHAR(50) NOT NULL
)
CREATE TRIGGER tr_StudentsDelete ON Students
INSTEAD OF DELETE
AS
INSERT INTO ExcludedStudents(StudentId, StudentName)
		SELECT Id, CONCAT(FirstName,' ',LastName) FROM deleted

 














