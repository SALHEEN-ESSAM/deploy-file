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
using System.Globalization;
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
    public class C_LanguagesController : ControllerHelper
    {
        public ViewResult Index(string sortOrder, string currentFilter, int? page)
        {
            var moduleItems = from a in db.C_Languages select a;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParam = sortOrder == "TitleAsc" ? "TitleDesc" : "TitleAsc";

            switch (sortOrder)
            {
                case "TitleDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.Name);
                    break;
                default:
                    moduleItems = moduleItems.OrderBy(m => m.Name);
                    break;
            }

            int pageNumber = (page ?? 1);
            return View(moduleItems.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Create(int? id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            cultures.OrderBy(x => x.NativeName);

            if (id == null)
            {
                ViewBag.Cultures = new SelectList(cultures, "Name", "NativeName", " ");
                return View();
            }
            else
            {
                C_Languages entity = db.C_Languages.Find(id);
                C_LanguagesViewModel entityVM = new C_LanguagesViewModel
                {
                    LanguageId = entity.LanguageId,
                    Culture = entity.Culture.Trim(),
                    Name = entity.Name
                };
                ViewBag.Cultures = new SelectList(cultures, "Name", "NativeName", entity.Culture.Trim());
                return View(entityVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable CS0246 // The type or namespace name 'C_LanguagesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(C_LanguagesViewModel entityVM)
#pragma warning restore CS0246 // The type or namespace name 'C_LanguagesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            ModelState.Remove("LanguageId");
            try
            {
                if (ModelState.IsValid)
                {
                    C_Languages entity = new C_Languages
                    {
                        LanguageId = entityVM.LanguageId,
                        Culture = entityVM.Culture,
                        Name = entityVM.Name
                    };
                    if (entityVM.LanguageId <= 0)
                    {
                        db.C_Languages.Add(entity);
                        db.SaveChanges();
                        return RedirectToAction("Create", new { isSuccess = true });
                    }
                    else
                    {
                        C_Languages currentEntity = db.C_Languages.Find(entityVM.LanguageId);
                        db.Entry(currentEntity).CurrentValues.SetValues(entity);
                        db.SaveChanges();
                        return RedirectToAction("Create", new { id = entityVM.LanguageId, isSuccess = true });
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
            C_Languages entity = db.C_Languages.Find(id);
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
            C_Languages entity = db.C_Languages.Find(id);
            try
            {
                db.C_Languages.Remove(entity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = true;
                return View(entity);
            }
        }
    }
}