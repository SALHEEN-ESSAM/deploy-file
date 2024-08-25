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
using Amana.ViewModels.MixDesigns;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System;
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
    public class MixDesignsLabController : ControllerHelper
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





        public ActionResult ResultConcClsInfo(int id, int? cls, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            cls = cls ?? 15;

            ViewBag.RequestItem = MixDesignsRepository.GetRequestItem(id);

            MD_RequestLabConcTrialMixViewModel itemVm = new MD_RequestLabConcTrialMixViewModel();
            itemVm.ConcClsId = cls.Value;
            itemVm.RequestId = id;
            var item = MixDesignsRepository.GetRequestLabConcTrialMixItem(id, cls.Value);

            if (item != null)
            {
                itemVm.ItemId = item.ItemId;
                itemVm.ConcClsId = item.ConcClsId;
                itemVm.RequestId = item.RequestId;
                itemVm.MD_AggCA34 = item.MD_AggCA34;
                itemVm.MD_AggCA38 = item.MD_AggCA38;
                itemVm.MD_AggCAAvgBulk = item.MD_AggCAAvgBulk;
                itemVm.MD_AggCADryRodded = item.MD_AggCADryRodded;
                itemVm.MD_AggFAAvgBulk = item.MD_AggFAAvgBulk;
                itemVm.MD_AggFAAvgFineness = item.MD_AggFAAvgFineness;
                itemVm.MD_AggFACrushedSand = item.MD_AggFACrushedSand;
                itemVm.MD_AggFADuneSand = item.MD_AggFADuneSand;
                itemVm.MD_MixProp34Size = item.MD_MixProp34Size;
                itemVm.MD_MixProp38Size = item.MD_MixProp38Size;
                itemVm.MD_MixPropAdmixture1 = item.MD_MixPropAdmixture1;
                itemVm.MD_MixPropAdmixture2 = item.MD_MixPropAdmixture2;
                itemVm.MD_MixPropCement = item.MD_MixPropCement;
                itemVm.MD_MixPropCrushedSand = item.MD_MixPropCrushedSand;
                itemVm.MD_MixPropDuneSand = item.MD_MixPropDuneSand;
                itemVm.MD_MixPropWater = item.MD_MixPropWater;
                itemVm.MD_MixPropWCRatio = item.MD_MixPropWCRatio;
                itemVm.MD_ReqAdmixtureType = item.MD_ReqAdmixtureType;
                itemVm.MD_ReqCementContent = item.MD_ReqCementContent;
                itemVm.MD_ReqCompStrength = item.MD_ReqCompStrength;
                itemVm.MD_ReqSlumbAtSite = item.MD_ReqSlumbAtSite;
                itemVm.TM_34SizeAbs = item.TM_34SizeAbs;
                itemVm.TM_34SizeCW = item.TM_34SizeCW;
                itemVm.TM_34SizeMC = item.TM_34SizeMC;
                itemVm.TM_38SizeAbs = item.TM_38SizeAbs;
                itemVm.TM_38SizeCW = item.TM_38SizeCW;
                itemVm.TM_38SizeMC = item.TM_38SizeMC;
                itemVm.TM_Admixture1CW = item.TM_Admixture1CW;
                itemVm.TM_Admixture2CW = item.TM_Admixture2CW;
                itemVm.TM_AirContent = item.TM_AirContent;
                itemVm.TM_AirTemp = item.TM_AirTemp;
                itemVm.TM_CementCW = item.TM_CementCW;
                itemVm.TM_CompStrength28Days1 = item.TM_CompStrength28Days1;
                itemVm.TM_CompStrength28Days2 = item.TM_CompStrength28Days2;
                itemVm.TM_CompStrength28Days3 = item.TM_CompStrength28Days3;
                itemVm.TM_CompStrength28DaysAvg = item.TM_CompStrength28DaysAvg;
                itemVm.TM_CompStrength7Days1 = item.TM_CompStrength7Days1;
                itemVm.TM_CompStrength7Days2 = item.TM_CompStrength7Days2;
                itemVm.TM_CompStrength7Days3 = item.TM_CompStrength7Days3;
                itemVm.TM_CompStrength7DaysAvg = item.TM_CompStrength7DaysAvg;
                itemVm.TM_CrushedSandAbs = item.TM_CrushedSandAbs;
                itemVm.TM_CrushedSandCW = item.TM_CrushedSandCW;
                itemVm.TM_CrushedSandMC = item.TM_CrushedSandMC;
                itemVm.TM_DensityOfConcrete = item.TM_DensityOfConcrete;
                itemVm.TM_DuneSandAbs = item.TM_DuneSandAbs;
                itemVm.TM_DuneSandCW = item.TM_DuneSandCW;
                itemVm.TM_DuneSandMC = item.TM_DuneSandMC;
                itemVm.TM_MixTemp = item.TM_MixTemp;
                itemVm.TM_Slumb30Min = item.TM_Slumb30Min;
                itemVm.TM_Slumb60Min = item.TM_Slumb60Min;
                itemVm.TM_Slumb90Min = item.TM_Slumb90Min;
                itemVm.TM_SlumbInitial = item.TM_SlumbInitial;
                itemVm.TM_WaterCW = item.TM_WaterCW;
            }

            //var materials = MixDesignsRepository.GetMdMaterials(1, true);
            //foreach (var mat in materials)
            //{
            //    MdRequestConClsMaterialsViewModel cm = new MdRequestConClsMaterialsViewModel();
            //    cm.ConcClsInfoItemId = itemVm.ItemId;
            //    cm.MaterialId = mat.MaterialId;
            //    cm.MaterialName = mat.Name;
            //    if (item != null)
            //    {
            //        var exist = item.MD_RequestConClsMaterials.FirstOrDefault(a => a.MaterialId == mat.MaterialId);
            //        if (exist != null)
            //        {
            //            cm.ID = exist.ID;
            //            cm.CorrectedWeight = exist.CorrectedWeight;
            //            cm.UncorrectedWeight = exist.UncorrectedWeight;
            //        }
            //    }

            //    itemVm.ListConClsMaterials.Add(cm);
            //}

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MD_RequestLabConcTrialMixViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult ResultConcClsInfo(MD_RequestLabConcTrialMixViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'MD_RequestLabConcTrialMixViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            MD_RequestLabConcTrialMix item = new MD_RequestLabConcTrialMix();
            var requestInfo = MixDesignsRepository.GetRequestItem(itemVm.RequestId);

            if (itemVm.ItemId > 0)
            {
                item = MixDesignsRepository.GetRequestLabConcTrialMixItem(itemVm.RequestId, itemVm.ConcClsId);
            }

            item.ConcClsId = itemVm.ConcClsId;
            item.ItemId = itemVm.ItemId;
            item.RequestId = itemVm.RequestId;
            item.MD_AggCA34 = itemVm.MD_AggCA34;
            item.MD_AggCA38 = itemVm.MD_AggCA38;
            item.MD_AggCAAvgBulk = itemVm.MD_AggCAAvgBulk;
            item.MD_AggCADryRodded = itemVm.MD_AggCADryRodded;
            item.MD_AggFAAvgBulk = itemVm.MD_AggFAAvgBulk;
            //item.CompressiveStrength28DaysAvg = Math.Round(((double)(item.CompressiveStrength28Days1 + item.CompressiveStrength28Days2 + item.CompressiveStrength28Days3)) / 3, 1);

            item.MD_AggFAAvgFineness = itemVm.MD_AggFAAvgFineness;
            item.MD_AggFACrushedSand = itemVm.MD_AggFACrushedSand;
            item.MD_AggFADuneSand = itemVm.MD_AggFADuneSand;
            //item.CompressiveStrength7DaysAvg = Math.Round(((double)(item.CompressiveStrength7Days1 + item.CompressiveStrength7Days2 + item.CompressiveStrength7Days3)) / 3, 1);

            item.MD_MixProp34Size = itemVm.MD_MixProp34Size;
            item.MD_MixProp38Size = itemVm.MD_MixProp38Size;
            item.MD_MixPropAdmixture1 = itemVm.MD_MixPropAdmixture1;
            item.MD_MixPropAdmixture2 = itemVm.MD_MixPropAdmixture2;
            item.MD_MixPropCement = itemVm.MD_MixPropCement;
            item.MD_MixPropCrushedSand = itemVm.MD_MixPropCrushedSand;
            item.MD_MixPropDuneSand = itemVm.MD_MixPropDuneSand;
            item.MD_MixPropWater = itemVm.MD_MixPropWater;
            item.MD_MixPropWCRatio = itemVm.MD_MixPropWCRatio;
            item.MD_ReqAdmixtureType = itemVm.MD_ReqAdmixtureType;
            item.MD_ReqCementContent = itemVm.MD_ReqCementContent;
            item.MD_ReqCompStrength = itemVm.MD_ReqCompStrength;
            item.MD_ReqSlumbAtSite = itemVm.MD_ReqSlumbAtSite;
            item.TM_34SizeAbs = itemVm.TM_34SizeAbs;
            item.TM_34SizeCW = itemVm.TM_34SizeCW;
            item.TM_34SizeMC = itemVm.TM_34SizeMC;
            item.TM_38SizeAbs = itemVm.TM_38SizeAbs;
            item.TM_38SizeCW = itemVm.TM_38SizeCW;
            item.TM_38SizeMC = itemVm.TM_38SizeMC;
            item.TM_Admixture1CW = itemVm.TM_Admixture1CW;
            item.TM_Admixture2CW = itemVm.TM_Admixture2CW;
            item.TM_AirContent = itemVm.TM_AirContent;
            item.TM_AirTemp = itemVm.TM_AirTemp;
            item.TM_CementCW = itemVm.TM_CementCW;
            item.TM_CompStrength28Days1 = itemVm.TM_CompStrength28Days1;
            item.TM_CompStrength28Days2 = itemVm.TM_CompStrength28Days2;
            item.TM_CompStrength28Days3 = itemVm.TM_CompStrength28Days3;
            item.TM_CompStrength28DaysAvg = Math.Round(((double)(item.TM_CompStrength28Days1 + item.TM_CompStrength28Days2 + item.TM_CompStrength28Days3)) / 3, 1);
            item.TM_CompStrength7Days1 = itemVm.TM_CompStrength7Days1;
            item.TM_CompStrength7Days2 = itemVm.TM_CompStrength7Days2;
            item.TM_CompStrength7Days3 = itemVm.TM_CompStrength7Days3;
            item.TM_CompStrength7DaysAvg = Math.Round(((double)(item.TM_CompStrength7Days1 + item.TM_CompStrength7Days2 + item.TM_CompStrength7Days3)) / 3, 1);
            item.TM_CrushedSandAbs = itemVm.TM_CrushedSandAbs;
            item.TM_CrushedSandCW = itemVm.TM_CrushedSandCW;
            item.TM_CrushedSandMC = itemVm.TM_CrushedSandMC;
            item.TM_DensityOfConcrete = itemVm.TM_DensityOfConcrete;
            item.TM_DuneSandAbs = itemVm.TM_DuneSandAbs;
            item.TM_DuneSandCW = itemVm.TM_DuneSandCW;
            item.TM_DuneSandMC = itemVm.TM_DuneSandMC;
            item.TM_MixTemp = itemVm.TM_MixTemp;
            item.TM_Slumb30Min = itemVm.TM_Slumb30Min;
            item.TM_Slumb60Min = itemVm.TM_Slumb60Min;
            item.TM_Slumb90Min = itemVm.TM_Slumb90Min;
            item.TM_SlumbInitial = itemVm.TM_SlumbInitial;
            item.TM_WaterCW = itemVm.TM_WaterCW;


            int conClsInfoId = MixDesignsRepository.SaveRequestLabConcTrialMix(item);





            switch (item.ConcClsId)
            {
                case 15:
                    {
                        requestInfo.IsLabC15Complete = true;
                        break;
                    }
                case 25:
                    {
                        requestInfo.IsLabC25Complete = true;
                        break;
                    }
                case 30:
                    {
                        requestInfo.IsLabC30Complete = true;
                        break;
                    }
                case 35:
                    {
                        requestInfo.IsLabC35Complete = true;
                        break;
                    }
            }


            MixDesignsRepository.SaveRequest(requestInfo);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("ResultConcClsInfo", new { id = item.RequestId, cls = item.ConcClsId, isSuccess = true });

        }


        public ActionResult GradationCA(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;



            var requestItem = MixDesignsRepository.GetRequestItem(id);
            ViewBag.RequestItem = requestItem;

            MD_LabGradationCAViewModel itemVm = new MD_LabGradationCAViewModel();

            itemVm.RequestId = id;
            itemVm.CA_34Dry = requestItem.CA_34Dry;
            itemVm.CA_34MaxSize = requestItem.CA_34MaxSize;
            itemVm.CA_34OvenDry = requestItem.CA_34OvenDry;
            itemVm.CA_34PanMassRetained = requestItem.CA_34PanMassRetained;
            itemVm.CA_34Wet = requestItem.CA_34Wet;
            itemVm.CA_38Dry = requestItem.CA_38Dry;
            itemVm.CA_38MaxSize = requestItem.CA_38MaxSize;
            itemVm.CA_38OvenDry = requestItem.CA_38OvenDry;
            itemVm.CA_38PanMassRetained = requestItem.CA_38PanMassRetained;
            itemVm.CA_38Wet = requestItem.CA_38Wet;

            itemVm.ListGradSettings = MixDesignsRepository.GetRequestGradSettingsList(itemVm.RequestId, "CA");



            return View(itemVm);

        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MD_LabGradationCAViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult GradationCA(MD_LabGradationCAViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'MD_LabGradationCAViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            var requestInfo = MixDesignsRepository.GetRequestItem(itemVm.RequestId);

            requestInfo.CA_34Dry = itemVm.CA_34Dry;
            requestInfo.CA_34MaxSize = itemVm.CA_34MaxSize;
            requestInfo.CA_34OvenDry = itemVm.CA_34OvenDry;
            requestInfo.CA_34PanMassRetained = itemVm.CA_34PanMassRetained;
            requestInfo.CA_34Wet = itemVm.CA_34Wet;
            requestInfo.CA_38Dry = itemVm.CA_38Dry;
            requestInfo.CA_38MaxSize = itemVm.CA_38MaxSize;
            requestInfo.CA_38OvenDry = itemVm.CA_38OvenDry;
            requestInfo.CA_38PanMassRetained = itemVm.CA_38PanMassRetained;
            requestInfo.CA_38Wet = itemVm.CA_38Wet;

            requestInfo.IsLabGradCAComplete = true;

            MixDesignsRepository.SaveRequest(requestInfo);
            MixDesignsRepository.SaveRequestGradSettingsList(itemVm.ListGradSettings, requestInfo, "CA");

            return RedirectToAction("GradationCA", new { id = itemVm.RequestId, isSuccess = true });

        }

        public ActionResult GradationFA(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;



            var requestItem = MixDesignsRepository.GetRequestItem(id);
            ViewBag.RequestItem = requestItem;

            MD_LabGradationFAViewModel itemVm = new MD_LabGradationFAViewModel();

            itemVm.RequestId = id;
            itemVm.FA_CsDry = requestItem.FA_CsDry;
            itemVm.FA_CsMaxSize = requestItem.FA_CsMaxSize;
            itemVm.FA_CsOvenDry = requestItem.FA_CsOvenDry;
            itemVm.FA_CsPanMassRetained = requestItem.FA_CsPanMassRetained;
            itemVm.FA_CsWet = requestItem.FA_CsWet;
            itemVm.FA_DsDry = requestItem.FA_DsDry;
            itemVm.FA_DsMaxSize = requestItem.FA_DsMaxSize;
            itemVm.FA_DsOvenDry = requestItem.FA_DsOvenDry;
            itemVm.FA_DsPanMassRetained = requestItem.FA_DsPanMassRetained;
            itemVm.FA_DsWet = requestItem.FA_DsWet;

            itemVm.ListGradSettings = MixDesignsRepository.GetRequestGradSettingsList(itemVm.RequestId, "FA");



            return View(itemVm);

        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MD_LabGradationFAViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult GradationFA(MD_LabGradationFAViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'MD_LabGradationFAViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            var requestInfo = MixDesignsRepository.GetRequestItem(itemVm.RequestId);

            requestInfo.FA_CsDry = itemVm.FA_CsDry;
            requestInfo.FA_CsMaxSize = itemVm.FA_CsMaxSize;
            requestInfo.FA_CsOvenDry = itemVm.FA_CsOvenDry;
            requestInfo.FA_CsPanMassRetained = itemVm.FA_CsPanMassRetained;
            requestInfo.FA_CsWet = itemVm.FA_CsWet;
            requestInfo.FA_DsDry = itemVm.FA_DsDry;
            requestInfo.FA_DsMaxSize = itemVm.FA_DsMaxSize;
            requestInfo.FA_DsOvenDry = itemVm.FA_DsOvenDry;
            requestInfo.FA_DsPanMassRetained = itemVm.FA_DsPanMassRetained;
            requestInfo.FA_DsWet = itemVm.FA_DsWet;

            requestInfo.IsLabGradFAComplete = true;

            MixDesignsRepository.SaveRequest(requestInfo);
            MixDesignsRepository.SaveRequestGradSettingsList(itemVm.ListGradSettings, requestInfo, "FA");

            return RedirectToAction("GradationFA", new { id = itemVm.RequestId, isSuccess = true });

        }

        public ActionResult SummaryCA(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;



            var requestItem = MixDesignsRepository.GetRequestItem(id);
            ViewBag.RequestItem = requestItem;

            MD_LabSummaryCAViewModel itemVm = new MD_LabSummaryCAViewModel();

            itemVm.RequestId = id;
            itemVm.CA_34MaxSize = requestItem.CA_34MaxSize;
            itemVm.CA_38MaxSize = requestItem.CA_38MaxSize;
            itemVm.SummCA_Absorption1 = requestItem.SummCA_Absorption1;
            itemVm.SummCA_Absorption2 = requestItem.SummCA_Absorption2;
            itemVm.SummCA_AbsorptionFileUrl = requestItem.SummCA_AbsorptionFileUrl;
            itemVm.SummCA_Bulk1 = requestItem.SummCA_Bulk1;
            itemVm.SummCA_Bulk2 = requestItem.SummCA_Bulk2;
            itemVm.SummCA_BulkFileUrl = requestItem.SummCA_BulkFileUrl;
            itemVm.SummCA_Chloride1 = requestItem.SummCA_Chloride1;
            itemVm.SummCA_Chloride2 = requestItem.SummCA_Chloride2;
            itemVm.SummCA_ChlorideFileUrl = requestItem.SummCA_ChlorideFileUrl;
            itemVm.SummCA_Clay1 = requestItem.SummCA_Clay1;
            itemVm.SummCA_Clay2 = requestItem.SummCA_Clay2;
            itemVm.SummCA_ClayFileUrl = requestItem.SummCA_ClayFileUrl;
            itemVm.SummCA_Flaky1 = requestItem.SummCA_Flaky1;
            itemVm.SummCA_Flaky2 = requestItem.SummCA_Flaky2;
            itemVm.SummCA_FlakyFileUrl = requestItem.SummCA_FlakyFileUrl;
            itemVm.SummCA_LosAngeles1 = requestItem.SummCA_LosAngeles1;
            itemVm.SummCA_LosAngeles2 = requestItem.SummCA_LosAngeles2;
            itemVm.SummCA_LosAngelesFileUrl = requestItem.SummCA_LosAngelesFileUrl;
            itemVm.SummCA_PassingSieve1 = requestItem.SummCA_PassingSieve1;
            itemVm.SummCA_PassingSieve2 = requestItem.SummCA_PassingSieve2;
            itemVm.SummCA_PassingSieveFileUrl = requestItem.SummCA_PassingSieveFileUrl;
            itemVm.SummCA_Soundness1 = requestItem.SummCA_Soundness1;
            itemVm.SummCA_Soundness2 = requestItem.SummCA_Soundness2;
            itemVm.SummCA_SoundnessFileUrl = requestItem.SummCA_SoundnessFileUrl;
            itemVm.SummCA_Sulphate1 = requestItem.SummCA_Sulphate1;
            itemVm.SummCA_Sulphate2 = requestItem.SummCA_Sulphate2;
            itemVm.SummCA_SulphateFileUrl = requestItem.SummCA_SulphateFileUrl;


            return View(itemVm);

        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MD_LabSummaryCAViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult SummaryCA(MD_LabSummaryCAViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'MD_LabSummaryCAViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            var requestInfo = MixDesignsRepository.GetRequestItem(itemVm.RequestId);

            requestInfo.SummCA_Absorption1 = itemVm.SummCA_Absorption1;
            requestInfo.SummCA_Absorption2 = itemVm.SummCA_Absorption2;

            if (itemVm.SummCA_AbsorptionFile != null)
                requestInfo.SummCA_AbsorptionFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummCA_AbsorptionFile);

            requestInfo.SummCA_Bulk1 = itemVm.SummCA_Bulk1;
            requestInfo.SummCA_Bulk2 = itemVm.SummCA_Bulk2;

            if (itemVm.SummCA_BulkFile != null)
                requestInfo.SummCA_BulkFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummCA_BulkFile);

            requestInfo.SummCA_Chloride1 = itemVm.SummCA_Chloride1;
            requestInfo.SummCA_Chloride2 = itemVm.SummCA_Chloride2;

            if (itemVm.SummCA_ChlorideFile != null)
                requestInfo.SummCA_ChlorideFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummCA_ChlorideFile);

            requestInfo.SummCA_Clay1 = itemVm.SummCA_Clay1;
            requestInfo.SummCA_Clay2 = itemVm.SummCA_Clay2;

            if (itemVm.SummCA_ClayFile != null)
                requestInfo.SummCA_ClayFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummCA_ClayFile);

            requestInfo.SummCA_Flaky1 = itemVm.SummCA_Flaky1;
            requestInfo.SummCA_Flaky2 = itemVm.SummCA_Flaky2;

            if (itemVm.SummCA_FlakyFile != null)
                requestInfo.SummCA_FlakyFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummCA_FlakyFile);

            requestInfo.SummCA_LosAngeles1 = itemVm.SummCA_LosAngeles1;
            requestInfo.SummCA_LosAngeles2 = itemVm.SummCA_LosAngeles2;

            if (itemVm.SummCA_LosAngelesFile != null)
                requestInfo.SummCA_LosAngelesFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummCA_LosAngelesFile);

            requestInfo.SummCA_PassingSieve1 = itemVm.SummCA_PassingSieve1;
            requestInfo.SummCA_PassingSieve2 = itemVm.SummCA_PassingSieve2;

            if (itemVm.SummCA_PassingSieveFile != null)
                requestInfo.SummCA_PassingSieveFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummCA_PassingSieveFile);

            requestInfo.SummCA_Soundness1 = itemVm.SummCA_Soundness1;
            requestInfo.SummCA_Soundness2 = itemVm.SummCA_Soundness2;

            if (itemVm.SummCA_SoundnessFile != null)
                requestInfo.SummCA_SoundnessFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummCA_SoundnessFile);

            requestInfo.SummCA_Sulphate1 = itemVm.SummCA_Sulphate1;
            requestInfo.SummCA_Sulphate2 = itemVm.SummCA_Sulphate2;

            if (itemVm.SummCA_SulphateFile != null)
                requestInfo.SummCA_SulphateFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummCA_SulphateFile);


            requestInfo.IsLabSummaryCAComplete = true;

            MixDesignsRepository.SaveRequest(requestInfo);


            return RedirectToAction("SummaryCA", new { id = itemVm.RequestId, isSuccess = true });

        }

        public ActionResult SummaryFA(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;



            var requestItem = MixDesignsRepository.GetRequestItem(id);
            ViewBag.RequestItem = requestItem;

            MD_LabSummaryFAViewModel itemVm = new MD_LabSummaryFAViewModel();

            itemVm.RequestId = id;
            itemVm.FA_CsMaxSize = requestItem.FA_CsMaxSize;
            itemVm.FA_DsMaxSize = requestItem.FA_DsMaxSize;
            itemVm.SummFA_Absorption1 = requestItem.SummFA_Absorption1;
            itemVm.SummFA_Absorption2 = requestItem.SummFA_Absorption2;
            itemVm.SummFA_AbsorptionFileUrl = requestItem.SummFA_AbsorptionFileUrl;
            itemVm.SummFA_Bulk1 = requestItem.SummFA_Bulk1;
            itemVm.SummFA_Bulk2 = requestItem.SummFA_Bulk2;
            itemVm.SummFA_BulkFileUrl = requestItem.SummFA_BulkFileUrl;
            itemVm.SummFA_Chloride1 = requestItem.SummFA_Chloride1;
            itemVm.SummFA_Chloride2 = requestItem.SummFA_Chloride2;
            itemVm.SummFA_ChlorideFileUrl = requestItem.SummFA_ChlorideFileUrl;
            itemVm.SummFA_Clay1 = requestItem.SummFA_Clay1;
            itemVm.SummFA_Clay2 = requestItem.SummFA_Clay2;
            itemVm.SummFA_ClayFileUrl = requestItem.SummFA_ClayFileUrl;
            itemVm.SummFA_FM1 = requestItem.SummFA_FM1;
            itemVm.SummFA_FM2 = requestItem.SummFA_FM2;
            itemVm.SummFA_FMFileUrl = requestItem.SummFA_FMFileUrl;
            itemVm.SummFA_PassingSieve1 = requestItem.SummFA_PassingSieve1;
            itemVm.SummFA_PassingSieve2 = requestItem.SummFA_PassingSieve2;
            itemVm.SummFA_PassingSieveFileUrl = requestItem.SummFA_PassingSieveFileUrl;
            itemVm.SummFA_Soundness1 = requestItem.SummFA_Soundness1;
            itemVm.SummFA_Soundness2 = requestItem.SummFA_Soundness2;
            itemVm.SummFA_SoundnessFileUrl = requestItem.SummFA_SoundnessFileUrl;
            itemVm.SummFA_Sulphate1 = requestItem.SummFA_Sulphate1;
            itemVm.SummFA_Sulphate2 = requestItem.SummFA_Sulphate2;
            itemVm.SummFA_SulphateFileUrl = requestItem.SummFA_SulphateFileUrl;


            return View(itemVm);

        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MD_LabSummaryFAViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult SummaryFA(MD_LabSummaryFAViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'MD_LabSummaryFAViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            var requestInfo = MixDesignsRepository.GetRequestItem(itemVm.RequestId);

            requestInfo.SummFA_Absorption1 = itemVm.SummFA_Absorption1;
            requestInfo.SummFA_Absorption2 = itemVm.SummFA_Absorption2;

            if (itemVm.SummFA_AbsorptionFile != null)
                requestInfo.SummFA_AbsorptionFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummFA_AbsorptionFile);

            requestInfo.SummFA_Bulk1 = itemVm.SummFA_Bulk1;
            requestInfo.SummFA_Bulk2 = itemVm.SummFA_Bulk2;

            if (itemVm.SummFA_BulkFile != null)
                requestInfo.SummFA_BulkFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummFA_BulkFile);

            requestInfo.SummFA_Chloride1 = itemVm.SummFA_Chloride1;
            requestInfo.SummFA_Chloride2 = itemVm.SummFA_Chloride2;

            if (itemVm.SummFA_ChlorideFile != null)
                requestInfo.SummFA_ChlorideFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummFA_ChlorideFile);

            requestInfo.SummFA_Clay1 = itemVm.SummFA_Clay1;
            requestInfo.SummFA_Clay2 = itemVm.SummFA_Clay2;

            if (itemVm.SummFA_ClayFile != null)
                requestInfo.SummFA_ClayFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummFA_ClayFile);

            requestInfo.SummFA_FM1 = itemVm.SummFA_FM1;
            requestInfo.SummFA_FM2 = itemVm.SummFA_FM2;

            if (itemVm.SummFA_FMFile != null)
                requestInfo.SummFA_FMFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummFA_FMFile);

            requestInfo.SummFA_PassingSieve1 = itemVm.SummFA_PassingSieve1;
            requestInfo.SummFA_PassingSieve2 = itemVm.SummFA_PassingSieve2;

            if (itemVm.SummFA_PassingSieveFile != null)
                requestInfo.SummFA_PassingSieveFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummFA_PassingSieveFile);

            requestInfo.SummFA_Soundness1 = itemVm.SummFA_Soundness1;
            requestInfo.SummFA_Soundness2 = itemVm.SummFA_Soundness2;

            if (itemVm.SummFA_SoundnessFile != null)
                requestInfo.SummFA_SoundnessFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummFA_SoundnessFile);

            requestInfo.SummFA_Sulphate1 = itemVm.SummFA_Sulphate1;
            requestInfo.SummFA_Sulphate2 = itemVm.SummFA_Sulphate2;

            if (itemVm.SummFA_SulphateFile != null)
                requestInfo.SummFA_SulphateFileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", itemVm.SummFA_SulphateFile);



            requestInfo.IsLabSummaryFAComplete = true;

            MixDesignsRepository.SaveRequest(requestInfo);


            return RedirectToAction("SummaryFA", new { id = itemVm.RequestId, isSuccess = true });

        }
    }
}