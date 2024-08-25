#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Enums;
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
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;

namespace Amana
{
    public partial class MonthlyReportPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                rvSiteMapping.ProcessingMode = ProcessingMode.Local;
                rvSiteMapping.LocalReport.ReportPath = Server.MapPath("~/Reports/MonthlyVisitsReport.rdlc");
                rvSiteMapping.LocalReport.DataSources.Clear();
                rvSiteMapping.LocalReport.EnableExternalImages = true;
                rvSiteMapping.ShowParameterPrompts = true;

                int month = int.Parse(Request["m"]);
                int year = int.Parse(Request["y"]);
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = HelperMethods.GetLastDayInMonth(startDate);


                List<ReportGeneralInfoViewModel> repInfoLst = new List<ReportGeneralInfoViewModel>();
                repInfoLst.Add(new ReportGeneralInfoViewModel { ReportDate = DateTime.Now.ToString("dddd dd / MM / yyyy", CultureInfo.CreateSpecificCulture("ar-EG")), ReportStartDate = startDate.ToString("dd / MM / yyyy"), ReportEndDate = endDate.ToString("dd / MM / yyyy") });
                ReportDataSource rdc = new ReportDataSource("RepInfo", repInfoLst);
                rvSiteMapping.LocalReport.DataSources.Add(rdc);

                ReportDataSource rdc2 = new ReportDataSource("MonthlyVisits", GetVisitsList(startDate, endDate));
                rvSiteMapping.LocalReport.DataSources.Add(rdc2);

                rvSiteMapping.DataBind();
                rvSiteMapping.LocalReport.Refresh();

            }

        }
#pragma warning disable CS0246 // The type or namespace name 'MonthlyVisitsReportViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public List<MonthlyVisitsReportViewModel> GetVisitsList(DateTime startDate, DateTime endDate)
#pragma warning restore CS0246 // The type or namespace name 'MonthlyVisitsReportViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Factories> fList = FactoriesRepository.GetAllFactories(null, factoryType: (int)FactoryTypesEnum.Concrete);

            List<MonthlyVisitsReportViewModel> visitsLst = new List<MonthlyVisitsReportViewModel>();
            foreach (var factoryItem in fList)
            {
                List<Visits> result = VisitsRepository.SearchVisits(null, startDate, endDate, null, factoryId: factoryItem.ID, isDone: true);
                MonthlyVisitsReportViewModel item = new MonthlyVisitsReportViewModel();
                item.FactoryId = factoryItem.ID;
                item.FactoryName = factoryItem.Name + (factoryItem.IdxLocations == null ? "" : " - " + factoryItem.IdxLocations.Name);
                if (result.Count > 0)
                    item.VisitDate1 = result[0].VisitDate.ToString("dd-MM-yyyy");
                if (result.Count > 1)
                    item.VisitDate2 = result[1].VisitDate.ToString("dd-MM-yyyy");
                if (result.Count > 2)
                    item.VisitDate3 = result[2].VisitDate.ToString("dd-MM-yyyy");

                visitsLst.Add(item);
            }
            return visitsLst;
        }

    }
}