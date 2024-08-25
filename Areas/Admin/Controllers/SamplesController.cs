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
    public class SamplesController : ControllerHelper
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



        public ActionResult Index(int? status, DateTime? startDate, DateTime? endDate, int? userId, int? factoryId, int? sampleId)
        {
            var factories = FactoriesRepository.GetAllFactories();
            var users = usersDi.GetItemsList(roleId: 2, isActive: null);

            ViewData["FactoriesLst"] = new SelectList(factories, "ID", "Name", factoryId);
            ViewData["UsersLst"] = new SelectList(users, "UserId", "Name", userId);
            startDate = (startDate ?? HelperMethods.GetFirstDayInMonth(HelperMethods.GetCurrentDateTime()));
            endDate = (endDate ?? HelperMethods.GetLastDayInMonth(HelperMethods.GetCurrentDateTime()));
            List<VisitsSample> result = new List<VisitsSample>();
            if (sampleId.HasValue)
            {
                var sample = VisitsRepository.GetSampleBySampleId(sampleId.Value, true);
                if (sample != null)
                    result.Add(sample);
            }
            else
            {
                result = VisitsRepository.SearchSamples(startDate, endDate, userId, factoryId, status);

            }

            return View(result);
        }

        public ActionResult HomeSamples(int? status)
        {
            var currentUser = GetCurrentUser();
            List<VisitsSample> result = new List<VisitsSample>();

            //if (currentUser.RoleId == null || currentUser.RoleId == 1003)
            result = VisitsRepository.GetMySamples(currentUser.UserId, currentUser.RoleId, status);

            return View(result);
        }
        public ActionResult LabSamples(int? sampleId)
        {

            var currentUser = GetCurrentUser();

            List<VisitsSample> result = new List<VisitsSample>();
            //var result = db.VisitsSample.Where(a=> a.SampleStatus==3).ToList();
            if (sampleId.HasValue)
            {
                var sample = VisitsRepository.GetSampleBySampleId(sampleId.Value, true);
                if (sample != null)
                    result.Add(sample);
            }
            else
            {
                result = VisitsRepository.GetMySamples(currentUser.UserId, currentUser.RoleId, 3);

            }


            return View(result);
        }
        public ActionResult FactorySamples()
        {

            var currentUser = GetCurrentUser();

            //var result = db.VisitsSample.Where(a=> a.SampleStatus==3).ToList();
            var result = VisitsRepository.GetMySamples(currentUser.UserId, currentUser.RoleId, null);

            return View(result);
        }
        public ActionResult DailyBreakingDateSamples(string startDate)
        {
            DateTime date = string.IsNullOrEmpty(startDate) ? HelperMethods.GetCurrentDateTime() : DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var currentUser = GetCurrentUser();

            var result = VisitsRepository.GetMySamples(currentUser.UserId, currentUser.RoleId, 3);

            ViewBag.List7Days = result.Where(a => (a.LabBreakingDate7Day.HasValue == true ? (a.LabBreakingDate7Day.Value.Date == date.Date) : false)).ToList();
            ViewBag.List28Days = result.Where(a => (a.LabBreakingDate28Day.HasValue == true ? (a.LabBreakingDate28Day.Value.Date == date.Date) : false)).ToList();

            return View(result);
        }
        public ActionResult FactoryBreakingDateSamples()
        {
            int userId = GetUserId();
            IdxLocations todayLocation = VisitsRepository.GetTodayUserLocation(userId, HelperMethods.GetCurrentDateTime());
            if (todayLocation != null)
            {
                var items = VisitsRepository.GetFactoryBreakingDateSamples(HelperMethods.GetCurrentDateTime(), todayLocation.ID);
                return View(items);

            }
            else
                return View();
        }
        public ActionResult MySamples(bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            return View();
        }

        public ActionResult ReceiveSamples()
        {
            int userId = GetUserId();
            IdxLocations todayLocation = VisitsRepository.GetTodayUserLocation(userId, HelperMethods.GetCurrentDateTime());
            if (todayLocation != null)
            {
                var items = VisitsRepository.SearchSamples(null, null, null, null, (int)SampleStatusEnum.ToBeRecievedFromFactory, todayLocation.ID).Where(a => a.SampleDate.Date < HelperMethods.GetCurrentDateTime().Date).ToList();
                return View(items);
            }
            else
            {
                var items = VisitsRepository.SearchSamples(null, null, userId, null, (int)SampleStatusEnum.ToBeRecievedFromFactory, null).Where(a => a.SampleDate.Date < HelperMethods.GetCurrentDateTime().Date).ToList();
                return View(items);
            }
        }
        public ActionResult SampleDetails(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            //var factories = FactoriesRepository.GetAllFactories();
            //var users = usersDi.GetItemsList(roleId: 2, isActive: null).Where(a=> a.IsAvailable==true);


            var concreteSourceLst = SettingsRepository.GetConcreteSources();
            var concreteClassLst = SettingsRepository.GetConcreteClasses();
            var additionTypesLst = SettingsRepository.GetAdditionTypes();

            additionTypesLst.Add(new IdxAdditionTypes { ID = 0, Name = "أخرى" });


            var item = VisitsRepository.GetVisitById(id, true);

            ViewBag.FactoryVisitNo = (db.Visits.Count(a => a.VisitDate < item.VisitDate && a.IsVisitDone == true && a.FactoryId == item.FactoryId)) + 1;


            SampleViewModel itemVm = VisitsRepository.MakeConcreteSampleViewModel(item);

            var currentUser = GetCurrentUser();
            ViewBag.CanEdit = ((currentUser.RoleId == 2 && (item.IsVisitDone == false || itemVm.SampleStatus == (int)SampleStatusEnum.RevisingByObserver)) || currentUser.RoleId == null || currentUser.RoleId == 1006);


            ViewData["ConcreteSourceList"] = new SelectList(concreteSourceLst, "SourceId", "Name", itemVm.ConcreteSourceId);
            ViewData["AdditionTypesList"] = new SelectList(additionTypesLst, "ID", "Name", itemVm.AdditionTypes);
            ViewData["AdditionTypesList2"] = new SelectList(additionTypesLst, "ID", "Name", itemVm.AdditionTypes2);
            //var concreteClassSL = new SelectList(concreteClassLst, "ClassNumber", "Name", itemVm.ConcreteClass);
            List<SelectListItem> concreteClassSL = new List<SelectListItem>();
            foreach (var clsItem in concreteClassLst)
            {
                concreteClassSL.Add(new SelectListItem() { Text = clsItem.Name, Value = clsItem.ClassNumber.ToString(), Selected = (itemVm.ConcreteClass == clsItem.ClassNumber) });
            }
            concreteClassSL.Add(new SelectListItem() { Text = "أخرى", Value = "0" });

            //concreteClassSL.Items.add
            ViewData["ConcreteClassList"] = concreteClassSL;


            //ViewData["FactoriesLst"] = new SelectList(factories, "ID", "Name", itemVm.FactoryId);
            //ViewData["UsersLst"] = new SelectList(users, "UserId", "Name", itemVm.UserId);

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'SampleViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult SampleDetails(SampleViewModel itemVm
#pragma warning restore CS0246 // The type or namespace name 'SampleViewModel' could not be found (are you missing a using directive or an assembly reference?)
            , HttpPostedFileBase ConcreteTemperturePhotoUrl
            , HttpPostedFileBase DesignMixturePhotoUrl
            , HttpPostedFileBase InvoicePhotoUrl
            , HttpPostedFileBase IsCleanLocationPhotoUrl
            , HttpPostedFileBase IsDustControlInStationPhotoUrl
            , HttpPostedFileBase IsFactorySamplePreparedPhotoUrl
            , HttpPostedFileBase IsLabEngineerPhotoUrl
            , HttpPostedFileBase IsMoldanatInTrunksPhotoUrl
            , HttpPostedFileBase IsPeopleSafetyPhotoUrl
            , HttpPostedFileBase IsRokamSummarPhotoUrl
            , HttpPostedFileBase SlumpPhotoUrl
            , HttpPostedFileBase WaterTemperturePhotoUrl
            , HttpPostedFileBase WeatherTemperturePhotoUrl
            )
        {
            var currentUser = GetCurrentUser();
            Visits item = new Models.Entities.Visits();

            item = VisitsRepository.GetVisitById(itemVm.VisitId, true);


            item.ClientName = itemVm.ClientName;
            item.InvoiceNumber = itemVm.InvoiceNumber;
            //item.ReportNo = itemVm.ReportNo;
            item.TruckNo = itemVm.TruckNo;
            //item.VisitDate = DateTime.ParseExact(itemVm.VisitDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //item.VisitNo = itemVm.VisitNo;

            VisitsSample sampleInfo = new VisitsSample();

            if (item.VisitsSample == null)
            {
                sampleInfo.VisitId = itemVm.VisitId;
                sampleInfo.CreatedBy = currentUser.UserId;
                sampleInfo.CreatedWhen = HelperMethods.GetCurrentDateTime();
                //item.VisitsSample.C_UserItems = new C_UserItems { UserId = currentUser.UserId };
            }
            else
                sampleInfo = item.VisitsSample;

            if (itemVm.AdditionTypes == "0" && !string.IsNullOrEmpty(itemVm.AdditionTypesOther))
            {
                sampleInfo.AdditionTypes = SettingsRepository.SaveAdditionType(new IdxAdditionTypes { Name = itemVm.AdditionTypesOther }).ToString();

            }
            else
                sampleInfo.AdditionTypes = itemVm.AdditionTypes;

            sampleInfo.AdditionAmount = itemVm.AdditionAmount;

            sampleInfo.AdditionAmount2 = itemVm.AdditionAmount2;
            if (itemVm.AdditionTypes2 == "0" && !string.IsNullOrEmpty(itemVm.AdditionTypesOther2))
            {
                sampleInfo.AdditionTypes2 = SettingsRepository.SaveAdditionType(new IdxAdditionTypes { Name = itemVm.AdditionTypesOther2 }).ToString();

            }
            else
                sampleInfo.AdditionTypes2 = itemVm.AdditionTypes2;
            if (itemVm.ConcreteClass > 0)
            {
                sampleInfo.ConcreteClass = itemVm.ConcreteClass.Value;
            }
            else if (itemVm.ConcreteClassOther.HasValue)
            {
                sampleInfo.ConcreteClass = SettingsRepository.SaveConcreteClass(new IdxConcreteClass { ClassNumber = itemVm.ConcreteClassOther.Value });
            }

            sampleInfo.ConcreteSourceId = itemVm.ConcreteSourceId;
            sampleInfo.ConcreteTemperture = itemVm.ConcreteTemperture.Value;
            sampleInfo.ConcreteType = itemVm.ConcreteType;
            sampleInfo.ConcreteUsage = itemVm.ConcreteUsage;
            sampleInfo.ConcreteUsageOther = itemVm.ConcreteUsageOther;
            sampleInfo.ConcreteWeight = itemVm.ConcreteWeight;
            sampleInfo.GeneralNotes = itemVm.GeneralNotes;
            sampleInfo.IsCleanLocation = itemVm.IsCleanLocation;
            sampleInfo.IsDustControlInStation = itemVm.IsDustControlInStation;
            sampleInfo.IsLabEngineer = itemVm.IsLabEngineer;
            sampleInfo.IsMoldanatInTrunks = itemVm.IsMoldanatInTrunks;
            sampleInfo.IsPeopleSafety = itemVm.IsPeopleSafety;
            sampleInfo.IsRokamSummar = itemVm.IsRokamSummar;
            sampleInfo.MixerNo = itemVm.MixerNo;
            sampleInfo.Rokam3By4 = itemVm.Rokam3By4;
            sampleInfo.Rokam3By8 = itemVm.Rokam3By8;
            //item.VisitsSample.SampleDate = DateTime.ParseExact(itemVm.SampleDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //sampleInfo.SampleDate = new DateTime(itemVm.SampleDateYear, itemVm.SampleDateMonth, itemVm.SampleDateDay);
            sampleInfo.SampleDate = item.VisitDate;
            //itemVm.SampleLabNo = item.VisitsSample.SampleLabNo;
            //sampleInfo.SampleNo = itemVm.SampleNo.Value;
            sampleInfo.SlumpValue = itemVm.SlumpValue.Value;
            sampleInfo.WashedSand = itemVm.WashedSand;
            sampleInfo.WaterTemperture = itemVm.WaterTemperture.Value;
            sampleInfo.WaterWeight = itemVm.WaterWeight;
            sampleInfo.WeatherTemperture = itemVm.WeatherTemperture.Value;
            sampleInfo.WhiteSand = itemVm.WhiteSand;
            sampleInfo.IsFactorySamplePrepared = itemVm.IsFactorySamplePrepared;
            sampleInfo.SampleType = itemVm.SampleType;
            sampleInfo.CurrentLocationLat = itemVm.CurrentLocationLat;
            sampleInfo.CurrentLocationLong = itemVm.CurrentLocationLong;

            if (ConcreteTemperturePhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.ConcreteTemperturePhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.ConcreteTemperturePhotoUrl);

                sampleInfo.ConcreteTemperturePhotoUrl = ImageResizer.SaveImage("Uploads/Samples", ConcreteTemperturePhotoUrl);
            }
            if (DesignMixturePhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.DesignMixturePhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.DesignMixturePhotoUrl);

                sampleInfo.DesignMixturePhotoUrl = ImageResizer.SaveImage("Uploads/Samples", DesignMixturePhotoUrl);
            }
            if (InvoicePhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.InvoicePhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.InvoicePhotoUrl);

                sampleInfo.InvoicePhotoUrl = ImageResizer.SaveImage("Uploads/Samples", InvoicePhotoUrl);
            }
            if (IsCleanLocationPhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.IsCleanLocationPhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.IsCleanLocationPhotoUrl);

                sampleInfo.IsCleanLocationPhotoUrl = ImageResizer.SaveImage("Uploads/Samples", IsCleanLocationPhotoUrl);
            }
            if (IsDustControlInStationPhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.IsDustControlInStationPhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.IsDustControlInStationPhotoUrl);

                sampleInfo.IsDustControlInStationPhotoUrl = ImageResizer.SaveImage("Uploads/Samples", IsDustControlInStationPhotoUrl);
            }
            if (IsFactorySamplePreparedPhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.IsFactorySamplePreparedPhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.IsFactorySamplePreparedPhotoUrl);

                sampleInfo.IsFactorySamplePreparedPhotoUrl = ImageResizer.SaveImage("Uploads/Samples", IsFactorySamplePreparedPhotoUrl);
            }
            if (IsLabEngineerPhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.IsLabEngineerPhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.IsLabEngineerPhotoUrl);

                sampleInfo.IsLabEngineerPhotoUrl = ImageResizer.SaveImage("Uploads/Samples", IsLabEngineerPhotoUrl);
            }
            if (IsMoldanatInTrunksPhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.IsMoldanatInTrunksPhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.IsMoldanatInTrunksPhotoUrl);

                sampleInfo.IsMoldanatInTrunksPhotoUrl = ImageResizer.SaveImage("Uploads/Samples", IsMoldanatInTrunksPhotoUrl);
            }
            if (IsPeopleSafetyPhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.IsPeopleSafetyPhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.IsPeopleSafetyPhotoUrl);

                sampleInfo.IsPeopleSafetyPhotoUrl = ImageResizer.SaveImage("Uploads/Samples", IsPeopleSafetyPhotoUrl);
            }
            if (IsRokamSummarPhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.IsRokamSummarPhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.IsRokamSummarPhotoUrl);

                sampleInfo.IsRokamSummarPhotoUrl = ImageResizer.SaveImage("Uploads/Samples", IsRokamSummarPhotoUrl);
            }
            if (SlumpPhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.SlumpPhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.SlumpPhotoUrl);

                sampleInfo.SlumpPhotoUrl = ImageResizer.SaveImage("Uploads/Samples", SlumpPhotoUrl);
            }
            if (WaterTemperturePhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.WaterTemperturePhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.WaterTemperturePhotoUrl);

                sampleInfo.WaterTemperturePhotoUrl = ImageResizer.SaveImage("Uploads/Samples", WaterTemperturePhotoUrl);
            }
            if (WeatherTemperturePhotoUrl != null)
            {
                if (!string.IsNullOrEmpty(sampleInfo.WeatherTemperturePhotoUrl))
                    HelperMethods.DeleteFile("Uploads/Samples/", sampleInfo.WeatherTemperturePhotoUrl);

                sampleInfo.WeatherTemperturePhotoUrl = ImageResizer.SaveImage("Uploads/Samples", WeatherTemperturePhotoUrl);
            }

            if (currentUser.RoleId == 2)
            {
                item.IsVisitDone = true;
                sampleInfo.SampleStatus = 1;
            }

            VisitsRepository.SaveVisit(item);

            VisitsRepository.SaveSample(sampleInfo);


            return RedirectToAction("SampleDetails", new { id = item.VisitId, isSuccess = true });

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
                itemVm = new AutoGeneratedVisits { MonthId = monthId, YearId = yearId };

            }

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'AutoGeneratedVisits' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult CreateMonthly(AutoGeneratedVisits itemVm)
#pragma warning restore CS0246 // The type or namespace name 'AutoGeneratedVisits' could not be found (are you missing a using directive or an assembly reference?)
        {
            VisitsRepository.SaveAutoGeneratedVisitsItem(itemVm);
            return RedirectToAction("CreateMonthly", new { isSuccess = true });

        }

        public ActionResult ChangeStatus(int id, int status)
        {
            var item = VisitsRepository.GetSampleByVisitId(id, true);
            item.SampleStatus = status;

            if (status == 3)//موظف الاستقبال ينقل العينة الى المختبر
            {
                item.LabRecieveDate = HelperMethods.GetCurrentDateTime();

            }

            VisitsRepository.SaveSample(item);

            if (status == (int)SampleStatusEnum.InReception)//المراقب يرسلها للاستقبال
            {
                return RedirectToAction("ReceiveSamples");
            }
            else if (status == (int)SampleStatusEnum.InLab)
            {
                //return RedirectToAction("PrintStickers", new { id = item.VisitId });
                return RedirectToAction("MySamples", new { isSuccess = true });
            }
            else if (status == 4)//انهاء العينة
            {
                if (item.V_IsThereViolations == true)
                {
                    VisitsRepository.GenerateWarning(item, GetUserId());
                }
                return RedirectToAction("LabSampleDetails", new { id = id, isSuccess = true });
            }
            else
                return RedirectToAction("SampleDetails", new { id = id, isSuccess = true });
        }


        public ActionResult LabSampleDetails(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            //var factories = FactoriesRepository.GetAllFactories();
            //var users = usersDi.GetItemsList(roleId: 2, isActive: null).Where(a=> a.IsAvailable==true);

            var currentUser = GetCurrentUser();
            ViewBag.CanEdit = (currentUser.RoleId == 1003 || currentUser.RoleId == null || currentUser.RoleId == 1006);



            SampleLabViewModel itemVm = new SampleLabViewModel();
            var item = VisitsRepository.GetVisitById(id, true);
            itemVm.VisitId = item.VisitId;

            if (item.VisitsSample != null)
            {
                if (!item.VisitsSample.LabBreakingDate28Day.HasValue || !item.VisitsSample.LabBreakingDate7Day.HasValue)
                {
                    if (!item.VisitsSample.LabBreakingDate28Day.HasValue)
                        item.VisitsSample.LabBreakingDate28Day = item.VisitsSample.SampleDate.AddDays(28);
                    if (!item.VisitsSample.LabBreakingDate7Day.HasValue)
                        item.VisitsSample.LabBreakingDate7Day = item.VisitsSample.SampleDate.AddDays(7);
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }


                itemVm.SampleStatus = item.VisitsSample.SampleStatus;
                itemVm.LabBreakingDate28Day = item.VisitsSample.LabBreakingDate28Day.Value.ToString("dd/MM/yyyy");
                itemVm.LabBreakingDate7Day = item.VisitsSample.LabBreakingDate7Day.Value.ToString("dd/MM/yyyy");
                itemVm.LabRecieveDate = item.VisitsSample.LabRecieveDate ?? HelperMethods.GetCurrentDateTime();
                itemVm.LabResist28days = item.VisitsSample.LabResist28days;
                itemVm.LabResist7days = item.VisitsSample.LabResist7days;
                itemVm.LabResist7days1 = item.VisitsSample.LabResist7days1;
                itemVm.LabResist7days2 = item.VisitsSample.LabResist7days2;
                itemVm.LabResist7days3 = item.VisitsSample.LabResist7days3;
                itemVm.LabResist28days1 = item.VisitsSample.LabResist28days1;
                itemVm.LabResist28days2 = item.VisitsSample.LabResist28days2;
                itemVm.LabResist28days3 = item.VisitsSample.LabResist28days3;
                itemVm.SampleDate = item.VisitsSample.SampleDate;
                itemVm.SampleId = item.VisitsSample.SampleId;
                itemVm.Edit28DayResults = true;
                itemVm.Edit7DayResults = true;
                if (currentUser.RoleId == 1003)
                {
                    if (HelperMethods.GetCurrentDateTime().Date != item.VisitsSample.LabBreakingDate7Day.Value.Date)
                    {
                        itemVm.Edit7DayResults = false;
                    }
                    if (HelperMethods.GetCurrentDateTime().Date != item.VisitsSample.LabBreakingDate28Day.Value.Date)
                    {
                        itemVm.Edit28DayResults = false;
                    }
                }
            }

            //ViewData["FactoriesLst"] = new SelectList(factories, "ID", "Name", itemVm.FactoryId);
            //ViewData["UsersLst"] = new SelectList(users, "UserId", "Name", itemVm.UserId);

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'SampleLabViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult LabSampleDetails(SampleLabViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'SampleLabViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            var currentUser = GetCurrentUser();
            Visits item = new Models.Entities.Visits();

            item = VisitsRepository.GetVisitById(itemVm.VisitId, true);

            VisitsSample sampleInfo = item.VisitsSample;
            sampleInfo.LabResist28days1 = itemVm.LabResist28days1;
            sampleInfo.LabResist28days2 = itemVm.LabResist28days2;
            sampleInfo.LabResist28days3 = itemVm.LabResist28days3;
            if (itemVm.LabResist28days1.HasValue && itemVm.LabResist28days2.HasValue && itemVm.LabResist28days3.HasValue)
            {
                sampleInfo.LabResist28days = Math.Round((itemVm.LabResist28days1.Value + itemVm.LabResist28days2.Value + itemVm.LabResist28days3.Value) / 3, 2);

            }

            sampleInfo.LabResist7days1 = itemVm.LabResist7days1;
            sampleInfo.LabResist7days2 = itemVm.LabResist7days2;
            sampleInfo.LabResist7days3 = itemVm.LabResist7days3;
            if (itemVm.LabResist7days1.HasValue && itemVm.LabResist7days2.HasValue && itemVm.LabResist7days3.HasValue)
            {
                sampleInfo.LabResist7days = Math.Round((itemVm.LabResist7days1.Value + itemVm.LabResist7days2.Value + itemVm.LabResist7days3.Value) / 3, 2);
            }

            //if (currentUser.RoleId == 2)
            //{
            //    item.IsVisitDone = true;
            //    sampleInfo.SampleStatus = 1;
            //}

            //VisitsRepository.SaveVisit(item);

            VisitsRepository.SaveSample(sampleInfo);


            return RedirectToAction("LabSampleDetails", new { id = item.VisitId, isSuccess = true });

        }

        public ActionResult PrintStickers(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var item = VisitsRepository.GetVisitById(id, true);

            return View(item.VisitsSample);
        }



        public ActionResult FactoryLabSampleDetails(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            //var factories = FactoriesRepository.GetAllFactories();
            //var users = usersDi.GetItemsList(roleId: 2, isActive: null).Where(a=> a.IsAvailable==true);
            SampleLabViewModel itemVm = new SampleLabViewModel();
            var item = VisitsRepository.GetVisitById(id, true);
            itemVm.VisitId = item.VisitId;





            if (item.VisitsSample != null)
            {
                var currentUser = GetCurrentUser();
                itemVm.Edit28DayResults = (((currentUser.RoleId == 2 || currentUser.RoleId == 1003) && !item.VisitsSample.FactoryResist28days.HasValue) || currentUser.RoleId == null || currentUser.RoleId == 1006);


                itemVm.SampleStatus = item.VisitsSample.SampleStatus;
                itemVm.LabBreakingDate28Day = item.VisitsSample.FactoryBreakingDate28Day.Value.ToString("dd/MM/yyyy");
                //itemVm.LabBreakingDate7Day = item.VisitsSample.LabBreakingDate7Day.Value.ToString("dd/MM/yyyy");
                itemVm.LabRecieveDate = item.VisitDate;
                itemVm.LabResist28days = item.VisitsSample.FactoryResist28days;
                itemVm.LabResist28days1 = item.VisitsSample.FactoryResist28days1;
                itemVm.LabResist28days2 = item.VisitsSample.FactoryResist28days2;
                itemVm.LabResist28days3 = item.VisitsSample.FactoryResist28days3;
                itemVm.SampleDate = item.VisitsSample.SampleDate;
                itemVm.SampleId = item.VisitsSample.SampleId;
                //itemVm.Edit28DayResults = true;

                if (currentUser.RoleId == 1003)
                {
                    if (HelperMethods.GetCurrentDateTime().Date != item.VisitsSample.FactoryBreakingDate28Day.Value.Date)
                    {
                        itemVm.Edit28DayResults = false;
                    }
                }
            }

            //ViewData["FactoriesLst"] = new SelectList(factories, "ID", "Name", itemVm.FactoryId);
            //ViewData["UsersLst"] = new SelectList(users, "UserId", "Name", itemVm.UserId);

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'SampleLabViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult FactoryLabSampleDetails(SampleLabViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'SampleLabViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            var currentUser = GetCurrentUser();
            Visits item = new Models.Entities.Visits();

            item = VisitsRepository.GetVisitById(itemVm.VisitId, true);

            VisitsSample sampleInfo = item.VisitsSample;
            sampleInfo.FactoryResist28days1 = itemVm.LabResist28days1;
            sampleInfo.FactoryResist28days2 = itemVm.LabResist28days2;
            sampleInfo.FactoryResist28days3 = itemVm.LabResist28days3;
            if (itemVm.LabResist28days1.HasValue && itemVm.LabResist28days2.HasValue && itemVm.LabResist28days3.HasValue)
            {
                sampleInfo.FactoryResist28days = Math.Round((itemVm.LabResist28days1.Value + itemVm.LabResist28days2.Value + itemVm.LabResist28days3.Value) / 3, 2);
            }


            //if (currentUser.RoleId == 2)
            //{
            //    item.IsVisitDone = true;
            //    sampleInfo.SampleStatus = 1;
            //}

            //VisitsRepository.SaveVisit(item);

            VisitsRepository.SaveSample(sampleInfo);


            return RedirectToAction("FactoryLabSampleDetails", new { id = item.VisitId, isSuccess = true });

        }
    }
}