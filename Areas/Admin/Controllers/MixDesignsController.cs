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
using Amana.ViewModels.MixDesigns;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
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
    public class MixDesignsController : ControllerHelper
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



        public ActionResult Index(int? status, DateTime? startDate, DateTime? endDate, int? factoryId)
        {
            var factories = FactoriesRepository.GetAllFactories();
            //var users = usersDi.GetItemsList(roleId: 2, isActive: null);

            ViewData["FactoriesLst"] = new SelectList(factories, "ID", "Name", factoryId);
            //ViewData["UsersLst"] = new SelectList(users, "UserId", "Name", userId);
            //startDate = (startDate ?? HelperMethods.GetFirstDayInMonth(HelperMethods.GetCurrentDateTime()));
            //endDate = (endDate ?? HelperMethods.GetLastDayInMonth(HelperMethods.GetCurrentDateTime()));
            List<MD_Requests> result = new List<MD_Requests>();
            result = MixDesignsRepository.SearchRequests(startDate: startDate, endDate: endDate, factoryId: factoryId, status: status, isComplete: true);

            C_UserItems currrentUser = GetCurrentUser();
            //if (currrentUser.RoleId == (int)UserRolesEnum.AmanaEmployees || currrentUser.RoleId == null)
            return View(result);
            //else
            //{
            //    if(currrentUser.RoleId==(int) UserRolesEnum.)
            //    return View(result);
            //}
        }
        public ActionResult ChangeRequestStatus(int id, int status)
        {
            var request = MixDesignsRepository.GetRequestItem(id);
            request.RequestStatus = status;
            MixDesignsRepository.SaveRequest(request);


            return RedirectToAction("CreateRequestConcClsInfo", new { id, isSuccess = true });

        }


        public ActionResult MyRequests(int? status, DateTime? startDate, DateTime? endDate)
        {
            var currentUser = GetCurrentUser();
            Factories factory = FactoriesRepository.GetFactoryByUserId(currentUser.UserId);
            List<MD_Requests> result = new List<MD_Requests>();
            result = MixDesignsRepository.SearchRequests(startDate: startDate, endDate: endDate, factoryId: (factory == null ? (int?)null : factory.ID), status: status, roleId: currentUser.RoleId);

            return View(result);
        }
        public ActionResult Create()
        {
            var currentUser = GetCurrentUser();
            Factories factory = FactoriesRepository.GetFactoryByUserId(currentUser.UserId);

            int requestId = 0;
            if (factory != null && currentUser.RoleId == (int)UserRolesEnum.Factories)
            {
                MD_Requests request = new MD_Requests();
                request.CreatedBy = GetUserId();
                request.CreatedWhen = DateTime.Now;
                request.FactoryId = factory.ID;
                requestId = MixDesignsRepository.SaveRequest(request);
            }
            else
                return RedirectToAction("Index", "Home");


            return RedirectToAction("CreateRequestConcClsInfo", new { id = requestId });
        }
        public ActionResult CreateRequestConcClsInfo(int id, int? cls, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            cls = cls ?? 15;

            ViewBag.RequestItem = MixDesignsRepository.GetRequestItem(id);

            MdRequestConcClsInfoViewModel itemVm = new MdRequestConcClsInfoViewModel();
            itemVm.ConcClsId = cls.Value;
            itemVm.RequestId = id;
            var item = MixDesignsRepository.GetConClsInfoItem(id, cls.Value);

            if (item != null)
            {
                itemVm.ItemId = item.ItemId;
                itemVm.AirContent = item.AirContent;
                itemVm.AmbTemp = item.AmbTemp;
                itemVm.CompressiveStrength28Days1 = item.CompressiveStrength28Days1;
                itemVm.CompressiveStrength28Days2 = item.CompressiveStrength28Days2;
                itemVm.CompressiveStrength28Days3 = item.CompressiveStrength28Days3;
                itemVm.CompressiveStrength28DaysAvg = item.CompressiveStrength28DaysAvg;
                itemVm.CompressiveStrength7Days1 = item.CompressiveStrength7Days1;
                itemVm.CompressiveStrength7Days2 = item.CompressiveStrength7Days2;
                itemVm.CompressiveStrength7Days3 = item.CompressiveStrength7Days3;
                itemVm.CompressiveStrength7DaysAvg = item.CompressiveStrength7DaysAvg;
                itemVm.ConcClsId = item.ConcClsId;
                itemVm.Density = item.Density;
                itemVm.MixTemp = item.MixTemp;
                itemVm.Remarks = item.Remarks;
                itemVm.RequestId = item.RequestId;
                itemVm.Slumb30Min = item.Slumb30Min;
                itemVm.Slumb60Min = item.Slumb60Min;
                itemVm.Slumb90Min = item.Slumb90Min;
                itemVm.SlumbInitial = item.SlumbInitial;
                itemVm.Standard = item.Standard;
                itemVm.TestSpecimen = item.TestSpecimen;
                itemVm.Yield = item.Yield;
            }

            var materials = MixDesignsRepository.GetMdMaterials(1, true);
            foreach (var mat in materials)
            {
                MdRequestConClsMaterialsViewModel cm = new MdRequestConClsMaterialsViewModel();
                cm.ConcClsInfoItemId = itemVm.ItemId;
                cm.MaterialId = mat.MaterialId;
                cm.MaterialName = mat.Name;
                if (item != null)
                {
                    var exist = item.MD_RequestConClsMaterials.FirstOrDefault(a => a.MaterialId == mat.MaterialId);
                    if (exist != null)
                    {
                        cm.ID = exist.ID;
                        cm.CorrectedWeight = exist.CorrectedWeight;
                        cm.UncorrectedWeight = exist.UncorrectedWeight;
                    }
                }

                itemVm.ListConClsMaterials.Add(cm);
            }

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MdRequestConcClsInfoViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult CreateRequestConcClsInfo(MdRequestConcClsInfoViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'MdRequestConcClsInfoViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            MD_RequestConClsInfo item = new MD_RequestConClsInfo();
            var requestInfo = MixDesignsRepository.GetRequestItem(itemVm.RequestId);

            if (itemVm.ItemId > 0)
            {
                item = MixDesignsRepository.GetConClsInfoItem(itemVm.RequestId, itemVm.ConcClsId);
            }

            item.AirContent = itemVm.AirContent;
            item.AmbTemp = itemVm.AmbTemp;
            item.CompressiveStrength28Days1 = itemVm.CompressiveStrength28Days1;
            item.CompressiveStrength28Days2 = itemVm.CompressiveStrength28Days2;
            item.CompressiveStrength28Days3 = itemVm.CompressiveStrength28Days3;
            item.CompressiveStrength28DaysAvg = Math.Round(((double)(item.CompressiveStrength28Days1 + item.CompressiveStrength28Days2 + item.CompressiveStrength28Days3)) / 3, 1);

            item.CompressiveStrength7Days1 = itemVm.CompressiveStrength7Days1;
            item.CompressiveStrength7Days2 = itemVm.CompressiveStrength7Days2;
            item.CompressiveStrength7Days3 = itemVm.CompressiveStrength7Days3;
            item.CompressiveStrength7DaysAvg = Math.Round(((double)(item.CompressiveStrength7Days1 + item.CompressiveStrength7Days2 + item.CompressiveStrength7Days3)) / 3, 1);

            item.ConcClsId = itemVm.ConcClsId;
            item.Density = itemVm.Density;
            item.ItemId = itemVm.ItemId;
            item.RequestId = itemVm.RequestId;
            item.MixTemp = itemVm.MixTemp;
            item.Slumb30Min = itemVm.Slumb30Min;
            item.Slumb60Min = itemVm.Slumb60Min;
            item.Slumb90Min = itemVm.Slumb90Min;
            item.SlumbInitial = itemVm.SlumbInitial;
            item.Standard = itemVm.Standard;
            item.TestSpecimen = itemVm.TestSpecimen;
            item.Yield = itemVm.Yield;
            item.Remarks = itemVm.Remarks;


            int conClsInfoId = MixDesignsRepository.SaveConClsInfo(item);


            bool isValidStrength = (item.CompressiveStrength28DaysAvg - item.ConcClsId >= 5);


            switch (item.ConcClsId)
            {
                case 15:
                    {
                        requestInfo.IsC15Complete = isValidStrength;
                        break;
                    }
                case 25:
                    {
                        requestInfo.IsC25Complete = isValidStrength;
                        break;
                    }
                case 30:
                    {
                        requestInfo.IsC30Complete = isValidStrength;
                        break;
                    }
                case 35:
                    {
                        requestInfo.IsC35Complete = isValidStrength;
                        break;
                    }
            }

            MixDesignsRepository.SaveConClsInfoMaterialsList(itemVm.ListConClsMaterials, conClsInfoId);

            MixDesignsRepository.SaveRequest(requestInfo);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("CreateRequestConcClsInfo", new { id = item.RequestId, cls = item.ConcClsId, isSuccess = true });

        }


        public ActionResult CreateRequestFiles(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;


            ViewBag.RequestItem = MixDesignsRepository.GetRequestItem(id);

            //List<MdRequestMaterialTestFilesViewModel> result = new List<MdRequestMaterialTestFilesViewModel>();
            MdRequestFilesViewModel vm = new MdRequestFilesViewModel();
            vm.RequestId = id;

            var mtAllFiles = MixDesignsRepository.GetMdMaterialTestFiles(null, isActive: true);
            ViewBag.CategoriesList = new CategoriesRepository(LangId, db).GetNestedCategories(1);

            var lstCurrentRequestFiles = MixDesignsRepository.GetRequestTestFiles(id);

            foreach (var file in mtAllFiles)
            {
                MdRequestMaterialTestFilesViewModel itemVm = new MdRequestMaterialTestFilesViewModel();
                itemVm.CategoryId = file.CategoryId;
                itemVm.FileId = file.FileId;
                itemVm.FileName = file.Name;
                itemVm.RequestId = id;

                var exist = lstCurrentRequestFiles.FirstOrDefault(a => a.FileId == file.FileId);
                if (exist != null)
                {
                    itemVm.ID = exist.ID;
                    itemVm.FileUrl = exist.FileUrl;
                    itemVm.Notes = exist.Notes;
                }

                vm.ListTestFiles.Add(itemVm);
            }

            return View(vm);
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MdRequestFilesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult CreateRequestFiles(MdRequestFilesViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'MdRequestFilesViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            //MD_RequestConClsInfo item = new MD_RequestConClsInfo();
            var requestInfo = MixDesignsRepository.GetRequestItem(itemVm.RequestId);

            foreach (var vm in itemVm.ListTestFiles)
            {
                MD_RequestMaterialTestFiles file = new MD_RequestMaterialTestFiles();
                if (vm.ID > 0)
                    file = MixDesignsRepository.GetRequestTestFileItem(vm.ID);

                file.FileId = vm.FileId;
                if (vm.UploadedFile != null)
                    file.FileUrl = HelperMethods.SaveFile("/Uploads/MixDesigns", vm.UploadedFile);

                file.Notes = vm.Notes;
                file.RequestId = itemVm.RequestId;

                MixDesignsRepository.SaveRequestTestFile(file);
            }

            requestInfo.IsFilesUploadComplete = true;
            MixDesignsRepository.SaveRequest(requestInfo);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("CreateRequestFiles", new { id = itemVm.RequestId, isSuccess = true });

        }



        //public ActionResult LabSamples(int? sampleId)
        //{

        //    var currentUser = GetCurrentUser();

        //    List<VisitsSample> result = new List<VisitsSample>();
        //    //var result = db.VisitsSample.Where(a=> a.SampleStatus==3).ToList();
        //    if (sampleId.HasValue)
        //    {
        //        var sample = VisitsRepository.GetSampleBySampleId(sampleId.Value, true);
        //        if (sample != null)
        //            result.Add(sample);
        //    }
        //    else
        //    {
        //        result = VisitsRepository.GetMySamples(currentUser.UserId, currentUser.RoleId, 3);
        //    }

        //    return View(result);
        //}



        //public ActionResult MySamples(bool? isSuccess)
        //{
        //    if (isSuccess == true)
        //        ViewBag.Success = true;

        //    return View();
        //}





    }
}