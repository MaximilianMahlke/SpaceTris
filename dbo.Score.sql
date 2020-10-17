CREATE TABLE [dbo].[Score] (
    [ScoreID]   INT IDENTITY (1, 1) NOT NULL,
    [Highscore] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ScoreID] DESC),
    CONSTRAINT [FK_Score_Player] FOREIGN KEY ([PlayerID]) REFERENCES [dbo].[Player] ([PlayerID])
);

