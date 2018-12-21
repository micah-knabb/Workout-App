using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkoutService;

namespace Capstone.Web.Controllers
{
    public class WorkoutManagerController : BaseController
    {
        private IWorkoutDAL _dal;

        public WorkoutManagerController(IWorkoutDAL dal)
        {
            _dal = dal;
        }

        // GET: WorkoutManager
        public ActionResult Index()
        {
            if (IsAuthenticated)
            {
                DashboardView model = new DashboardView();
                User user = Session[UserKey] as User;
                model.User = user;
                model.Workouts = _dal.GetAllWorkoutByUserId(user.Id);
                return GetAuthenticatedView("Index", model);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        #region Workouts
        [HttpGet]
        public ActionResult Workout(int id)
        {
            WorkoutView workout = _dal.GetWorkoutView(id);
            TempData["WorkoutID"] = id;
            return GetAuthenticatedView("Workout", workout);
        }

        [HttpPost]
        public ActionResult SubmitNewWorkout(string WorkoutName)
        {
            AddWorkout newWorkout = new AddWorkout();
            newWorkout.WorkoutName = WorkoutName;
            User user = Session[UserKey] as User;
            newWorkout.UserId = user.Id;
            int workoutID = _dal.AddWorkout(newWorkout);
            if (workoutID != BaseModel.InvalidId)
            {
                return RedirectToAction("Workout", "WorkoutManager", new { id = workoutID });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Add/Edit Exercise

        [HttpGet]
        public ActionResult EditStrength(int id)
        {
            ExerciseObject model = _dal.GetCompleteExercise(id);
            return GetAuthenticatedView("AddStrength", model);
        }

        [HttpPost]
        [Route("WorkoutManager/Workout/AddStrength")]
        public ActionResult AddStrength(string name)
        {
            int id = (int)TempData["WorkoutID"];
            int newExerciseId = _dal.AddExercise(id, name);
            ExerciseObject model = _dal.GetCompleteExercise(newExerciseId);
            return RedirectToAction("EditStrength", new { id = model.Id });
        }

        [HttpPost]
        [Route("WorkoutManager/Workout/AddEndurance")]
        public ActionResult AddEndurance(string name)
        {
            int id = (int)TempData["WorkoutID"];
            int newExerciseId = _dal.AddExercise(id, name);
            Set newSet = new Set()
            {
                WorkoutExerciseId = newExerciseId
            };
            _dal.AddSet(newSet);
            ExerciseObject model = _dal.GetCompleteExercise(newExerciseId);
            return RedirectToAction("EditEndurance", new { id = model.Id });
        }

        [HttpGet]
        public ActionResult EditEndurance(int id)
        {
            ExerciseObject model = _dal.GetCompleteExercise(id);
            return GetAuthenticatedView("AddEndurance", model);
        }



        [HttpPost]
        [Route("WorkoutManager/Workout/AddBodyWeight")]
        public ActionResult AddBodyWeight(string name)
        {
            int id = (int)TempData["WorkoutID"];
            int newExerciseId = _dal.AddExercise(id, name);
            ExerciseObject model = _dal.GetCompleteExercise(newExerciseId);
            return RedirectToAction("EditBodyweight", new { id = model.Id });
        }

        [HttpGet]
        public ActionResult EditBodyweight(int id)
        {
            ExerciseObject model = _dal.GetCompleteExercise(id);
            return GetAuthenticatedView("AddBodyweight", model);
        }
        #endregion

        #region Sets
        [HttpPost]
        [Route("WorkoutManager/AddStrengthSet")]
        public ActionResult AddStrengthSet(int reps, int weight, int workoutExercise)
        {
            Set newSet = new Set();

            newSet.WorkoutExerciseId = workoutExercise;
            newSet.Reps = reps;
            newSet.Weight = weight;
            newSet.Time = 0;
            newSet.Distance = 0;
            newSet.Intensity = 0;
            int setId = _dal.AddSet(newSet);
            newSet = _dal.GetSet(setId);
            var json = Json(newSet, JsonRequestBehavior.AllowGet);
            return json;
        }

        [HttpPost]
        [Route("WorkoutManager/AddEnduranceSet")]
        public ActionResult AddEnduranceSet(decimal time, decimal distance, int intensity, int workoutExercise, int setId)
        {
            Set updatedSet = _dal.GetSet(setId);

            updatedSet.WorkoutExerciseId = workoutExercise;
            updatedSet.Reps = 0;
            updatedSet.Weight = 0;
            updatedSet.Time = time;
            updatedSet.Distance = distance;
            updatedSet.Intensity = intensity;
            _dal.UpdateSet(updatedSet, setId);

            var json = Json(updatedSet, JsonRequestBehavior.AllowGet);
            return json;
        }

        [HttpPost]
        [Route("WorkoutManager/AddBodyweightSet")]
        public ActionResult AddBodyweightSet(int reps, int workoutExercise)
        {
            Set newSet = new Set();

            newSet.WorkoutExerciseId = workoutExercise;
            newSet.Reps = reps;
            newSet.Weight = 0;
            newSet.Time = 0;
            newSet.Distance = 0;
            newSet.Intensity = 0;
            int setId = _dal.AddSet(newSet);
            newSet = _dal.GetSet(setId);
            var json = Json(newSet, JsonRequestBehavior.AllowGet);
            return json;
        }

        #endregion

        #region API
        [HttpGet]
        public ActionResult GetAnExercise(int id)
        {
            ExerciseObject result = _dal.GetCompleteExercise(id);
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(json);
            
        }

        [HttpGet]
        public ActionResult GetExercisesByType(string type)
        {
            List<string> result = _dal.GetExercisesByType(type);
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(json);
        }

        [HttpDelete]
        [Route("WorkoutManager/Delete")]
        public ActionResult DeleteWorkout(int id)
        {
            bool isDeleted = _dal.DeleteWorkout(id);
            var json = Json(isDeleted, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(json);
        }

        [HttpDelete]
        [Route("WorkoutManager/DeleteExercise")]
        public ActionResult DeleteExercise(int id)
        {
            int WorkoutId = (int)TempData["WorkoutID"];
            bool isDeleted = _dal.DeleteWorkoutExercise(id, WorkoutId);
            var json = Json(isDeleted, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(json);
        }

        [HttpPut]
        [Route("WorkoutManager/ExerciseDown")]
        public ActionResult ExerciseDown(int Order)
        {
            int WorkoutId = (int)TempData["WorkoutID"];
            _dal.MoveExerciseDown(WorkoutId, Order);
            var json = Json(JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(json);
        }

        [HttpPut]
        [Route("WorkoutManager/ExerciseUp")]
        public ActionResult ExerciseUp(int Order)
        {
            int WorkoutId = (int)TempData["WorkoutID"];
            _dal.MoveExerciseUp(WorkoutId, Order);
            var json = Json(JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(json);
        }

        [HttpDelete]
        [Route("WorkoutManager/DeleteSet")]
        public ActionResult DeleteSet(int SetId, int WE_Id)
        {
            bool isDeleted = _dal.DeleteSet(SetId, WE_Id);
            var json = Json(isDeleted, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(json);
        }

        [HttpPut]
        [Route("WorkoutManager/SetDown")]
        public ActionResult SetDown(int WE_Id, int Order)
        {
            _dal.MoveSetDown(WE_Id, Order);
            var json = Json(JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(json);
        }

        [HttpPut]
        [Route("WorkoutManager/SetUp")]
        public ActionResult SetUp(int WE_Id, int Order)
        {
            _dal.MoveSetUp(WE_Id, Order);
            var json = Json(JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(json);
        }
        #endregion
    }
}