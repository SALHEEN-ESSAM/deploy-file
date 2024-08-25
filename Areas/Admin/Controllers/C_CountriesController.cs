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
    public class C_CountriesController : ControllerHelper
    {
        public ViewResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            IQueryable<C_CountriesLoc> moduleItems = db.C_CountriesLoc.Include(x => x.C_Countries).Where(x => x.LanguageId == LangId);

            ViewBag.LangId = LangId;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParam = sortOrder == "DateAsc" ? "DateDesc" : "DateAsc";
            ViewBag.TitleSortParam = sortOrder == "TitleAsc" ? "TitleDesc" : "TitleAsc";
            ViewBag.ActiveSortParam = sortOrder == "ActiveAsc" ? "ActiveDesc" : "ActiveAsc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            switch (sortOrder)
            {
                case "TitleDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.Title);
                    break;
                case "TitleAsc":
                    moduleItems = moduleItems.OrderBy(m => m.Title);
                    break;
                case "ActiveAsc":
                    moduleItems = moduleItems.OrderBy(m => m.C_Countries.IsActive);
                    break;
                case "ActiveDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.C_Countries.IsActive);
                    break;
                default:
                    moduleItems = moduleItems.OrderByDescending(m => m.C_Countries.IsActive).ThenByDescending(m => m.CountryId);
                    break;
            }

            if (!string.IsNullOrEmpty(searchString))
                moduleItems = moduleItems.Where(m => m.Title.ToUpper().Contains(searchString.ToUpper()));

            int pageNumber = (page ?? 1);
            return View(moduleItems.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Activate(int id, int? page)
        {
            C_Countries entity = db.C_Countries.Find(id);
            if (entity.IsActive)
                entity.IsActive = false;
            else
                entity.IsActive = true;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { page = page });
        }

        public ActionResult Create(int? id, bool? isSuccess, string command)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            if (id == null)
            {
                return View();
            }
            else
            {
                C_Countries entity = db.C_Countries.Include(x => x.C_CountriesLoc).SingleOrDefault(x => x.CountryId == id);
                C_CountriesLoc entityLoc = entity.C_CountriesLoc.SingleOrDefault(x => x.LanguageId == LangId);
                C_CountriesViewModel entityVM = new C_CountriesViewModel
                {
                    CountryId = entity.CountryId,
                    Code = entity.Code,
                    IsActive = entity.IsActive
                };
                if (entityLoc != null)
                {
                    entityVM.LanguageId = LangId;
                    entityVM.Title = entityLoc.Title;
                }
                return View(entityVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable CS0246 // The type or namespace name 'C_CountriesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(C_CountriesViewModel entityVM, string command)
#pragma warning restore CS0246 // The type or namespace name 'C_CountriesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            ModelState.Remove("CountryId");
            try
            {
                if (ModelState.IsValid)
                {
                    entityVM.LanguageId = LangId;
                    C_Countries entity = new C_Countries
                    {
                        CountryId = entityVM.CountryId,
                        Code = entityVM.Code,
                        IsActive = entityVM.IsActive
                    };
                    C_CountriesLoc entityLoc = new C_CountriesLoc
                    {
                        LanguageId = LangId,
                        CountryId = entityVM.CountryId,
                        Title = entityVM.Title,
                        IsTranslated = true
                    };
                    if (entityVM.CountryId <= 0)
                    {
                        db.C_Countries.Add(entity);

                        List<C_Languages> langs = db.C_Languages.ToList();
                        foreach (var item in langs)
                        {
                            C_CountriesLoc LocsList = new C_CountriesLoc
                            {
                                LanguageId = item.LanguageId,
                                CountryId = entityVM.CountryId,
                                Title = entityVM.Title,
                                IsTranslated = item.LanguageId == LangId ? true : false
                            };
                            db.C_CountriesLoc.Add(LocsList);
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        C_CountriesLoc currentEtityLoc = db.C_CountriesLoc.SingleOrDefault(x => x.CountryId == entityVM.CountryId && x.LanguageId == LangId);
                        C_Countries currentEntity = db.C_Countries.Find(entityVM.CountryId);

                        db.Entry(currentEntity).CurrentValues.SetValues(entity);

                        if (currentEtityLoc != null)
                            db.Entry(currentEtityLoc).CurrentValues.SetValues(entityLoc);
                        else
                            db.C_CountriesLoc.Add(entityLoc);
                        db.SaveChanges();
                    }
                    if (command == Amana.GlobalResources.Cpanel.Save)
                        return RedirectToAction("Create", new { id = entity.CountryId, isSuccess = true });
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
            C_CountriesLoc entity = db.C_CountriesLoc.SingleOrDefault(x => x.CountryId == id && x.LanguageId == LangId);
            if (entity != null)
                return View(entity);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C_Countries entity = db.C_Countries.Find(id);
            List<C_CountriesLoc> entityLoc = db.C_CountriesLoc.Where(x => x.CountryId == id).ToList();
            try
            {
                foreach (var item in entityLoc)
                {
                    db.C_CountriesLoc.Remove(item);
                }

                db.C_Countries.Remove(entity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                C_CountriesLoc c = (from b in entityLoc where b.LanguageId == LangId select b).SingleOrDefault();
                ViewBag.Error = true;
                return View(c);
            }
        }
    }
}