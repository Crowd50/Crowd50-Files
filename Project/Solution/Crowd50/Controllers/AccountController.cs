namespace Crowd50.Controllers
{
    using AutoMapping;
    using Crowd50.Models;
    using Crowd50_DataAccess;
    using Crowd50_DataAccess.Models;
    using global::Custom.Cryptography;
    using System;
    using System.Configuration;
    using System.Web.Mvc;

    public class AccountController : Controller
    {
        public AccountController()
        {
            string logFile = ConfigurationManager.AppSettings["LogFile"];
            string connectionString = ConfigurationManager.ConnectionStrings["Datasource"].ConnectionString;
            userDataAccess = new UserDataAccess(connectionString, logFile);
        }
        private UserDataAccess userDataAccess;

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Account/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        public ActionResult Register(RegisterPO Form)
        {
            ActionResult oResponse = null;
            if(ModelState.IsValid)
            {
                try
                {
                    User userDataObject = AutoMap<RegisterPO>.To<User>(Form);
                    userDataObject.PrependSalt = aCrypt.GenerateSalt(20);
                    userDataObject.AppendSalt = aCrypt.GenerateSalt(20);
                    userDataObject.Password = aCrypt.HashPassword(
                        userDataObject.PrependSalt, 
                        Form.RegistrationPassword, 
                        userDataObject.AppendSalt);
                    userDataObject.LastLogin = DateTime.Now;
                    userDataAccess.CreateNewUser(userDataObject);

                    UserPO user = AutoMap<User>.To<UserPO>(userDataObject);
                    SetSessionVariables(user);

                    // TODO: Add insert logic here
                    oResponse = RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    oResponse = View(Form);
                }
            }
            else
            {
                oResponse = View(Form);
            }
            return oResponse;
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Account/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginPO Form)
        {
            ActionResult oRespose = null;
            if(ModelState.IsValid)
            {
                //View User by username
                UserPO user = AutoMap<User>.To<UserPO>(userDataAccess.ViewUserByUsername(Form.Username));


                byte[] currentAttempt = aCrypt.HashPassword(user.PrependSalt, Form.Password, user.AppendSalt);
                int len = currentAttempt.Length;
                for (int i = 0; i < currentAttempt.Length && i < user.Password.Length; i++)
                {
                    if(currentAttempt[i] != user.Password[i])
                    {
                        string setBP = "";
                    }
                }
                if(user != null && aCrypt.Compare(user.Password,currentAttempt))
                {
                    SetSessionVariables(user);
                    oRespose = RedirectToAction("Index", "Home");
                }
                else
                {
                    oRespose = View(Form);
                }
            }
            else
            {
                oRespose = View(Form);
            }
            return oRespose;
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        private void SetSessionVariables(UserPO user)
        {
            Session["FirstName"] = user.FirstName;
            Session["LastName"] = user.LastName;
            Session["EmailAddress"] = user.EmailAddress;
            Session["LastLogin"] = user.LastLogin;
        }
    }
}
