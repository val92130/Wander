if not exists(select * from sys.databases where name = 'WanderDB')
begin
create database WanderDB
end;
GO

use WanderDB;
GO




if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'PropertiesToSell')
begin
drop table dbo.PropertiesToSell;
end;
GO

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'ListProperties')
begin
drop table dbo.ListProperties;
end;
GO

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'Properties')
begin
drop table dbo.Properties;
end;
GO

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'UserProperties')
begin
drop table dbo.UserProperties;
end;
GO

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'UserMessages')
begin
drop table dbo.UserMessages;
end;
GO

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'Jobs')
begin
drop table dbo.Jobs;
end;
GO

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'Users')
begin
drop table dbo.Users;
end;
GO

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'MessageLogs')
begin
drop table dbo.MessageLogs;
end;
GO


create table dbo.Jobs
(
JobId int identity(0,1),
JobDescription nvarchar(255) not null,
Salary int not null,
Threshold int not null


constraint PK_JobId primary key(JobId),
constraint CK_Jobs_JobDescription check(JobDescription <> N'')
);
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

create table dbo.PropertiesToSell
(
PropertyToSellId int identity(0,1),
UserId int not null,
ListPropertyId int not null,
Price int not null

constraint PK_PropertiesToSellId primary key(PropertyToSellId),
constraint FK_PropertiesToSell_UserId foreign key(UserId) references dbo.Users(UserId),
constraint FK_PropertiesToSell_ListPropertyId foreign key(ListPropertyId) references dbo.ListProperties(ListPropertyId)

);




create table dbo.UserProperties
(
UserPropertyId int identity(0,1),
UserId int not null,
ListPropertyId int not null


constraint PK_UserPropertiesId primary key(UserPropertyId),
constraint FK_UserProperties_UserId foreign key(UserId) references dbo.Users(UserId)  ON UPDATE CASCADE ON DELETE CASCADE,
constraint FK_UserProperties_ListPropertyId foreign key(ListPropertyId) references dbo.ListProperties(ListPropertyId)
);

create table dbo.UserMessages
(
UserMessageId int identity(0,1),
UserId int not null,
messageDescription nvarchar(255) not null,
MessageDate datetime2 not null


constraint PK_UserMessagesId primary key(UserMessageId),
constraint FK_UserMessages_UserId foreign key(UserId) references dbo.Users(UserId)
);


create table dbo.MessageLogs
(
		MessageId int identity(0,1),
		UserId int not null,
		Message nvarchar(100) not null,
		Time datetime2

constraint PK_MessageId primary key(MessageId),
constraint FK_UserIdMessage foreign key(UserId) references dbo.Users(UserId) 
)


INSERT INTO dbo.Jobs(JobDescription, Salary, Threshold) values('unemployed', 0, 0);


