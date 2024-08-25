#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Enums;
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
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amana
{
    public partial class ClassificationSummaryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rvSiteMapping.ProcessingMode = ProcessingMode.Local;

                rvSiteMapping.LocalReport.ReportPath = Request["t"] == "1" ? Server.MapPath("~/Reports/ClassificationSummary.rdlc") : Server.MapPath("~/Reports/ClassificationSummary2.rdlc");
                rvSiteMapping.LocalReport.DataSources.Clear();
                rvSiteMapping.LocalReport.EnableExternalImages = true;

                int periodId = int.Parse(Request["pId"]);


                StngTimePeriods reportPeriod = SettingsRepository.GetTimePeriodItem(periodId);
                StngTimePeriods clsPeriod = SettingsRepository.GetCurrentTimePeriodItem(true);
                if (reportPeriod != null && clsPeriod != null)
                {
                    var clsSummaryList = FactoriyClassificationRepository.GetClassificationSummaryReport(clsPeriod, reportPeriod);
                    rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ClassificationSummary", clsSummaryList));


                    List<ReportInfo> repInfoLst = new List<ReportInfo>();
                    repInfoLst.Add(new ReportInfo { ReportDate = clsPeriod.Title });
                    rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("RepInfo", repInfoLst));


                    UsersRepository userRepository = new UsersRepository(1, new AmanaConcreteDBEntities1());
                    //get absolute path to Project folder
                    string path = new Uri(Server.MapPath("~/Uploads")).AbsoluteUri; // adjust path to Project folder here

                    var consultantLst = userRepository.GetItemsList(roleId: 1008, isActive: true).ToList();

                    AmanaConcreteDBEntities1 db = new AmanaConcreteDBEntities1();
                    var rptApprovals = db.RptApproval.Where(a => a.ReportType == (int)ReportTypesEnum.ClassificationSummaryReport && a.TimePeriodId == periodId);

                    List<ReportSignaturesViewModel> signatures1 = new List<ReportSignaturesViewModel>();
                    ReportSignaturesViewModel user1 = new ReportSignaturesViewModel();
                    if (consultantLst.Count() > 0)
                    {
                        user1.FullName = consultantLst[0].Name;
                        if (!string.IsNullOrEmpty(consultantLst[0].ImageUrl) && rptApprovals.Any(a => a.EmployeeId == consultantLst[0].UserId && a.IsApproved == true))
                        {
                            user1.SignPhotoUrl = path + "/Users/" + consultantLst[0].ImageUrl;
                        }
                    }
                    signatures1.Add(user1);

                    rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("Signatures1", signatures1));

                    List<ReportSignaturesViewModel> signatures2 = new List<ReportSignaturesViewModel>();
                    ReportSignaturesViewModel user2 = new ReportSignaturesViewModel();
                    if (consultantLst.Count() > 1)
                    {
                        user2.FullName = consultantLst[1].Name;
                        if (!string.IsNullOrEmpty(consultantLst[1].ImageUrl) && rptApprovals.Any(a => a.EmployeeId == consultantLst[1].UserId && a.IsApproved == true))
                        {
                            user2.SignPhotoUrl = path + "/Users/" + consultantLst[1].ImageUrl;
                        }
                    }
                    signatures2.Add(user2);
                    rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("Signatures2", signatures2));

                    List<ReportSignaturesViewModel> signatures3 = new List<ReportSignaturesViewModel>();
                    //مشرف المشروع
                    C_UserItems supervisorData = userRepository.SingleItem(AppSettings.ProjectSupervisorId);
                    ReportSignaturesViewModel user3 = new ReportSignaturesViewModel();
                    if (supervisorData != null)
                    {
                        user3.FullName = supervisorData.Name;
                        if (!string.IsNullOrEmpty(supervisorData.ImageUrl) && rptApprovals.Any(a => a.EmployeeId == supervisorData.UserId && a.IsApproved == true))
                        {
                            user3.SignPhotoUrl = path + "/Users/" + supervisorData.ImageUrl;
                        }
                    }
                    signatures3.Add(user3);
                    rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("Signatures3", signatures3));


                    List<ReportSignaturesViewModel> signatures4 = new List<ReportSignaturesViewModel>();
                    //مدير المشروع
                    C_UserItems managerData = userRepository.SingleItem(AppSettings.GeneralManagerId);
                    ReportSignaturesViewModel user4 = new ReportSignaturesViewModel();
                    if (managerData != null)
                    {
                        user4.FullName = managerData.Name;
                        if (!string.IsNullOrEmpty(managerData.ImageUrl) && rptApprovals.Any(a => a.EmployeeId == managerData.UserId && a.IsApproved == true))
                        {
                            user4.SignPhotoUrl = path + "/Users/" + managerData.ImageUrl;
                        }
                    }
                    signatures4.Add(user4);
                    rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("Signatures4", signatures4));

                }
                else
                {
                    rvSiteMapping.Visible = false;
                    lblError.Text = "لا يوجد بيانات";
                }
            }

        }




    }


}