--Record`s Count
SELECT COUNT(*) AS [Count] FROM WizzardDeposits;
--Longest Magic Wand
SELECT MAX(MagicWandSize) AS LongestMagicWand FROM WizzardDeposits
--Longest Magic Wand Per Deposit Groups
SELECT DepositGroup,MAX(MagicWandSize) AS LongestMagicWand
FROM WizzardDeposits
GROUP BY DepositGroup
--Smallest Deposit Group per Magic Wand Size
SELECT TOP 2 DepositGroup
FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY AVG(MagicWandSize)
--Deposit Sum
SELECT DepositGroup,SUM(DepositAmount) AS [TotalSum]
FROM WizzardDeposits
GROUP BY DepositGroup
--Deposit Sum for Ollivander Family
SELECT DepositGroup,SUM(DepositAmount) AS [TotalSum]
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
--Deposit Filter
SELECT DepositGroup,SUM(DepositAmount) AS [TotalSum]
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
ORDER BY [TotalSum] DESC
--Deposit Charge
SELECT DepositGroup,MagicWandCreator,MIN(DepositCharge) AS [MinDepositCharge]
FROM WizzardDeposits
GROUP BY DepositGroup,MagicWandCreator
--Age Groups
SELECT
CASE
WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
WHEN Age >=61 THEN '[61+]'
END AS [AgeGroup],
COUNT(*) AS [WizzardCount]
FROM WizzardDeposits
GROUP By CASE
WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
WHEN Age >=61 THEN '[61+]'
END
--First Letter without GROUP BY
SELECT DISTINCT(SUBSTRING(FirstName,1,1)) AS [FirstLetter]
FROM WizzardDeposits
WHERE DepositGroup = 'Troll Chest'
ORDER BY [FirstLetter]
--First Letter with GROUP BY
SELECT LEFT(FirstName,1) AS [FirstLetter]
FROM WizzardDeposits
WHERE DepositGroup = 'Troll Chest'
GROUP BY LEFT([FirstName], 1)
--Average Interest
SELECT DepositGroup,IsDepositExpired,AVG(DepositInterest) AS [AverageInterest]
FROM WizzardDeposits
WHERE DepositStartDate > '01-01-1985'
GROUP BY DepositGroup,IsDepositExpired
ORDER BY DepositGroup DESC,IsDepositExpired ASC
--Rich Wizzard, Poor Wizzard
 SELECT SUM(DepositAmountsTable.[Difference]) AS SumDifference
 FROM (SELECT DepositAmount - LEAD(DepositAmount, 1) OVER (ORDER BY Id) AS [Difference]
	   FROM WizzardDeposits) AS DepositAmountsTable
--Departments Total Salaries



USE SoftUni