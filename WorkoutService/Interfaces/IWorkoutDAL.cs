using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace WorkoutService
{
    public interface IWorkoutDAL
    {
        //User

        #region User DAL
        int AddUser(User item);
        User GetUser(string username);
        User GetUserItemFromReader(SqlDataReader reader);
        void UpdateProfile(User item);
        #endregion

        #region DB Setup
        void AddExerciseTypeDB(string TypeName);
        void AddExerciseDB(AddExercise db);
        #endregion

        #region WorkoutDAL

        int AddWorkout(AddWorkout item);
        List<WorkoutModel> GetAllWorkoutByUserId(int Id);
        
        int AddExercise(int WorkoutId, string ExerciseName);
        int AddSet(Set item);
        Set GetSet(int id);
        bool DeleteSet(int Set_Id, int WE_Id);
        bool UpdateSet(Set item, int setId);
        void MoveSetUp(int WE_Id, int Order);
        void MoveSetDown(int WE_Id, int Order);

        void MoveExerciseDown(int WorkoutId, int Order);
        void MoveExerciseUp(int WorkoutId, int Order);
        WorkoutView GetWorkoutView(int WorkoutId);

        List<string> GetExercisesByType(string type);
        bool DeleteWorkout(int Id);
        ExerciseObject GetCompleteExercise(int WE_Id);
        List<ExerciseObject> GetCompleteWorkout(int WorkoutId);
        bool DeleteWorkoutExercise(int WE_Id, int workoutId);
        
        #endregion

        #region Retired code
        //User GetUser(int userId);
        //List<User> GetUserLists();
        //WorkoutModel GetWorkoutById(int WorkoutId);
        //int AddExerciseSet(WorkoutExercise item, List<Set> sets);
        //int AddExercise(WorkoutExercise item);
        #endregion
    }
}
