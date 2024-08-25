using Amana.ControllerHelpers;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Repository;
#pragma warning restore CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System;
using System.Web.Mvc;

namespace Amana.Areas.Admin.Controllers
{
    public class C_LoginController : ControllerHelper
    {
        #region helpers

#pragma warning disable CS0246 // The type or namespace name 'UsersRepository' could not be found (are you missing a using directive or an assembly reference?)
        private UsersRepository usersDi;
#pragma warning restore CS0246 // The type or namespace name 'UsersRepository' could not be found (are you missing a using directive or an assembly reference?)
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usersDi = new UsersRepository(LangId, db);
            base.OnActionExecuting(filterContext);
        }

        #endregion

        public ActionResult Index()
        {
            if (HttpContext.Session["UserDetails"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'C_LoginViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Index(C_LoginViewModel entity, string returnUrl)
#pragma warning restore CS0246 // The type or namespace name 'C_LoginViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            return UserLogin(entity, returnUrl);
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'C_LoginViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult ForgetPassword(C_LoginViewModel entityVM)
#pragma warning restore CS0246 // The type or namespace name 'C_LoginViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            try
            {
                var results = usersDi.ForgetPassword(entityVM.Email);
                C_UserItems entity = results.Item2;
                int result = results.Item1;
                if (result > 0)
                    ViewBag.CustomSuccess = Amana.GlobalResources.Users.SentPassword;
                else
                    ViewBag.CustomError = Amana.GlobalResources.Users.InvalidEmail;
            }
            catch (Exception ex)
            {
                ViewBag.CustomError = ex.Message;
            }

            return View("Index", entityVM);
        }
    }
}