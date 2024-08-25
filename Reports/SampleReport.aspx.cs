#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Repository;
#pragma warning restore CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels.VisitsAndSamples;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Amana
{
    public partial class SampleReportPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rvSiteMapping.ProcessingMode = ProcessingMode.Local;
                rvSiteMapping.LocalReport.ReportPath = Server.MapPath("~/Reports/VisitSampleReport.rdlc");
                rvSiteMapping.LocalReport.DataSources.Clear();
                rvSiteMapping.LocalReport.EnableExternalImages = true;
                rvSiteMapping.ShowParameterPrompts = true;

                int visitId = int.Parse(Request["visitId"]);
                Visits visitItem = VisitsRepository.GetVisitById(visitId, true);
                List<SampleViewModel> samplesLst = new List<SampleViewModel>();
                samplesLst.Add(VisitsRepository.MakeConcreteSampleViewModel(visitItem, forReport: true));

                ReportDataSource rdc = new ReportDataSource("SampleModel", samplesLst);
                rvSiteMapping.LocalReport.DataSources.Add(rdc);


                /* begin added part */

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

    }
}