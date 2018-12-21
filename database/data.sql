-- *****************************************************************************
-- This script contains INSERT statements for populating tables with seed data
-- *****************************************************************************

USE [master];
GO

BEGIN TRY
	DROP DATABASE [WorkoutCompanion];
END TRY
BEGIN CATCH	
END CATCH
GO

CREATE DATABASE [WorkoutCompanion];
GO

USE [WorkoutCompanion];
GO

CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Hash] [varchar](50) NOT NULL,
	[Salt] [varchar](50) NOT NULL,
	[RoleId] [int] NULL,
	[PictureUrl] [varchar](100) NULL,

	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

CREATE TABLE [dbo].[Workout](
	[WorkoutId] [int] IDENTITY(1,1) NOT NULL,
	[WorkoutName] [varchar](50) NOT NULL,
	[UserId] [int] NOT NULL,

 CONSTRAINT [PK_Workout] PRIMARY KEY CLUSTERED ([WorkoutId] ASC),
 constraint [FK_department] foreign key (UserId) references [User] (UserId),
 );

 CREATE TABLE [dbo].[Type](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,

 CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED ([TypeId] ASC)
 );

 CREATE TABLE [dbo].[Exercise](
	[ExerciseId] [int] IDENTITY(1,1) NOT NULL,
	[TypeID] [int] NOT NULL,
	[Description] [varchar](100) NOT NULL,
	[VideoLink] [varchar](100) NOT NULL,
	[Name] [varchar](50) NOT NULL,

	CONSTRAINT [PK_Exercise] PRIMARY KEY CLUSTERED ([ExerciseId] ASC),
	constraint fk_TypeID foreign key (TypeID) references [Type] (TypeID),
);

CREATE TABLE [dbo].[Workout_Exercise](

	[WE_Id]				int IDENTITY(1,1) NOT NULL,
	[WorkoutId]			int				not null,
	[ExerciseId]		int				not null,
	[Order]				int				not null,

	CONSTRAINT [PK_Workout_Exercise] PRIMARY KEY CLUSTERED ([WE_Id] ASC),
	constraint fk_WorkoutID foreign key (WorkoutId) references Workout (WorkoutId),
	constraint fk_ExerciseId foreign key (ExerciseId) references Exercise (ExerciseId),
);

CREATE TABLE [dbo].[Set](

	[Set_Id]			int IDENTITY(1,1) NOT NULL,
	[WE_Id]				int				not null,
	[Reps]				int				not null,
	[Weight]			int				not null,
	[Order]				int				not null,
	[Duration]			int				not null

	CONSTRAINT [PK_Set] PRIMARY KEY CLUSTERED ([Set_Id] ASC),
	constraint [fk_WE_Id] foreign key (WE_Id) references Workout_Exercise (WE_Id),
);
