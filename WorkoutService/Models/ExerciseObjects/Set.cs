using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutService
{
    public class Set : BaseModel
    {
        public int WorkoutExerciseId { get; set; }
        public int Order { get; set; }
        public int Reps { get; set; } = 0;
        public int Weight { get; set; } = 0;
        public decimal Time { get; set; } = 0;
        public decimal Distance { get; set; } = 0;
        public int Intensity { get; set; } = 0;
    }
}
