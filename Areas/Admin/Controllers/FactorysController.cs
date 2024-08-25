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
using Amana.ViewModels.Factories;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Amana.Areas.Admin.Controllers
{
#pragma warning disable CS0246 // The type or namespace name 'CustomAuthorizeAttribute' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IsBackend' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CustomAuthorize' could not be found (are you missing a using directive or an assembly reference?)
    [CustomAuthorize(IsBackend = true)]
#pragma warning restore CS0246 // The type or namespace name 'CustomAuthorize' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IsBackend' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'CustomAuthorizeAttribute' could not be found (are you missing a using directive or an assembly reference?)
    public class FactorysController : ControllerHelper
    {
#pragma warning disable CS0246 // The type or namespace name 'AmanaConcreteDBEntities1' could not be found (are you missing a using directive or an assembly reference?)
        readonly AmanaConcreteDBEntities1 _context = new AmanaConcreteDBEntities1();
#pragma warning restore CS0246 // The type or namespace name 'AmanaConcreteDBEntities1' could not be found (are you missing a using directive or an assembly reference?)

        public ActionResult Index(int? factoryType, int? locationId, int? close)
        {
            var locations = SettingsRepository.GetLocations();
            ViewData["LocationsLst"] = new SelectList(locations, "ID", "Name");

            return View(FactoriesRepository.GetAllFactories(locationId, factoryType));
        }

        public ActionResult ClosedFactorys()
        {

            return View(FactoriesRepository.GetAllFactories(locationId: null, isClosed: true));
        }
        public ActionResult IntensiveFactorys()
        {

            return View(FactoriesRepository.GetAllFactories(locationId: null, isIntensiveVisits: true));
        }
        public ActionResult FactoriesMap()
        {
            return View(FactoriesRepository.GetAllFactories());
        }

        public ActionResult DeleteFactory(int id, int? factoryType, int? locationId)
        {

            Factories item = FactoriesRepository.GetFactoryById(id, false);
            int fId = item.ID;
            FactoriesRepository.DeleteItem(item);

            return RedirectToAction("Index", new { factoryType, locationId });
        }
        public ActionResult AddFactory(int? id, bool? isSuccess)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            var locationsLst = SettingsRepository.GetLocations();


            FactoryViewModel itemVm = new FactoryViewModel();

            if (id.HasValue)
            {
                var item = FactoriesRepository.GetFactoryById(id.Value);
                itemVm.ID = item.ID;
                itemVm.CodeNo = item.CodeNo ?? 0;
                itemVm.DailyProductRating = item.DailyProductRating;
                itemVm.Description = item.Description;
                itemVm.FaxNo = item.FaxNo;

                itemVm.Location = item.Location;
                itemVm.LocationId = item.LocationId;
                itemVm.Name = item.Name;
                itemVm.OwnerName = item.OwnerName;
                itemVm.Photo = item.Photo;
                itemVm.ProductEnergy = item.ProductEnergy;
                itemVm.ResponsibleManager = item.ResponsibleManager;
                itemVm.ResponsibleManagerCertified = item.ResponsibleManagerCertified;
                itemVm.TechManager = item.TechManager;
                itemVm.TechManagerCertified = item.TechManagerCertified;
                itemVm.TelephoneNo = item.TelephoneNo;
                itemVm.FactoryType = item.FactoryType;

                FactoryLocationViewModel fLocationVm = new FactoryLocationViewModel();
                fLocationVm.ID = item.ID;
                fLocationVm.Lang = item.Lang;
                fLocationVm.Lat = item.Lat;
                ViewBag.FactoryLocationVm = fLocationVm;
                ViewBag.FactoryItem = item;
            }
            ViewData["LocationsList"] = new SelectList(locationsLst, "ID", "Name", itemVm.LocationId);

            return View(itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'FactoryViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddFactory(FactoryViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'FactoryViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            Factories item = new Factories();

            if (itemVm.ID > 0)
            {
                item = FactoriesRepository.GetFactoryById(itemVm.ID.Value, false);
            }

            item.CodeNo = itemVm.CodeNo;
            item.DailyProductRating = itemVm.DailyProductRating;
            item.Description = itemVm.Description;
            item.FaxNo = itemVm.FaxNo;
            item.Location = itemVm.Location;
            item.Name = itemVm.Name;
            item.OwnerName = itemVm.OwnerName;
            item.Photo = itemVm.Photo;
            item.ProductEnergy = itemVm.ProductEnergy;
            item.ResponsibleManager = itemVm.ResponsibleManager;
            item.ResponsibleManagerCertified = itemVm.ResponsibleManagerCertified;
            item.TechManager = itemVm.TechManager;
            item.TechManagerCertified = itemVm.TechManagerCertified;
            item.TelephoneNo = itemVm.TelephoneNo;
            item.LocationId = itemVm.LocationId;
            item.FactoryType = itemVm.FactoryType;

            FactoriesRepository.SaveFactoryItem(item);


            return RedirectToAction("AddFactory", new { id = item.ID, isSuccess = true });

        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'FactoryLocationViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult FactoryLocation(FactoryLocationViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'FactoryLocationViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            Factories item = new Factories();

            if (itemVm.ID > 0)
            {
                item = FactoriesRepository.GetFactoryById(itemVm.ID.Value, false);
            }

            item.Lat = itemVm.Lat;
            item.Lang = itemVm.Lang;


            FactoriesRepository.SaveFactoryItem(item);


            return RedirectToAction("AddFactory", new { id = item.ID, isSuccess = true, t = 2 });

        }

        #region FactoryEquipments

        public ActionResult AddEquipment(int? id, int factoryId)
        {
            //ViewBag.Category = SettingsRepository.GetCategoryById(propertyId);

            FactoryEquipmentViewModel itemVm = new FactoryEquipmentViewModel();

            itemVm.FactoryId = factoryId;

            if (id.HasValue)
            {
                var item = FactoriesRepository.GetFactoryEquipment(id.Value);
                itemVm.ID = item.ID;
                itemVm.EquipmentName = item.EquipmentName;
                itemVm.EquipNoWorking = item.EquipNoWorking.Value;
                itemVm.EquipSpare = item.EquipSpare.Value;
                itemVm.EquipWorking = item.EquipWorking.Value;
                itemVm.Notes = item.Notes;
                itemVm.TotalCount = item.TotalCount.Value;
            }

            //List<StngPropPeriodics> periodicProps = SettingsRepository.GetPropPeriodics();
            //ViewData["PeriodicProperties"] = new SelectList(periodicProps, "ID", "Peroidic", itemVm.PeriodicPropertyId);

            return View("_AddEquipment", itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'FactoryEquipmentViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddEquipment(FactoryEquipmentViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'FactoryEquipmentViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            FactoryEquipments item = new FactoryEquipments();

            if (itemVm.ID > 0)
            {
                item = FactoriesRepository.GetFactoryEquipment(itemVm.ID.Value);
            }

            item.EquipmentName = itemVm.EquipmentName;
            item.EquipNoWorking = itemVm.EquipNoWorking;
            item.EquipSpare = itemVm.EquipSpare;
            item.EquipWorking = itemVm.EquipWorking;
            item.Notes = itemVm.Notes;
            item.TotalCount = itemVm.TotalCount;
            item.FactoryId = itemVm.FactoryId;
            FactoriesRepository.SaveFactoryEquipment(item);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("AddFactory", new { id = item.FactoryId, isSuccess = true, t = 3 });

        }
        public ActionResult DeleteEquipment(int id)
        {

            FactoryEquipments item = FactoriesRepository.GetFactoryEquipment(id);
            int fId = item.FactoryId.Value;
            FactoriesRepository.DeleteFactoryEquipment(item);

            return RedirectToAction("AddFactory", new { id = fId, isSuccess = true, t = 3 });
        }

        #endregion

        #region FactoryTechnicalStuff

        public ActionResult AddTechnicalStuff(int? id, int factoryId)
        {
            //ViewBag.Category = SettingsRepository.GetCategoryById(propertyId);

            FactoryTechnicalStuffViewModel itemVm = new FactoryTechnicalStuffViewModel();

            itemVm.FactoryId = factoryId;

            if (id.HasValue)
            {
                var item = FactoriesRepository.GetFactoryTechnicalStuff(id.Value);
                itemVm.ID = item.ID;
                itemVm.JobDescriptionId = item.JobDescriptionId.Value;
                itemVm.Name = item.Name;
                itemVm.TechnicalCertified = item.TechnicalCertified;
            }

            List<IdxJobDescription> jobs = SettingsRepository.GetJobDescriptions();
            ViewData["JobDescriptions"] = new SelectList(jobs, "ID", "Name", itemVm.JobDescriptionId);

            return View("_AddTechnicalStuff", itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'FactoryTechnicalStuffViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddTechnicalStuff(FactoryTechnicalStuffViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'FactoryTechnicalStuffViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            FactoryTechnicalStuff item = new FactoryTechnicalStuff();

            if (itemVm.ID > 0)
            {
                item = FactoriesRepository.GetFactoryTechnicalStuff(itemVm.ID.Value);
            }

            item.JobDescriptionId = itemVm.JobDescriptionId;
            item.Name = itemVm.Name;
            item.TechnicalCertified = itemVm.TechnicalCertified;
            item.FactoryId = itemVm.FactoryId;
            FactoriesRepository.SaveFactoryTechnicalStuff(item);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("AddFactory", new { id = item.FactoryId, isSuccess = true, t = 4 });

        }
        public ActionResult DeleteTechnicalStuff(int id)
        {

            FactoryTechnicalStuff item = FactoriesRepository.GetFactoryTechnicalStuff(id);
            int fId = item.FactoryId.Value;
            FactoriesRepository.DeleteFactoryTechnicalStuff(item);

            return RedirectToAction("AddFactory", new { id = fId, isSuccess = true, t = 4 });
        }

        #endregion

        #region FactoryMaterialSupplier

        public ActionResult AddMaterialSupplier(int? id, int factoryId)
        {
            //ViewBag.Category = SettingsRepository.GetCategoryById(propertyId);

            FactoryMaterialSupplierViewModel itemVm = new FactoryMaterialSupplierViewModel();

            itemVm.FactoryId = factoryId;

            if (id.HasValue)
            {
                var item = FactoriesRepository.GetFactoryMaterialSupplier(id.Value);
                itemVm.ID = item.ID;
                itemVm.Location = item.Location;
                itemVm.Name = item.Name;
                itemVm.Fax = item.Fax;
                itemVm.MaterialTypeId = item.MaterialTypeId.Value;
                itemVm.Telephone = item.Telephone;
                itemVm.TypeofMaterial = item.TypeofMaterial;
            }

            List<IdxMaterialTypes> mTypes = SettingsRepository.GetMaterialTypes();
            ViewData["MaterialTypes"] = new SelectList(mTypes, "ID", "Name", itemVm.MaterialTypeId);

            return View("_AddMaterialSupplier", itemVm);
        }
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'FactoryMaterialSupplierViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult AddMaterialSupplier(FactoryMaterialSupplierViewModel itemVm)
#pragma warning restore CS0246 // The type or namespace name 'FactoryMaterialSupplierViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            FactoryMaterialSuppliers item = new FactoryMaterialSuppliers();

            if (itemVm.ID > 0)
            {
                item = FactoriesRepository.GetFactoryMaterialSupplier(itemVm.ID.Value);
            }

            item.FactoryId = itemVm.FactoryId;
            item.Name = itemVm.Name;
            item.Fax = itemVm.Fax;
            item.Location = itemVm.Location;
            item.MaterialTypeId = itemVm.MaterialTypeId;
            item.Telephone = itemVm.Telephone;
            item.TypeofMaterial = itemVm.TypeofMaterial;
            FactoriesRepository.SaveFactoryMaterialSupplier(item);

            //var ctgry = SettingsRepository.GetCategoryById(item.ConClsCategoryId);
            return RedirectToAction("AddFactory", new { id = item.FactoryId, isSuccess = true, t = 5 });

        }
        public ActionResult DeleteMaterialSupplier(int id)
        {

            FactoryMaterialSuppliers item = FactoriesRepository.GetFactoryMaterialSupplier(id);
            int fId = item.FactoryId.Value;
            FactoriesRepository.DeleteFactoryMaterialSuppliers(item);

            return RedirectToAction("AddFactory", new { id = fId, isSuccess = true, t = 5 });
        }

        #endregion

        public ActionResult ShowLocation(string latitude, string longitude)
        {
            return View("_ShowLocation");
        }


        [HttpPost]
        public ActionResult GetFactoriesByLocation(string locationId)
        {
            int locationID;
            List<SelectListItem> factoryNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(locationId))
            {
                locationID = Convert.ToInt32(locationId);
                List<Factories> factories = FactoriesRepository.GetAllFactories(locationID, factoryType: (int)FactoryTypesEnum.Concrete);
                factories.ForEach(x =>
                {
                    factoryNames.Add(new SelectListItem { Text = x.Name, Value = x.ID.ToString() });
                });
            }
            return Json(factoryNames, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult UserCredentials(int? id, bool? isSuccess, int typeId)
        //{
        //    if (isSuccess == true)
        //        ViewBag.Success = true;

        //    PageTitle(typeId);
        //    TempData["EventId"] = id;

        //    if (id == null)
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        P_Events entity = eventsDi.SingleItem(id ?? 0);
        //        List<C_UserItems> usersLst = new List<C_UserItems>();
        //        if (entity.C_UserItems1 != null)
        //        {
        //            usersLst.Add(entity.C_UserItems1);
        //        }

        //        return View(usersLst);
        //    }
        //}

        public ActionResult OpenCloseFactory(int id, bool? redirectToIndex)
        {
            Factories item = FactoriesRepository.GetFactoryById(id, false);
            item.IsTemporaryClosed = !item.IsTemporaryClosed;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;


            if (item.IsTemporaryClosed == false)//اعادة فتح المصنع
            {
                var warning = db.FactoryWarnings.Where(a => a.FactoryId == item.ID && a.IsApproved == true && a.DeserveCloseFactory == true && a.DidClosed == true && !a.CloseFactoryEndDate.HasValue).OrderByDescending(a => a.CreatedWhen).FirstOrDefault();
                if (warning != null)
                {
                    warning.CloseFactoryEndDate = HelperMethods.GetCurrentDateTime();
                    db.Entry(warning).State = System.Data.Entity.EntityState.Modified;
                }
            }

            db.SaveChanges();
            if (redirectToIndex == true)
                return RedirectToAction("Index", new { close = 1 });
            else
                return RedirectToAction("ClosedFactorys");
        }
        public ActionResult ToggleIntensiveVisits(int id, bool? redirectToIndex)
        {

            Factories item = FactoriesRepository.GetFactoryById(id, false);
            item.IsIntensiveVisits = !item.IsIntensiveVisits;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;

            if (item.IsIntensiveVisits == false)//نقل للمراقبة العادية
            {
                var warning = db.FactoryWarnings.Where(a => a.FactoryId == item.ID && a.IsApproved == true && a.DeserveIntensiveVisits == true && a.DidIntensiveVisits == true && !a.IntensiveVisitsEndDate.HasValue).OrderByDescending(a => a.CreatedWhen).FirstOrDefault();
                if (warning != null)
                {
                    warning.IntensiveVisitsEndDate = HelperMethods.GetCurrentDateTime();
                    db.Entry(warning).State = System.Data.Entity.EntityState.Modified;
                }
            }

            db.SaveChanges();
            if (redirectToIndex == true)
                return RedirectToAction("Index", new { close = 1 });
            else
                return RedirectToAction("IntensiveFactorys");
        }
    }
}