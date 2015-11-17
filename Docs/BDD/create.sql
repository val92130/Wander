use master
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'WanderDB')
DROP DATABASE WanderDB
GO

CREATE DATABASE WanderDB
GO

use WanderDB
GO

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'Jobs')
begin
drop table dbo.Jobs;
end;
GO

create table dbo.Jobs
(
JobId int identity(0,1),
JobDescription nvarchar(255) not null,
EarningPoints int DEFAULT(0),
NecessaryPoints int DEFAULT(0),
Salary int not null,
Threshold int not null


constraint PK_JobId primary key(JobId),
constraint CK_Jobs_JobDescription check(JobDescription <> N'')
);

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'Users')
begin
drop table dbo.Users;
end;
GO

create table dbo.Users
(
UserId int identity(0,1),
UserLogin nvarchar(32) not null,
UserPassword nvarchar(32) not null,
Email varchar(30) not null,
Sex int,
Account int,
Points int,
Connected bit,
Activated bit,
JobId int DEFAULT(0)


constraint PK_UserId primary key(UserId),
constraint UK_Users_UserLogin unique(UserLogin, UserPassword),
constraint UK_Users_UserEmail unique(Email),
constraint CK_Users_UserLogin check(UserLogin <> N''),
constraint CK_Users_UserPassword check(UserPassword <> N''),
constraint FK_Users_JobId foreign key(JobId) references dbo.Jobs(JobId),

);

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'ListProperties')
begin
drop table dbo.ListProperties;
end;
GO


create table dbo.ListProperties
(
ListPropertyId int identity(0,1),
NameProperty nvarchar(60) not null,
PropertyDescription nvarchar(255) not null,
Threshold int not null,
Price int not null


constraint PK_ListPropertyId primary key(ListPropertyId),
constraint CK_ListProperties_ListPropertiesThreshold check(Threshold <> N'')

);

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'PropertiesToSell')
begin
drop table dbo.PropertiesToSell;
end;
GO


create table dbo.PropertiesToSell
(
PropertyToSellId int identity(0,1),
UserId int not null ,
ListPropertyId int not null,
Price int not null

constraint PK_PropertiesToSellId primary key(PropertyToSellId),
constraint FK_PropertiesToSell_UserId foreign key(UserId) references dbo.Users(UserId) ON UPDATE CASCADE ON DELETE CASCADE,
constraint FK_PropertiesToSell_ListPropertyId foreign key(ListPropertyId) references dbo.ListProperties(ListPropertyId)

);


if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'UserProperties')
begin
drop table dbo.UserProperties;
end;
GO


create table dbo.UserProperties
(
UserPropertyId int identity(0,1),
UserId int not null,
ListPropertyId int not null


constraint PK_UserPropertiesId primary key(UserPropertyId),
constraint FK_UserProperties_UserId foreign key(UserId) references dbo.Users(UserId)  ON UPDATE CASCADE ON DELETE CASCADE,
constraint FK_UserProperties_ListPropertyId foreign key(ListPropertyId) references dbo.ListProperties(ListPropertyId)
);


if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'UserMessages')
begin
drop table dbo.UserMessages;
end;
GO


create table dbo.UserMessages
(
UserMessageId int identity(0,1),
UserId int not null,
messageDescription nvarchar(255) not null,
MessageDate datetime2 not null


constraint PK_UserMessagesId primary key(UserMessageId),
constraint FK_UserMessages_UserId foreign key(UserId) references dbo.Users(UserId)
);

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'MessageLogs')
begin
drop table dbo.MessageLogs;
end;
GO

create table dbo.MessageLogs
(
		MessageId int identity(0,1),
		UserId int not null,
		Message nvarchar(100) not null,
		Time datetime2

constraint PK_MessageId primary key(MessageId),
constraint FK_UserIdMessage foreign key(UserId) references dbo.Users(UserId) 
)



INSERT INTO dbo.Jobs(JobDescription, Salary, Threshold, EarningPoints, NecessaryPoints) values('unemployed', 0, 0,2,0);

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Barman', 50, 5,70,100); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Taxi Driver', 200, 10,20,4); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Dj', 300, 15,130,2); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Computer engineer', 500, 30,150,20); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Jeweller', 100, 7,85,10); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Accountant', 500, 25,60,5); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Cook', 100, 7,70,50); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Dentist', 500, 35,130,40); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Electrician', 70, 5,90,100); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Journalist', 250, 12,100,20); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Doctor', 600, 35,180,10); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Plumber', 70, 5,90,100); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Policeman', 150, 8,95,30); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Secretary', 20, 3,60,250); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Technician', 250, 12,110,25); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Teacher', 80, 6,85,35); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Office worker', 80, 6,75,60); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Medic', 550, 30,120,15); 

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Firefighter', 450, 26,120,20); 

