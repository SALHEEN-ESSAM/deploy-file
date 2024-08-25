using Amana.ControllerHelpers;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Filters;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System.Collections.Generic;
using System.Linq;
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
    public class C_UserPermissionsController : ControllerHelper
    {
        public ViewResult Index(bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            List<C_UserRolesLoc> roles = db.C_UserRolesLoc.Where(x => x.LanguageId == LangId).ToList();
            ViewBag.RoleId = new SelectList(roles, "RoleId", "Title", "");

            return View(new List<C_UserPermissionsViewModel>());
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            int roleId = 0;
            if (!string.IsNullOrEmpty(form["RoleId"]))
                roleId = int.Parse(form["RoleId"]);
            else
                return RedirectToAction("Index");

            List<C_UserRolesLoc> roles = db.C_UserRolesLoc.Where(x => x.LanguageId == LangId).ToList();
            ViewBag.RoleId = new SelectList(roles, "RoleId", "Title", roleId);

            List<C_NodeTypeLoc> nodeTypes = db.C_NodeTypeLoc.Where(x => x.LanguageId == LangId && x.C_NodeType.IsActive == true).ToList();
            List<C_UserPermissionsViewModel> permissions = new List<C_UserPermissionsViewModel>();
            foreach (var item in nodeTypes)
            {
                C_UserPermissions userPerm = db.C_UserPermissions.SingleOrDefault(x => x.TypeId == item.TypeId && x.RoleId == roleId);
                C_UserPermissionsViewModel userPermVm = new C_UserPermissionsViewModel
                {
                    TypeId = item.TypeId,
                    Title = item.Title,
                    RoleId = roleId
                };
                if (userPerm != null)
                {
                    userPermVm.IsDelete = userPerm.IsDelete;
                    userPermVm.IsInsert = userPerm.IsInsert;
                    userPermVm.IsRead = userPerm.IsRead;
                    userPermVm.IsUpdate = userPerm.IsUpdate;
                }
                permissions.Add(userPermVm);
            }
            return View(permissions);
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'C_UserPermissionsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(List<C_UserPermissionsViewModel> entity, FormCollection form)
#pragma warning restore CS0246 // The type or namespace name 'C_UserPermissionsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            int roleId = int.Parse(form["HiddenRoleId"]);
            C_UserPermissions[] permToRemove = db.C_UserPermissions.Where(x => x.RoleId == roleId).ToArray();
            foreach (var item in permToRemove)
                db.C_UserPermissions.Remove(item);

            db.SaveChanges();

            foreach (var item in entity)
            {
                C_UserPermissions perm = new C_UserPermissions
                {
                    IsDelete = item.IsDelete,
                    IsInsert = item.IsInsert,
                    IsRead = item.IsRead,
                    IsUpdate = item.IsUpdate,
                    RoleId = roleId,
                    TypeId = item.TypeId
                };
                db.C_UserPermissions.Add(perm);
            }
            db.SaveChanges();
            return RedirectToAction("Index", new { isSuccess = true });
        }
    }
}