using Amana.ControllerHelpers;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Filters;
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
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Amana.Areas.Admin.Controllers
{
#pragma warning disable CS0246 // The type or namespace name 'CustomAuthorize' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CustomAuthorizeAttribute' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IsBackend' could not be found (are you missing a using directive or an assembly reference?)
    [CustomAuthorize(IsBackend = true)]
#pragma warning restore CS0246 // The type or namespace name 'IsBackend' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'CustomAuthorizeAttribute' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'CustomAuthorize' could not be found (are you missing a using directive or an assembly reference?)
    public class C_UserItemsController : ControllerHelper
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

        public ViewResult Index(string sortOrder, string searchString, string currentFilter, int? page, int id)
        {
            var moduleItems = usersDi.GetItemsList(roleId: id, isActive: null);

            ViewBag.RoleLoc = db.C_UserRolesLoc.FirstOrDefault(a => a.RoleId == id && a.LanguageId == LangId);

            ViewBag.LangId = LangId;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParam = sortOrder == "DateAsc" ? "DateDesc" : "DateAsc";
            ViewBag.TitleSortParam = sortOrder == "TitleAsc" ? "TitleDesc" : "TitleAsc";
            ViewBag.EmailSortParam = sortOrder == "EmailAsc" ? "EmailDesc" : "EmailAsc";
            ViewBag.ActiveSortParam = sortOrder == "ActiveAsc" ? "ActiveDesc" : "ActiveAsc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            switch (sortOrder)
            {
                case "TitleDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.Name);
                    break;
                case "TitleAsc":
                    moduleItems = moduleItems.OrderBy(m => m.Name);
                    break;
                case "DateAsc":
                    moduleItems = moduleItems.OrderBy(m => m.DateJoin);
                    break;
                case "DateDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.DateJoin);
                    break;
                case "ActiveDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.IsActive);
                    break;
                case "ActiveAsc":
                    moduleItems = moduleItems.OrderBy(m => m.IsActive);
                    break;
                case "EmailAsc":
                    moduleItems = moduleItems.OrderBy(m => m.Email);
                    break;
                case "EmailDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.Email);
                    break;
                default:
                    moduleItems = moduleItems.OrderByDescending(m => m.UserId);
                    break;
            }

            if (!string.IsNullOrEmpty(searchString))
                moduleItems = moduleItems.Where(m => m.Name.ToUpper().Contains(searchString.ToUpper())
                || m.Email.ToUpper().Contains(searchString.ToUpper())
                || m.CellPhone.Substring(m.CellPhone.Length - 9) == searchString.Substring(searchString.Length - 9));

            int pageNumber = (page ?? 1);
            return View(moduleItems.ToList());
        }

        public ActionResult Activate(int id)
        {
            C_UserItems userEntity = usersDi.SingleItem(id);
            if (userEntity.IsAvailable)
                userEntity.IsAvailable = false;
            else
                userEntity.IsAvailable = true;

            db.Entry(userEntity).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { id = userEntity.RoleId });
        }
        public ActionResult LoginWithUserAccount(int id)
        {
            C_UserItems userEntity = usersDi.SingleItem(id);

            Session["UserDetails"] = userEntity;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create(int? id, bool? isSuccess, int roleId, int? factoryId)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            ViewBag.RoleLoc = db.C_UserRolesLoc.FirstOrDefault(a => a.RoleId == roleId && a.LanguageId == LangId);

            C_UserItemsViewModel entityVM = new C_UserItemsViewModel();
            entityVM.RoleId = roleId;
            if (id == null)
            {
                UserRegisterViewData(roleId);
                return View(entityVM);
            }
            else
            {
                C_UserItems entity = usersDi.SingleItem(id ?? 0);
                entityVM = new C_UserItemsViewModel
                {
                    UserId = entity.UserId,
                    RoleId = entity.RoleId,
                    Name = entity.Name,
                    JobTitle = entity.JobTitle,
                    Email = entity.Email,
                    //Password = entity.Password,
                    Phone = entity.Phone,
                    CellPhone = entity.CellPhone,
                    ImageUrl = entity.ImageUrl,
                    IsMale = entity.IsMale,
                    IsActive = entity.IsActive,
                    IsAdmin = entity.IsAdmin,
                    IsDeveloper = entity.IsDeveloper,
                    FactoryId = factoryId
                };

                UserRegisterViewData(entity.RoleId);
                return View(entityVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable CS0246 // The type or namespace name 'C_UserItemsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(C_UserItemsViewModel entityVM, int roleId, HttpPostedFileBase ImageUrl)
#pragma warning restore CS0246 // The type or namespace name 'C_UserItemsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            ModelState.Remove("UserId");
            ModelState.Remove("Password");
            try
            {
                if (ModelState.IsValid)
                {
                    int result = usersDi.CreateUpdate(entityVM, roleId, ImageUrl).Item1;
                    if (result > 0)
                    {
                        if (entityVM.FactoryId.HasValue)
                        {
                            var factory = FactoriesRepository.GetFactoryById(entityVM.FactoryId.Value, false);
                            factory.LoginUserId = result;
                            FactoriesRepository.SaveFactoryItem(factory);
                            return RedirectToAction("AddFactory", "Factorys", new { id = entityVM.FactoryId.Value, isSuccess = true });

                        }
                        if (entityVM.UserId <= 0)
                            return RedirectToAction("Create", new { roleId = entityVM.RoleId, isSuccess = true });
                        else
                            return RedirectToAction("Create", new { id = entityVM.UserId, roleId = entityVM.RoleId, isSuccess = true });
                    }
                    else if (result == -1)
                    {
                        ModelState.AddModelError("Email", Amana.GlobalResources.Validation.EmailExist);
                        UserRegisterViewData(roleId);
                        return View(entityVM);
                    }
                    else if (result == -2)
                    {
                        ModelState.AddModelError("CellPhone", Amana.GlobalResources.Validation.CellPhoneExist);
                        UserRegisterViewData(roleId);
                        return View(entityVM);
                    }
                }
                return View("UcCustomError", "_Layout", ValidationErrors());
            }
            catch (Exception ex)
            {
                return View("UcCustomError", "_Layout", ex.Message);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_UserItems entity = usersDi.SingleItem(id ?? 0);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                C_UserItems entity = usersDi.SingleItem(id);
                int? roleId = entity.RoleId;
                List<ImagePathesHelper> imgsPaths = usersDi.DeleteItem(entity);
                foreach (var item in imgsPaths)
                    HelperMethods.DeleteFile(item.FoldersPath, item.FileName);
                return RedirectToAction("Index", new { id = roleId });
            }
            catch (Exception ex)
            {
                return View("UcCustomError", "_Layout", ex.Message);
            }
        }

        public ActionResult DeleteImage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_UserItems entity = db.C_UserItems.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }

            usersDi.DeleteImage(entity);

            return RedirectToAction("Create", new { id = entity.UserId, roleId = entity.RoleId });
        }

        #region privateMethods

        private void UserRegisterViewData(int? roleId)
        {
            List<C_UserRolesLoc> roles = usersDi.GetRolesLocList().ToList();
            ViewData["Roles"] = new SelectList(roles, "RoleId", "Title", roleId);
        }

        #endregion
    }
}