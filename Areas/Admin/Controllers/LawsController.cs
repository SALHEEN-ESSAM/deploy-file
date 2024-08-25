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
using Amana.ViewModels.Laws;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
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
    public class LawsController : ControllerHelper
    {
#pragma warning disable CS0246 // The type or namespace name 'AmanaConcreteDBEntities1' could not be found (are you missing a using directive or an assembly reference?)
        readonly AmanaConcreteDBEntities1 _context = new AmanaConcreteDBEntities1();
#pragma warning restore CS0246 // The type or namespace name 'AmanaConcreteDBEntities1' could not be found (are you missing a using directive or an assembly reference?)

        public ActionResult Index()
        {
            return View(LawsRepository.GetAllLaws());
        }
        public ActionResult MyLaws()
        {
            return View(LawsRepository.GetAllLaws(true));
        }
        public ActionResult AddLaw(int? id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;


            LawViewModel itemVm = new LawViewModel();

            if (id.HasValue)
            {
                var item = LawsRepository.GetLawById(id.Value);
                itemVm.LawId = item.LawId;
                itemVm.Details = item.Details;
                itemVm.Title = item.Title;
                itemVm.IsActive = item.IsActive;
                itemVm.FileUrl = item.FileUrl;
            }

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'LawViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddLaw(LawViewModel itemVm, HttpPostedFileBase FileUrl)
#pragma warning restore CS0246 // The type or namespace name 'LawViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            Laws item = new Laws();

            if (itemVm.LawId > 0)
            {
                item = LawsRepository.GetLawById(itemVm.LawId.Value);
            }
            else
            {
                item.CreatedBy = GetUserId();
                item.CreatedWhen = HelperMethods.GetCurrentDateTime();
            }

            item.IsActive = itemVm.IsActive;
            item.Title = itemVm.Title;
            item.Details = itemVm.Details;

            if (FileUrl != null)
            {
                if (!string.IsNullOrEmpty(item.FileUrl))
                    HelperMethods.DeleteFile("Uploads/Laws/", item.FileUrl);

                item.FileUrl = HelperMethods.SaveFile("Uploads/Laws", FileUrl);
            }

            LawsRepository.SaveLawItem(item);

            return RedirectToAction("AddLaw", new { id = item.LawId, isSuccess = true });

        }

        public ActionResult DeleteLaw(int id)
        {

            Laws prop = LawsRepository.GetLawById(id);

            LawsRepository.DeleteItem(prop);


            return RedirectToAction("Index", new { isSuccess = true });

        }


    }
}