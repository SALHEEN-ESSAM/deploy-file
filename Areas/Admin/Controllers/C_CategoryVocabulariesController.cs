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
using System.Linq;
using System.Net;
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
    public class C_CategoryVocabulariesController : ControllerHelper
    {

        public ViewResult Index(string sortOrder, string currentFilter, int? page)
        {
            var moduleItems = from a in db.C_CategoryVocabularies select a;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParam = sortOrder == "TitleAsc" ? "TitleDesc" : "TitleAsc";

            switch (sortOrder)
            {
                case "TitleDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.Title);
                    break;
                default:
                    moduleItems = moduleItems.OrderBy(m => m.Title);
                    break;
            }

            int pageNumber = (page ?? 1);
            return View(moduleItems.ToPagedList(pageNumber, PageSize));
        }


        public ActionResult Create(int? id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            if (id == null)
            {
                return View();
            }
            else
            {
                C_CategoryVocabularies entity = db.C_CategoryVocabularies.Find(id);
                C_CategoryVocabulariesViewModel entityVM = new C_CategoryVocabulariesViewModel
                {
                    CatVocId = entity.CatVocId,
                    Title = entity.Title
                };
                return View(entityVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable CS0246 // The type or namespace name 'C_CategoryVocabulariesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(C_CategoryVocabulariesViewModel entityVM)
#pragma warning restore CS0246 // The type or namespace name 'C_CategoryVocabulariesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            ModelState.Remove("CatVocId");
            try
            {
                if (ModelState.IsValid)
                {
                    C_CategoryVocabularies entity = new C_CategoryVocabularies
                    {
                        CatVocId = entityVM.CatVocId,
                        Title = entityVM.Title
                    };
                    if (entityVM.CatVocId <= 0)
                    {
                        db.C_CategoryVocabularies.Add(entity);
                        db.SaveChanges();
                        return RedirectToAction("Create", new { isSuccess = true });
                    }
                    else
                    {
                        C_CategoryVocabularies items = db.C_CategoryVocabularies.Find(entityVM.CatVocId);
                        db.Entry(items).CurrentValues.SetValues(entity);
                        db.SaveChanges();
                        return RedirectToAction("Create", new { id = entityVM.CatVocId, isSuccess = true });
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
            C_CategoryVocabularies entity = db.C_CategoryVocabularies.Find(id);
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
            C_CategoryVocabularies entity = db.C_CategoryVocabularies.Find(id);
            try
            {
                db.C_CategoryVocabularies.Remove(entity);
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