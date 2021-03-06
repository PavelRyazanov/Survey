BEGIN TRANSACTION;
GO

CREATE TABLE [Surveys] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [Created] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Surveys] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Interviews] (
    [Id] int NOT NULL IDENTITY,
    [IdSurvey] int NOT NULL,
    [Created] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Interviews] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Interviews_Surveys_IdSurvey] FOREIGN KEY ([IdSurvey]) REFERENCES [Surveys] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Questions] (
    [Id] int NOT NULL IDENTITY,
    [IdSurvey] int NOT NULL,
    [Order] int NOT NULL,
    [Text] nvarchar(50) NOT NULL,
    [IsRequired] bit NOT NULL,
    CONSTRAINT [PK_Questions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Questions_Surveys_IdSurvey] FOREIGN KEY ([IdSurvey]) REFERENCES [Surveys] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Results] (
    [IdInterview] int NOT NULL,
    [IdQuestion] int NOT NULL,
    [IdAnswer] int NOT NULL,
    [InterviewId] int NULL,
    CONSTRAINT [PK_Results] PRIMARY KEY ([IdInterview], [IdQuestion], [IdAnswer]),
    CONSTRAINT [FK_Results_Interviews_InterviewId] FOREIGN KEY ([InterviewId]) REFERENCES [Interviews] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Answers] (
    [Id] int NOT NULL IDENTITY,
    [IdQuestion] int NOT NULL,
    [Order] int NOT NULL,
    [Text] nvarchar(150) NOT NULL,
    CONSTRAINT [PK_Answers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Answers_Questions_IdQuestion] FOREIGN KEY ([IdQuestion]) REFERENCES [Questions] ([Id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_Answers_IdQuestion_Text] ON [Answers] ([IdQuestion], [Text]);
GO

CREATE INDEX [IX_Interviews_IdSurvey] ON [Interviews] ([IdSurvey]);
GO

CREATE UNIQUE INDEX [IX_Questions_IdSurvey_Text] ON [Questions] ([IdSurvey], [Text]);
GO

CREATE INDEX [IX_Results_InterviewId] ON [Results] ([InterviewId]);
GO

CREATE UNIQUE INDEX [IX_Surveys_Title] ON [Surveys] ([Title]);
GO

COMMIT;
GO


