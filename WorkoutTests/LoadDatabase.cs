using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkoutService;

namespace WorkoutTests
{
    //[TestClass]
    public class LoadDatabase
    {
        private const string _connectionString = @"Server=4481c9ba-2989-4d9a-acaf-a9bc00f115f5.sqlserver.sequelizer.com;Database=db4481c9ba29894d9aacafa9bc00f115f5;User ID=wailfirfjrewortv;Password=TYvWCXwZsTXKpKL7JeGDbkM6WHQqvCFZV7kmGSxcLAhAEf7TdmrDhpYGmy84hfCX;";

        [TestMethod]
        public void PopulateDatabase()
        {
            IWorkoutDAL _db = new WorkoutDAL(_connectionString);
            
            using (TransactionScope scope = new TransactionScope())
            {
                PopulateExercise(_db);
                scope.Complete();
            }
        }

        public static void PopulateExercise(IWorkoutDAL db)
        {
            db.AddExerciseTypeDB("Strength");
            db.AddExerciseTypeDB("Endurance");
            db.AddExerciseTypeDB("Bodyweight");

            AddExercise newExercise = new AddExercise();

            //Add Exercises for Strength
            newExercise.Name = "Bench Press";
            newExercise.TypeId = 1;
            newExercise.Description = "https://cdn2.coachmag.co.uk/sites/coachmag/files/styles/insert_main_wide_image/public/2016/07/1-1-bench-press.jpg?itok=bJYGPFGO";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=TPqdrWcffdk";
            db.AddExerciseDB(newExercise);

            newExercise.Name = "Biceps curl";
            newExercise.TypeId = 1;
            newExercise.Description = "http://www.getfit-studio.com/wp-content/uploads/2016/07/bicep-curls-350x321.jpg";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=ktFMqoa4R5A";
            db.AddExerciseDB(newExercise);

            newExercise.Name = "Squat";
            newExercise.TypeId = 1;
            newExercise.Description = "http://seannal.com/wp-content/uploads/2013/04/squat-form.jpg";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=nEQQle9-0NA";
            db.AddExerciseDB(newExercise);

            newExercise.Name = "Deadlift";
            newExercise.TypeId = 1;
            newExercise.Description = "http://ispgym.ie/wp-content/uploads/2017/04/deadlift_1.jpg";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=ytGaGIn3SjE";
            db.AddExerciseDB(newExercise);

            newExercise.Name = "Overhead Press";
            newExercise.TypeId = 1;
            newExercise.Description = "https://www.oxygenmag.com/.image/t_share/MTQ1MzQ3MzE1MTU2NjU3NzE5/standing-barbell-strict-press.jpg";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=ECWxumBMLVQ";
            db.AddExerciseDB(newExercise);

            //Add Endurance Exercises
            newExercise.Name = "Jog";
            newExercise.TypeId = 2;
            newExercise.Description = "https://media.tenor.com/images/f85efb3960ed019939c7d4b936f927a5/tenor.png";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=fQ7ewHFw_I8";
            db.AddExerciseDB(newExercise);

            newExercise.Name = "Cycling Bike";
            newExercise.TypeId = 2;
            newExercise.Description = "https://images-na.ssl-images-amazon.com/images/I/81qWOUKaDDL._SX425_.jpg";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=MOIhG2GbZ3c";
            db.AddExerciseDB(newExercise);

            newExercise.Name = "Rowing";
            newExercise.TypeId = 2;
            newExercise.Description = "https://cdn.powerhouse-fitness.co.uk/media/catalog/product/cache/10f519365b01716ddb90abc57de5a837/i/m/imgpsh_fullsize_3.jpg";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=H0r_ZPXJLtg";
            db.AddExerciseDB(newExercise);

            newExercise.Name = "Stairmaster";
            newExercise.TypeId = 2;
            newExercise.Description = "https://www.kingsofcardio.com/media/catalog/product/cache/1/image/800x800/9df78eab33525d08d6e5fb8d27136e95/s/m/sm5_lady_1.jpg";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=vUqHqBQ-wK0";
            db.AddExerciseDB(newExercise);

            //Add Bodyweight Exercises
            newExercise.Name = "Pushups";
            newExercise.TypeId = 3;
            newExercise.Description = "https://qph.fs.quoracdn.net/main-qimg-18bb2ae51bff1f3bd0c673d3594bd765-c";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=IODxDxX7oi4";
            db.AddExerciseDB(newExercise);

            newExercise.Name = "Lunge";
            newExercise.TypeId = 3;
            newExercise.Description = "https://www.popworkouts.com/wp-content/uploads/2012/07/walking-lunge.jpg";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=QOVaHwm-Q6U";
            db.AddExerciseDB(newExercise);

            newExercise.Name = "Pullups";
            newExercise.TypeId = 3;
            newExercise.Description = "https://1h6wllf3f4qfut1832zlo21e-wpengine.netdna-ssl.com/wp-content/uploads/rookie-mistakes-the-pullup-main-594x442.jpg";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=eGo4IYlbE5g";
            db.AddExerciseDB(newExercise);

            newExercise.Name = "Burpee";
            newExercise.TypeId = 3;
            newExercise.Description = "https://media1.popsugar-assets.com/files/thumbor/D1XViPexj8W8d7OUJEHi0IznS_I/fit-in/1024x1024/filters:format_auto-!!-:strip_icc-!!-/2015/11/19/900/n/1922729/9cdb44dde915ed97_Burpees/i/Burpees.jpg";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=dZgVxmf6jkA";
            db.AddExerciseDB(newExercise);

            newExercise.Name = "Jumping Jacks";
            newExercise.TypeId = 3;
            newExercise.Description = "https://i2.wp.com/www.bodybuildingestore.com/wp-content/uploads/2016/09/Jumping-Jacks-1.jpg";
            newExercise.VideoLink = "https://www.youtube.com/watch?v=UpH7rm0cYbM";
            db.AddExerciseDB(newExercise);
        }
    }
}
