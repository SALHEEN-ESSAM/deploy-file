using Amana.ControllerHelpers;
#pragma warning disable CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Repository;
#pragma warning restore CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System.Linq;
using System.Web.Mvc;

namespace Amana.Areas.Admin.Controllers
{
    public class C_BaseController : ControllerHelper
    {
        [ChildActionOnly]
        public PartialViewResult ToBeRevisedSamplesCount()
        {
            var currentUser = GetCurrentUser();
            int count = VisitsRepository.GetMySamples(currentUser.UserId, currentUser.RoleId).Count();

            if (count > 0)
                ViewData["unreadCount"] = count;
            return PartialView("UcUnreadItems");
        }

        //[ChildActionOnly]
        //public PartialViewResult UnreadItems(int typeId)
        //{
        //    int count = db.CW_Inquiries.Where(x => x.IsRead == false && x.TypeId == typeId).Count();
        //    if (count > 0)
        //        ViewData["unreadCount"] = count;
        //    return PartialView("UcUnreadItems");
        //}

        [ChildActionOnly]
        public PartialViewResult Languages()
        {
            return PartialView("UcLanguages", db.C_Languages);
        }

        [ChildActionOnly]
        public PartialViewResult NodeMenu()
        {
            ViewBag.LangId = LangId;
            return PartialView("UcNodeMenu", GetMenuRoles(null));
        }

        public ActionResult SignOut()
        {
            UserSignOut();
            return RedirectToAction("Index", "C_Login");
        }
    }
}
