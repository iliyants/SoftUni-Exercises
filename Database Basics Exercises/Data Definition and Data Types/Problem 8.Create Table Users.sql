CREATE TABLE Users (
Id BIGINT PRIMARY KEY IDENTITY(1,1),
Username VARCHAR(30) UNIQUE NOT NULL,
[Password] VARCHAR(26) NOT NULL,
ProfilePicture VARBINARY(max)
CHECK(DATALENGTH (ProfilePicture) <= 921600),
LastLoginTime DATETIME,
IsDeleted BIT
)


INSERT INTO Users ( Username, [Password], ProfilePicture, IsDeleted)
VALUES ( 'Ivan', '4frysfs', Null, 1),
		( 'Ivana', 'ete5325', Null, 1),
		( 'Stoqn', 'ygfh3255', Null, 1),
		( 'Gosho', 'nu6436ll', Null, 0),
		( 'Pencho', 'nyteyuell', Null, 0)

ALTER TABLE Users
DROP CONSTRAINT [PK__Users__3214EC07B560CE40]

ALTER TABLE Users
ADD CONSTRAINT PK_Users PRIMARY KEY (Id, Username)


ALTER TABLE Users
ADD CONSTRAINT PasswordLength 
CHECK (DATALENGTH([Password]) > 5)

INSERT INTO Users ( Username, [Password], ProfilePicture, IsDeleted)
VALUES ( 'Trutka', '4frssss', Null, 1)

ALTER TABLE Users
ADD DEFAULT GETDATE()
FOR LastLoginTime

SELECT * FROM Users

INSERT INTO Users ( Username, [Password], ProfilePicture, IsDeleted)
VALUES ( 'PUTKAA', '4frssss', Null, 1)

ALTER TABLE Users
DROP CONSTRAINT [CK__Users__ProfilePi__6D0D32F4]


