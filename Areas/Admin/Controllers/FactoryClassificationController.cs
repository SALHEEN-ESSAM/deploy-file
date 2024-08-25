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
using Amana.ViewModels.Classifications;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System.Collections.Generic;
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
    public class FactoryClassificationController : ControllerHelper
    {
#pragma warning disable CS0246 // The type or namespace name 'AmanaConcreteDBEntities1' could not be found (are you missing a using directive or an assembly reference?)
        readonly AmanaConcreteDBEntities1 _context = new AmanaConcreteDBEntities1();
#pragma warning restore CS0246 // The type or namespace name 'AmanaConcreteDBEntities1' could not be found (are you missing a using directive or an assembly reference?)

        public ActionResult Index(int? factoryId)
        {
            ViewData["LocationsLst"] = new SelectList(SettingsRepository.GetLocations(), "ID", "Name");
            ViewData["FactoriesLst"] = new SelectList(FactoriesRepository.GetAllFactories(), "ID", "Name", "");
            //ViewData["PeriodsLst"] = new SelectList(SettingsRepository.GetTimePeriods(true), "TimePeriodId", "Title", "");

            var currentPeriod = SettingsRepository.GetCurrentTimePeriodItem(true);

            ViewBag.CurrentPeriodId = currentPeriod == null ? 0 : currentPeriod.TimePeriodId;
            ViewBag.CurrentPeriodInfo = currentPeriod;

            List<Cls_FactoryClassifications> result = new List<Cls_FactoryClassifications>();
            if (factoryId.HasValue && currentPeriod != null)
            {
                result = FactoriyClassificationRepository.GetAllClassifications(factoryId, currentPeriod.TimePeriodId);
            }
            return View(result);
        }

        public ActionResult AddClassification(int factoryId, int periodId)
        {
            Cls_FactoryClassifications item = new Cls_FactoryClassifications();
            item.CreatedBy = GetUserId();
            item.CreatedWhen = HelperMethods.GetCurrentDateTime();
            item.FactoryId = factoryId;
            item.TimePeriodId = periodId;

            int clsId = FactoriyClassificationRepository.SaveClassificationItem(item);

            return RedirectToAction("BasicRequirements", new { id = clsId });
        }



        public ActionResult BasicRequirements(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var entity = FactoriyClassificationRepository.GetClassificationById(id, false);

            BasicRequirementsViewModel entityVm = new BasicRequirementsViewModel();
            entityVm.ClassificationId = entity.ClassificationId;
            entityVm.ListPropLocation = FactoriyClassificationRepository.GetClassificationPropertyList(id, 1);
            entityVm.ListPropConcrete = FactoriyClassificationRepository.GetClassificationPropertyList(id, 2);
            entityVm.ListPropRokam = FactoriyClassificationRepository.GetClassificationPropertyList(id, 3);
            entityVm.ListPropWater = FactoriyClassificationRepository.GetClassificationPropertyList(id, 4);
            entityVm.ListPropExtra = FactoriyClassificationRepository.GetClassificationPropertyList(id, 5);
            entityVm.ListPropMixStations = FactoriyClassificationRepository.GetClassificationPropertyList(id, 6);
            entityVm.ListPropMixTraffics = FactoriyClassificationRepository.GetClassificationPropertyList(id, 7);
            entityVm.ListPropConcreteProduction = FactoriyClassificationRepository.GetClassificationPropertyList(id, 8);
            entityVm.ListPropFawtara = FactoriyClassificationRepository.GetClassificationPropertyList(id, 9);



            ViewBag.ClassificationItem = entity;
            return View(entityVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'BasicRequirementsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult BasicRequirements(BasicRequirementsViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'BasicRequirementsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {


            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListPropLocation, itemVm.ClassificationId, 1);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListPropConcrete, itemVm.ClassificationId, 2);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListPropRokam, itemVm.ClassificationId, 3);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListPropWater, itemVm.ClassificationId, 4);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListPropExtra, itemVm.ClassificationId, 5);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListPropMixStations, itemVm.ClassificationId, 6);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListPropMixTraffics, itemVm.ClassificationId, 7);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListPropConcreteProduction, itemVm.ClassificationId, 8);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListPropFawtara, itemVm.ClassificationId, 9);

            var entity = FactoriyClassificationRepository.GetClassificationById(itemVm.ClassificationId, false);
            entity.IsBasicReqDone = true;
            FactoriyClassificationRepository.SaveClassificationItem(entity);


            return RedirectToAction("BasicRequirements", new { id = itemVm.ClassificationId, isSuccess = true });

        }
        public ActionResult AdditionalRequirements(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var entity = FactoriyClassificationRepository.GetClassificationById(id, false);

            AdditionalRequirementsViewModel entityVm = new AdditionalRequirementsViewModel();
            entityVm.ClassificationId = entity.ClassificationId;
            entityVm.ListProp10 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 10);
            entityVm.ListProp11 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 11);
            entityVm.ListProp12 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 12);
            entityVm.ListProp15 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 15);
            entityVm.ListProp16 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 16);
            entityVm.ListProp17 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 17);


            ViewBag.ClassificationItem = entity;
            return View(entityVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'AdditionalRequirementsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AdditionalRequirements(AdditionalRequirementsViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'AdditionalRequirementsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {


            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp10, itemVm.ClassificationId, 10);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp11, itemVm.ClassificationId, 11);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp12, itemVm.ClassificationId, 12);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp15, itemVm.ClassificationId, 15);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp16, itemVm.ClassificationId, 16);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp17, itemVm.ClassificationId, 17);

            var entity = FactoriyClassificationRepository.GetClassificationById(itemVm.ClassificationId, false);
            entity.IsAddReqDone = true;
            FactoriyClassificationRepository.SaveClassificationItem(entity);


            return RedirectToAction("AdditionalRequirements", new { id = itemVm.ClassificationId, isSuccess = true });

        }

        public ActionResult LabBasicRequirements(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var entity = FactoriyClassificationRepository.GetClassificationById(id, false);

            LabBasicRequirementsViewModel entityVm = new LabBasicRequirementsViewModel();
            entityVm.ClassificationId = entity.ClassificationId;
            entityVm.ListProp18 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 18);
            entityVm.ListProp19 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 19);
            entityVm.ListProp33 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 33);


            ViewBag.ClassificationItem = entity;
            return View(entityVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'LabBasicRequirementsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult LabBasicRequirements(LabBasicRequirementsViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'LabBasicRequirementsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {


            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp18, itemVm.ClassificationId, 18);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp19, itemVm.ClassificationId, 19);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp33, itemVm.ClassificationId, 33);

            var entity = FactoriyClassificationRepository.GetClassificationById(itemVm.ClassificationId, false);
            entity.IsBasicLabReqDone = true;
            FactoriyClassificationRepository.SaveClassificationItem(entity);


            return RedirectToAction("LabBasicRequirements", new { id = itemVm.ClassificationId, isSuccess = true });

        }
        public ActionResult LabAdditionalRequirements(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var entity = FactoriyClassificationRepository.GetClassificationById(id, false);

            LabAdditionalRequirementsViewModel entityVm = new LabAdditionalRequirementsViewModel();
            entityVm.ClassificationId = entity.ClassificationId;
            entityVm.ListProp20 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 20);
            entityVm.ListProp21 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 21);
            entityVm.ListProp22 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 22);

            LabAdditionalRequirementsFormViewModel formVm = new LabAdditionalRequirementsFormViewModel { ClassificationId = entity.ClassificationId };

            var info = FactoriyClassificationRepository.GetLabAdditionalReqInfo(id);
            if (info != null)
            {
                if (info.CylindricalMolds.HasValue)
                    entityVm.CylindricalMolds = info.CylindricalMolds.Value;

                if (info.Cubes.HasValue)
                    entityVm.Cubes = info.Cubes.Value;

                formVm.ConcreteMachine = info.ConcreteMachine;
                formVm.ControlSpeedWay = info.ControlSpeedWay ?? 0;
                formVm.IsProcessingCheck1 = info.IsProcessingCheck1 ?? false;
                formVm.IsProcessingCheck2 = info.IsProcessingCheck2 ?? false;
                formVm.IsProcessingCheck3 = info.IsProcessingCheck3 ?? false;
                formVm.MachineType = info.MachineType ?? 0;
                formVm.ManufacturingYear = info.ManufacturingYear;

            }

            ViewBag.FormViewModel = formVm;
            ViewBag.ClassificationItem = entity;
            return View(entityVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'LabAdditionalRequirementsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult LabAdditionalRequirements(LabAdditionalRequirementsViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'LabAdditionalRequirementsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {


            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp20, itemVm.ClassificationId, 20);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp21, itemVm.ClassificationId, 21);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp22, itemVm.ClassificationId, 22);

            var info = FactoriyClassificationRepository.GetLabAdditionalReqInfo(itemVm.ClassificationId);
            bool isAdd = false;
            if (info == null)
            {
                isAdd = true;
                info = new Cls_LabAdditionalReqInfo { ClassificationId = itemVm.ClassificationId };
            }
            info.CylindricalMolds = itemVm.CylindricalMolds;
            info.Cubes = itemVm.Cubes;
            FactoriyClassificationRepository.SaveLabAdditionalReqInfo(info, isAdd);


            var entity = FactoriyClassificationRepository.GetClassificationById(itemVm.ClassificationId, false);
            entity.IsAddLabReqDone = true;
            FactoriyClassificationRepository.SaveClassificationItem(entity);


            return RedirectToAction("LabAdditionalRequirements", new { id = itemVm.ClassificationId, isSuccess = true });

        }



        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'LabAdditionalRequirementsFormViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult LabAdditionalReqForm(LabAdditionalRequirementsFormViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'LabAdditionalRequirementsFormViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            var info = FactoriyClassificationRepository.GetLabAdditionalReqInfo(itemVm.ClassificationId);
            bool isAdd = false;
            if (info == null)
            {
                isAdd = true;
                info = new Cls_LabAdditionalReqInfo { ClassificationId = itemVm.ClassificationId };
            }
            info.ConcreteMachine = itemVm.ConcreteMachine;
            info.ControlSpeedWay = itemVm.ControlSpeedWay;
            info.IsProcessingCheck1 = itemVm.IsProcessingCheck1;
            info.IsProcessingCheck2 = itemVm.IsProcessingCheck2;
            info.IsProcessingCheck3 = itemVm.IsProcessingCheck3;
            info.MachineType = itemVm.MachineType;
            info.ManufacturingYear = itemVm.ManufacturingYear;
            FactoriyClassificationRepository.SaveLabAdditionalReqInfo(info, isAdd);


            var entity = FactoriyClassificationRepository.GetClassificationById(itemVm.ClassificationId, false);
            entity.IsAddLabReqDone = true;
            FactoriyClassificationRepository.SaveClassificationItem(entity);


            return RedirectToAction("LabAdditionalRequirements", new { id = itemVm.ClassificationId, isSuccess = true });

        }

        public ActionResult ConcreteMixturesInfo(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var entity = FactoriyClassificationRepository.GetClassificationById(id, false);

            ConcreteMixturesInfoFormViewModel entityVm = new ConcreteMixturesInfoFormViewModel();


            var info = FactoriyClassificationRepository.GetConcreteMixturesInfo(id);
            if (info != null)
            {
                entityVm = FactoriyClassificationRepository.GetConcreteMixturesInfoViewModel(info);
            }

            entityVm.ClassificationId = entity.ClassificationId;

            ViewBag.ClassificationItem = entity;
            return View(entityVm);
        }


        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'ConcreteMixturesInfoFormViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult ConcreteMixturesInfo(ConcreteMixturesInfoFormViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'ConcreteMixturesInfoFormViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            var entity = FactoriyClassificationRepository.GetConcreteMixturesInfo(itemVm.ClassificationId);
            bool isAdd = false;
            if (entity == null)
            {
                isAdd = true;
                entity = new Cls_ConcreteMixturesInfo { ClassificationId = itemVm.ClassificationId };
            }

            entity.CementKg20 = itemVm.CementKg20;
            entity.CementKg30 = itemVm.CementKg30;
            entity.CementKg35 = itemVm.CementKg35;
            entity.CementKg40 = itemVm.CementKg40;
            entity.HotWater20 = itemVm.HotWater20;
            entity.HotWater30 = itemVm.HotWater30;
            entity.HotWater35 = itemVm.HotWater35;
            entity.HotWater40 = itemVm.HotWater40;
            entity.MixResult20 = itemVm.MixResult20;
            entity.MixResult30 = itemVm.MixResult30;
            entity.MixResult35 = itemVm.MixResult35;
            entity.MixResult40 = itemVm.MixResult40;
            entity.Rookam3by4Kg20 = itemVm.Rookam3by4Kg20;
            entity.Rookam3by4Kg30 = itemVm.Rookam3by4Kg30;
            entity.Rookam3by4Kg35 = itemVm.Rookam3by4Kg35;
            entity.Rookam3by4Kg40 = itemVm.Rookam3by4Kg40;
            entity.Rookam3by8Kg20 = itemVm.Rookam3by8Kg20;
            entity.Rookam3by8Kg30 = itemVm.Rookam3by8Kg30;
            entity.Rookam3by8Kg35 = itemVm.Rookam3by8Kg35;
            entity.Rookam3by8Kg40 = itemVm.Rookam3by8Kg40;
            entity.RP264Litter20 = itemVm.RP264Litter20;
            entity.RP264Litter30 = itemVm.RP264Litter30;
            entity.RP264Litter35 = itemVm.RP264Litter35;
            entity.RP264Litter40 = itemVm.RP264Litter40;
            entity.SelicaSandKg20 = itemVm.SelicaSandKg20;
            entity.SelicaSandKg30 = itemVm.SelicaSandKg30;
            entity.SelicaSandKg35 = itemVm.SelicaSandKg35;
            entity.SelicaSandKg40 = itemVm.SelicaSandKg40;
            entity.WachingSandKg20 = itemVm.WachingSandKg20;
            entity.WachingSandKg30 = itemVm.WachingSandKg30;
            entity.WachingSandKg35 = itemVm.WachingSandKg35;
            entity.WachingSandKg40 = itemVm.WachingSandKg40;

            entity.ConcWater20 = itemVm.ConcWater20;
            entity.ConcWater30 = itemVm.ConcWater30;
            entity.ConcWater35 = itemVm.ConcWater35;
            entity.ConcWater40 = itemVm.ConcWater40;
            entity.FinishDate = itemVm.FinishDate;

            FactoriyClassificationRepository.SaveConcreteMixturesInfo(entity, isAdd);


            var clsItem = FactoriyClassificationRepository.GetClassificationById(itemVm.ClassificationId, false);
            clsItem.IsConcMixturesDone = true;
            FactoriyClassificationRepository.SaveClassificationItem(clsItem);


            return RedirectToAction("ConcreteMixturesInfo", new { id = itemVm.ClassificationId, isSuccess = true });

        }



        public ActionResult PerformanceEffEvalInfo(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var entity = FactoriyClassificationRepository.GetClassificationById(id, false);

            Cls_PerformanceEffEvalInfoViewModel entityVm = new Cls_PerformanceEffEvalInfoViewModel();
            entityVm.ClassificationId = entity.ClassificationId;


            var info = FactoriyClassificationRepository.GetPerformanceEffEvalInfo(id);
            if (info != null)
            {
                entityVm.CompressiveStrength = info.CompressiveStrength;
                entityVm.Slump = info.Slump;
                entityVm.TempGeneralComment = info.TempGeneralComment;
                entityVm.TempQualtyMaxComment = info.TempQualtyMaxComment;
                entityVm.TempQualtyMinComment1 = info.TempQualtyMinComment1;
            }

            ViewBag.ClassificationItem = entity;
            return View(entityVm);
        }


        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'Cls_PerformanceEffEvalInfoViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult PerformanceEffEvalInfo(Cls_PerformanceEffEvalInfoViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'Cls_PerformanceEffEvalInfoViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            var entity = FactoriyClassificationRepository.GetPerformanceEffEvalInfo(itemVm.ClassificationId);
            bool isAdd = false;
            if (entity == null)
            {
                isAdd = true;
                entity = new Cls_PerformanceEffEvalInfo { ClassificationId = itemVm.ClassificationId };
            }

            entity.CompressiveStrength = itemVm.CompressiveStrength;
            entity.Slump = itemVm.Slump;
            entity.TempGeneralComment = itemVm.TempGeneralComment;
            entity.TempQualtyMaxComment = itemVm.TempQualtyMaxComment;
            entity.TempQualtyMinComment1 = itemVm.TempQualtyMinComment1;

            FactoriyClassificationRepository.SavePerformanceEffEvalInfo(entity, isAdd);


            var clsItem = FactoriyClassificationRepository.GetClassificationById(itemVm.ClassificationId, false);
            clsItem.IsOperatingEffDone = true;
            FactoriyClassificationRepository.SaveClassificationItem(clsItem);


            return RedirectToAction("PerformanceEffEvalInfo", new { id = itemVm.ClassificationId, isSuccess = true });

        }


        public ActionResult FinalSummary(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var entity = FactoriyClassificationRepository.GetClassificationById(id, false);

            Cls_FinalSummaryViewModel entityVm = new Cls_FinalSummaryViewModel();
            entityVm.ClassificationId = entity.ClassificationId;
            entityVm.ClassificationGrade = entity.ClassificationGrade;
            entityVm.ClassificationText = entity.ClassificationText;

            ViewBag.ClassificationItem = entity;
            return View(entityVm);
        }


        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'Cls_FinalSummaryViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult FinalSummary(Cls_FinalSummaryViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'Cls_FinalSummaryViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {

            var clsItem = FactoriyClassificationRepository.GetClassificationById(itemVm.ClassificationId, false);
            clsItem.IsOveralReport = true;
            clsItem.ClassificationText = itemVm.ClassificationText;
            clsItem.ClassificationGrade = itemVm.ClassificationGrade;
            FactoriyClassificationRepository.SaveClassificationItem(clsItem);


            return RedirectToAction("FinalSummary", new { id = itemVm.ClassificationId, isSuccess = true });

        }



        #region FactoryMixingStations

        public ActionResult AddMixingStations(int? id, int classificationId)
        {
            //ViewBag.Category = SettingsRepository.GetCategoryById(propertyId);

            Cls_MixingStationsViewModel itemVm = new Cls_MixingStationsViewModel();

            itemVm.ClassificationId = classificationId;

            if (id.HasValue)
            {
                var item = FactoriyClassificationRepository.GetMixingStation(id.Value);
                itemVm.StationId = item.StationId;
                itemVm.CanGetRecordAnyTime = item.CanGetRecordAnyTime.Value;
                itemVm.CanReadDevices = item.CanReadDevices.Value;
                itemVm.IsEasyRead = item.IsEasyRead.Value;
                itemVm.IsEveryScaleTwo = item.IsEveryScaleTwo.Value;
                itemVm.IsIndicatorsClosed = item.IsIndicatorsClosed.Value;
                itemVm.IsQuantityRecordExist = item.IsQuantityRecordExist.Value;
                itemVm.IsStandardWeights = item.IsStandardWeights.Value;
                itemVm.MixerCapacity = item.MixerCapacity;
                itemVm.MixerPeriod = item.MixerPeriod;
                itemVm.MixerType = item.MixerType;
                itemVm.ScalesIndicator = item.ScalesIndicator;
                itemVm.SizeControlSystem = item.SizeControlSystem;
                itemVm.StationNumber = item.StationNumber;
                itemVm.WeightControlSystem = item.WeightControlSystem;
            }


            return View("_AddMixingStations", itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'Cls_MixingStationsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddMixingStations(Cls_MixingStationsViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'Cls_MixingStationsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            Cls_MixingStations item = new Cls_MixingStations();

            if (itemVm.StationId > 0)
            {
                item = FactoriyClassificationRepository.GetMixingStation(itemVm.StationId.Value);
            }

            item.ClassificationId = itemVm.ClassificationId;
            item.CanGetRecordAnyTime = itemVm.CanGetRecordAnyTime;
            item.CanReadDevices = itemVm.CanReadDevices;
            item.IsEasyRead = itemVm.IsEasyRead;
            item.IsEveryScaleTwo = itemVm.IsEveryScaleTwo;
            item.IsIndicatorsClosed = itemVm.IsIndicatorsClosed;
            item.IsQuantityRecordExist = itemVm.IsQuantityRecordExist;
            item.IsStandardWeights = itemVm.IsStandardWeights;
            item.MixerCapacity = itemVm.MixerCapacity;
            item.MixerPeriod = itemVm.MixerPeriod;
            item.MixerType = itemVm.MixerType;
            item.ScalesIndicator = itemVm.ScalesIndicator;
            item.SizeControlSystem = itemVm.SizeControlSystem;
            item.StationNumber = itemVm.StationNumber;
            item.WeightControlSystem = itemVm.WeightControlSystem;
            FactoriyClassificationRepository.SaveMixingStation(item);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("AdditionalRequirements", new { id = item.ClassificationId, isSuccess = true });

        }
        public ActionResult DeleteMixingStation(int id)
        {

            Cls_MixingStations item = FactoriyClassificationRepository.GetMixingStation(id);
            int cId = item.ClassificationId;
            FactoriyClassificationRepository.DeleteMixingStation(item);

            return RedirectToAction("AdditionalRequirements", new { id = cId, isSuccess = true });
        }

        #endregion


        #region LabQcRecords

        public ActionResult LabQCRequirements(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var entity = FactoriyClassificationRepository.GetClassificationById(id, false);

            QCTestOfConcreteViewModel entityVm = new QCTestOfConcreteViewModel();
            entityVm.ClassificationId = entity.ClassificationId;
            //entityVm.ListProp26 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 26);
            entityVm.ListLabProp26 = FactoriyClassificationRepository.GetClassificationLabPropertyList(id, 26);
            entityVm.ListLabProp24 = FactoriyClassificationRepository.GetClassificationLabPropertyList(id, 24);
            entityVm.ListLabProp25 = FactoriyClassificationRepository.GetClassificationLabPropertyList(id, 25);
            entityVm.ListLabProp34 = FactoriyClassificationRepository.GetClassificationLabPropertyList(id, 34);

            QCConcreteTestingViewModel qCConcreteTestingVm = new QCConcreteTestingViewModel { ClassificationId = entity.ClassificationId };
            qCConcreteTestingVm.ListLabProp28 = FactoriyClassificationRepository.GetClassificationLabPropertyList(id, 28);


            QCLaboratoryRecordsViewModel qCLaboratoryRecordsVm = new QCLaboratoryRecordsViewModel { ClassificationId = entity.ClassificationId };
            qCLaboratoryRecordsVm.ListProp29 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 29);
            qCLaboratoryRecordsVm.ListProp30 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 30);
            qCLaboratoryRecordsVm.ListProp31 = FactoriyClassificationRepository.GetClassificationPropertyList(id, 31);

            var info = FactoriyClassificationRepository.GetLabQCReqInfo(id);

            if (info != null)
            {
                //entityVm.IsTechniqcalSheet = info.IsTechniqcalSheet?? false;
                //entityVm.TrialMixesEvalutePeriodicId = info.TrialMixesEvalutePeriodicId ?? 0;

                qCConcreteTestingVm.CompressiveStrengthA = info.CompressiveStrengthA ?? false;
                qCConcreteTestingVm.CompressiveStrengthB = info.CompressiveStrengthB ?? false;
                qCConcreteTestingVm.CuringConcreteSamplesA = info.CuringConcreteSamplesA ?? false;
                qCConcreteTestingVm.CuringConcreteSamplesB = info.CuringConcreteSamplesB ?? false;
                qCConcreteTestingVm.IsProperProtection = info.IsProperProtection ?? false;
            }

            ViewBag.QCConcreteTestingVm = qCConcreteTestingVm;
            ViewBag.QCLaboratoryRecordsVm = qCLaboratoryRecordsVm;
            ViewBag.ClassificationItem = entity;
            return View(entityVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'QCTestOfConcreteViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult LabQCTestOfConcreteForm(QCTestOfConcreteViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'QCTestOfConcreteViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {


            //FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp26, itemVm.ClassificationId, 26);
            FactoriyClassificationRepository.SaveClassificationLabPropertyList(itemVm.ListLabProp26, itemVm.ClassificationId, 26);
            FactoriyClassificationRepository.SaveClassificationLabPropertyList(itemVm.ListLabProp24, itemVm.ClassificationId, 24);
            FactoriyClassificationRepository.SaveClassificationLabPropertyList(itemVm.ListLabProp25, itemVm.ClassificationId, 25);
            FactoriyClassificationRepository.SaveClassificationLabPropertyList(itemVm.ListLabProp34, itemVm.ClassificationId, 34);

            var info = FactoriyClassificationRepository.GetLabQCReqInfo(itemVm.ClassificationId);
            bool isAdd = false;
            if (info == null)
            {
                isAdd = true;
                info = new Cls_LabQCReqInfo { ClassificationId = itemVm.ClassificationId };
            }
            //info.IsTechniqcalSheet = itemVm.IsTechniqcalSheet;
            //info.TrialMixesEvalutePeriodicId = itemVm.TrialMixesEvalutePeriodicId;
            FactoriyClassificationRepository.SaveLabQCReqInfo(info, isAdd);


            var entity = FactoriyClassificationRepository.GetClassificationById(itemVm.ClassificationId, false);
            entity.IsQCLabReqDone = true;
            FactoriyClassificationRepository.SaveClassificationItem(entity);


            return RedirectToAction("LabQCRequirements", new { id = itemVm.ClassificationId, isSuccess = true });

        }



        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'QCConcreteTestingViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult LabQCConcreteTestingForm(QCConcreteTestingViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'QCConcreteTestingViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            FactoriyClassificationRepository.SaveClassificationLabPropertyList(itemVm.ListLabProp28, itemVm.ClassificationId, 28);


            var info = FactoriyClassificationRepository.GetLabQCReqInfo(itemVm.ClassificationId);
            bool isAdd = false;
            if (info == null)
            {
                isAdd = true;
                info = new Cls_LabQCReqInfo { ClassificationId = itemVm.ClassificationId };
            }
            info.CompressiveStrengthA = itemVm.CompressiveStrengthA;
            info.CompressiveStrengthB = itemVm.CompressiveStrengthB;
            info.CuringConcreteSamplesA = itemVm.CuringConcreteSamplesA;
            info.CuringConcreteSamplesB = itemVm.CuringConcreteSamplesB;
            info.IsProperProtection = itemVm.IsProperProtection;

            FactoriyClassificationRepository.SaveLabQCReqInfo(info, isAdd);


            var entity = FactoriyClassificationRepository.GetClassificationById(itemVm.ClassificationId, false);
            entity.IsQCLabReqDone = true;
            FactoriyClassificationRepository.SaveClassificationItem(entity);


            return RedirectToAction("LabQCRequirements", new { id = itemVm.ClassificationId, isSuccess = true });

        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'QCLaboratoryRecordsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult LabQCLaboratoryRecordsForm(QCLaboratoryRecordsViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'QCLaboratoryRecordsViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp29, itemVm.ClassificationId, 29);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp30, itemVm.ClassificationId, 30);
            FactoriyClassificationRepository.SaveClassificationPropertyList(itemVm.ListProp31, itemVm.ClassificationId, 31);




            var entity = FactoriyClassificationRepository.GetClassificationById(itemVm.ClassificationId, false);
            entity.IsQCLabReqDone = true;
            FactoriyClassificationRepository.SaveClassificationItem(entity);


            return RedirectToAction("LabQCRequirements", new { id = itemVm.ClassificationId, isSuccess = true });

        }

        #endregion
    }
}