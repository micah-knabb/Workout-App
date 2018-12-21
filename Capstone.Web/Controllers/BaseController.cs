using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkoutService;

namespace Capstone.Web.Controllers
{
    public class BaseController : Controller
    {
        public const string UserKey = "UserKey";


        public ActionResult GetAuthenticatedView(string viewName, object model = null)
        {
            ActionResult result = null;
            if (IsAuthenticated)
            {
                result = View(viewName, model);
            }
            else
            {
                result = RedirectToAction("Login", "User");
            }
            return result;
        }

        public JsonResult GetAuthenticatedJson(JsonResult json)
        {
            if (IsAuthenticated)
            {
                return json;
            }
            else
            {
                return Json(new { error = "User is not authenticated." }, JsonRequestBehavior.AllowGet);
            }
        }

        #region User Authentication Properties
        public bool IsAuthenticated
        {
            get
            {
                return Session[UserKey] != null;
            }
        }

        public User CurrentUser
        {
            get
            {
                return Session[UserKey] as User;
            }
        }

        public void LogUserIn(User user)
        {
            Session[UserKey] = user;
        }

        public void LogUserOut()
        {
            Session[UserKey] = null;
        }

        #endregion
    }
}