using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkoutService;

namespace Capstone.Web.Controllers
{
    public class HomeController : BaseController
    {

        private IWorkoutDAL _dal;

        public HomeController(IWorkoutDAL dal)
        {
            _dal = dal;
        }

        //This method checks for user authentication. Used in every action.

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //User Dashboard

        
    }
}