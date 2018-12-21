using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace WorkoutService
{
    public class WorkoutDAL : IWorkoutDAL
    {
        private const string getLastIdSql = "SELECT CAST(SCOPE_IDENTITY() as int);";

        #region Variables

        private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";
        private string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=WorkoutCompanion;Integrated Security=true";

        #endregion

        #region Constructors
        public WorkoutDAL()
        {
        }

        public WorkoutDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region DB Setup

        public void AddExerciseTypeDB(string TypeName)
        {
            const string sql = "Insert Into [Type] (Name) VALUES(@TypeName)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@TypeName", TypeName);
                cmd.ExecuteNonQuery();
            }
        }

        public void AddExerciseDB(AddExercise db)
        {
            const string sql = "INSERT Into Exercise(TypeID, Description, VideoLink, Name) " +
                            "VALUES(@TypeId, @Description, @VideoLink, @ExerciseName)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TypeId", db.TypeId);
                cmd.Parameters.AddWithValue("@Description", db.Description);
                cmd.Parameters.AddWithValue("@VideoLink", db.VideoLink);
                cmd.Parameters.AddWithValue("@ExerciseName", db.Name);
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region User DAL

        public int AddUser(User item)
        {
            int Id;

            const string sql = "INSERT [User] (FirstName, LastName, Username, Email, Hash, Salt) " +
                               "VALUES (@FirstName, @LastName, @Username, @Email, @Hash, @Salt); ";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@FirstName", item.FirstName);
                cmd.Parameters.AddWithValue("@LastName", item.LastName);
                cmd.Parameters.AddWithValue("@Username", item.Username);
                cmd.Parameters.AddWithValue("@Email", item.Email);
                cmd.Parameters.AddWithValue("@Hash", item.Hash);
                cmd.Parameters.AddWithValue("@Salt", item.Salt);
                Id = (int)cmd.ExecuteScalar();
            }

            return Id;
        }

        public User GetUser(string username)
        {
            User user = null;
            const string sql = "SELECT * From [User] WHERE Username = @Username;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = GetUserItemFromReader(reader);
                }
            }

            if (user == null)
            {
                throw new Exception("User does not exist.");
            }
            return user;
        }

        public User GetUserItemFromReader(SqlDataReader reader)
        {
            User item = new User();

            item.Id = Convert.ToInt32(reader["UserId"]);
            item.FirstName = Convert.ToString(reader["FirstName"]);
            item.LastName = Convert.ToString(reader["LastName"]);
            item.Username = Convert.ToString(reader["Username"]);
            item.Email = Convert.ToString(reader["Email"]);
            item.Salt = Convert.ToString(reader["Salt"]);
            item.Hash = Convert.ToString(reader["Hash"]);
            //item.RoleID = Convert.ToInt32(reader["RoleId"]);
            item.PictureUrl = Convert.ToString(reader["PictureUrl"]);

            return item;
        }

        public void UpdateProfile(User item)
        {
            const string sql = "UPDATE[User] SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PictureUrl = @Image WHERE UserId = @Id;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@FirstName", item.FirstName);
                cmd.Parameters.AddWithValue("@LastName", item.LastName);
                cmd.Parameters.AddWithValue("@Email", item.Email);
                cmd.Parameters.AddWithValue("@Image", item.PictureUrl);
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
        }

        #endregion
        
        #region View Model DALs

        /// <summary>
        /// Gets all information for a set via its set ID
        /// </summary>
        /// <param name="id">The set ID for which to return information on</param>
        /// <returns>A set class object containing all information for a set</returns>
        public Set GetSet(int id)
        {
            Set item = new Set();
            const string sql = "Select * from [Set] where Set_Id = @id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    item.Id = (int)reader["Set_Id"];
                    item.Intensity = (int)reader["Intensity"];
                    item.Order = (int)reader["Order"];
                    item.Reps = (int)reader["Reps"];
                    item.Time = Convert.ToDecimal(reader["Time"]);
                    item.Weight = (int)reader["Weight"];
                    item.WorkoutExerciseId = (int)reader["WE_Id"];
                    item.Distance = Convert.ToDecimal(reader["Distance"]);
                }
            }

            return item;
        }

        /// <summary>
        /// Returns the names of all the exercises of a given type of exercise
        /// </summary>
        /// <param name="type">The type of exercise to be returned: Strength, Endurence, or Bodyweight</param>
        /// <returns>Returns a list of all possible exercises for a given type</returns>
        public List<string> GetExercisesByType(string type)
        {
            List<string> Exercises = new List<string>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "Select Exercise.[Name] from Exercise JOIN [Type] on Exercise.TypeID = [Type].TypeId where [Type].[Name] = @Type";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Type", type);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string Exercise = (string)reader["Name"];
                    Exercises.Add(Exercise);
                }

            }
            return Exercises;
        }

        /// <summary>
        /// Returns a unique view model that is used by the "Workout" view page, returning a range of data for a workout
        /// </summary>
        /// <param name="WorkoutId">The workout ID for a workout that is being displayed on the "Workout" view page</param>
        /// <returns>A WorkoutView model</returns>
        public WorkoutView GetWorkoutView(int WorkoutId)
        {
            WorkoutView Workout = new WorkoutView();
            List<WorkoutViewExercise> list = new List<WorkoutViewExercise>();

            const string sql = "Select Workout.WorkoutId, WorkoutName, Workout_Exercise.WE_Id, " +
                "(Select Exercise.Name from Exercise where Exercise.ExerciseId = Workout_Exercise.ExerciseId) as 'Exercise', " +
                "(Select Exercise.TypeID from Exercise where Exercise.ExerciseId = Workout_Exercise.ExerciseId) as 'TypeId', " +
                "(Select Exercise.[Description] from Exercise where Exercise.ExerciseId = Workout_Exercise.ExerciseId) as 'Description', " +
                "(Select Exercise.VideoLink from Exercise where Exercise.ExerciseId = Workout_Exercise.ExerciseId) as 'Video', " +
                "(Select [Name] from [Type] where [Type].TypeId = (Select Exercise.TypeID from Exercise where Exercise.ExerciseId = Workout_Exercise.ExerciseId)) as 'Type', " +
                "Workout_Exercise.[Order],(Select Count(*) from [Set] where [Set].WE_Id = Workout_Exercise.WE_Id) as 'Number of Sets' " +
                "From Workout FULL OUTER JOIN Workout_Exercise on Workout.WorkoutId = Workout_Exercise.WorkoutId Where Workout.WorkoutId = @Id " +
                "Order by [Order] Asc";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", WorkoutId);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Workout.WorkoutName = Convert.ToString(reader["WorkoutName"]);
                    WorkoutViewExercise workout = new WorkoutViewExercise();
                    if(!DBNull.Value.Equals(reader["WE_Id"]))
                    {
                        workout.Id = Convert.ToInt32(reader["WE_Id"]);
                        workout.ExerciseName = Convert.ToString(reader["Exercise"]);
                        workout.Order = Convert.ToInt32(reader["Order"]);
                        workout.Type = Convert.ToInt32(reader["TypeId"]);
                        workout.SetCount = Convert.ToInt32(reader["Number of Sets"]);
                        workout.TypeName = Convert.ToString(reader["Type"]);
                        workout.Description = Convert.ToString(reader["Description"]);
                        workout.VideoLink = Convert.ToString(reader["Video"]);
                        list.Add(workout);
                    }
                    if(workout.Type == 2)
                    {
                        workout.Duration = Convert.ToInt32(reader["Number of Sets"]);
                    }
                    
                }

            }
            Workout.Exercises = list;
            return Workout;
        }

        /// <summary>
        /// Returbs a list of all workouts belonging to a user for use on the "Dashboard" view
        /// </summary>
        /// <param name="Id">The user ID of the user who is accessing the "Dashboard" view</param>
        /// <returns>A list of WorkoutModels that is used by the "Dashboard" view</returns>
        public List<WorkoutModel> GetAllWorkoutByUserId(int Id)
        {
            List<WorkoutModel> List = new List<WorkoutModel>();

            const string sql = "Select Workout.WorkoutId, WorkoutName, (SELECT Count(*) from Workout_Exercise where Workout_Exercise.WorkoutId = Workout.WorkoutId) as 'Count' from Workout " +
                                "full outer join Workout_Exercise on Workout_Exercise.WorkoutId = Workout.WorkoutId " +
                                "where Workout.UserID = @Id " +
                                "group by WorkoutName, Workout.WorkoutId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    WorkoutModel workout = new WorkoutModel();

                    workout.Id = Convert.ToInt32(reader["WorkoutId"]);
                    workout.NumberofExercises = Convert.ToInt32(reader["Count"]);
                    workout.WorkoutName = Convert.ToString(reader["WorkoutName"]);
                    List.Add(workout);
                }

            }
            return List;

        }

        /// <summary>
        /// Returns all information for an exercise on the Workout_Exercise, Exercise and Set tables
        /// </summary>
        /// <param name="WE_Id">The ID for the exercise on the Workout_Exercise table</param>
        /// <returns>An ExerciseObject, which contains all information from the Workout_Exercise, Exercise and Set tables for one exercise instance</returns>
        public ExerciseObject GetCompleteExercise(int WE_Id)
        {
            ExerciseObject ExObject = new ExerciseObject();
            List<Set> List = new List<Set>();
            const string sql = "SELECT Workout_Exercise.WE_Id as 'Id', Workout_Exercise.WorkoutId as 'WorkoutId', Workout_Exercise.[Order] as 'Exercise Order', [Set].[Order] as 'Set Order', (Select [Name] from [Type] where [Type].TypeId = Exercise.TypeID ) as 'Type', * from [Set]" +
                               " Full outer Join Workout_Exercise on Workout_Exercise.WE_Id = [Set].WE_Id" +
                               " Full outer Join Exercise on Exercise.ExerciseId = Workout_Exercise.ExerciseId" +
                               " where Workout_Exercise.WE_Id = @WE_Id Order by [Set].[Order]";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@WE_Id", WE_Id);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    

                    ExObject.Id = Convert.ToInt32(reader["Id"]);
                    ExObject.WorkoutID = Convert.ToInt32(reader["WorkoutId"]);
                    ExObject.Order = Convert.ToInt32(reader["Exercise Order"]);
                    ExObject.Exercise.Id = Convert.ToInt32(reader["ExerciseId"]);
                    ExObject.Exercise.Name = Convert.ToString(reader["Name"]);
                    ExObject.Exercise.Type = Convert.ToString(reader["Type"]);
                    ExObject.Exercise.VideoLink = Convert.ToString(reader["VideoLink"]);
                    ExObject.Exercise.Description = Convert.ToString(reader["Description"]);

                    if (!DBNull.Value.Equals(reader["Set_Id"]))
                    {
                        Set set = new Set();
                        set.Id = Convert.ToInt32(reader["Set_Id"]);
                        set.WorkoutExerciseId = Convert.ToInt32(reader["WE_Id"]);
                        set.Reps = Convert.ToInt32(reader["Reps"]);
                        set.Weight = Convert.ToInt32(reader["Weight"]);
                        set.Order = Convert.ToInt32(reader["Set Order"]);
                        set.Time = Convert.ToDecimal(reader["Time"]);
                        set.Distance = Convert.ToDecimal(reader["Distance"]);
                        set.Intensity = Convert.ToInt32(reader["Intensity"]);
                        List.Add(set);
                    }
                    

                    
                }
                
            }
            ExObject.SetList = List;
            return ExObject;
        }

        /// <summary>
        /// Returns a full list of exercises in an individual workout plan, with all information returned from Workout_Exercise, Exercise and Set tables for the exercise instance.
        /// </summary>
        /// <param name="WorkoutId">The WorkoutId of the workout from the Workout table</param>
        /// <returns>A list of ExerciseObject objects, which contains all information from the Workout_Exercise, Exercise and Set tables for one exercise instance</returns>
        public List<ExerciseObject> GetCompleteWorkout(int WorkoutId)
        {
            List<ExerciseObject> Workout = new List<ExerciseObject>();
            const string WE_Id_sql = " SELECT * from Workout_Exercise where Workout_Exercise.WorkoutId = @WorkoutId ORDER BY [Order]";
            List<int> intWork = new List<int>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(WE_Id_sql, conn);
                cmd.Parameters.AddWithValue("@WorkoutId", WorkoutId);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["WE_Id"]);
                    intWork.Add(id);
                }
            }

            foreach(var id in intWork)
            {
                ExerciseObject item = GetCompleteExercise(id);
                Workout.Add(item);
            }
            return Workout;
        }

        #endregion

        #region WorkoutDAL

        /// <summary>
        /// Creates a new workout in the database
        /// </summary>
        /// <param name="item">A AddWorkout class object, containing the User ID and the name of the workout to be created</param>
        /// <returns>The WorkoutId of the new workout for the user</returns>
        public int AddWorkout(AddWorkout item)
        {
            int WorkoutID;

            const string sql = "INSERT [Workout] (WorkoutName, UserId) " +
                               "VALUES (@WorkoutName, @UserId);";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@WorkoutName", item.WorkoutName);
                cmd.Parameters.AddWithValue("@UserId", item.UserId);
                WorkoutID = (int)cmd.ExecuteScalar();
            }

            return WorkoutID;
        }

        /// <summary>
        /// Creates and adds a new exercise to a workout in the Workout_Exercise table
        /// </summary>
        /// <param name="WorkoutId">ID of the Workout the exercise will be added to</param>
        /// <param name="ExerciseName">Name of the exercise that will be created</param>
        /// <returns>The new Workout_Exercise ID</returns>
        public int AddExercise(int WorkoutId, string ExerciseName)
        {
            const string sql = "INSERT[Workout_Exercise](WorkoutId, ExerciseId, [Order]) VALUES(@WorkoutId, (Select ExerciseId from Exercise where name = @ExerciseName), ((select Count(*) from Workout_Exercise where WorkoutId = @WorkoutId)+1))";
            int WorkoutExerciseId = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@WorkoutId", WorkoutId);
                cmd.Parameters.AddWithValue("@ExerciseName", ExerciseName);
                WorkoutExerciseId = (int)cmd.ExecuteScalar();
            }
            return WorkoutExerciseId;
        }

        /// <summary>
        /// Deletes an individual Workout_Exercise and all of its associated sets, and adjusts the order of the other exercises.
        /// </summary>
        /// <param name="WE_Id">Workout_Exercise ID of the specific Workout_Exercise</param>
        /// <param name="WorkoutId">The WorkoutId of the Workout that the exercise to be deleted belongs to</param>
        /// <returns>Returns true if the exercise is deleted</returns>
        public bool DeleteWorkoutExercise(int WE_Id, int WorkoutId)
        {
            bool deleted = false;
            int item = 0;
            const string sql = "DECLARE @order Int; SET @order = (Select [Order] from Workout_Exercise where WE_Id = @WE_Id); Delete from Workout_Exercise where WE_Id = @WE_Id; UPDATE Workout_Exercise SET [Order] = [Order] - 1 WHERE [Order] > @order AND WorkoutId = @workoutId; ";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + getLastIdSql, conn);
                cmd.Parameters.AddWithValue("@WE_Id", WE_Id);
                cmd.Parameters.AddWithValue("@workoutId", WorkoutId);
                item = (int)cmd.ExecuteNonQuery();
            }

            if (item > 0)
            {
                deleted = true;
            }
            return deleted;
        }

        /// <summary>
        /// Deletes a complete workout by its WorkoutId from the database, along with all of its associated exercises and sets
        /// </summary>
        /// <param name="Id">The WorkoutId from the associated workout in the DB</param>
        /// <returns></returns>
        public bool DeleteWorkout(int Id)
        {
            bool deleted = false;
            int item = 0;
            const string sql = "DELETE FROM Workout WHERE WorkoutId = @WorkoutId; ";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@WorkoutId", Id);
                item = (int)cmd.ExecuteNonQuery();
            }

            if (item > 0)
            {
                deleted = true;
            }
            return deleted;
        }
        
        /// <summary>
        /// Moves a complete Workout_Exercise down in order in a Workout
        /// </summary>
        /// <param name="WorkoutId">ID of the workout that the Workout_Exercise belongs to</param>
        /// <param name="Order">The current order number of the Workout_Exercise in a Workout</param>
        public void MoveExerciseDown(int WorkoutId, int Order)
        {
            //needs logic check that Order is > 1
            const string sql = "DECLARE @replace Int; SET @replace = (Select WE_Id from Workout_Exercise where[Order] = (@Order - 1) AND WorkoutId = @WorkoutId); " +
                "UPDATE Workout_Exercise SET[Order] = (@Order - 1) WHERE WorkoutId = @WorkoutId AND [Order] = @Order; " +
                "UPDATE Workout_Exercise SET[Order] = @Order WHERE WorkoutId = @WorkoutId AND WE_Id = @replace";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Order", Order);
                cmd.Parameters.AddWithValue("@WorkoutId", WorkoutId);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Moves a complete Workout_Exercise up in order in a Workout
        /// </summary>
        /// <param name="WorkoutId">ID of the workout that the Workout_Exercise belongs to</param>
        /// <param name="Order">The current order number of the Workout_Exercise in a Workout</param>
        public void MoveExerciseUp(int WorkoutId, int Order)
        {
            //needs logic check that Order is not the highest Order
            const string sql = "DECLARE @replace Int; SET @replace = (Select WE_Id from Workout_Exercise where[Order] = (@Order + 1) AND WorkoutId = @WorkoutId); " +
                "UPDATE Workout_Exercise SET[Order] = (@Order + 1) WHERE WorkoutId = @WorkoutId AND [Order] = @Order; " +
                "UPDATE Workout_Exercise SET[Order] = @Order WHERE WorkoutId = @WorkoutId AND WE_Id = @replace";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Order", Order);
                cmd.Parameters.AddWithValue("@WorkoutId", WorkoutId);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Moves a set within an Workout_Exercise up one order
        /// </summary>
        /// <param name="WE_Id">Workout_Exercise ID the set belongs to</param>
        /// <param name="Order">The order number of the set</param>
        public void MoveSetUp(int WE_Id, int Order)
        {
            const string sql = "DECLARE @replace Int; SET @replace = (Select Set_Id from [Set] where [Order] = (@Order + 1) AND WE_Id = @WE_Id); " +
                "UPDATE [Set] SET[Order] = (@Order + 1) WHERE WE_Id = @WE_Id AND [Order] = @Order; " +
                "UPDATE [Set] SET[Order] = @Order WHERE Set_Id = @replace"; 

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Order", Order);
                cmd.Parameters.AddWithValue("@WE_Id", WE_Id);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Moves a set within an Workout_Exercise down one order
        /// </summary>
        /// <param name="WE_Id">Workout_Exercise ID the set belongs to</param>
        /// <param name="Order">The order number of the set</param>
        public void MoveSetDown(int WE_Id, int Order)
        {
            const string sql = "DECLARE @replace Int; SET @replace = (Select Set_Id from [Set] where [Order] = (@Order - 1) AND WE_Id = @WE_Id); " +
                "UPDATE [Set] SET[Order] = (@Order - 1) WHERE WE_Id = @WE_Id AND [Order] = @Order; " +
                "UPDATE [Set] SET[Order] = @Order WHERE Set_Id = @replace";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Order", Order);
                cmd.Parameters.AddWithValue("@WE_Id", WE_Id);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates the attributes of an Endurance set
        /// </summary>
        /// <param name="set">A Set model populated with the set details</param>
        /// <param name="setId">ID of the the set to be updated</param>
        /// <returns>Returns true if the set was updated.</returns>
        public bool UpdateSet(Set set, int setId)
        {
            bool success = true;
            const string sql = "UPDATE [Set] SET Reps = @reps, [Weight] = @weight, Time = @time, Distance = @distance, Intensity = @intensity " +
                                "WHERE Set_Id = @id; ";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@reps", set.Reps);
                cmd.Parameters.AddWithValue("@weight", set.Weight);
                cmd.Parameters.AddWithValue("@time", set.Time);
                cmd.Parameters.AddWithValue("@distance", set.Distance);
                cmd.Parameters.AddWithValue("@intensity", set.Intensity);
                cmd.Parameters.AddWithValue("@id", setId);
                cmd.ExecuteNonQuery();
            }
            return success;
        }

        /// <summary>
        /// Deletes a set within an Exercise, and adjusts the order of the remaining sets  
        /// </summary>
        /// <param name="Set_Id">ID of the set to be deleted</param>
        /// <param name="WE_Id">Workout_Exercise ID the set belongs to</param>
        /// <returns>Returns true if workout was deleted</returns>
        public bool DeleteSet(int Set_Id, int WE_Id)
        {
            bool deleted = false;
            int item = 0;
            const string sql = "DECLARE @order Int; SET @order = (Select[Order] from[Set] where Set_Id = @Set_Id); " +
                "Delete from[Set] where Set_Id = @Set_Id; UPDATE[Set] SET[Order] = [Order] - 1 WHERE[Order] > @order AND WE_Id = @WE_Id; ";


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + getLastIdSql, conn);
                cmd.Parameters.AddWithValue("@WE_Id", WE_Id);
                cmd.Parameters.AddWithValue("@Set_Id", Set_Id);
                item = (int)cmd.ExecuteNonQuery();
            }

            if (item > 0)
            {
                deleted = true;
            }
            return deleted;
        }

        /// <summary>
        /// Adds a set within a Workout_Exercise, and sets it's order to the next highest in an exercise
        /// </summary>
        /// <param name="item">A Set class object, including the WE_Id that it belongs to</param>
        /// <returns></returns>
        public int AddSet(Set item)
        {
            const string sql = "INSERT[Set](WE_Id, Reps, Weight, Time, Distance, Intensity, [Order]) " +
                "VALUES(@WE_Id, @Reps, @Weight, @Time, @Distance, @Intensity, ((select Count(*) from[Set] where WE_Id = @WE_Id) + 1))";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@WE_Id", item.WorkoutExerciseId);
                cmd.Parameters.AddWithValue("@Reps", item.Reps);
                cmd.Parameters.AddWithValue("@Weight", item.Weight);
                cmd.Parameters.AddWithValue("@Time", item.Time);
                cmd.Parameters.AddWithValue("@Distance", item.Distance);
                cmd.Parameters.AddWithValue("@Intensity", item.Intensity);
                item.Id = (int)cmd.ExecuteScalar();
            }

            return item.Id;
        }

        #endregion

        #region Retired code

        //public int AddExercise(WorkoutExercise item)
        //{
        //    const string sql = "INSERT [Workout_Exercise] (WorkoutId, ExerciseId, Order) VALUES (@WorkoutId, @ExerciseId, @Order);";
        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();

        //        SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
        //        cmd.Parameters.AddWithValue("@WorkoutId", item.WorkoutID);
        //        cmd.Parameters.AddWithValue("@ExerciseId", item.ExerciseId);
        //        cmd.Parameters.AddWithValue("@Order", item.ExerciseOrder);
        //        item.Id = (int)cmd.ExecuteScalar();
        //    }

        //    return item.Id;
        //}

        //public int AddExerciseSet(WorkoutExercise item, List<Set> sets)
        //{
        //    int WorkExerciseID = BaseModel.InvalidId;

        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        int WorkoutExersizeId = AddExercise(item);
        //        foreach (var set in sets)
        //        {
        //            item.Id = WorkoutExersizeId;
        //            AddSet(set);
        //        }
        //        scope.Complete();
        //    }

        //    return WorkExerciseID;
        //}

        //public User GetUser(int userId)
        //{
        //    User user = null;
        //    const string sql = "SELECT * From [User] WHERE Id = @Id;";

        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@Id", userId);
        //        var reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            user = GetUserItemFromReader(reader);
        //        }
        //    }

        //    if (user == null)
        //    {
        //        throw new Exception("User does not exist.");
        //    }

        //    return user;
        //}

        //public List<User> GetUserLists()
        //{
        //    List<User> users = new List<User>();
        //    const string sql = "Select * From [User];";

        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        var reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            users.Add(GetUserItemFromReader(reader));
        //        }
        //    }

        //    return users;
        //}

        #endregion

    }
}
