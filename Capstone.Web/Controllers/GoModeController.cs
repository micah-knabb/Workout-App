using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkoutService;

namespace Capstone.Web.Controllers
{
    public class GoModeController : BaseController
    {
        private IWorkoutDAL _dal;
        public GoModeController(IWorkoutDAL dal)
        {
            _dal = dal;
        }

        [HttpGet]
        [Route("GoMode/Start/{workoutId}")]
        public ActionResult GoMode(int workoutId)
        {
            List<ExerciseObject> workout = _dal.GetCompleteWorkout(workoutId);
            if (workout.Count < 1)
            {
                return GetAuthenticatedView("EmptyWorkout");
            }
            Session["Workout"] = workout;
            return GetAuthenticatedView("GoMode", workoutId);
        }
        
        public ActionResult Exit()
        {
            return RedirectToAction("Index", "WorkoutManager");
        }



        #region API
        [HttpGet]
        public ActionResult GetWorkout()
        {
            List<ExerciseObject> result = Session["Workout"] as List<ExerciseObject>;
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(json);
        }

        [HttpGet]
        public ActionResult NextExercise(int counter)
        {
            List<ExerciseObject> workout = Session["Workout"] as List<ExerciseObject>;
            ExerciseObject result = workout[counter];
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(json);

        }
        #endregion
    }
}