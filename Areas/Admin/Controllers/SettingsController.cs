using Amana.ControllerHelpers;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Filters;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Repository;
#pragma warning restore CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels.MixDesigns;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels.Settings;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System;
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
    public class SettingsController : ControllerHelper
    {
        // GET: Admin/Category
        public ActionResult Index(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            return View(SettingsRepository.GetCategories(id));
        }


        public ActionResult AddProperty(int? id, int categoryId)
        {
            ViewBag.Category = SettingsRepository.GetCategoryById(categoryId);

            StngPropertyViewModel itemVm = new StngPropertyViewModel();
            itemVm.ConClsCategoryId = categoryId;

            if (id.HasValue)
            {
                var item = SettingsRepository.GetProperty(id.Value);
                itemVm.ID = item.ID;
                itemVm.PropertyText = item.PropertyText;
                itemVm.PropertyWeight = item.PropertyWeight;
            }

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'StngPropertyViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddProperty(StngPropertyViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'StngPropertyViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            StngProperties item = new StngProperties();

            if (itemVm.ID > 0)
            {
                item = SettingsRepository.GetProperty(itemVm.ID.Value);
            }

            item.PropertyWeight = itemVm.PropertyWeight;
            item.PropertyText = itemVm.PropertyText;
            item.ConClsCategoryId = itemVm.ConClsCategoryId;
            SettingsRepository.SaveProperty(item);

            var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("Index", new { id = ctgry.SuperCategory, isSuccess = true });

        }

        public ActionResult AddLabProperty(int? id, int categoryId)
        {
            ViewBag.Category = SettingsRepository.GetCategoryById(categoryId);

            StngLabPropertyViewModel itemVm = new StngLabPropertyViewModel();
            itemVm.ConClsCategoryId = categoryId;

            if (id.HasValue)
            {
                var item = SettingsRepository.GetLabProperty(id.Value);
                itemVm.ID = item.ID;
                itemVm.PropertyText = item.PropertyText;
                itemVm.RequiredChoice = item.RequiredChoice;

            }

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'StngLabPropertyViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddLabProperty(StngLabPropertyViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'StngLabPropertyViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            StngLabProperties item = new StngLabProperties();

            if (itemVm.ID > 0)
            {
                item = SettingsRepository.GetLabProperty(itemVm.ID.Value);
            }

            item.PropertyText = itemVm.PropertyText;
            item.RequiredChoice = itemVm.RequiredChoice;
            item.ConClsCategoryId = itemVm.ConClsCategoryId;
            SettingsRepository.SaveLabProperty(item);

            var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("Index", new { id = ctgry.SuperCategory, isSuccess = true });

        }



        public ActionResult DeleteProperty(int id)
        {

            StngProperties prop = SettingsRepository.GetProperty(id);
            var ctgry = SettingsRepository.GetCategoryById(prop.ConClsCategoryId);

            SettingsRepository.DeleteProperty(prop);


            return RedirectToAction("Index", new { id = ctgry.SuperCategory, isSuccess = true });

        }
        public ActionResult DeleteLabProperty(int id)
        {

            StngLabProperties prop = SettingsRepository.GetLabProperty(id);
            var ctgry = SettingsRepository.GetCategoryById(prop.ConClsCategoryId);

            SettingsRepository.DeleteLabProperty(prop);


            return RedirectToAction("Index", new { id = ctgry.SuperCategory, isSuccess = true });

        }

        public ActionResult PeriodicProperties(int id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            return View(SettingsRepository.GetLabProperty(id, true));

        }
        public ActionResult DeletePeriodicProperty(int id)
        {

            StngPeriodicLabProperties prop = SettingsRepository.GetPeriodicProperty(id);

            SettingsRepository.DeletePeriodicProperty(prop);


            return RedirectToAction("PeriodicProperties", new { id = prop.ConClsLabPropertyId, isSuccess = true });

        }

        public ActionResult AddPeriodicProperty(int? id, int propertyId)
        {
            //ViewBag.Category = SettingsRepository.GetCategoryById(propertyId);

            StngPeriodicPropertyViewModel itemVm = new StngPeriodicPropertyViewModel();

            itemVm.LabPropertyId = propertyId;

            if (id.HasValue)
            {
                var item = SettingsRepository.GetPeriodicProperty(id.Value);
                itemVm.ID = item.ID;
                itemVm.LabPropertyId = item.ConClsLabPropertyId;
                itemVm.PropertyWeight = item.PropertyWeight;
                itemVm.PeriodicPropertyId = item.ConClsPropPeriodicId;
            }


            List<StngPropPeriodics> periodicProps = SettingsRepository.GetPropPeriodics();
            ViewData["PeriodicProperties"] = new SelectList(periodicProps, "ID", "Peroidic", itemVm.PeriodicPropertyId);

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'StngPeriodicPropertyViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddPeriodicProperty(StngPeriodicPropertyViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'StngPeriodicPropertyViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            StngPeriodicLabProperties item = new StngPeriodicLabProperties();

            if (itemVm.ID > 0)
            {
                item = SettingsRepository.GetPeriodicProperty(itemVm.ID.Value);
            }

            item.PropertyWeight = itemVm.PropertyWeight;
            item.ConClsLabPropertyId = itemVm.LabPropertyId;
            item.ConClsPropPeriodicId = itemVm.PeriodicPropertyId;
            SettingsRepository.SavePeriodicProperty(item);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("PeriodicProperties", new { id = item.ConClsLabPropertyId, isSuccess = true });

        }

        #region TimePeriods

        public ActionResult TimePeriods(bool? isSuccess, int c)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            bool isClassificationPeriod = (c == 1);
            return View(SettingsRepository.GetTimePeriods(isClassificationPeriod));

        }
        public ActionResult DeleteTimePeriod(int id)
        {

            StngTimePeriods prop = SettingsRepository.GetTimePeriodItem(id);
            int c = prop.IsClassificationPeriod ? 1 : 0;

            SettingsRepository.DeleteTimePeriod(prop);


            return RedirectToAction("TimePeriods", new { isSuccess = true, c = c });

        }

        public ActionResult AddTimePeriod(int? id, int c)
        {
            //ViewBag.Category = SettingsRepository.GetCategoryById(propertyId);

            StngTimePeriodViewModel itemVm = new StngTimePeriodViewModel();
            itemVm.IsClassificationPeriod = (c == 1);
            if (id.HasValue)
            {
                var item = SettingsRepository.GetTimePeriodItem(id.Value);
                itemVm.TimePeriodId = item.TimePeriodId;
                itemVm.StartDate = item.StartDate;
                itemVm.EndDate = item.EndDate;
                itemVm.Title = item.Title;
                itemVm.IsClassificationPeriod = item.IsClassificationPeriod;
                itemVm.IsCurrentPeriod = item.IsCurrentPeriod;

            }


            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'StngTimePeriodViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddTimePeriod(StngTimePeriodViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'StngTimePeriodViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            StngTimePeriods item = new StngTimePeriods();

            if (itemVm.TimePeriodId > 0)
            {
                item = SettingsRepository.GetTimePeriodItem(itemVm.TimePeriodId.Value);
            }

            item.Title = itemVm.Title;
            item.StartDate = itemVm.StartDate;
            item.EndDate = itemVm.EndDate;
            item.IsClassificationPeriod = itemVm.IsClassificationPeriod;
            item.IsCurrentPeriod = itemVm.IsCurrentPeriod;
            SettingsRepository.SaveTimePeriod(item);

            if (itemVm.IsCurrentPeriod == true)
            {
                SettingsRepository.MakeOtherNotCurrent(item.TimePeriodId, item.IsClassificationPeriod);
            }

            int c = item.IsClassificationPeriod ? 1 : 0;

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("TimePeriods", new { isSuccess = true, c = c });

        }
        #endregion



        #region ConcreteClasses

        public ActionResult ConcreteClasses(bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            return View(SettingsRepository.GetConcreteClasses());

        }
        public ActionResult DeleteConcreteClass(int id)
        {

            IdxConcreteClass prop = SettingsRepository.GetConcreteClass(id);

            SettingsRepository.DeleteConcreteClass(prop);


            return RedirectToAction("ConcreteClasses", new { isSuccess = true });

        }

        public ActionResult AddConcreteClass(int? id)
        {
            //ViewBag.Category = SettingsRepository.GetCategoryById(propertyId);

            //StngTimePeriodViewModel itemVm = new StngTimePeriodViewModel();
            IdxConcreteClass item = new IdxConcreteClass();
            if (id.HasValue)
            {
                item = SettingsRepository.GetConcreteClass(id.Value);

            }
            return View(item);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'IdxConcreteClass' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddConcreteClass(IdxConcreteClass itemVm)
#pragma warning restore CS0246 // The type or namespace name 'IdxConcreteClass' could not be found (are you missing a using directive or an assembly reference?)
        {


            SettingsRepository.SaveConcreteClass(itemVm);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("ConcreteClasses", new { isSuccess = true });

        }
        #endregion





        #region ConcreteSources

        public ActionResult ConcreteSources(bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            return View(SettingsRepository.GetConcreteSources());

        }
        public ActionResult DeleteConcreteSource(int id)
        {

            IdxConcreteSources prop = SettingsRepository.GetConcreteSourceItem(id);

            SettingsRepository.DeleteConcreteSource(prop);


            return RedirectToAction("ConcreteSources", new { isSuccess = true });

        }

        public ActionResult AddConcreteSource(int? id)
        {
            //ViewBag.Category = SettingsRepository.GetCategoryById(propertyId);

            IdxConcreteSourceViewModel itemVm = new IdxConcreteSourceViewModel();

            if (id.HasValue)
            {
                var item = SettingsRepository.GetConcreteSourceItem(id.Value);
                itemVm.SourceId = item.SourceId;
                itemVm.Name = item.Name;
            }

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'IdxConcreteSourceViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddConcreteSource(IdxConcreteSourceViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'IdxConcreteSourceViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            IdxConcreteSources item = new IdxConcreteSources();

            if (itemVm.SourceId > 0)
            {
                item = SettingsRepository.GetConcreteSourceItem(itemVm.SourceId.Value);
            }

            item.Name = itemVm.Name;
            SettingsRepository.SaveConcreteSource(item);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("ConcreteSources", new { isSuccess = true });

        }
        #endregion


        #region Locations

        public ActionResult Locations(bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            return View(SettingsRepository.GetLocations());

        }
        public ActionResult DeleteLocation(int id)
        {

            IdxLocations prop = SettingsRepository.GetLocationItem(id);

            SettingsRepository.DeleteLocation(prop);


            return RedirectToAction("Locations", new { isSuccess = true });

        }

        public ActionResult AddLocation(int? id)
        {
            //ViewBag.Category = SettingsRepository.GetCategoryById(propertyId);

            IdxConcreteSourceViewModel itemVm = new IdxConcreteSourceViewModel();

            if (id.HasValue)
            {
                var item = SettingsRepository.GetLocationItem(id.Value);
                itemVm.SourceId = item.ID;
                itemVm.Name = item.Name;
            }

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'IdxConcreteSourceViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddLocation(IdxConcreteSourceViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'IdxConcreteSourceViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            IdxLocations item = new IdxLocations();

            if (itemVm.SourceId > 0)
            {
                item = SettingsRepository.GetLocationItem(itemVm.SourceId.Value);
            }

            item.Name = itemVm.Name;
            SettingsRepository.SaveLocation(item);

            return RedirectToAction("Locations", new { isSuccess = true });

        }
        #endregion


        #region AdditionsTypes

        public ActionResult AdditionsTypes(bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            return View(SettingsRepository.GetAdditionTypes());

        }
        public ActionResult DeleteAdditionsType(int id)
        {

            IdxAdditionTypes prop = SettingsRepository.GetAdditionTypeItem(id);

            SettingsRepository.DeleteAdditionType(prop);


            return RedirectToAction("AdditionsTypes", new { isSuccess = true });

        }

        public ActionResult AddAdditionsType(int? id)
        {

            IdxConcreteSourceViewModel itemVm = new IdxConcreteSourceViewModel();

            if (id.HasValue)
            {
                var item = SettingsRepository.GetAdditionTypeItem(id.Value);
                itemVm.SourceId = item.ID;
                itemVm.Name = item.Name;
            }

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'IdxConcreteSourceViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddAdditionsType(IdxConcreteSourceViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'IdxConcreteSourceViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            IdxAdditionTypes item = new IdxAdditionTypes();

            if (itemVm.SourceId > 0)
            {
                item = SettingsRepository.GetAdditionTypeItem(itemVm.SourceId.Value);
            }

            item.Name = itemVm.Name;
            SettingsRepository.SaveAdditionType(item);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("AdditionsTypes", new { isSuccess = true });

        }
        #endregion


        #region MdMaterials

        public ActionResult MdMaterials(int typeId, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            return View(MixDesignsRepository.GetMdMaterials(typeId));

        }
        public ActionResult DeleteMdMaterials(int id)
        {

            MD_IdxMaterials prop = MixDesignsRepository.GetMdMaterialItem(id);
            int typeId = prop.TypeId;
            MixDesignsRepository.DeleteMdMaterial(prop);


            return RedirectToAction("MdMaterials", new { typeId, isSuccess = true });

        }

        public ActionResult AddMdMaterials(int? id, int typeId)
        {
            //ViewBag.Category = SettingsRepository.GetCategoryById(propertyId);

            MD_IdxMaterials itemVm = new MD_IdxMaterials();
            itemVm.IsActive = true;
            itemVm.TypeId = typeId;
            if (id.HasValue)
            {
                var item = MixDesignsRepository.GetMdMaterialItem(id.Value);
                itemVm.MaterialId = item.MaterialId;
                itemVm.TypeId = item.TypeId;
                itemVm.Name = item.Name;
                itemVm.IsActive = item.IsActive;
                itemVm.NumOrder = item.NumOrder;
            }

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MD_IdxMaterials' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddMdMaterials(MD_IdxMaterials itemVm)
#pragma warning restore CS0246 // The type or namespace name 'MD_IdxMaterials' could not be found (are you missing a using directive or an assembly reference?)
        {
            MD_IdxMaterials item = new MD_IdxMaterials();

            if (itemVm.MaterialId > 0)
            {
                item = MixDesignsRepository.GetMdMaterialItem(itemVm.MaterialId);
            }

            item.Name = itemVm.Name;
            item.IsActive = itemVm.IsActive;
            item.TypeId = itemVm.TypeId;
            MixDesignsRepository.SaveMdMaterials(item);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("MdMaterials", new { isSuccess = true, typeId = item.TypeId });

        }
        #endregion

        #region MdMaterialTestFiles

        public ActionResult MdMaterialTestFiles(bool? isSuccess, int? catId)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var cats = new CategoriesRepository(LangId, db).GetNestedCategories(1);
            ViewData["CatId"] = new SelectList(cats, "CatId", "Title", catId);


            return View(MixDesignsRepository.GetMdMaterialTestFiles(catId));

        }
        public ActionResult DeleteMdMaterialTestFiles(int id)
        {

            MD_IdxMaterialTestFiles prop = MixDesignsRepository.GetMdMaterialTestFileItem(id);

            MixDesignsRepository.DeleteMdMaterialTestFile(prop);


            return RedirectToAction("MdMaterialTestFiles", new { isSuccess = true });

        }

        public ActionResult AddMdMaterialTestFiles(int? id)
        {
            //ViewBag.Category = SettingsRepository.GetCategoryById(propertyId);
            var cats = new CategoriesRepository(LangId, db).GetNestedCategories(1);

            MD_IdxMaterialTestFileViewModel itemVm = new MD_IdxMaterialTestFileViewModel();
            itemVm.IsActive = true;

            if (id.HasValue)
            {
                var item = MixDesignsRepository.GetMdMaterialTestFileItem(id.Value);
                itemVm.FileId = item.FileId;
                itemVm.CategoryId = item.CategoryId;
                itemVm.Name = item.Name;
                itemVm.IsActive = item.IsActive;
                itemVm.NumOrder = item.NumOrder;
            }

            ViewData["Categories"] = new SelectList(cats, "CatId", "Title", itemVm.CategoryId);

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MD_IdxMaterialTestFileViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddMdMaterialTestFiles(MD_IdxMaterialTestFileViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'MD_IdxMaterialTestFileViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            MD_IdxMaterialTestFiles item = new MD_IdxMaterialTestFiles();

            if (itemVm.FileId > 0)
            {
                item = MixDesignsRepository.GetMdMaterialTestFileItem(itemVm.FileId);
            }
            else
            {
                item.CreatedBy = GetUserId();
                item.CreatedWhen = DateTime.Now;
            }

            item.Name = itemVm.Name;
            item.CategoryId = itemVm.CategoryId;
            item.IsActive = itemVm.IsActive;
            MixDesignsRepository.SaveMdMaterialTestFile(item);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("MdMaterialTestFiles", new { isSuccess = true });

        }
        #endregion
    }
}