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
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.UI;

namespace Amana
{
    public partial class MonthlyWarningsPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                rvSiteMapping.ProcessingMode = ProcessingMode.Local;
                rvSiteMapping.LocalReport.ReportPath = Server.MapPath("~/Reports/MonthlyWarnings.rdlc");
                rvSiteMapping.LocalReport.DataSources.Clear();
                rvSiteMapping.LocalReport.EnableExternalImages = true;
                rvSiteMapping.ShowParameterPrompts = true;

                int month = int.Parse(Request["m"]);
                int year = int.Parse(Request["y"]);
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = HelperMethods.GetLastDayInMonth(startDate);


                List<ReportGeneralInfoViewModel> repInfoLst = new List<ReportGeneralInfoViewModel>();
                repInfoLst.Add(new ReportGeneralInfoViewModel { ReportDate = startDate.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("ar-EG")), ReportStartDate = startDate.ToString("dd / MM / yyyy"), ReportEndDate = endDate.ToString("dd / MM / yyyy") });
                ReportDataSource rdc = new ReportDataSource("RepInfo", repInfoLst);
                rvSiteMapping.LocalReport.DataSources.Add(rdc);

                ReportDataSource rdc2 = new ReportDataSource("MonthlyWarnings", GetWarningsList(startDate, endDate));
                rvSiteMapping.LocalReport.DataSources.Add(rdc2);

                rvSiteMapping.DataBind();
                rvSiteMapping.LocalReport.Refresh();

            }

        }
#pragma warning disable CS0246 // The type or namespace name 'WarningReportViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public List<WarningReportViewModel> GetWarningsList(DateTime startDate, DateTime endDate)
#pragma warning restore CS0246 // The type or namespace name 'WarningReportViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {

            var allWarnings = WarningsRepository.GetAllWarnings(isGeneralManagerApproved: true, startDate: startDate, endDate: endDate);

            List<Factories> fList = FactoriesRepository.GetAllFactories(null, factoryType: (int)FactoryTypesEnum.Concrete);

            List<WarningReportViewModel> wLst = new List<WarningReportViewModel>();
            foreach (var factoryItem in fList)
            {
                var factoryWarnings = allWarnings.Where(a => a.FactoryId == factoryItem.ID);
                if (factoryWarnings.Count() > 0)
                {
                    CommaDelimitedStringCollection violationsText = new CommaDelimitedStringCollection();
                    WarningReportViewModel item = new WarningReportViewModel();
                    item.FactoryLocation = (factoryItem.IdxLocations == null ? "" : factoryItem.IdxLocations.Name);
                    item.FactoryName = factoryItem.Name;
                    int v_lowPresuureCount = 0;
                    int v_BaseColumn35Count = 0;
                    int v_IsCleanLocationCount = 0;
                    int v_IsHighTempCount = 0;
                    int v_IsSlumbCount = 0;
                    int v_TechnicalStuffCount = 0;

                    foreach (var warningItem in factoryWarnings)
                    {
                        v_lowPresuureCount += warningItem.FactoryWarningSamples.Count(w => w.VisitsSample.V_IsLowPressure == true);
                        v_BaseColumn35Count += warningItem.FactoryWarningSamples.Count(w => w.VisitsSample.V_BaseColumn35 == true);
                        v_IsCleanLocationCount += warningItem.FactoryWarningSamples.Count(w => w.VisitsSample.V_IsCleanLocation == true);
                        v_IsHighTempCount += warningItem.FactoryWarningSamples.Count(w => w.VisitsSample.V_IsHighTemp == true);
                        v_IsSlumbCount += warningItem.FactoryWarningSamples.Count(w => w.VisitsSample.V_IsSlumb == true);
                        v_TechnicalStuffCount += warningItem.FactoryWarningSamples.Count(w => w.VisitsSample.V_TechnicalStuff == true);
                    }

                    if (v_IsHighTempCount > 0)
                        violationsText.Add(v_IsHighTempCount.ToString() + " حرارة");
                    if (v_IsSlumbCount > 0)
                        violationsText.Add(v_IsSlumbCount.ToString() + " هبوط");
                    if (v_lowPresuureCount > 0)
                        violationsText.Add(v_lowPresuureCount.ToString() + " مقاومة");
                    if (v_BaseColumn35Count > 0)
                        violationsText.Add(v_BaseColumn35Count.ToString() + " إستخدام");
                    if (v_IsCleanLocationCount > 0)
                        violationsText.Add(v_IsCleanLocationCount.ToString() + " نظافة");
                    if (v_TechnicalStuffCount > 0)
                        violationsText.Add(v_TechnicalStuffCount.ToString() + " الجهاز الفني");

                    item.ViolationsText = violationsText.Count > 0 ? violationsText.ToString().Replace(",", " + ") : "";
                    wLst.Add(item);
                }
            }
            return wLst;
        }

    }
}