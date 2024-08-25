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
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.UI;

namespace Amana
{
    public partial class WarningPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rvSiteMapping.ProcessingMode = ProcessingMode.Local;
                rvSiteMapping.LocalReport.DataSources.Clear();
                rvSiteMapping.LocalReport.EnableExternalImages = true;
                rvSiteMapping.ShowParameterPrompts = true;

                int warningId = int.Parse(Request["id"]);
                var warningItem = WarningsRepository.GetWarningById(warningId);
                if (warningItem != null)
                {

                    List<SampleViewModel> samplesLst = new List<SampleViewModel>();
                    //Visits visitItem = VisitsRepository.GetVisitById(warningItem.VisitSampleId.Value, true);
                    //samplesLst.Add(VisitsRepository.MakeConcreteSampleViewModel(visitItem));
                    foreach (var sample in warningItem.FactoryWarningSamples)
                    {
                        Visits vItem = VisitsRepository.GetVisitById(sample.VisitSampleId, true);
                        samplesLst.Add(VisitsRepository.MakeConcreteSampleViewModel(vItem));
                    }

                    List<ReportGeneralInfoViewModel> repInfoLst = new List<ReportGeneralInfoViewModel>();
                    repInfoLst.Add(new ReportGeneralInfoViewModel { ReportDate = warningItem.WarningDate.ToString("dddd dd / MM / yyyy", CultureInfo.CreateSpecificCulture("ar-EG")) });
                    ReportDataSource rdc2 = new ReportDataSource("RepInfo", repInfoLst);
                    rvSiteMapping.LocalReport.DataSources.Add(rdc2);


                    List<WarningReportViewModel> warningInfoLst = new List<WarningReportViewModel>();
                    WarningReportViewModel wInfo = new WarningReportViewModel();
                    wInfo.WarningDate = warningItem.WarningDate.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("ar-EG"));
                    wInfo.VBaseColumn35Count = samplesLst.Count(a => a.V_BaseColumn35 == true);
                    wInfo.VIsCleanLocationCount = samplesLst.Count(a => a.V_IsCleanLocation == true);
                    wInfo.VIsHighTempCount = samplesLst.Count(a => a.V_IsHighTemp == true);
                    wInfo.VIsSlumbCount = samplesLst.Count(a => a.V_IsSlumb == true);
                    wInfo.VLowPresuureCount = samplesLst.Count(a => a.V_IsLowPressure == true);
                    wInfo.VTechnicalStuffCount = samplesLst.Count(a => a.V_TechnicalStuff == true);
                    wInfo.VQualityRecordsCount = 0;
                    wInfo.VStandardDevicesCount = 0;
                    warningInfoLst.Add(wInfo);
                    ReportDataSource wInfoDS = new ReportDataSource("WarningInfo", warningInfoLst);
                    rvSiteMapping.LocalReport.DataSources.Add(wInfoDS);


                    if (warningItem.IsFinalWarning == false)
                    {
                        rvSiteMapping.LocalReport.ReportPath = Server.MapPath("~/Reports/WarningReport.rdlc");

                        ReportDataSource rdc = new ReportDataSource("SampleModel", samplesLst);
                        rvSiteMapping.LocalReport.DataSources.Add(rdc);
                    }
                    else
                    {
                        rvSiteMapping.LocalReport.ReportPath = Server.MapPath("~/Reports/FinalWarningReport.rdlc");

                        List<WarningSummaryViewModel> wSummaryDataSet = new List<WarningSummaryViewModel>();
                        //List<WarningViewModel> warningsDataSet = new List<WarningViewModel>();
                        foreach (var sample in samplesLst)
                        {
                            WarningSummaryViewModel wsVM = new WarningSummaryViewModel();
                            wsVM.SampleDate = string.Format("{0} / {1} / {2}", sample.SampleDateDay, sample.SampleDateMonth, sample.SampleDateYear);
                            wsVM.SampleId = sample.SampleId;
                            CommaDelimitedStringCollection violationText = new CommaDelimitedStringCollection();
                            if (sample.V_BaseColumn35 == true)
                                violationText.Add("عدم إستخدام خلطة صنف 30 للأساسات والأعمدة");
                            if (sample.V_IsCleanLocation == true)
                                violationText.Add("عدم الإهتمام بنظافة الموقع وحماية البيئة");
                            if (sample.V_IsHighTemp == true)
                                violationText.Add("إرتفاع درجة حرارة الخرسانة");
                            if (sample.V_IsLowPressure == true)
                                violationText.Add("انخفاض مقاومة الضغط");
                            if (sample.V_IsSlumb == true)
                                violationText.Add("انخفاض مقدار الهبوط");
                            if (sample.V_TechnicalStuff == true)
                                violationText.Add("عدم تواجد الجهاز الفني للمختبر");

                            wsVM.ViolationsText = violationText.ToString().Replace(",", " + ");
                            wSummaryDataSet.Add(wsVM);


                            var warn = WarningsRepository.GetWarningBySampleVisitId(sample.VisitId);
                            if (warn != null)
                            {
                                sample.WarningDate = warn.WarningDate.ToString("dd / MM / yyyy");
                                sample.WarningId = warn.WarningId;
                            }

                        }

                        ReportDataSource rdcWS = new ReportDataSource("WarningSummary", wSummaryDataSet);
                        rvSiteMapping.LocalReport.DataSources.Add(rdcWS);

                        ReportDataSource rdcW = new ReportDataSource("SubWarnings", samplesLst.Where(a => a.VisitId != warningItem.VisitSampleId));
                        rvSiteMapping.LocalReport.DataSources.Add(rdcW);
                    }




                    rvSiteMapping.DataBind();
                    rvSiteMapping.LocalReport.Refresh();
                }




            }

        }

    }
}