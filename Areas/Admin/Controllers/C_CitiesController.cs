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
    public class C_CitiesController : ControllerHelper
    {
        public ViewResult Index(string sortOrder, string searchString, string currentFilter, int id, int? page)
        {
            IQueryable<C_CitiesLoc> moduleItems = db.C_CitiesLoc.Include(x => x.C_Cities).Where(x => x.C_Cities.CountryId == id && x.LanguageId == LangId);

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
                default:
                    moduleItems = moduleItems.OrderBy(m => m.CityId);
                    break;
            }

            if (!string.IsNullOrEmpty(searchString))
                moduleItems = moduleItems.Where(m => m.Title.ToUpper().Contains(searchString.ToUpper()));

            int pageNumber = (page ?? 1);
            return View(moduleItems.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Create(int? id, int countryId, bool? isSuccess, string command)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            if (id == null)
            {
                return View();
            }
            else
            {
                C_Cities entity = db.C_Cities.Include(x => x.C_CitiesLoc).SingleOrDefault(x => x.CityId == id);
                C_CitiesLoc entityLoc = entity.C_CitiesLoc.SingleOrDefault(x => x.LanguageId == LangId);
                C_CitiesViewModel entityVM = new C_CitiesViewModel
                {
                    CityId = entity.CityId,
                    CountryId = entity.CountryId
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
#pragma warning disable CS0246 // The type or namespace name 'C_CitiesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(C_CitiesViewModel entityVM, int countryId, string command)
#pragma warning restore CS0246 // The type or namespace name 'C_CitiesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            ModelState.Remove("CityId");
            ModelState.Remove("CountryId");
            try
            {
                if (ModelState.IsValid)
                {
                    entityVM.LanguageId = LangId;
                    C_Cities entity = new C_Cities
                    {
                        CityId = entityVM.CityId,
                        CountryId = entityVM.CountryId
                    };
                    C_CitiesLoc entityLoc = new C_CitiesLoc
                    {
                        LanguageId = LangId,
                        CityId = entityVM.CityId,
                        Title = entityVM.Title,
                        IsTranslated = true
                    };
                    if (entityVM.CityId <= 0)
                    {
                        db.C_Cities.Add(entity);

                        List<C_Languages> langs = db.C_Languages.ToList();
                        foreach (var item in langs)
                        {
                            C_CitiesLoc LocsList = new C_CitiesLoc
                            {
                                LanguageId = item.LanguageId,
                                CityId = entityVM.CityId,
                                Title = entityVM.Title,
                                IsTranslated = item.LanguageId == LangId ? true : false
                            };
                            db.C_CitiesLoc.Add(LocsList);
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        C_CitiesLoc currentEtityLoc = db.C_CitiesLoc.SingleOrDefault(x => x.CityId == entityVM.CityId && x.LanguageId == LangId);
                        C_Cities currentEntity = db.C_Cities.Find(entityVM.CityId);

                        db.Entry(currentEntity).CurrentValues.SetValues(entity);

                        if (currentEtityLoc != null)
                            db.Entry(currentEtityLoc).CurrentValues.SetValues(entityLoc);
                        else
                            db.C_CitiesLoc.Add(entityLoc);
                        db.SaveChanges();
                    }
                    if (command == Amana.GlobalResources.Cpanel.Save)
                        return RedirectToAction("Create", new { id = entity.CityId, countryId = countryId, isSuccess = true });
                    else
                        return RedirectToAction("Create", new { id = "", countryId = countryId, isSuccess = true });
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
            C_CitiesLoc entity = db.C_CitiesLoc.SingleOrDefault(x => x.CityId == id && x.LanguageId == LangId);
            if (entity != null)
                return View(entity);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C_Cities entity = db.C_Cities.Find(id);
            List<C_CitiesLoc> entityLoc = db.C_CitiesLoc.Where(x => x.CityId == id).ToList();
            try
            {
                foreach (var item in entityLoc)
                {
                    db.C_CitiesLoc.Remove(item);
                }

                db.C_Cities.Remove(entity);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = entity.CountryId });
            }
            catch
            {
                C_CitiesLoc c = (from b in entityLoc where b.LanguageId == LangId select b).SingleOrDefault();
                ViewBag.Error = true;
                return View(c);
            }
        }
    }
}