#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Utilities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Repository;
#pragma warning restore CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Amana.ControllerHelpers
{
    public class ControllerHelper : Controller
    {
        #region Properties

#pragma warning disable CS0246 // The type or namespace name 'AmanaConcreteDBEntities1' could not be found (are you missing a using directive or an assembly reference?)
        protected AmanaConcreteDBEntities1 db = new AmanaConcreteDBEntities1();
#pragma warning restore CS0246 // The type or namespace name 'AmanaConcreteDBEntities1' could not be found (are you missing a using directive or an assembly reference?)
        protected int LangId { get; set; }
        protected int PageSize = 20;
        protected string SiteCulture = "ar";

        protected enum EnumPermissions { Read, Insert, Update, Delete }

        #endregion

        #region events

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            try
            {
                if (requestContext.HttpContext.Session["UserDetails"] == null)
                {
                    HttpCookie userCookie = requestContext.HttpContext.Request.Cookies["UserLoginDetails"];
                    if (userCookie != null)
                    {
                        string email = userCookie["Email"];
                        string password = StringCipher.Decrypt(userCookie["Passowrd"], true);
                        C_UserItems user = db.C_UserItems.SingleOrDefault(x => x.Email == email && x.Password == password);
                        if (user != null)
                        {
                            requestContext.HttpContext.Session["UserDetails"] = user;
                        }
                    }
                }
            }
            catch { UserSignOut(); }

            string language = (string)requestContext.RouteData.Values["language"];
            HttpCookie cultureCookie = requestContext.HttpContext.Request.Cookies["SiteCulture"];
            if (!string.IsNullOrEmpty(language))
            {
                SiteCulture = language;
            }
            else if (cultureCookie != null)
            {
                if (HelperMethods.IsValidCultureName(cultureCookie.Value))
                    SiteCulture = cultureCookie.Value;
            }
            else
            {
                cultureCookie = new HttpCookie("SiteCulture");
                cultureCookie.Value = SiteCulture;
                cultureCookie.Expires = HelperMethods.GetCurrentDateTime().AddYears(1);
                requestContext.HttpContext.Response.Cookies.Add(cultureCookie);
            }

            try
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteCulture);
            }
            catch
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("ar");
                requestContext.HttpContext.Response.Redirect("/");
            }
            LangId = (from a in db.C_Languages where a.Culture == SiteCulture select a.LanguageId).SingleOrDefault();

            //string baseName = this.GetType().BaseType.Name;
            //string df = this.GetType().Name;
            //string ewrewr = this.GetType().UnderlyingSystemType.Name;
            //if (baseName == "Controller")

            base.Initialize(requestContext);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region helperMethods

#pragma warning disable CS0246 // The type or namespace name 'C_NodeType' could not be found (are you missing a using directive or an assembly reference?)
        protected List<C_NodeType> GetMenuRoles(bool? isWidget)
#pragma warning restore CS0246 // The type or namespace name 'C_NodeType' could not be found (are you missing a using directive or an assembly reference?)
        {
            C_UserItems user = GetCurrentUser();
            List<C_NodeType> items = db.C_NodeType
                .Include(x => x.C_NodeTypeLoc)
                .Include(x => x.C_UserPermissions)
                .Where(x => x.IsActive == true && x.IsMenu == true)
                .OrderByDescending(x => x.NumOrder)
                .ThenByDescending(x => x.TypeId).ToList();

            if (isWidget != null)
                items = items.Where(x => x.IsWidget == isWidget).ToList();

            if (user.RoleId != null && user.RoleId > 0)
            {
                items = items.Where(x => x.C_UserPermissions.SingleOrDefault(a => a.TypeId == x.TypeId && a.RoleId == user.RoleId) != null
                    && x.C_UserPermissions.SingleOrDefault(a => a.TypeId == x.TypeId && a.RoleId == user.RoleId).IsRead == true).ToList();
            }

            return items;
        }

#pragma warning disable CS0246 // The type or namespace name 'C_LoginViewModel' could not be found (are you missing a using directive or an assembly reference?)
        protected ActionResult UserLogin(C_LoginViewModel entity, string returnUrl)
#pragma warning restore CS0246 // The type or namespace name 'C_LoginViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            string hashedPassword = HelperMethods.HashPasswordWithSalt(entity.Password);
            C_UserItems user = db.C_UserItems.SingleOrDefault(x => x.Email == entity.Email
            && (x.Password == hashedPassword || (x.ResetCode != null && x.ResetCode == hashedPassword)));

            if (user != null)
            {
                if (user.IsActive)
                {
                    if (entity.IsRemember)
                    {
                        HttpCookie LoginCookie = new HttpCookie("UserLoginDetails");
                        LoginCookie.Values.Add("Email", user.Email);
                        LoginCookie.Values.Add("Passowrd", StringCipher.Encrypt(user.Password, true));
                        LoginCookie.Expires = HelperMethods.GetCurrentDateTime().AddMinutes(30);// DateTime.MaxValue;
                        Response.Cookies.Add(LoginCookie);
                    }
                    Session["UserDetails"] = user;
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.CustomError = Amana.GlobalResources.Users.Inactive;
                    return View();
                }
            }
            else
            {
                ViewBag.CustomError = Amana.GlobalResources.Users.InvalidLogin;
                return View();
            }
        }

        protected void UserSignOut()
        {
            HelperMethods.SignOutUser();
        }

        protected int GetUserId()
        {
            return new UsersRepository(LangId, db).GetUserId();
        }

#pragma warning disable CS0246 // The type or namespace name 'C_UserItems' could not be found (are you missing a using directive or an assembly reference?)
        protected C_UserItems GetCurrentUser()
#pragma warning restore CS0246 // The type or namespace name 'C_UserItems' could not be found (are you missing a using directive or an assembly reference?)
        {
            return new UsersRepository(LangId, db).GetCurrentUser();
        }

        protected bool CheckPermission(int typeId, EnumPermissions operations)
        {
            C_UserItems user = GetCurrentUser();
            if (user.IsAdmin && user.RoleId == null)
                return true;
            C_UserPermissions perm = db.C_UserPermissions.SingleOrDefault(x => x.RoleId == user.RoleId && x.TypeId == typeId);
            if (perm != null)
            {
                switch (operations)
                {
                    case EnumPermissions.Read:
                        return perm.IsRead;
                    case EnumPermissions.Insert:
                        return perm.IsInsert;
                    case EnumPermissions.Update:
                        return perm.IsUpdate;
                    case EnumPermissions.Delete:
                        return perm.IsDelete;
                }
            }
            return false;
        }

        protected void CheckAllPermissions(int typeId)
        {
            ViewBag.IsDeveloper = false; ViewBag.IsUpdate = false; ViewBag.IsDelete = false; ViewBag.IsInsert = false; ViewBag.IsRead = false;
            C_UserItems user = GetCurrentUser();
            if (user.IsDeveloper && user.RoleId == null)
                ViewBag.IsDeveloper = true;
            if (user.IsAdmin && user.RoleId == null)
            {
                ViewBag.IsUpdate = true; ViewBag.IsDelete = true; ViewBag.IsInsert = true; ViewBag.IsRead = true;
                return;
            }

            if (user != null)
            {
                C_UserPermissions perm = db.C_UserPermissions.SingleOrDefault(x => x.RoleId == user.RoleId && x.TypeId == typeId);
                if (perm != null)
                {
                    if (perm.IsRead)
                        ViewBag.IsRead = true;
                    if (perm.IsInsert)
                        ViewBag.IsInsert = true;
                    if (perm.IsUpdate)
                        ViewBag.IsUpdate = true;
                    if (perm.IsDelete)
                        ViewBag.IsDelete = true;
                }
            }
        }

        protected string ValidationErrors()
        {
            string errorMessage = "VALIDATION ERROR ---- ";
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    errorMessage += " - " + error.ErrorMessage;
                }
            }
            return errorMessage;
        }

        protected void PageTitle(int typeId)
        {
            List<C_NodeTypeLoc> locs = db.C_NodeTypeLoc.Where(x => x.TypeId == typeId).ToList();
            if (locs.FirstOrDefault(a => a.LanguageId == LangId) != null)
            {
                ViewBag.Title = locs.FirstOrDefault(a => a.LanguageId == LangId).Title;
            }
            else if (locs.FirstOrDefault(a => a.LanguageId == 2) != null)
            {
                ViewBag.Title = locs.FirstOrDefault(a => a.LanguageId == 2).Title + " - " + @Amana.GlobalResources.Cpanel.NotTranslated;
            }
            else if (locs.FirstOrDefault(a => a.LanguageId == 1) != null)
            {
                ViewBag.Title = locs.FirstOrDefault(a => a.LanguageId == 1).Title + " - " + @Amana.GlobalResources.Cpanel.NotTranslated;
            }
            else
            {
                ViewBag.Title = @Amana.GlobalResources.Cpanel.NotTranslated;
            }

            //try { ViewBag.Title = db.C_NodeTypeLoc.SingleOrDefault(x => x.TypeId == typeId && x.LanguageId == LangId).Title; }
            //catch { ViewBag.Title = Amana.GlobalResources.Cpanel.NotTranslated; };
        }

        #endregion

        #region sharedActions

        public ActionResult DownloadFile(string filename, string filePath)
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + filePath + filename;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            //var cd = new System.Net.Mime.ContentDisposition
            //{
            //    FileName = filename,
            //    Inline = true,
            //};
            //Response.AppendHeader("Content-Disposition", cd.ToString());

            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");

            return File(filedata, contentType);
        }

        #endregion
    }
}