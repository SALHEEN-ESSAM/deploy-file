using Amana.ControllerHelpers;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Enums;
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
using Amana.ViewModels.VisitsAndSamples;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Amana.Areas.Admin.Controllers
{
#pragma warning disable CS0246 // The type or namespace name 'IsBackend' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CustomAuthorize' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CustomAuthorizeAttribute' could not be found (are you missing a using directive or an assembly reference?)
    [CustomAuthorize(IsBackend = true)]
#pragma warning restore CS0246 // The type or namespace name 'CustomAuthorizeAttribute' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'CustomAuthorize' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IsBackend' could not be found (are you missing a using directive or an assembly reference?)
    public class VisitsController : ControllerHelper
    {
        #region helpers


#pragma warning disable CS0246 // The type or namespace name 'UsersRepository' could not be found (are you missing a using directive or an assembly reference?)
        private UsersRepository usersDi;
#pragma warning restore CS0246 // The type or namespace name 'UsersRepository' could not be found (are you missing a using directive or an assembly reference?)
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            usersDi = new UsersRepository(LangId, db);
            base.OnActionExecuting(filterContext);
        }

        #endregion




        public ActionResult Visits(int? locationId, string startDate, string endDate, int? userId, int? factoryId, bool? isDone, bool? isSuccess, int? error)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            if (error == 1)
                ViewBag.CustomError = "لا يمكن حذف الزيارة لوجود بيانات متعلقة بها";

            var locatiosLst = SettingsRepository.GetLocations();
            var factories = FactoriesRepository.GetAllFactories();
            var users = usersDi.GetItemsList(roleId: 2, isActive: null);

            ViewData["LocationsLst"] = new SelectList(locatiosLst, "ID", "Name", locationId);
            ViewData["FactoriesLst"] = new SelectList(factories, "ID", "Name", factoryId);
            ViewData["UsersLst"] = new SelectList(users, "UserId", "Name", userId);


            DateTime? stDate = string.IsNullOrEmpty(startDate) ? HelperMethods.GetFirstDayInMonth(HelperMethods.GetCurrentDateTime()) : DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? eDate = string.IsNullOrEmpty(endDate) ? HelperMethods.GetLastDayInMonth(HelperMethods.GetCurrentDateTime()) : DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            List<Visits> result = VisitsRepository.SearchVisits(locationId, stDate, eDate, userId, factoryId, isDone);

            return View(result);
        }
        public ActionResult UsersLocations(string startDate, string endDate, int? userId, bool? isSuccess, int? error)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            if (error == 1)
                ViewBag.CustomError = "لا يمكن حذف الزيارة لوجود بيانات متعلقة بها";

            var users = usersDi.GetItemsList(roleId: 2, isActive: null).Where(a => a.IsAvailable == true).ToList();

            ViewData["UsersLst"] = new SelectList(users, "UserId", "Name", userId);

            ViewBag.Users = users;

            DateTime? stDate = string.IsNullOrEmpty(startDate) ? HelperMethods.GetFirstDayInMonth(HelperMethods.GetCurrentDateTime()) : DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? eDate = string.IsNullOrEmpty(endDate) ? HelperMethods.GetLastDayInMonth(HelperMethods.GetCurrentDateTime()) : DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            ViewBag.UserLocation = db.UserLocations.Include(a => a.IdxLocations).Where(a => DbFunctions.TruncateTime(a.DayDate) >= DbFunctions.TruncateTime(stDate) && DbFunctions.TruncateTime(a.DayDate) <= DbFunctions.TruncateTime(eDate)).ToList();

            List<Visits> result = VisitsRepository.SearchVisits(null, stDate, eDate, userId, null, null);

            return View(result);
        }

        //public ActionResult HomeVisits()
        //{
        //    var currentUser = GetCurrentUser();


        //    List<Visits> result = VisitsRepository.SearchVisits(null, null, null, currentUser.UserId, null, false);

        //    return View(result);
        //}

        public ActionResult Create(int? id, bool? isSuccess, int? error)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            if (error == 1)
                ViewBag.CustomError = "لا يمكن اضافة زيارات يومي الخميس والجمعة";
            //if (error == 2)
              //  ViewBag.CustomError = "لا يمكن اضافة زيارات أخرى لهذا المصنع خلال هذا الشهر";

            var locations = SettingsRepository.GetLocations();
            var factories = FactoriesRepository.GetAllFactories(null, factoryType: (int)FactoryTypesEnum.Concrete, isClosed: false);
            var users = usersDi.GetItemsList(roleId: 2, isActive: null).Where(a => a.IsAvailable == true);

            VisitsViewModel itemVm = new VisitsViewModel();

            if (id.HasValue)
            {
                var item = VisitsRepository.GetVisitById(id.Value);
                itemVm.VisitId = item.VisitId;
                itemVm.FactoryId = item.FactoryId;
                itemVm.UserId = item.UserId;
                itemVm.VisitDate = item.VisitDate.ToString("dd/MM/yyyy");
            }

            ViewData["FactoriesLst"] = new SelectList(factories, "ID", "Name", itemVm.FactoryId);
            ViewData["LocationsLst"] = new SelectList(locations, "ID", "Name");
            ViewData["UsersLst"] = new SelectList(users, "UserId", "Name", itemVm.UserId);

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'VisitsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(VisitsViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'VisitsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            DateTime visitDate = DateTime.ParseExact(itemVm.VisitDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (visitDate.DayOfWeek == DayOfWeek.Thursday || visitDate.DayOfWeek == DayOfWeek.Friday)
            {
                return RedirectToAction("Create", new { id = itemVm.VisitId, error = 1 });

            }
            else if (!itemVm.VisitId.HasValue && !VisitsRepository.CanAddVisits(visitDate, itemVm.FactoryId))
            {
                return RedirectToAction("Create", new { id = itemVm.VisitId, error = 2 });

            }
            else
            {
                Visits item = new Models.Entities.Visits();
                if (itemVm.VisitId > 0)
                {
                    item = VisitsRepository.GetVisitById(itemVm.VisitId.Value);
                }
                else
                {
                    item.CreatedWhen = HelperMethods.GetCurrentDateTime();
                    item.CreatedBy = GetUserId();
                    item.IsVisitDone = false;
                }

                item.VisitDate = visitDate;
                item.UserId = itemVm.UserId;
                item.FactoryId = itemVm.FactoryId;
                VisitsRepository.SaveVisit(item);

                return RedirectToAction("Visits", new { isSuccess = true });
            }


        }

        //public ActionResult Delete(int? id, int? page, int? catId, int? parentId)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    C_NodeLoc entityLoc = nodeDi.SingleLocItem(id.Value);
        //    if (entityLoc != null)
        //        return View(entityLoc);
        //    else
        //        return RedirectToAction("Index", "Home");
        //}

        public ActionResult CreateMonthly(bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            int monthId = HelperMethods.GetCurrentDateTime().Month;
            int yearId = HelperMethods.GetCurrentDateTime().Year;

            AutoGeneratedVisits itemVm = VisitsRepository.GetAutoGeneratedVisitsItem(monthId, yearId);
            ViewBag.CreateMonthlyVisits = false;
            if (itemVm == null)
            {
                ViewBag.CreateMonthlyVisits = true;
                itemVm = new AutoGeneratedVisits { MonthId = monthId, YearId = yearId, MaxLocationVisitsInDay = 3, MaxVisitsInMonth = 130 };
            }

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'AutoGeneratedVisits' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult CreateMonthly(AutoGeneratedVisits itemVm)
#pragma warning restore CS0246 // The type or namespace name 'AutoGeneratedVisits' could not be found (are you missing a using directive or an assembly reference?)
        {
            VisitsRepository.SaveAutoGeneratedVisitsItem(itemVm);

            VisitsRepository.GenerateMonthVisits(itemVm);

            return RedirectToAction("CreateMonthly", new { isSuccess = true });

        }

        public ActionResult DeleteVisit(int id)
        {
            var visit = VisitsRepository.GetVisitById(id);
            bool success = VisitsRepository.DeleteVisit(visit);
            if (success == true)
                return RedirectToAction("Visits", new { isSuccess = true });
            else
                return RedirectToAction("Visits", new { error = 1 });

        }

        public ActionResult TodayVisits(bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var currentUser = GetCurrentUser();
            List<Visits> result = VisitsRepository.GetTodayVisits(currentUser.UserId, HelperMethods.GetCurrentDateTime());

            return View(result);
        }


        public ActionResult DelayVisit(int? id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;


            DelayVisitViewModel itemVm = new DelayVisitViewModel();

            if (id.HasValue)
            {
                var item = VisitsRepository.GetVisitById(id.Value, fullData: true);
                itemVm.VisitId = item.VisitId;
                ViewBag.VisitInfo = item;

                var alternativeVisit = VisitsRepository.GetAlternativeVisit(item);
                if (alternativeVisit != null)
                {
                    itemVm.AlternativeVisitId = alternativeVisit.VisitId;
                    ViewBag.AlternativeVisitInfo = alternativeVisit;
                }
            }

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'DelayVisitViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult DelayVisit(DelayVisitViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'DelayVisitViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {

            Visits currentVisit = VisitsRepository.GetVisitById(itemVm.VisitId);
            int cvFactoryId = currentVisit.FactoryId;

            if (itemVm.AlternativeVisitId.HasValue)
            {
                Visits alternativeVisit = VisitsRepository.GetVisitById(itemVm.AlternativeVisitId.Value);
                currentVisit.FactoryId = alternativeVisit.FactoryId;
                VisitsRepository.SaveVisit(currentVisit);

                alternativeVisit.FactoryId = cvFactoryId;
                VisitsRepository.SaveVisit(alternativeVisit);
            }
            else
            {
                if (currentVisit.VisitDate.DayOfWeek == DayOfWeek.Wednesday)
                    currentVisit.VisitDate = currentVisit.VisitDate.AddDays(3);
                else
                    currentVisit.VisitDate = currentVisit.VisitDate.AddDays(1);

                VisitsRepository.SaveVisit(currentVisit);
            }


            VisitNotes note = new VisitNotes { CreatedBy = GetUserId(), CreatedWhen = HelperMethods.GetCurrentDateTime(), NoteText = itemVm.DelayNote, VisitId = currentVisit.VisitId };
            VisitsRepository.SaveNote(note);

            return RedirectToAction("TodayVisits", new { isSuccess = true, success = 1 });

        }

        public ActionResult EditUserLocation(int userId, DateTime dt, bool? isSuccess, int? error)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            if (error == 1)
                ViewBag.CustomError = "لا يمكن اضافة زيارات يومي الخميس والجمعة";


            var locations = SettingsRepository.GetLocations();

            UserLocationViewModel itemVm = new UserLocationViewModel();
            itemVm.DayDate = dt;
            itemVm.UserID = userId;
            var userInfo = new UsersRepository(LangId, db).SingleItem(userId);
            itemVm.UserName = userInfo.Name;

            var item = db.UserLocations.FirstOrDefault(a => DbFunctions.TruncateTime(a.DayDate) == DbFunctions.TruncateTime(dt) && a.UserId == userId);

            if (item != null)
            {
                itemVm.ID = item.ID;
                itemVm.LocationId = item.LocationId;

            }

            ViewData["LocationsLst"] = new SelectList(locations, "ID", "Name");

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'UserLocationViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult EditUserLocation(UserLocationViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'UserLocationViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            UserLocations uc = new UserLocations();
            uc.UserId = itemVm.UserID;
            uc.DayDate = itemVm.DayDate;

            if (itemVm.ID > 0)
            {
                uc = db.UserLocations.FirstOrDefault(a => a.ID == itemVm.ID);
            }
            uc.LocationId = itemVm.LocationId;

            if (uc.ID > 0)
            {
                db.Entry(uc).State = EntityState.Modified;
            }
            else
                db.UserLocations.Add(uc);

            db.SaveChanges();

            return RedirectToAction("UsersLocations", new { isSuccess = true });
        }


    }


}