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
using Amana.ViewModels;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
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
    public class HomeController : ControllerHelper
    {

        public ActionResult Index()
        {

            //var items = db.VisitsSample.Include(a => a.Visits).Where(a => a.IsFactorySamplePrepared == true);
            //foreach (var item in items)
            //{
            //    item.FactoryBreakingDate28Day = item.Visits.VisitDate.AddDays(28);
            //    db.Entry(item).State = EntityState.Modified;
            //}
            //db.SaveChanges();

            var currentUser = GetCurrentUser();
            if (currentUser.RoleId == 2)
            {
                ViewBag.ToBeRevisedCount = VisitsRepository.GetMySamples(currentUser.UserId, currentUser.RoleId).Count();
                int visitCount = VisitsRepository.GetTodayVisits(currentUser.UserId, HelperMethods.GetCurrentDateTime()).Count();
                ViewBag.VisitsCount = visitCount;
                IdxLocations todayLocation = VisitsRepository.GetTodayUserLocation(currentUser.UserId, HelperMethods.GetCurrentDateTime());
                if (todayLocation != null)
                {
                    ViewBag.ToBeRecievedCount = VisitsRepository.SearchSamples(null, null, null, null, (int)SampleStatusEnum.ToBeRecievedFromFactory, todayLocation.ID).Count;
                    ViewBag.FactoryBreakingCount = VisitsRepository.GetFactoryBreakingDateSamples(HelperMethods.GetCurrentDateTime(), todayLocation.ID).Count;
                }
                else
                {
                    ViewBag.ToBeRecievedCount = VisitsRepository.SearchSamples(null, null, currentUser.UserId, null, (int)SampleStatusEnum.ToBeRecievedFromFactory, null).Count;

                }
            }
            if (currentUser.RoleId == 1002)
            {
                Factories factory = FactoriesRepository.GetFactoryByUserId(GetUserId());
                if (factory.IsTemporaryClosed == true)
                    ViewBag.IsFactoryClosed = true;
                List<FactoryWarnings> result = new List<FactoryWarnings>();
                if (factory != null)
                    ViewBag.WarningCount = WarningsRepository.GetAllWarnings(isGeneralManagerApproved: true, factoryId: factory.ID).Count;

            }
            ViewBag.LawsCount = LawsRepository.GetAllLaws(true).Count();
            if (currentUser.RoleId == 1007 || currentUser.RoleId == 1006 || currentUser.RoleId == null)
            {
                int wCount = 0;
                int reportsCount = 0;
                DateTime currentDate = HelperMethods.GetCurrentDateTime();
                var todayRep = db.RptDailyVisitsApproval.FirstOrDefault(a => DbFunctions.TruncateTime(a.ReportDate) == DbFunctions.TruncateTime(currentDate));
                if (currentUser.UserId == AppSettings.ProjectManagerId)
                {
                    wCount = WarningsRepository.GetAllWarnings(isProjectManagerApproved: false).Count;
                    if (todayRep == null)
                        reportsCount = 1;
                    else if (!todayRep.IsApprovedByProjectManager)
                        reportsCount = 1;
                }
                else if (currentUser.UserId == AppSettings.ProjectSupervisorId)
                {
                    wCount = WarningsRepository.GetAllWarnings(isProjectManagerApproved: true, isProjectSupervisorApproved: false).Count;
                    if (todayRep == null)
                        reportsCount = 1;
                    else if (!todayRep.IsApprovedByProjectSupervisor)
                        reportsCount = 1;
                }
                else if (currentUser.UserId == AppSettings.GeneralManagerId)
                {
                    wCount = WarningsRepository.GetAllWarnings(isProjectManagerApproved: true, isProjectSupervisorApproved: true, isGeneralManagerApproved: false).Count;
                }
                else
                    wCount = WarningsRepository.GetAllWarnings(isGeneralManagerApproved: false).Count;


                ViewBag.WarningCount = wCount;
                ViewBag.ReportsCount = reportsCount;

                ViewBag.FactoriesCount = db.Factories.Count();

                DateTime? stDate = HelperMethods.GetFirstDayInMonth(HelperMethods.GetCurrentDateTime());
                DateTime? eDate = HelperMethods.GetLastDayInMonth(HelperMethods.GetCurrentDateTime());

                ViewBag.VisitsCount = VisitsRepository.SearchVisits(null, stDate, eDate, null, null, null).Count();
                ViewBag.SamplesCount = db.VisitsSample.Count(a => a.SampleStatus == (int)SampleStatusEnum.RevisingByObserver);
                ViewBag.MixDesignCount = db.MD_Requests.Count(a => a.RequestStatus == (int)MixDesignStatusEnum.AtSupervisor && a.IsComplete == true);
            }

            return View();
        }

        public ActionResult Test()
        {
            //var items = db.VisitsSample.Include(a => a.Visits);
            //foreach (var item in items)
            //{
            //    item.ReportNo = item.Visits.ReportNo;
            //    item.SampleDate = item.Visits.VisitDate;
            //    item.SampleTime = item.CreatedWhen.AddHours(10).ToString("hh:mm tt");
            //    item.LabBreakingDate28Day = item.Visits.VisitDate.AddDays(28);
            //    item.LabBreakingDate7Day = item.Visits.VisitDate.AddDays(7);
            //    if (item.IsFactorySamplePrepared == true)
            //        item.FactoryBreakingDate28Day = item.Visits.VisitDate.AddDays(28);
            //    db.Entry(item).State = EntityState.Modified;
            //}

            //db.SaveChanges();
            return View();
        }
        public ActionResult NewIndex()
        {
            return View();
        }

        public ActionResult Permission()
        {
            return View();
        }

        public ActionResult UserDetails(bool? isSuccess, int? error, string module)
        {
            if (error == 1)
                ViewBag.CustomError = Amana.GlobalResources.Validation.CellPhoneExist;
            else if (error == 2)
                ViewBag.CustomError = Amana.GlobalResources.Validation.EmailExist;
            else if (error == -1 && module == "changePass")
                ViewBag.CustomError = Amana.GlobalResources.Users.IncorrectCurrentPassword;

            if (isSuccess != null && isSuccess == true)
                ViewBag.CustomSuccess = Amana.GlobalResources.Cpanel.MessageSent;

            ViewBag.CurrentUser = GetCurrentUser();
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult AccountInformation()
        {
            C_UserItems entity = GetCurrentUser();
            C_UserItemsViewModel entityVM = new C_UserItemsViewModel
            {
                Name = entity.Name,
                Email = entity.Email,
                Phone = entity.Phone,
                CellPhone = entity.CellPhone
            };
            return PartialView(entityVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable CS0246 // The type or namespace name 'C_UserItemsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AccountInformation(C_UserItemsViewModel entityVM)
#pragma warning restore CS0246 // The type or namespace name 'C_UserItemsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            int result = new UsersRepository(LangId, db).UpdateUserDetails(entityVM);

            if (result > 0)
                return RedirectToAction("UserDetails", new { isSuccess = true });
            else if (result == -1)
                return RedirectToAction("UserDetails", new { error = 2 });
            else if (result == -2)
                return RedirectToAction("UserDetails", new { error = 1 });

            return RedirectToAction("UserDetails");
        }

        [ChildActionOnly]
        public PartialViewResult AccountPassword()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable CS0246 // The type or namespace name 'S_ChangePassword' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AccountPassword(S_ChangePassword entityVM)
#pragma warning restore CS0246 // The type or namespace name 'S_ChangePassword' could not be found (are you missing a using directive or an assembly reference?)
        {
            int userId = GetUserId();
            var results = new UsersRepository(LangId, db).ChangePassword(entityVM, userId);
            C_UserItems entity = results.Item2;
            int result = results.Item1;
            if (result > 0)
                return RedirectToAction("UserDetails", new { isSuccess = true });
            else
                return RedirectToAction("UserDetails", new { error = result, module = "changePass" });
        }



        public ActionResult ImportSamples()
        {
            var dataToImport = db.ImportedSamplesData.Where(a => a.SampleDate <= new DateTime(2021, 4, 20)).OrderBy(a => a.SampleNo).ToList();

            int importedCount = 0;
            foreach (var item in dataToImport)
            {
                if (item.SampleDate.HasValue && item.SampleNo.HasValue)
                {
                    var existedSample = db.VisitsSample.FirstOrDefault(a => a.SampleId == item.SampleNo);

                    if (existedSample != null)
                    {
                        item.IsImported = false;
                        item.Reason = 3; //Sample no already exist
                        db.Entry(item).State = EntityState.Modified;
                    }
                    else
                    {
                        var concClass = db.IdxConcreteClass.FirstOrDefault(a => a.ClassNumber == item.ConcreteClass.Value);
                        if (concClass == null)
                        {
                            db.IdxConcreteClass.Add(new IdxConcreteClass { ClassNumber = item.ConcreteClass.Value, Name = "C-" + item.ConcreteClass.Value });
                            db.SaveChanges();
                        }

                        var factory = db.Factories.FirstOrDefault(a => a.CodeNo == item.FactoryId);
                        if (factory == null)
                        {
                            item.IsImported = false;
                            item.Reason = 1; //factory Not Found
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            Visits vItem = new Visits();
                            vItem.CreatedBy = 1;
                            vItem.CreatedWhen = DateTime.Now;
                            vItem.FactoryId = factory.ID;
                            vItem.IsImported = true;
                            vItem.IsVisitDone = true;
                            vItem.UserId = 13;
                            vItem.VisitDate = item.SampleDate.Value;

                            vItem.VisitsSample = new VisitsSample();

                            vItem.VisitsSample.ConcreteClass = item.ConcreteClass.Value;
                            vItem.VisitsSample.ConcreteTemperture = item.Temptarues.Value;
                            vItem.VisitsSample.ConcreteType = item.ConcreteType;

                            switch (item.Usage)
                            {
                                case "نظافة":
                                    vItem.VisitsSample.ConcreteUsage = 1;
                                    break;
                                case "أساسات":
                                    vItem.VisitsSample.ConcreteUsage = 2;
                                    break;
                                case "أعمدة":
                                    vItem.VisitsSample.ConcreteUsage = 3;
                                    break;
                                case "أسقف":
                                    vItem.VisitsSample.ConcreteUsage = 4;
                                    break;
                                case "قواعد":
                                    vItem.VisitsSample.ConcreteUsage = 6;
                                    break;
                                case "رقاب الاعمدة":
                                    vItem.VisitsSample.ConcreteUsage = 7;
                                    break;
                                case "ميد":
                                    vItem.VisitsSample.ConcreteUsage = 8;
                                    break;
                                default:
                                    vItem.VisitsSample.ConcreteUsage = 5;
                                    vItem.VisitsSample.ConcreteUsageOther = item.Usage;
                                    break;
                            }
                            vItem.VisitsSample.CreatedBy = 13;
                            vItem.VisitsSample.CreatedWhen = DateTime.Now;
                            vItem.VisitsSample.IsCleanLocation = true;
                            vItem.VisitsSample.IsDustControlInStation = true;
                            vItem.VisitsSample.IsFactorySamplePrepared = false;
                            vItem.VisitsSample.IsImported = true;
                            vItem.VisitsSample.IsLabEngineer = true;
                            vItem.VisitsSample.IsMoldanatInTrunks = true;
                            vItem.VisitsSample.IsPeopleSafety = true;
                            vItem.VisitsSample.IsRokamSummar = true;
                            vItem.VisitsSample.LabResist28days = item.Compress28Days;
                            vItem.VisitsSample.LabResist28days1 = 0;
                            vItem.VisitsSample.LabResist28days2 = 0;
                            vItem.VisitsSample.LabResist28days3 = 0;
                            vItem.VisitsSample.LabResist7days = item.Compress7Days;
                            vItem.VisitsSample.LabResist7days1 = 0;
                            vItem.VisitsSample.LabResist7days2 = 0;
                            vItem.VisitsSample.LabResist7days3 = 0;
                            vItem.VisitsSample.ReportNo = 0;
                            vItem.VisitsSample.SampleDate = item.SampleDate.Value;

                            vItem.VisitsSample.SampleId = item.SampleNo.Value;

                            vItem.VisitsSample.SampleStatus = (int)SampleStatusEnum.Finished;
                            vItem.VisitsSample.SampleType = 0;
                            vItem.VisitsSample.SlumpValue = item.SlumbVal.HasValue ? item.SlumbVal.Value : 0;


                            vItem.VisitsSample.V_TechnicalStuff = !vItem.VisitsSample.IsLabEngineer;
                            vItem.VisitsSample.V_IsCleanLocation = !vItem.VisitsSample.IsCleanLocation;
                            vItem.VisitsSample.V_IsSlumb = (vItem.VisitsSample.SlumpValue <= 100);
                            vItem.VisitsSample.V_IsHighTemp = (vItem.VisitsSample.ConcreteTemperture >= 34);
                            vItem.VisitsSample.V_BaseColumn35 = ((vItem.VisitsSample.ConcreteUsage == 2 || vItem.VisitsSample.ConcreteUsage == 3 || vItem.VisitsSample.ConcreteUsage == 6 || vItem.VisitsSample.ConcreteUsage == 7 || vItem.VisitsSample.ConcreteUsage == 8) && vItem.VisitsSample.ConcreteClass < 30);
                            if (vItem.VisitsSample.LabResist28days.HasValue)
                            {
                                var val = vItem.VisitsSample.ConcreteClass - vItem.VisitsSample.LabResist28days.Value;
                                vItem.VisitsSample.V_IsLowPressure = (val >= 3.5);
                            }
                            vItem.VisitsSample.V_IsThereViolations = (vItem.VisitsSample.V_IsSlumb == true || vItem.VisitsSample.V_IsHighTemp == true || vItem.VisitsSample.V_BaseColumn35 == true || vItem.VisitsSample.V_IsLowPressure == true);


                            db.Visits.Add(vItem);

                            importedCount++;
                        }
                    }

                }
                else
                {
                    db.Entry(item).State = EntityState.Modified;
                    item.IsImported = false;
                    item.Reason = 2; //Sample date or no has no value
                }

                db.SaveChanges();

            }
            object result = string.Format("Imported {0}  From  {1}", importedCount, dataToImport.Count);

            return View("ImportSamples", result);
        }



        //public ActionResult EditWarnings()
        //{
        //    var allWarnings = db.FactoryWarnings.ToList();
        //    List<int> toDelete = new List<int>();
        //    foreach (var item in allWarnings)
        //    {
        //        if (!toDelete.Contains(item.WarningId))
        //        {
        //            if (!item.FactoryWarningSamples.Any(f => f.VisitSampleId == item.VisitSampleId))
        //                item.FactoryWarningSamples.Add(new FactoryWarningSamples { VisitSampleId = item.VisitSampleId.Value });

        //            DateTime startDate = HelperMethods.GetFirstDayInMonth(item.WarningDate);
        //            DateTime endDate = HelperMethods.GetLastDayInMonth(item.WarningDate);

        //            var wInMonth = db.FactoryWarnings.Where(a => a.FactoryId == item.FactoryId && a.WarningDate >= startDate && a.WarningDate <= endDate && a.WarningId != item.WarningId);
        //            foreach (var ww in wInMonth)
        //            {
        //                if (ww.IsFinalWarning == false)
        //                {
        //                    item.FactoryWarningSamples.Add(new FactoryWarningSamples { VisitSampleId = ww.VisitSampleId.Value });
        //                    toDelete.Add(ww.WarningId);

        //                }
        //            }

        //        }
        //    }
        //    foreach (int wId in toDelete)
        //    {
        //      var delW=  db.FactoryWarnings.FirstOrDefault(a=> a.WarningId==wId);
        //        if (delW != null)
        //            db.FactoryWarnings.Remove(delW);
        //    }

        //    db.SaveChanges();

        //    object str = "";
        //    return View(str);

        //}
    }
}
