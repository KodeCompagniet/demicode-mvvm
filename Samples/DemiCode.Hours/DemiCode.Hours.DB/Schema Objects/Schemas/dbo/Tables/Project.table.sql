CREATE TABLE [dbo].[Project] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (50) NOT NULL,
    [Manager] INT           NOT NULL
);

