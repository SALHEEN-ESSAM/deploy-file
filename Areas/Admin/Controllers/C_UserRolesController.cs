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
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace Amana.Areas.Admin.Controllers
{
#pragma warning disable CS0246 // The type or namespace name 'IsBackend' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CustomAuthorize' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CustomAuthorizeAttribute' could not be found (are you missing a using directive or an assembly reference?)
    [CustomAuthorize(IsBackend = true)]
#pragma warning restore CS0246 // The type or namespace name 'CustomAuthorizeAttribute' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'CustomAuthorize' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IsBackend' could not be found (are you missing a using directive or an assembly reference?)
    public class C_UserRolesController : ControllerHelper
    {
        public ViewResult Index(string sortOrder, string currentFilter, int? page)
        {
            var moduleItems = db.C_UserRolesLoc.Include(x => x.C_UserRoles).Where(x => x.LanguageId == LangId).ToList();
            ViewBag.LangId = LangId;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParam = sortOrder == "TitleAsc" ? "TitleDesc" : "TitleAsc";

            switch (sortOrder)
            {
                case "TitleDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.Title).ToList();
                    break;
                case "TitleAsc":
                    moduleItems = moduleItems.OrderBy(m => m.Title).ToList();
                    break;
                default:
                    moduleItems = moduleItems.OrderByDescending(m => m.RoleId).ToList();
                    break;
            }

            int pageNumber = (page ?? 1);
            return View(moduleItems.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Create(int? id, bool? isSuccess, string command)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            List<C_NodeTypeLoc> nodeTypes = db.C_NodeTypeLoc.Include(x => x.C_NodeType).Where(x => x.LanguageId == LangId).ToList();

            if (id == null)
            {
                ViewData["NodeTypes"] = new SelectList(nodeTypes, "TypeId", "Title", "");
                return View();
            }
            else
            {
                C_UserRoles entity = db.C_UserRoles.Include(x => x.C_UserRolesLoc).SingleOrDefault(x => x.RoleId == id);
                C_UserRolesLoc entityLoc = entity.C_UserRolesLoc.SingleOrDefault(x => x.LanguageId == LangId);
                C_UserRolesViewModel entityVM = new C_UserRolesViewModel
                {
                    RoleId = entity.RoleId,
                    TypeId = entity.TypeId,
                    IsAdmin = entity.IsAdmin
                };
                if (entityLoc != null)
                {
                    entityVM.LanguageId = LangId;
                    entityVM.Title = entityLoc.Title;
                }
                ViewData["NodeTypes"] = new SelectList(nodeTypes, "TypeId", "Title", entityVM.TypeId);
                return View(entityVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable CS0246 // The type or namespace name 'C_UserRolesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(C_UserRolesViewModel entityVM, string command)
#pragma warning restore CS0246 // The type or namespace name 'C_UserRolesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            ModelState.Remove("RoleId");
            try
            {
                if (ModelState.IsValid)
                {
                    entityVM.LanguageId = LangId;
                    C_UserRoles entity = new C_UserRoles
                    {
                        RoleId = entityVM.RoleId,
                        TypeId = entityVM.TypeId,
                        IsAdmin = false
                    };
                    C_UserRolesLoc entityLoc = new C_UserRolesLoc
                    {
                        LanguageId = LangId,
                        RoleId = entityVM.RoleId,
                        Title = entityVM.Title,
                        IsTranslated = true
                    };
                    if (entityVM.RoleId <= 0)
                    {
                        db.C_UserRoles.Add(entity);

                        List<C_Languages> langs = db.C_Languages.ToList();
                        foreach (var item in langs)
                        {
                            C_UserRolesLoc LocsList = new C_UserRolesLoc
                            {
                                LanguageId = item.LanguageId,
                                RoleId = entityVM.RoleId,
                                Title = entityVM.Title,
                                IsTranslated = item.LanguageId == LangId ? true : false
                            };
                            db.C_UserRolesLoc.Add(LocsList);
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        C_UserRolesLoc currentEtityLoc = db.C_UserRolesLoc.SingleOrDefault(a => a.RoleId == entityVM.RoleId && a.LanguageId == LangId);
                        C_UserRoles currentEntity = db.C_UserRoles.Find(entityVM.RoleId);
                        db.Entry(currentEntity).CurrentValues.SetValues(entity);
                        if (currentEtityLoc != null)
                            db.Entry(currentEtityLoc).CurrentValues.SetValues(entityLoc);
                        else
                            db.C_UserRolesLoc.Add(entityLoc);
                        db.SaveChanges();
                    }
                    if (command == Amana.GlobalResources.Cpanel.Save)
                        return RedirectToAction("Create", new { id = entity.RoleId, isSuccess = true });
                    else
                        return RedirectToAction("Create", new { id = "", isSuccess = true });
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
            C_UserRolesLoc entity = (from a in db.C_UserRolesLoc where a.RoleId == id && a.LanguageId == LangId select a).SingleOrDefault();
            if (entity != null)
                return View(entity);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C_UserRoles roleEntity = db.C_UserRoles.Find(id);
            C_UserRolesLoc[] roleLocEntity = (from a in db.C_UserRolesLoc where a.RoleId == roleEntity.RoleId select a).ToArray();
            try
            {
                foreach (var item in roleLocEntity)
                {
                    db.C_UserRolesLoc.Remove(item);
                }

                C_UserItems[] users = db.C_UserItems.Where(x => x.RoleId == id).ToArray();
                foreach (var item in users)
                    db.C_UserItems.Remove(item);

                C_UserPermissions[] userPerm = db.C_UserPermissions.Where(x => x.RoleId == id).ToArray();
                foreach (var item in userPerm)
                    db.C_UserPermissions.Remove(item);

                db.C_UserRoles.Remove(roleEntity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = true;
                return View(roleLocEntity.SingleOrDefault(a => a.LanguageId == LangId));
            }
        }
    }
}