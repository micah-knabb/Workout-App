using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutService
{
    public class WorkoutView
    {
        public static List<string> Types = new List<string>()
        {
            "Strength",
            "Endurance",
            "Bodyweight"
        };
        public string WorkoutName { get; set; }
        public List<WorkoutViewExercise> Exercises { get; set; }
    }
}
