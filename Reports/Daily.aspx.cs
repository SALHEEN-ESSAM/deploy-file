#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Repository;
#pragma warning restore CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels.Reports;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels.VisitsAndSamples;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amana
{
    public partial class DailyReportPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                rvSiteMapping.ProcessingMode = ProcessingMode.Local;
                rvSiteMapping.LocalReport.ReportPath = Server.MapPath("~/Reports/DailyVisitsSummary.rdlc");
                rvSiteMapping.LocalReport.DataSources.Clear();
                rvSiteMapping.LocalReport.EnableExternalImages = true;
                rvSiteMapping.ShowParameterPrompts = true;

                ReportDataSource rdc = new ReportDataSource("SampleModel", GetSamplesList());
                rvSiteMapping.LocalReport.DataSources.Add(rdc);

                ReportGeneralInfoViewModel repInfoItem = new ReportGeneralInfoViewModel();
                UsersRepository userRepository = new UsersRepository(1, new AmanaConcreteDBEntities1());
                DateTime? date = string.IsNullOrEmpty(Request["startDate"]) ? DateTime.Now : DateTime.ParseExact(Request["startDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //مدير المشروع
                C_UserItems pmData = userRepository.SingleItem(AppSettings.ProjectManagerId);
                if (pmData != null)
                {
                    repInfoItem.ProjectManager = pmData.Name;
                    repInfoItem.ProjectManagerSignPhotoUrl = pmData.ImageUrl;
                }
                //مشرف المشروع
                C_UserItems supervisorData = userRepository.SingleItem(AppSettings.ProjectSupervisorId);
                if (supervisorData != null)
                {
                    repInfoItem.ProjectSupervisor = supervisorData.Name;
                    repInfoItem.ProjectSupervisorSignPhotoUrl = supervisorData.ImageUrl;
                }
                repInfoItem.ReportDate = date.Value.ToString("dd / MM / yyyy", CultureInfo.CreateSpecificCulture("ar-EG"));
                repInfoItem.ReportFullDate = date.Value.ToString("dddd dd / MM / yyyy", CultureInfo.CreateSpecificCulture("ar-EG"));

                repInfoItem.ContractNo = SettingsRepository.GetContractNumber(DateTime.Now).ToString();

                List<ReportGeneralInfoViewModel> repInfoLst = new List<ReportGeneralInfoViewModel>();
                repInfoLst.Add(repInfoItem);
                ReportDataSource rdc2 = new ReportDataSource("RepInfo", repInfoLst);
                rvSiteMapping.LocalReport.DataSources.Add(rdc2);


                rvSiteMapping.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;

                //get absolute path to Project folder
                string path = new Uri(Server.MapPath("~/Uploads")).AbsoluteUri; // adjust path to Project folder here

                // set above path to report parameter
                var parameter = new ReportParameter[1];
                parameter[0] = new ReportParameter("ImagePath", path); // adjust parameter name here
                rvSiteMapping.LocalReport.SetParameters(parameter);
                /* end of added part */

                rvSiteMapping.DataBind();
                rvSiteMapping.LocalReport.Refresh();

            }

        }
#pragma warning disable CS0246 // The type or namespace name 'SampleViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public List<SampleViewModel> GetSamplesList()
#pragma warning restore CS0246 // The type or namespace name 'SampleViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            DateTime? date = string.IsNullOrEmpty(Request["startDate"]) ? DateTime.Now : DateTime.ParseExact(Request["startDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            List<Visits> result = VisitsRepository.SearchVisits(null, date, date, null, null, true);

            List<SampleViewModel> samplesLst = new List<SampleViewModel>();
            foreach (var visitItem in result)
            {

                samplesLst.Add(VisitsRepository.MakeConcreteSampleViewModel(visitItem, forReport: true));
            }
            return samplesLst.OrderBy(a => a.SampleId).ToList();
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            //var ID = Convert.ToInt32(e.Parameters[0].Values[0]);

            if (e.ReportPath == "VisitSampleReport")
            {
                var samplesDetails = new ReportDataSource() { Name = "SampleModel", Value = GetSamplesList() };
                e.DataSources.Add(samplesDetails);
            }
        }
    }
}