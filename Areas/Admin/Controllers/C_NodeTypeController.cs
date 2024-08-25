using Amana.ControllerHelpers;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Filters;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.SEO;
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
    public class C_NodeTypeController : ControllerHelper
    {
        public ViewResult Index(string sortOrder, string currentFilter, int? page)
        {
            CheckAllPermissions(1);

            var moduleItems = db.C_NodeTypeLoc.Include(x => x.C_NodeType).Where(x => x.LanguageId == LangId);
            if (!ViewBag.IsDeveloper)
                moduleItems = moduleItems.Where(x => x.C_NodeType.IsSeo);

            ViewBag.LangId = LangId;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParam = sortOrder == "TitleAsc" ? "TitleDesc" : "TitleAsc";
            ViewBag.ActiveSortParam = sortOrder == "ActiveAsc" ? "ActiveDesc" : "ActiveAsc";

            switch (sortOrder)
            {
                case "TitleDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.Title);
                    break;
                case "TitleAsc":
                    moduleItems = moduleItems.OrderBy(m => m.Title);
                    break;
                case "ActiveDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.C_NodeType.IsActive);
                    break;
                case "ActiveAsc":
                    moduleItems = moduleItems.OrderBy(m => m.C_NodeType.IsActive);
                    break;
                default:
                    moduleItems = moduleItems.OrderByDescending(m => m.C_NodeType.NumOrder).ThenByDescending(m => m.TypeId);
                    break;
            }

            int pageNumber = (page ?? 1);
            return View(moduleItems.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Activate(int id, int? page)
        {
            C_NodeType typeEntity = db.C_NodeType.Find(id);
            if (typeEntity.IsActive)
                typeEntity.IsActive = false;
            else
                typeEntity.IsActive = true;

            db.Entry(typeEntity).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { page = page });
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'C_NodeTypeLoc' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult UpdateNum(IEnumerable<C_NodeTypeLoc> entity)
#pragma warning restore CS0246 // The type or namespace name 'C_NodeTypeLoc' could not be found (are you missing a using directive or an assembly reference?)
        {
            foreach (C_NodeTypeLoc item in entity)
            {
                C_NodeType node = db.C_NodeType.SingleOrDefault(x => x.TypeId == item.TypeId);
                node.NumOrder = item.C_NodeType.NumOrder;
                db.Entry(node).State = EntityState.Modified;
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Create(int? id, bool? isSuccess, string command)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            CheckAllPermissions(1);

            if (id == null)
            {
                ViewBag.CatVocId = new SelectList(db.C_CategoryVocabularies, "CatVocId", "Title", "");
                return View();
            }
            else
            {
                C_NodeType entity = db.C_NodeType.Include(x => x.C_NodeTypeLoc).SingleOrDefault(x => x.TypeId == id);
                C_NodeTypeLoc entityLoc = entity.C_NodeTypeLoc.SingleOrDefault(x => x.LanguageId == LangId);
                C_NodeTypeViewModel entityVM = new C_NodeTypeViewModel
                {
                    TypeId = entity.TypeId,
                    CatVocId = entity.CatVocId,
                    IsAlbum = entity.IsAlbum,
                    IsWidget = entity.IsWidget,
                    IsActive = entity.IsActive,
                    IsMenu = entity.IsMenu,
                    IsSeo = entity.IsSeo,
                    IsMaps = entity.IsMaps,
                    IsShowToSite = entity.IsShowToSite,
                    Controller = entity.Controller
                };
                if (entityLoc != null)
                {
                    entityVM.LanguageId = LangId;
                    entityVM.Title = entityLoc.Title;
                    entityVM.MetaDescription = entityLoc.MetaDescription;
                    entityVM.MetaKeywords = entityLoc.MetaKeywords;
                    entityVM.MetaTitle = entityLoc.MetaTitle;
                    entityVM.PermaLink = entityLoc.PermaLink;
                }
                ViewBag.CatVocId = new SelectList(db.C_CategoryVocabularies, "CatVocId", "Title", entityVM.CatVocId);
                return View(entityVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable CS0246 // The type or namespace name 'C_NodeTypeViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(C_NodeTypeViewModel entityVM, FormCollection form, string command)
#pragma warning restore CS0246 // The type or namespace name 'C_NodeTypeViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            ModelState.Remove("TypeId");
            try
            {
                if (ModelState.IsValid)
                {
                    entityVM.LanguageId = LangId;
                    C_NodeType entity = new C_NodeType
                    {
                        TypeId = entityVM.TypeId,
                        CatVocId = entityVM.CatVocId,
                        IsAlbum = entityVM.IsAlbum,
                        IsWidget = entityVM.IsWidget,
                        IsActive = entityVM.IsActive,
                        IsMenu = entityVM.IsMenu,
                        IsSeo = entityVM.IsSeo,
                        IsMaps = entityVM.IsMaps,
                        IsShowToSite = entityVM.IsShowToSite,
                        Controller = entityVM.Controller
                    };
                    C_NodeTypeLoc entityLoc = new C_NodeTypeLoc
                    {
                        LanguageId = LangId,
                        TypeId = entityVM.TypeId,
                        Title = entityVM.Title,
                        MetaDescription = entityVM.MetaDescription,
                        MetaKeywords = entityVM.MetaKeywords,
                        MetaTitle = entityVM.MetaTitle,
                        IsTranslated = true,
                        PermaLink = SeoUrlHelper.URLFriendly(entityVM.PermaLink)
                    };
                    if (entityVM.TypeId <= 0)
                    {
                        entity.NumOrder = 0;
                        db.C_NodeType.Add(entity);

                        List<C_Languages> langs = db.C_Languages.ToList();
                        foreach (var item in langs)
                        {
                            C_NodeTypeLoc LocsList = new C_NodeTypeLoc
                            {
                                LanguageId = item.LanguageId,
                                TypeId = entityVM.TypeId,
                                Title = entityVM.Title,
                                MetaDescription = entityVM.MetaDescription,
                                MetaKeywords = entityVM.MetaKeywords,
                                MetaTitle = entityVM.MetaTitle,
                                IsTranslated = item.LanguageId == LangId ? true : false
                            };
                            db.C_NodeTypeLoc.Add(LocsList);
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        C_NodeTypeLoc currentEtityLoc = db.C_NodeTypeLoc.SingleOrDefault(a => a.TypeId == entityVM.TypeId && a.LanguageId == LangId);
                        C_NodeType currentEntity = db.C_NodeType.Find(entityVM.TypeId);
                        entity.NumOrder = currentEntity.NumOrder;

                        db.Entry(currentEntity).CurrentValues.SetValues(entity);
                        if (currentEtityLoc != null)
                            db.Entry(currentEtityLoc).CurrentValues.SetValues(entityLoc);
                        else
                            db.C_NodeTypeLoc.Add(entityLoc);
                        db.SaveChanges();
                    }
                    if (command == Amana.GlobalResources.Cpanel.Save)
                        return RedirectToAction("Create", new { id = entity.TypeId, isSuccess = true });
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
            C_NodeTypeLoc entity = (from a in db.C_NodeTypeLoc where a.TypeId == id && a.LanguageId == LangId select a).SingleOrDefault();
            if (entity != null)
                return View(entity);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C_NodeType typeEntity = db.C_NodeType.Find(id);
            C_NodeTypeLoc[] typeLocEntity = (from a in db.C_NodeTypeLoc where a.TypeId == typeEntity.TypeId select a).ToArray();
            try
            {
                foreach (var item in typeLocEntity)
                {
                    db.C_NodeTypeLoc.Remove(item);
                }

                C_UserPermissions[] userPerm = db.C_UserPermissions.Where(x => x.TypeId == id).ToArray();
                foreach (var item in userPerm)
                    db.C_UserPermissions.Remove(item);

                db.C_NodeType.Remove(typeEntity);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = typeEntity.CatVocId });
            }
            catch
            {
                ViewBag.Error = true;
                return View(typeLocEntity.SingleOrDefault(a => a.LanguageId == LangId));
            }
        }
    }
}