INSERT INTO dbo.ListProperties (NameProperty, PropertyDescription, Threshold, Price) values ('test1', 'description1', 2, 500);
INSERT INTO dbo.ListProperties (NameProperty, PropertyDescription, Threshold, Price) values ('test2', 'description2', 2, 300);
INSERT INTO dbo.ListProperties (NameProperty, PropertyDescription, Threshold, Price) values ('test3', 'description3', 2, 150);
INSERT INTO dbo.ListProperties (NameProperty, PropertyDescription, Threshold, Price) values ('test4', 'description4', 2, 3000);
INSERT INTO dbo.ListProperties (NameProperty, PropertyDescription, Threshold, Price) values ('test5', 'description5', 2, 250);

INSERT INTO dbo.Users(UserLogin, UserPassword, Email, Sex, Account, Points, Connected, Activated, JobId) values ('testLogin', 'test','test @gmail.com',1,0,0,1,0,0);

INSERT INTO dbo.ListProperties (NameProperty, PropertyDescription, Threshold, Price) values ('test5', 'description5', 2, 250);

SELECT l.ListPropertyId, l.NameProperty, l.PropertyDescription, l.Threshold, l.Price
from dbo.UserProperties p
JOIN dbo.Users u on p.UserId = u.UserId
JOIN dbo.ListProperties l ON l.ListPropertyId = p.UserPropertyId
WHERE u.UserId = 0

INSERT INTO dbo.UserProperties (UserId, ListPropertyId) values (0, 0);
SELECT COUNT (u.ListPropertyId) as count, l.Threshold FROM dbo.ListProperties l JOIN UserProperties u ON u.ListPropertyId = l.ListPropertyId WHERE u.ListPropertyId = 0 GROUP BY Threshold;