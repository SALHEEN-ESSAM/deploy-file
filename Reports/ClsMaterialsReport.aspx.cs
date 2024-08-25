#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Repository;
#pragma warning restore CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels.Factories;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels.Reports;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amana
{
    public partial class ClsMaterialsReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rvSiteMapping.ProcessingMode = ProcessingMode.Local;
                rvSiteMapping.LocalReport.ReportPath = Server.MapPath("~/Reports/ClsMaterialsReport.rdlc");
                rvSiteMapping.LocalReport.DataSources.Clear();
                rvSiteMapping.LocalReport.EnableExternalImages = true;
                rvSiteMapping.ShowParameterPrompts = true;

                int periodId = int.Parse(Request["pId"]);


                StngTimePeriods reportPeriod = SettingsRepository.GetTimePeriodItem(periodId);
                StngTimePeriods clsPeriod = SettingsRepository.GetCurrentTimePeriodItem(true);
                if (reportPeriod != null && clsPeriod != null)
                {
                    //var clsSummaryList = FactoriyClassificationRepository.GetClassificationSummaryReport(clsPeriod, reportPeriod);
                    //rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ClassificationSummary", clsSummaryList));


                    List<ReportInfo> repInfoLst = new List<ReportInfo>();
                    repInfoLst.Add(new ReportInfo { ReportDate = clsPeriod.Title });
                    rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("RepInfoo", repInfoLst));


                    rvSiteMapping.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;

                    rvSiteMapping.DataBind();
                    rvSiteMapping.LocalReport.Refresh();

                }
                else
                {
                    rvSiteMapping.Visible = false;
                    lblError.Text = "لا يوجد بيانات";
                }
            }

        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            //var ID = Convert.ToInt32(e.Parameters["FactoryId"].Values[0]);
            int materialType = int.Parse(Request["m"]);
            if (e.ReportPath == "ClsSuppliersSubReport")
            {
                var lst = new AmanaConcreteDBEntities1().FactoryMaterialSuppliers.Include(a => a.IdxMaterialTypes).Where(a => a.MaterialTypeId == materialType).ToList();
                List<FactoryMaterialSupplierViewModel> result = new List<FactoryMaterialSupplierViewModel>();
                foreach (var item in lst)
                {
                    FactoryMaterialSupplierViewModel vm = new FactoryMaterialSupplierViewModel
                    {
                        FactoryId = item.FactoryId.HasValue ? item.FactoryId.Value : 0,
                        MaterialTypeId = item.MaterialTypeId.HasValue ? item.MaterialTypeId.Value : 0,
                        TypeofMaterial = item.TypeofMaterial,
                        Name = item.Name,
                        MaterialName = item.IdxMaterialTypes.Name,
                        MaterialDesc = GetMaterialDescribtion(item.MaterialTypeId.HasValue ? item.MaterialTypeId.Value : 0)
                    };

                    result.Add(vm);
                }
                //var samplesDetails = new ReportDataSource() { Name = "MaterialSuppliers", Value = lst };
                var samplesDetails = new ReportDataSource() { Name = "MaterialSuppliers", Value = result };
                e.DataSources.Add(samplesDetails);
            }
        }

        string GetMaterialDescribtion(int materialType)
        {
            string name = "";
            switch (materialType)
            {
                case 1:
                    {
                        name = "مقاس الركام";
                        break;
                    }
                case 2:
                    {
                        name = "نوع الركام";
                        break;
                    }
                case 3:
                    {
                        name = "نوع الأسمنت";
                        break;
                    }
                case 4:
                    {
                        name = "مصدر الماء";
                        break;
                    }
                case 5:
                    {
                        name = "النوع";
                        break;
                    }

            }

            return name;
        }




    }


}