CREATE TABLE [dbo].[WorkItem] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [Employee]  INT             NOT NULL,
    [Project]   INT             NOT NULL,
    [StartTime] DATETIME        NOT NULL,
    [EndTime]   DATETIME        NOT NULL,
    [Comments]  NVARCHAR (1000) NULL
);

