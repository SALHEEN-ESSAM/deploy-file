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
using System.Data.Entity;
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
    public class C_GeneralSettingsController : ControllerHelper
    {
        public ActionResult Index(bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;
            else if (isSuccess == false)
                ViewBag.Success = false;

            C_GeneralSettings entity = db.C_GeneralSettings.SingleOrDefault();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable CS0246 // The type or namespace name 'C_GeneralSettings' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(C_GeneralSettings entity)
#pragma warning restore CS0246 // The type or namespace name 'C_GeneralSettings' could not be found (are you missing a using directive or an assembly reference?)
        {
            entity.SettingId = 1;
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'C_GeneralSettings' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult SendTestEmail(C_GeneralSettings entity)
#pragma warning restore CS0246 // The type or namespace name 'C_GeneralSettings' could not be found (are you missing a using directive or an assembly reference?)
        {
            //try
            //{
            HelperMethods.SendEmail("Email success!", entity.Email, "Email success!", db.C_GeneralSettings.SingleOrDefault());
            return RedirectToAction("Index", new { isSuccess = true });
            //}
            //catch
            //{
            //    return RedirectToAction("Index", new { isSuccess = false });
            //}
        }
    }
}