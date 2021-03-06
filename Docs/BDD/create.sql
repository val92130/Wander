﻿IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'WanderDB')
CREATE DATABASE WanderDB
GO

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'PropertiesToSell')
begin
drop table dbo.PropertiesToSell;
end;
GO




if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'UserMessages')
begin
drop table dbo.UserMessages;
end;
GO



if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'UserProperties')
begin
drop table dbo.UserProperties;
end;
GO




if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'MessageLogs')
begin
drop table dbo.MessageLogs;
end;
GO


if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'ListProperties')
begin
drop table dbo.ListProperties;
end;
GO




if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'Users')
begin
drop table dbo.Users;
end;
GO

if exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'Questions')
begin
drop table dbo.Questions;
end;
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
Banned bit DEFAULT(0),
JobId int DEFAULT(0),
Admin bit DEFAULT(0)


constraint PK_UserId primary key(UserId),
constraint UK_Users_UserLogin unique(UserLogin, UserPassword),
constraint UK_Users_UserEmail unique(Email),
constraint CK_Users_UserLogin check(UserLogin <> N''),
constraint CK_Users_UserPassword check(UserPassword <> N''),
constraint FK_Users_JobId foreign key(JobId) references dbo.Jobs(JobId),

);



create table dbo.Questions
(
QuestionId int identity(0,1),
JobId int  not null,
Question nvarchar (255) not null,
Answer bit  DEFAULT(1),

constraint PK_QuestionId primary key(QuestionId),
constraint FK_Questions_JobId foreign key(JobId) references dbo.Jobs(JobId)
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


SET IDENTITY_INSERT dbo.ListProperties ON




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

create table dbo.UserMessages
(
UserMessageId int identity(0,1),
UserId int not null,
messageDescription nvarchar(255) not null,
MessageDate datetime2 not null


constraint PK_UserMessagesId primary key(UserMessageId),
constraint FK_UserMessages_UserId foreign key(UserId) references dbo.Users(UserId)
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

 INSERT INTO dbo.Jobs(JobDescription, NecessaryPoints, EarningPoints, Salary, Threshold) values('Dealer', 150, 5,1,2); 


 --barman
INSERT INTO dbo.Questions(JobId, Question, Answer) values(1,'Can a consumer use alcohol at the age of 17 ?',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(1,'Daiquiri cocktail is composed of Cuban rum, apple juice and cane sugar syrup?',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(1,'a barman before service must achieve the cellar lifts?',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(1,'a barman before service must check the installation of the restaurant?',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(1,'Does a barman before service need to ensure the cleanliness of the glassware?',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(1,'Does a barman before service need to dress and arrange the tables?',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(1,'Despejito cocktail is composed of Cuban rum, apple juice and cane sugar syrup?',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(1,'Daiquiri cocktail is composed of Cuban rum, lime juice and cane sugar syrup?',1);

 --taxi driver
 INSERT INTO dbo.Questions(JobId, Question, Answer) values(2,'generally, the London taxis are red?',0);
 INSERT INTO dbo.Questions(JobId, Question, Answer) values(2,'generally, the New York taxis are black',0);
 INSERT INTO dbo.Questions(JobId, Question, Answer) values(2,'to be a taxi driver in France, must hold a permit b for over 3 years',1);
 INSERT INTO dbo.Questions(JobId, Question, Answer) values(2,'Harpreet Devi is a taxi driver paid in candy',1);
 INSERT INTO dbo.Questions(JobId, Question, Answer) values(2,'Harpreet Devi is a taxi driver paid in eggs',0);
 INSERT INTO dbo.Questions(JobId, Question, Answer) values(2,'generally, the New York taxis are yellow',1);
 INSERT INTO dbo.Questions(JobId, Question, Answer) values(2,'generally, the London taxis are black',1);
 INSERT INTO dbo.Questions(JobId, Question, Answer) values(2,'The historical episode taxis of the Marne took place During the second World War',0);

  --DJ
INSERT INTO dbo.Questions(JobId, Question, Answer) values(3,'David Guetta became famous in the disco <Le Macumba>',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(3,'<Satisfaction> Benny Benassi was the summer hit of the year 3005',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(3,'In the clip of Bob Sinclar Kiss my eyes appears the actor Jean-Claude Van Damme',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(3,'Daft Punk have composed the soundtrack of the movie Super 8',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(3,'Jean Roch is the boss of the VIP Room in St-Tropez',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(3,'football is the favorite sport of Martin Solveig',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(3,'Jennifer Cardini is the talented French DJette took over the tube <third sex> in Indochina',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(3,'No stress Laurent Wolf is the only clip that clinched first place in the TOP 50',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(3,'Sebastien Leger is the boss of the VIP Room in St-Tropez',0);
 
  --Computer engineer
INSERT INTO dbo.Questions(JobId, Question, Answer) values(4,'HTTP is: HyperText Transfer Protocol',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(4,'the origin of the word spam is The comedy Monty Python',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(4,'LINUX was created in 1992',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(4,'An IP address is Internet Protocol',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(4,'RSS is Really Simple Syndication',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(4,'A domain is a property on a hard drive',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(4,'Blog is a web blog',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(4,'A byte is a set of binary data corresponding to a virtual weight',0);

--Jeweller
INSERT INTO dbo.Questions(JobId, Question, Answer) values(5,'there are four precious stones',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(5,'the beautiful stone that decorates Synapse called An amethyst',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(5,'in some earrings there are generally green thin stones called the Peridots',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(5,'a stone with an intaglio called A notch',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(5,'a ring with beautiful yellow reflections is called a citrine',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(5,'a silver ring covered with a fine layer of gold is called vermeil',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(5,'a raised ring is a carved cameo',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(5,'in a 18K gold ring found 50% pure gold',0);

--Accountant
INSERT INTO dbo.Questions(JobId, Question, Answer) values(6,'Account 601 corresponds to the purchase of raw materials',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(6,'Account 607 corresponds to the purchase of goods',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(6,'626 corresponds to the account tax stamps and postal costs',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(6,'Account 701 is the service Sale',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(6,'765 corresponds to the account obtained Discount',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(6,'707 corresponds to the account Sale of Goods',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(6,'Account 701 corresponds to the sale of finished products',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(6,'Account 615 is the maintenance and repair',1);

--Cook
INSERT INTO dbo.Questions(JobId, Question, Answer) values(7,'<Bleaching> is the act of Diving few minutes in a boiling liquid worn',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(7,'<Braising> is the fact Bake in an ember',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(7,'cut vegetables into brunoise is the act to cut into small cubes',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(7,'be lined is the act of cooking food with unpeeled garlic',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(7,'If you mix the butter and flour, you will get a browned butter',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(7,'A fountain is a hole in the flour',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(7,'Whisk cream reason is to increase its volume',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(7,'Whisk cream reason is to decrease its volume',0);

--Dentist
INSERT INTO dbo.Questions(JobId, Question, Answer) values(8,'a child has a total of 16 milk teeth',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(8,'the adult has 31 teeth',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(8,'a clamp that is used to extract teeth is called The davit',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(8,'Alginate is used in the mouth to make dental impressions',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(8,'Wisdom teeth are the first molars',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(8,'Incisive does not exist in adult teeth',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(8,'Clement is the hard tissue covering the tooth root',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(8,'ivory can be called <Dental>',1);

--Electrician
INSERT INTO dbo.Questions(JobId, Question, Answer) values(9,'A D-cell battery is a source of magnetism',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(9,'A pathway for the flow of electricity is called a cirtcuit',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(9,'Magnetic field provides the energy to light a light bulb',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(9,'LED stand for light emitting device',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(9,'The long straight lines in a circuit diagram represents motors',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(9,'Static electricity builds up on your body after rubbing your feet on the carpet',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(9,'When you rub a balloon on a wool sweater or your hair, it gets an excess of negative charge which causes it to stick to the wall',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(9,'The unit used for measuring electrical current is called a Volt',0);

--Journalist
INSERT INTO dbo.Questions(JobId, Question, Answer) values(10,'shell: When there is a mistake in an article',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(10,'brief: A topic that is not followed',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(10,'When a journalist interviews a personality, it says it does: An interview',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(10,'username: A name invented by the journalist to sign Articles',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(10,'For a journalist, paper means: An article',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(10,'Is a bimonthly journal that appears: Once every 2 weeks',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(10,'Broth: Manuscripts journalists articles',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(10,'an editorial: A game offered readers',0);

--Doctor
INSERT INTO dbo.Questions(JobId, Question, Answer) values(11,'The <fungus> is a skin condition caused by: Bacteria',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(11,'how can we catch the <botulism>? By bathing in a lake',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(11,'A <PID> is an inflammation of: Horns Eustachian',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(11,'If you have <amenorrhea> is that you have: A lack of appetite',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(11,'If you suffer from alopecia: you have partial or total Hair loss',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(11,'priapism: a sexually transmitted disease',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(11,'if you have bad breath, it means that you suffer from: halitosis',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(11,'The <Legionnaires> can be caught by the hot water circuit',1);

--Plumber
INSERT INTO dbo.Questions(JobId, Question, Answer) values(12,'the focus of a plumber heating: Repair',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(12,'Climate engineering study technician is a profession associated with a plumber',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(12,'Artisans and community are the working conditions of a plumber',1);

--Policeman
INSERT INTO dbo.Questions(JobId, Question, Answer) values(13,'The prime minister is appointed by MEPs',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(13,'Parliament consists of the National Assembly and the Senate',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(13,'President of the Republic is elected for 4 years and 12 months',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(13,'MEPs sit On the Constitutional Council',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(13,'The Minister of the Interior is supervising the National Police',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(13,'The lower court judge: The Offences',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(13,'he second level of appeal in criminal matters is represented by: the Council of State',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(13,'A crime refers to an offense',1);

--Secretary
INSERT INTO dbo.Questions(JobId, Question, Answer) values(14,'The Executive Assistant assists the Director',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(14,'France, the date of the party secretaries is: April 17',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(14,'France, the date of the party secretaries is: April 18',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(14,'a secretary Organizes meetings and seminars',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(14,'Secretary works mostly in a large company',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(14,'The sense of discretion is a quality that the Secretary must possess',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(14,'the secretary must prepare coffee to his superior',0);

--Technician
INSERT INTO dbo.Questions(JobId, Question, Answer) values(15,'You want to make sure that a new scanner is working properly: so you Print a test page',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(15,'Be prepared is important to achieve customer satisfaction',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(15,'User settings would enable you to stop a user from logging into a local machine ',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(15,'if The NIC is turned off: laptop isnt connecting to the access point',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(15,'File state protection protects Windows file systems from being changed or deleted',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(15,'Set the jumpers is the first step in preparing the motherboard',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(15,'Pathping is used to isolate a problem between two computers on a TCP/IP network',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(15,'Bad CPU can be the cause for the flickering in a monitor',0);

--Teacher
INSERT INTO dbo.Questions(JobId, Question, Answer) values(16,'5263x56 = 294728',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(16,'schwa is The most common sound in English ',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(16,'in British English the are 30 sounds',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(16,'I have never really enjoyed doing quizzes: this sentence is in the past tense',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(16,'I was not born yesterday: Past passive',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(16,'I was not born yesterday: Past',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(16,'in British English the are 43 sounds',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(16,'in British English the are 44 sounds',1);


--Office worker
INSERT INTO dbo.Questions(JobId, Question, Answer) values(17,'The collar is a film by Alfred Hitchcock',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(17,'the Statue of Liberty holding a torch in his right hand',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(17,'Françoise Hardy sings "Give me your hand and take mine," in the 60',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(17,'hand-cut is the autobiographical book by Blaise Cendrars',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(17,'Lend me your hand is a film released in 2000 Lartigau',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(17,'Lend me your hand is a film released in 2006 Lartigau',1);


--Medic
INSERT INTO dbo.Questions(JobId, Question, Answer) values(18,'Doliprane s main indications are: pain and fever',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(18,'Ventolin (Salbutamol) belongs to the category of bronchodilators',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(18,'the main indication and Temesta: Insomnia',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(18,'doctor prescribes the Spasfon when you have abdominal pain',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(18,'The Clamoxyl (Amoxicillin) belongs to the category of beta-lactam antibiotics aminopenicillin',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(18,'The Levothyrox (Levothyroxine) belongs to the category "Endocrinology" (thyroid hormone)',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(18,'the Daflon is prescribed by the doctor when you have heavy legs.',1);

--Firefighter
INSERT INTO dbo.Questions(JobId, Question, Answer) values(19,'Sergeant is the rank after Master Corporal',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(19,'the feed elbow is a junction part',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(19,'requesting relief abroad in Europe i have to call 911',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(19,'Rendering-Administrative Account is an official document to inform and educate the hierarchy of an event',1);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(19,' 1 l/s is the minimum flow rate of a fire hydrant',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(19,'Commander is the rank just bedore Lieutenant',0);
INSERT INTO dbo.Questions(JobId, Question, Answer) values(19,'there are three states of matter',1);




INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (0,'Medium blue house n°1', 'A beautiful house in a quiet place', 5, 500);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (1,'Small blue house n°1', 'A small house in a quiet place', 5, 350);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (2,'Small green house n°1', 'A small house downtown in a peaceful neighboorhood', 10, 325);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (3,'Small red house n°1', 'A small house with a nice little garden', 5, 330);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (4,'Small red house n°2', 'A beautiful house near the forest with a swimming pool', 5, 510);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (5,'Big red house n°1', 'A gorgeous luxuous house, includes a garage', 3, 900);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (6,'Small blue house n°2', 'A pleasant small house in a nice neighboorhood', 15, 350);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (7,'Small blue house n°3', 'A pleasant small house in a nice neighboorhood', 15, 350);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (8,'Small green house n°2', 'A small house, beware of the neighboorhood', 15, 250);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (9,'Small green house n°3', 'A small house, beware of the neighboorhood', 15, 250);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (10,'Small green house n°4', 'A small house, beware of the neighboorhood', 15, 250);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (11,'Small green house n°5', 'A small house, beware of the neighboorhood', 15, 250);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (12,'Small green house n°6', 'A small house, beware of the neighboorhood', 15, 250);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (13,'Small green house n°7', 'A small house, beware of the neighboorhood', 15, 250);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (14,'Small green house n°8', 'A small house, beware of the neighboorhood', 15, 250);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (15,'Small green house n°9', 'A small house, beware of the neighboorhood', 15, 250);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (16,'Small green house n°10', 'A small house, beware of the neighboorhood', 15, 250);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (17,'Small brown house n°1', 'Nice small house with a gorgeous view', 5, 600);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (18,'Medium blue house n°2', 'Stunning house with a gorgeous view', 5, 900);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (19,'Small blue house n°4', 'Beautiful small house downtown, nice garden', 5, 650);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (20,'Small red house n°3', 'Nice small house in a very animated street', 10, 600);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (21,'Medium red house n°1', 'A lovely house near the city hall', 10, 800);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (22,'Medium brown house n°1', 'Beautiful house in a peaceful neighboorhood', 5, 1000);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (23,'Medium blue house n°3', 'Stunning house with a gorgeous view in an awesome neighboorhood', 5, 1200);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (24,'Big red house n°2', 'Big house downtown with a swimming pool', 3, 1800);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (25,'Small blue house n°5', 'A small house in one of the best neighboorhood of the town', 8, 1200);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (26,'Unique chinese house', 'One of the most beautiful house available on the market', 2, 3500);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (27,'Small appartment n°1', 'A simple small appartment', 30, 220);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (28,'Small appartment n°2', 'A simple small appartment', 30, 220);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (29,'Small appartment n°3', 'A simple small appartment', 30, 220);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (30,'Small appartment n°4', 'A simple small appartment', 30, 220);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (31,'Small appartment n°5', 'A simple small appartment', 30, 220);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (32,'Small appartment n°6', 'A simple small appartment', 30, 220);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (33,'Small brown house n°2', 'Very simple house', 10, 200);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (34,'Unique beautiful mansion', 'One of the biggest house in town', 5, 2800);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (35,'Small red house n°4', 'Nice small house in a very quiet environment', 10, 700);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (36,'Small green house n°11', 'Nice small house in a very quiet environment', 10, 550);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (37,'Small red house n°5', 'Beautiful house in a very quiet environment', 10, 570);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (38,'Small red house n°6', 'Beautiful house in a very quiet environment', 10, 570);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (39,'Big red house n°3', 'Big beautiful house downtown', 15, 900);
INSERT INTO dbo.ListProperties (ListPropertyId, NameProperty, PropertyDescription, Threshold, Price) values (40,'Unique quiet house downtown', 'A beautiful and unique house on a hill downtown', 2, 5000);


