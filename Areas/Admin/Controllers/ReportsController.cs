using Amana.ControllerHelpers;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
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
using Amana.ViewModels.Reports;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels.VisitsAndSamples;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
    public class ReportsController : ControllerHelper
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

        public ActionResult Index(int? locationId, int? month, int? year)
        {
            var locatiosLst = SettingsRepository.GetLocations();

            ViewData["LocatiosLst"] = new SelectList(locatiosLst, "ID", "Name", locationId);

            if (!month.HasValue || !year.HasValue)
            {
                month = HelperMethods.GetCurrentDateTime().Month;
                year = HelperMethods.GetCurrentDateTime().Year;
            }

            List<FactoryVisitsReport> result = VisitsRepository.GetFactoryVisitsReport(locationId, month.Value, year.Value);

            return View(result);
        }

        public ActionResult DailyVisits(string startDate)
        {
            DateTime? date = string.IsNullOrEmpty(startDate) ? HelperMethods.GetCurrentDateTime() : DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);


            List<Visits> result = VisitsRepository.SearchVisits(null, date, date, null, null, true);

            return View(result);
        }


        public ActionResult DailyVisitsReport(string startDate)
        {
            DateTime currentDate = string.IsNullOrEmpty(startDate) ? HelperMethods.GetCurrentDateTime() : DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var todayRep = db.RptDailyVisitsApproval.FirstOrDefault(a => DbFunctions.TruncateTime(a.ReportDate) == DbFunctions.TruncateTime(currentDate));
            ViewBag.TodayReportApproval = todayRep;
            return View();
        }
        public ActionResult ApproveReport(string startDate)
        {
            DateTime currentDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var currentUser = GetCurrentUser();
            var todayRep = db.RptDailyVisitsApproval.FirstOrDefault(a => DbFunctions.TruncateTime(a.ReportDate) == DbFunctions.TruncateTime(currentDate));
            if (currentUser.UserId == AppSettings.ProjectManagerId || currentUser.UserId == AppSettings.ProjectSupervisorId)
            {
                if (todayRep == null)
                {
                    RptDailyVisitsApproval item = new RptDailyVisitsApproval();
                    item.ReportDate = currentDate;
                    item.IsApprovedByProjectManager = (currentUser.UserId == AppSettings.ProjectManagerId);
                    item.IsApprovedByProjectSupervisor = (currentUser.UserId == AppSettings.ProjectSupervisorId);
                    db.RptDailyVisitsApproval.Add(item);
                }
                else
                {
                    if (currentUser.UserId == AppSettings.ProjectManagerId)
                        todayRep.IsApprovedByProjectManager = true;

                    if (currentUser.UserId == AppSettings.ProjectSupervisorId)
                        todayRep.IsApprovedByProjectSupervisor = true;

                    db.Entry(todayRep).State = EntityState.Modified;
                }
            }

            db.SaveChanges();

            return RedirectToAction("DailyVisitsReport", new { startDate = startDate });
        }
        public ActionResult MonthlyWarningsReport()
        {
            return View();
        }


        public ActionResult SamplesSummaryReport()
        {
            ViewData["LocationsLst"] = new SelectList(SettingsRepository.GetLocations(), "ID", "Name");
            ViewData["FactoriesLst"] = new SelectList(FactoriesRepository.GetAllFactories(null, factoryType: (int)FactoryTypesEnum.Concrete), "ID", "Name", "");

            return View();
        }
        public ActionResult SampleReport(int visitId)
        {
            return View();
        }

        public ActionResult MonthlyVisitsReport()
        {
            return View();
        }

        public ActionResult ClassificationReport()
        {
            ViewData["LocationsLst"] = new SelectList(SettingsRepository.GetLocations(), "ID", "Name");

            ViewData["FactoriesLst"] = new SelectList(FactoriesRepository.GetAllFactories(null, factoryType: (int)FactoryTypesEnum.Concrete), "ID", "Name", "");
            ViewData["PeriodsLst"] = new SelectList(SettingsRepository.GetTimePeriods(false), "TimePeriodId", "Title", "");

            return View();
        }
        public ActionResult ClassificationSummaryReport(int? periodId, int? repType)
        {
            ViewData["PeriodsLst"] = new SelectList(SettingsRepository.GetTimePeriods(false), "TimePeriodId", "Title", periodId);

            return View();
        }

        public ActionResult SuppliersReport()
        {
            ViewData["PeriodsLst"] = new SelectList(SettingsRepository.GetTimePeriods(false), "TimePeriodId", "Title", "");

            return View();
        }
        public ActionResult MaterialsReport()
        {
            ViewData["PeriodsLst"] = new SelectList(SettingsRepository.GetTimePeriods(false), "TimePeriodId", "Title", "");

            return View();
        }
        public ActionResult RptApprovalView(int reportTypeId, int? timePeriodId)
        {
            var rptStng = db.RptSettings.Include(a => a.C_UserItems).Where(a => a.ReportType == reportTypeId).OrderBy(a => a.NumOrder);
            var rptApprovals = db.RptApproval.Where(a => a.ReportType == reportTypeId && a.TimePeriodId == timePeriodId);
            List<RptApprovalViewModel> vmLst = new List<RptApprovalViewModel>();
            int currentUserId = GetUserId();

            foreach (var item in rptStng)
            {
                var exist = rptApprovals.FirstOrDefault(a => a.EmployeeId == item.EmployeeId);
                RptApprovalViewModel vm = new RptApprovalViewModel();
                vm.EmployeeId = item.EmployeeId;
                vm.EmployeeName = item.C_UserItems.Name;
                vm.NumOrder = item.NumOrder;
                vm.TimePeriodId = timePeriodId.Value;
                if (exist != null)
                {
                    vm.IsApproved = exist.IsApproved;
                    vm.Reason = exist.Reason;
                    vm.ApprovalDate = exist.ApprovalDate;
                }
                if (item.EmployeeId == currentUserId)
                {
                    vm.CanApprove = !vm.IsApproved;
                    var currentEmpStng = rptStng.FirstOrDefault(a => a.EmployeeId == item.EmployeeId);
                    if (currentEmpStng != null)
                    {
                        if (vmLst.Any(a => a.NumOrder < currentEmpStng.NumOrder && a.IsApproved == false))
                            vm.CanApprove = false;
                        else
                            vm.CanApprove = true;
                    }
                }
                vmLst.Add(vm);
            }

            ViewBag.ReportTypeId = reportTypeId;
            return PartialView("_RptApproval", vmLst);
        }

        public ActionResult RptApproveReport(int reportTypeId, int? timePeriodId)
        {
            int currentUserId = GetUserId();

            var rptStng = db.RptSettings.Include(a => a.C_UserItems).Where(a => a.ReportType == reportTypeId).OrderBy(a => a.NumOrder);
            var exist = db.RptApproval.FirstOrDefault(a => a.ReportType == reportTypeId && a.TimePeriodId == timePeriodId && a.EmployeeId == currentUserId);
            if (rptStng.Any(a => a.EmployeeId == currentUserId) && exist == null)
            {

                RptApproval item = new RptApproval();
                item.ApprovalDate = DateTime.Now;
                item.EmployeeId = currentUserId;
                item.IsApproved = true;
                item.ReportType = reportTypeId;
                item.TimePeriodId = timePeriodId;

                db.RptApproval.Add(item);
                db.SaveChanges();
            }

            return RedirectToAction("ClassificationSummaryReport", new { periodId = timePeriodId, repType = reportTypeId });
        }


    }
}