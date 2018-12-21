using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkoutService;

namespace Capstone.Web.Controllers
{
    public class UserController : BaseController
    {
        private IWorkoutDAL _dal;
        public UserController(IWorkoutDAL dal)
        {
            _dal = dal;
        }

        //User functions (This can be moved to an inherited controller later)

        [HttpGet]
        public ActionResult Login()
        {
            if (IsAuthenticated)
            {
                LogUserOut();
            }

            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel credentials)
        {
            ActionResult result = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                User user = null;

                try
                {
                    user = _dal.GetUser(credentials.Username);
                }
                catch
                {
                    ModelState.AddModelError("invalid-user", "Either the username or the password is invalid.");
                }

                PasswordHelper passHelper = new PasswordHelper(credentials.Password, user.Salt);
                if (!passHelper.Verify(user.Hash))
                {
                    ModelState.AddModelError("invalid-user", "Either the username or the password is invalid.");
                    throw new Exception();
                }

                LogUserIn(user);
                result = RedirectToAction("Index", "WorkoutManager");

            }
            catch
            {
                result = View("Login");
            }

            return result;
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (IsAuthenticated)
            {
                LogUserOut();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel register)
        {
            ActionResult result = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                User user = null;
                try
                {
                    user = _dal.GetUser(register.Username);
                }
                catch
                {
                }

                if (user != null)
                {
                    ModelState.AddModelError("invalid-user", "The username is already taken.");
                    throw new Exception();
                }

                PasswordHelper passHelper = new PasswordHelper(register.Password);
                User newUser = new User()
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Email = register.Email,
                    Username = register.Username,
                    Salt = passHelper.Salt,
                    Hash = passHelper.Hash
                };

                newUser.Id = _dal.AddUser(newUser);
                LogUserIn(newUser);

                result = RedirectToAction("Index", "WorkoutManager");
            }
            catch (Exception)
            {
                result = View("Register");
            }

            return result;
        }

        [HttpPost]
        public ActionResult UpdateProfile(User profile)
        {
            User user = Session[UserKey] as User;
            if (profile.FirstName == null)
            {
                profile.FirstName = user.FirstName;
            }
            if (profile.LastName == null)
            {
                profile.LastName = user.LastName;
            }
            if (profile.Email == null)
            {
                profile.Email = user.Email;
            }
            if (profile.PictureUrl == null)
            {
                profile.PictureUrl = user.PictureUrl;
            }
            profile.Id = user.Id;
            _dal.UpdateProfile(profile);
            Session[UserKey] = profile;
            return RedirectToAction("Index", "WorkoutManager");
            
        }
    }
}