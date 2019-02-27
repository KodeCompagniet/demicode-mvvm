CREATE TABLE [dbo].[Employee] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [UserName]  NVARCHAR (100) NOT NULL,
    [FirstName] NVARCHAR (100) NOT NULL,
    [LastName]  NVARCHAR (100) NOT NULL,
    [Title]     NVARCHAR (100) NULL
);

