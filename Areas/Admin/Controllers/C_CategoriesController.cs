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
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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
    public class C_CategoriesController : ControllerHelper
    {
        #region helpers

#pragma warning disable CS0246 // The type or namespace name 'CategoriesRepository' could not be found (are you missing a using directive or an assembly reference?)
        private CategoriesRepository catsDi;
#pragma warning restore CS0246 // The type or namespace name 'CategoriesRepository' could not be found (are you missing a using directive or an assembly reference?)
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            catsDi = new CategoriesRepository(LangId, db);
            base.OnActionExecuting(filterContext);
        }

        #endregion

        public ViewResult Index(string sortOrder, string currentFilter, int? page, int id, int? parentId)
        {
            ViewBag.CatVocabulary = db.C_CategoryVocabularies.Find(id);
            IQueryable<C_CategoriesLoc> moduleItems = catsDi.GetItemsLocList(catVocId: id, parentId: parentId, isActive: null);
            if (parentId != null)
                ViewBag.ParentCat = catsDi.SingleLocItem(parentId ?? 0);

            ViewBag.LangId = LangId;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParam = sortOrder == "TitleAsc" ? "TitleDesc" : "TitleAsc";

            switch (sortOrder)
            {
                case "TitleDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.Title);
                    break;
                case "TitleAsc":
                    moduleItems = moduleItems.OrderBy(m => m.Title);
                    break;
                default:
                    moduleItems = moduleItems.OrderByDescending(m => m.C_Categories.NumOrder).ThenByDescending(m => m.CatId);
                    break;
            }

            int pageNumber = (page ?? 1);
            return View(moduleItems.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Activate(int id, int? parentId, int? page)
        {
            C_Categories entity = catsDi.SingleItem(id);
            if (entity.IsActive)
                entity.IsActive = false;
            else
                entity.IsActive = true;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { id = entity.CatVocId, parentId = parentId, page = page });
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'C_CategoriesLoc' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult UpdateNum(IEnumerable<C_CategoriesLoc> entity, int catVocId, int? parentId)
#pragma warning restore CS0246 // The type or namespace name 'C_CategoriesLoc' could not be found (are you missing a using directive or an assembly reference?)
        {
            foreach (C_CategoriesLoc item in entity)
            {
                C_Categories cat = catsDi.SingleItem(item.CatId);
                cat.NumOrder = item.C_Categories.NumOrder;
                db.Entry(cat).State = EntityState.Modified;
            }

            db.SaveChanges();

            return RedirectToAction("Index", new { id = catVocId, parentId = parentId });
        }

        public ActionResult Create(int? id, bool? isSuccess, int catVocId, int? parentId, string command)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            if (id == null)
            {
                return View();
            }
            else
            {
                C_Categories entity = catsDi.SingleItem(id ?? 0);
                C_CategoriesLoc entityLoc = entity.C_CategoriesLoc.SingleOrDefault(x => x.LanguageId == LangId);
                C_CategoriesViewModel entityVM = new C_CategoriesViewModel
                {
                    CatId = entity.CatId,
                    CatVocId = entity.CatVocId,
                    ParentId = entity.ParentId,
                    ImageUrl = entity.ImageUrl,
                    IsActive = entity.IsActive,
                    LinkUrl = entity.LinkUrl,
                    HeaderImageUrl = entity.HeaderImageUrl
                };
                if (entityLoc != null)
                {
                    entityVM.LanguageId = LangId;
                    entityVM.Title = entityLoc.Title;
                    entityVM.Details = entityLoc.Details;
                    entityVM.Title2 = entityLoc.Title2;
                    entityVM.Brief = entityLoc.Brief;
                    entityVM.Brief2 = entityLoc.Brief2;
                    entityVM.LinkLocUrl = entityLoc.LinkLocUrl;
                }
                return View(entityVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable CS0246 // The type or namespace name 'C_CategoriesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(C_CategoriesViewModel entityVM, int catVocId, HttpPostedFileBase ImageUrl, HttpPostedFileBase HeaderImageUrl, int? parentId, string command)
#pragma warning restore CS0246 // The type or namespace name 'C_CategoriesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            ModelState.Remove("CatId");
            try
            {
                if (ModelState.IsValid)
                {
                    entityVM.LanguageId = LangId;
                    C_Categories entity = new C_Categories
                    {
                        CatId = entityVM.CatId,
                        CatVocId = catVocId,
                        ParentId = parentId,
                        ImageUrl = entityVM.ImageUrl,
                        HeaderImageUrl = entityVM.HeaderImageUrl,
                        IsActive = entityVM.IsActive,
                        LinkUrl = entityVM.LinkUrl
                    };
                    C_CategoriesLoc entityLoc = new C_CategoriesLoc
                    {
                        LanguageId = LangId,
                        CatId = entityVM.CatId,
                        Title = entityVM.Title,
                        Details = entityVM.Details,
                        Title2 = entityVM.Title2,
                        Brief = entityVM.Brief,
                        Brief2 = entityVM.Brief2,
                        LinkLocUrl = entityVM.LinkLocUrl,
                        IsTranslated = true
                    };
                    if (entityVM.CatId <= 0)
                    {
                        entity.NumOrder = 0;
                        entity.ImageUrl = ImageResizer.SaveImage("Uploads/Categories", ImageUrl);
                        entity.HeaderImageUrl = ImageResizer.SaveImage("Uploads/Categories", HeaderImageUrl);
                        db.C_Categories.Add(entity);

                        List<C_Languages> langs = db.C_Languages.ToList();
                        foreach (var item in langs)
                        {
                            C_CategoriesLoc LocsList = new C_CategoriesLoc
                            {
                                LanguageId = item.LanguageId,
                                CatId = entityVM.CatId,
                                Title = entityVM.Title,
                                Details = entityVM.Details,
                                Title2 = entityVM.Title2,
                                Brief = entityVM.Brief,
                                Brief2 = entityVM.Brief2,
                                LinkLocUrl = entityVM.LinkLocUrl,
                                IsTranslated = item.LanguageId == LangId ? true : false
                            };
                            db.C_CategoriesLoc.Add(LocsList);
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        C_CategoriesLoc currentEtityLoc = catsDi.SingleLocItem(entityVM.CatId);
                        C_Categories currentEntity = catsDi.SingleItem(entityVM.CatId);
                        entity.NumOrder = currentEntity.NumOrder;

                        if (ImageUrl != null)
                        {
                            HelperMethods.DeleteFile("Uploads/Categories/", currentEntity.ImageUrl);
                            entity.ImageUrl = ImageResizer.SaveImage("Uploads/Categories", ImageUrl);
                        }
                        else
                            entity.ImageUrl = currentEntity.ImageUrl;

                        if (HeaderImageUrl != null)
                        {
                            HelperMethods.DeleteFile("Uploads/Categories/", currentEntity.HeaderImageUrl);
                            entity.HeaderImageUrl = ImageResizer.SaveImage("Uploads/Categories", HeaderImageUrl);
                        }
                        else
                            entity.HeaderImageUrl = currentEntity.HeaderImageUrl;

                        db.Entry(currentEntity).CurrentValues.SetValues(entity);

                        if (currentEtityLoc != null)
                            db.Entry(currentEtityLoc).CurrentValues.SetValues(entityLoc);
                        else
                            db.C_CategoriesLoc.Add(entityLoc);
                        db.SaveChanges();
                    }
                    if (command == Amana.GlobalResources.Cpanel.Save)
                        return RedirectToAction("Create", new { id = entity.CatId, catVocId = catVocId, parentId = parentId, isSuccess = true });
                    else
                        return RedirectToAction("Create", new { id = "", catVocId = catVocId, parentId = parentId, isSuccess = true });
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
            C_CategoriesLoc entity = catsDi.SingleLocItem(id ?? 0);
            if (entity != null)
                return View(entity);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C_Categories entity = catsDi.SingleItem(id);
            int? parentId = entity.ParentId;
            try
            {
                List<ImagePathesHelper> imgsPaths = catsDi.DeleteItem(entity);
                foreach (var item in imgsPaths)
                    HelperMethods.DeleteFile(item.FoldersPath, item.FileName);
                return RedirectToAction("Index", new { id = entity.CatVocId, parentId = parentId });
            }
            catch
            {
                C_CategoriesLoc c = catsDi.SingleLocItem(id);
                ViewBag.Error = true;
                return View(c);
            }
        }

        public ActionResult DeleteImage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_Categories catEntity = catsDi.SingleItem(id ?? 0);
            if (catEntity == null)
            {
                return HttpNotFound();
            }

            HelperMethods.DeleteFile("/Uploads/Categories/", catEntity.ImageUrl);
            catEntity.ImageUrl = null;
            db.Entry(catEntity).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Create", new { id = catEntity.CatId, catVocId = catEntity.CatVocId, parentId = catEntity.ParentId });
        }
    }
}