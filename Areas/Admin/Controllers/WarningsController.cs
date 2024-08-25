using Amana.ControllerHelpers;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
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
    public class WarningsController : ControllerHelper
    {
#pragma warning disable CS0246 // The type or namespace name 'AmanaConcreteDBEntities1' could not be found (are you missing a using directive or an assembly reference?)
        readonly AmanaConcreteDBEntities1 _context = new AmanaConcreteDBEntities1();
#pragma warning restore CS0246 // The type or namespace name 'AmanaConcreteDBEntities1' could not be found (are you missing a using directive or an assembly reference?)

        public ActionResult Index(int? month, int? year)
        {
            DateTime? stDate = null;
            DateTime? eDate = null;
            if (month.HasValue && year.HasValue)
            {
                stDate = new DateTime(year.Value, month.Value, 1);
                eDate = HelperMethods.GetLastDayInMonth(stDate.Value);
            }

            return View(WarningsRepository.GetAllWarnings(isGeneralManagerApproved: true, startDate: stDate, endDate: eDate));
        }
        public ActionResult NewWarnings()
        {
            int userId = GetUserId();

            if (userId == AppSettings.ProjectManagerId)
            {
                return View(WarningsRepository.GetAllWarnings(isProjectManagerApproved: false));
            }
            else if (userId == AppSettings.ProjectSupervisorId)
            {
                return View(WarningsRepository.GetAllWarnings(isProjectManagerApproved: true, isProjectSupervisorApproved: false));
            }
            else if (userId == AppSettings.GeneralManagerId)
            {
                return View(WarningsRepository.GetAllWarnings(isProjectManagerApproved: true, isProjectSupervisorApproved: true, isGeneralManagerApproved: false));
            }
            else
                return View(WarningsRepository.GetAllWarnings(isGeneralManagerApproved: false));
        }

        public ActionResult ApproveWarning(int id)
        {

            var item = WarningsRepository.GetWarningById(id);
            int userId = GetUserId();

            if (userId == AppSettings.ProjectManagerId)
            {
                item.IsProjectManagerApproved = true;
            }
            else if (userId == AppSettings.ProjectSupervisorId)
            {
                item.IsProjectSupervisorApproved = true;
            }
            else if (userId == AppSettings.GeneralManagerId)
            {
                item.IsApproved = true;
                item.IsGeneralManagerApproved = true;

                if (item.DeserveCloseFactory == true || item.DeserveIntensiveVisits == true)
                {
                    Factories factoryItem = FactoriesRepository.GetFactoryById(item.FactoryId, false);
                    if (item.DeserveCloseFactory)
                    {
                        factoryItem.IsTemporaryClosed = true;
                        FactoryManualClose close = new FactoryManualClose { FactoryId = factoryItem.ID, CloseDate = HelperMethods.GetCurrentDateTime(), CreatedBy = GetUserId(), WarningId = id };

                        FactoriesRepository.SaveFactoryCloseItem(close);

                        item.CloseFactoryStartDate = HelperMethods.GetCurrentDateTime();
                        item.DidClosed = true;
                    }
                    if (item.DeserveIntensiveVisits)
                    {
                        factoryItem.IsIntensiveVisits = true;

                        item.IntensiveVisitsStartDate = HelperMethods.GetCurrentDateTime();
                        item.DidIntensiveVisits = true;
                    }
                    FactoriesRepository.SaveFactoryItem(factoryItem);
                }
            }


            WarningsRepository.SaveWarningItem(item);

            return RedirectToAction("NewWarnings");
        }


        public ActionResult WarningDetails(int id)
        {
            return View();
        }
        public ActionResult MyWarnings()
        {
            Factories factory = FactoriesRepository.GetFactoryByUserId(GetUserId());
            List<FactoryWarnings> result = new List<FactoryWarnings>();
            if (factory != null)
                result = WarningsRepository.GetAllWarnings(isGeneralManagerApproved: true, factoryId: factory.ID);

            return View(result);
        }



    }
}