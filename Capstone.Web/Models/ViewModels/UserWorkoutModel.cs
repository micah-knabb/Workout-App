using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkoutService;

namespace Capstone.Web.Models.ViewModels
{
    public class UserWorkoutModel
    {
        public User User { get; set; }
        public List<WorkoutModel> AllWorkouts { get; set; }
    }
}