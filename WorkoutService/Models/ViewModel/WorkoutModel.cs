using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutService
{
    public class WorkoutModel : BaseModel
    {
        public int UserId { get; set; }
        public string WorkoutName { get; set; }
        public int NumberofExercises { get; set; }

    }
}
