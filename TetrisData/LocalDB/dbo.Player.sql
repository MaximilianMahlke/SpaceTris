CREATE TABLE [dbo].[Player] (
    [PlayerID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([PlayerID] ASC)
);

