using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web
{
    public class ModifySetsViewModel
    {
        public string ExerciseName { get; set; }
        public int WorkoutID { get; set; }
        public int Workout_ExerciseID { get; set; }

        public ModifySetsViewModel(string name, int workoutId, int workoutExerciseId)
        {
            ExerciseName = name;
            WorkoutID = workoutId;
            Workout_ExerciseID = workoutExerciseId;
        }
    }
}