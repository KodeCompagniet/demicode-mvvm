DELETE FROM [dbo].[WorkItem]
DELETE FROM [dbo].[Project]
DELETE FROM [dbo].[Employee]

SET IDENTITY_INSERT [dbo].[Employee] ON
INSERT INTO [dbo].[Employee] ([Id], [UserName], [FirstName], [LastName], [Title])
VALUES (1, N'CORP\arjan', N'Arjan', N'Einbu', N'Manager')
INSERT INTO [dbo].[Employee] ([Id], [UserName], [FirstName], [LastName], [Title])
VALUES (2, N'CORP\peter', N'Peter', N'Lillevold', N'System Developer')
INSERT INTO [dbo].[Employee] ([Id], [UserName], [FirstName], [LastName], [Title])
VALUES (3, N'CORP\thomas', N'Thomas', N'Sandberg', N'System Developer')
INSERT INTO [dbo].[Employee] ([Id], [UserName], [FirstName], [LastName], [Title])
VALUES (4, N'CORP\tor', N'Tor Kristen', N'Haugen', N'System Developer')
INSERT INTO [dbo].[Employee] ([Id], [UserName], [FirstName], [LastName], [Title])
VALUES (5, N'CORP\ingar', N'Ingar', N'Hillestad', N'System Developer')
SET IDENTITY_INSERT [dbo].[Employee] OFF

SET IDENTITY_INSERT [dbo].[Project] ON
INSERT INTO [dbo].[Project] ([Id], [Name], [Manager])
VALUES (1, N'4Subsea Wellhead', 2)
INSERT INTO [dbo].[Project] ([Id], [Name], [Manager])
VALUES (2, N'KC MVVM', 4)
INSERT INTO [dbo].[Project] ([Id], [Name], [Manager])
VALUES (3, N'KC GeekOut', 1)
SET IDENTITY_INSERT [dbo].[Project] OFF

SET IDENTITY_INSERT [dbo].[WorkItem] ON
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (1, 4, 2, '2010-01-04 09:00', '2010-01-04 12:00', N'Start MVVM')
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (2, 4, 2, '2010-01-04 12:30', '2010-01-04 17:00', NULL)
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (3, 4, 2, '2010-01-05 09:00', '2010-01-05 12:00', NULL)
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (4, 4, 2, '2010-01-05 12:30', '2010-01-05 17:00', NULL)
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (5, 4, 2, '2010-01-06 09:00', '2010-01-06 12:00', NULL)
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (6, 4, 2, '2010-01-06 12:30', '2010-01-06 17:00', NULL)
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (7, 4, 3, '2010-01-07 09:00', '2010-01-07 12:00', N'GeekOut hos Peter, MVVM')
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (8, 4, 3, '2010-01-07 12:30', '2010-01-07 17:00', N'GeekOut')
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (9, 4, 2, '2010-01-08 09:00', '2010-01-08 12:00', NULL)
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (10, 4, 2, '2010-01-08 12:30', '2010-01-08 17:00', NULL)
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (11, 4, 2, '2010-01-11 09:00', '2010-01-11 13:00', N'Hos 4Subsea, møte med Peter')
INSERT INTO [dbo].[WorkItem] ([Id], [Employee], [Project], [StartTime], [EndTime], [Comments])
VALUES (12, 4, 2, '2010-01-12 09:00', '2010-01-12 12:00', NULL)
SET IDENTITY_INSERT [dbo].[WorkItem] OFF

