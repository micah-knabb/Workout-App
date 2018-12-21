using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutService
{
    public class WorkoutViewExercise:BaseModel
    {
        public int Order { get; set; }
        public string ExerciseName { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public int SetCount { get; set; }
        public string VideoLink { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; } = 0;

    }
}
