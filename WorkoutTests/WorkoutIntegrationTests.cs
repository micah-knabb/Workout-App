using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using WorkoutService;



namespace WorkoutIntegrationTests
{
    //[TestClass]
    public class WorkoutIntegrationTests
    {
        private TransactionScope _tran;
        private IWorkoutDAL _db = new WorkoutDAL();
        private int testID = BaseModel.InvalidId;

        [TestInitialize]
        public void Initialize()
        {
            _tran = new TransactionScope();

            PasswordHelper pass = new PasswordHelper("password");
            User test = new User();
            WorkoutDAL _dal = new WorkoutDAL();
            test.Email = "knabbm@gmail.com";
            test.FirstName = "Micah";
            test.LastName = "knabb";

            test.Hash = pass.Hash;
            test.Salt = pass.Salt;

            test.PictureUrl = "Picture";
            test.Username = "mknabb";
            test.RoleID = 1;

            testID = _dal.AddUser(test);
            Assert.IsNotNull(testID);

        }

        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back

        }
        
        [TestMethod]
        public void AddWorkoutTest()
        {
            AddWorkout workout = new AddWorkout();
            workout.UserId = testID;
            workout.WorkoutName = "test workout";
            int worked = _db.AddWorkout(workout); //tests AddWorkout method
            Assert.IsNotNull(worked);

            int testWorkout_Exervise = _db.AddExercise(worked, "Test Exercise"); // tests AddExercise method, returns WE_Id
            Assert.IsNotNull(testWorkout_Exervise); 



        }

        [TestMethod]
        public void TestMethod1()
        {
            WorkoutView test = _db.GetWorkoutView(1);
            Assert.IsNotNull(test);



        }

    }
}
