using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutService
{
    public class DashboardView
    {
        public User User { get; set; }
        public List<WorkoutModel> Workouts { get; set; }
    }
}
