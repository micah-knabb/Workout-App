using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutService
{
    public class ExerciseObject : BaseModel
    {
        public int WorkoutID { get; set; }
        public Exercise Exercise { get; set; }
        public List<Set> SetList { get; set; }
        public int Order { get; set; }

        public ExerciseObject()
        {
            Exercise = new Exercise();
            SetList = new List<Set>();
        }
    }
}
