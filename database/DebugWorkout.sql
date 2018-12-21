Insert Into [Type] (Name)
VALUES ('Strength')
Insert Into [Type] (Name)
VALUES ('Endurance')
Insert Into [Type] (Name)
VALUES ('Bodyweight')

Insert Into Exercise (TypeID, Description, VideoLink, Name)
VALUES (3, '', '', 'Pushups');
Insert Into Exercise (TypeID, Description, VideoLink, Name)
VALUES (1, '', '', 'Bench Press');
Insert Into Exercise (TypeID, Description, VideoLink, Name)
VALUES (2, '', '', 'Jog');

INSERT INTO Workout(WorkoutName, UserID)
VALUES ('Debug', 1)

INSERT INTO Workout_Exercise(WorkoutId, ExerciseId, [Order])
VALUES (1, 1, 1)
INSERT INTO Workout_Exercise(WorkoutId, ExerciseId, [Order])
VALUES (1, 2, 2) 
INSERT INTO Workout_Exercise(WorkoutId, ExerciseId, [Order])
VALUES (1, 3, 3)

INSERT INTO [Set](WE_id, Reps, Weight, [Order], Time, Distance, Intensity)
VALUES (1, 10, 0, 1, 0, 0, 0)
INSERT INTO [Set](WE_id, Reps, Weight, [Order], Time, Distance, Intensity)
VALUES (1, 10, 0, 2, 0, 0, 0)
INSERT INTO [Set](WE_id, Reps, Weight, [Order], Time, Distance, Intensity)
VALUES (1, 10, 0, 3, 0, 0, 0)

INSERT INTO [Set](WE_id, Reps, Weight, [Order], Time, Distance, Intensity)
VALUES (2, 10, 100, 1, 0, 0, 0)
INSERT INTO [Set](WE_id, Reps, Weight, [Order], Time, Distance, Intensity)
VALUES (2, 10, 100, 2, 0, 0, 0)
INSERT INTO [Set](WE_id, Reps, Weight, [Order], Time, Distance, Intensity)
VALUES (2, 10, 100, 3, 0, 0, 0)

INSERT INTO [Set](WE_id, Reps, Weight, [Order], Time, Distance, Intensity)
VALUES (3, 0, 0, 1, 60, 5, 0)

--Select * From Workout
--JOIN Workout_Exercise on Workout.WorkoutId = Workout_Exercise.WorkoutId
--JOIN [Set] on Workout_Exercise.WE_Id = [Set].WE_Id

--Select 
--Workout.WorkoutId,
--WorkoutName, 
--UserId, 
--(Select Username from [User] where UserId = 1) as 'Username', 
--Workout_Exercise.WE_Id, 
--Workout.WorkoutId, 
--(Select Exercise.Name from Exercise where Exercise.ExerciseId = Workout_Exercise.ExerciseId) as 'Exercise',
--Workout_Exercise.WE_Id,
--Reps,
--Weight,
--Time,
--Workout_Exercise.[Order],
--[Set].[Order]
--From Workout
--JOIN Workout_Exercise on Workout.WorkoutId = Workout_Exercise.WorkoutId
--JOIN [Set] on Workout_Exercise.WE_Id = [Set].WE_Id

--Select 
--Workout.WorkoutId,
--WorkoutName, 
--Workout_Exercise.WE_Id, 
--(Select Exercise.Name from Exercise where Exercise.ExerciseId = Workout_Exercise.ExerciseId) as 'Exercise',
--Workout_Exercise.[Order],
--(Select Count(*) from [Set] where [Set].WE_Id = Workout_Exercise.WE_Id) as 'Number of Sets'
--From Workout
--JOIN Workout_Exercise on Workout.WorkoutId = Workout_Exercise.WorkoutId
--Where Workout.WorkoutId = x