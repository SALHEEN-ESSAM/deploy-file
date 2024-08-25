#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Utilities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Repository;
#pragma warning restore CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels.Classifications;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels.Reports;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels.VisitsAndSamples;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amana
{
    public partial class ClassificationReportPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rvSiteMapping.ProcessingMode = ProcessingMode.Local;
                rvSiteMapping.LocalReport.ReportPath = Server.MapPath("~/Reports/ClassificationReport.rdlc");
                rvSiteMapping.LocalReport.DataSources.Clear();

                //List<C_CountriesViewModel> test = new List<C_CountriesViewModel>();
                //test.Add(new C_CountriesViewModel { Code = "test", CountryId = 5, IsActive = true, LanguageId = 1, Title = "Country Number 1" });
                //test.Add(new C_CountriesViewModel { Code = "test 1", CountryId = 5, IsActive = true, LanguageId = 1, Title = "Country Number 1" });
                //test.Add(new C_CountriesViewModel { Code = "test 2", CountryId = 5, IsActive = true, LanguageId = 1, Title = "Country Number 1" });
                //test.Add(new C_CountriesViewModel { Code = "test 3", CountryId = 5, IsActive = true, LanguageId = 1, Title = "Country Number 1" });
                //test.Add(new C_CountriesViewModel { Code = "test 5", CountryId = 5, IsActive = true, LanguageId = 1, Title = "Country Number 1" });

                List<ReportInfo> repInfoLst = new List<ReportInfo>();
                ReportInfo repInfo = new ReportInfo();


                repInfo.ReportDate = Request["month"];// HelperMethods.GetCurrentDateTime().ToString("MMMM yyyy" , new CultureInfo("ar-EG")) + " م" ;

                int factoryId = int.Parse(Request["fId"]);
                int periodId = int.Parse(Request["pId"]);


                StngTimePeriods clsPeriod = SettingsRepository.GetCurrentTimePeriodItem(true);
                if (clsPeriod != null)
                {
                    Cls_FactoryClassifications clsItem = FactoriyClassificationRepository.GetClassificationByPeriodIdAndFactoryId(factoryId, clsPeriod.TimePeriodId);

                    StngTimePeriods reportPeriod = SettingsRepository.GetTimePeriodItem(periodId);
                    //Cls_FactoryClassifications clsItem = FactoriyClassificationRepository.GetClassificationById(classificationId, true);
                    if (clsItem != null)
                    {
                        List<ConcreteMixturesInfoFormViewModel> concMixtureVMLst = new List<ConcreteMixturesInfoFormViewModel>();
                        ConcreteMixturesInfoFormViewModel concMixtureVM = FactoriyClassificationRepository.GetConcreteMixturesInfoViewModel(clsItem.Cls_ConcreteMixturesInfo);
                        if (concMixtureVM.FinishDate.HasValue)
                        {
                            if (concMixtureVM.FinishDate < clsItem.StngTimePeriods.EndDate)
                                concMixtureVM.IsFinishDateExpired = true;

                            concMixtureVM.FinishDateString = concMixtureVM.FinishDate.Value.ToString("dd / MM / yyyy") + " م";
                        }

                        concMixtureVMLst.Add(concMixtureVM);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ConcreteMixtureInfo", concMixtureVMLst));

                        List<Visits> allSamplesRes = VisitsRepository.SearchVisits(locationId: null, startDate: reportPeriod.StartDate, endDate: reportPeriod.EndDate, userId: null, factoryId: factoryId, isDone: true);
                        List<SampleViewModel> allSamplesLst = new List<SampleViewModel>();
                        foreach (var visitItem in allSamplesRes)
                        {
                            if (visitItem.VisitsSample != null)
                            {
                                if (visitItem.VisitsSample.LabResist28days.HasValue)
                                    allSamplesLst.Add(VisitsRepository.MakeConcreteSampleViewModel(visitItem, forReport: true));
                            }
                        }

                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("AllSamples", allSamplesLst));

                        var samples30Lst = allSamplesLst.Where(a => a.ConcreteClass == 30).ToList();
                        double sum30 = samples30Lst.Sum(a => a.LabResist28days).HasValue ? samples30Lst.Sum(a => a.LabResist28days).Value : 0;
                        double avg30 = samples30Lst.Count() > 0 ? Math.Round(sum30 / samples30Lst.Count(), 1) : 0;
                        repInfo.SamplesAverage35 = avg30;
                        //average 3 results for class 35
                        for (int i = 0; i < samples30Lst.Count; i++)
                        {
                            if (i >= 2)
                            {
                                var samp = samples30Lst[i];
                                double currentVal = samples30Lst[i].LabResist28days.HasValue ? samples30Lst[i].LabResist28days.Value : 0;
                                double prevVal = samples30Lst[i - 1].LabResist28days.HasValue ? samples30Lst[i - 1].LabResist28days.Value : 0;
                                double prev2Val = samples30Lst[i - 2].LabResist28days.HasValue ? samples30Lst[i - 2].LabResist28days.Value : 0;

                                samp.AverageThreeResults = Math.Round((currentVal + prev2Val + prevVal) / 3, 1);
                            }
                        }

                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("Samples35", samples30Lst));

                        var samples25Lst = allSamplesLst.Where(a => a.ConcreteClass == 25).ToList();
                        double sum25 = samples25Lst.Sum(a => a.LabResist28days).HasValue ? samples25Lst.Sum(a => a.LabResist28days).Value : 0;
                        double avg25 = samples25Lst.Count() > 0 ? Math.Round(sum25 / samples25Lst.Count(), 1) : 0;
                        repInfo.SamplesAverage30 = avg25;
                        //average 3 results for class 30
                        for (int i = 0; i < samples25Lst.Count; i++)
                        {
                            if (i >= 2)
                            {
                                var samp = samples25Lst[i];
                                double currentVal = samples25Lst[i].LabResist28days.HasValue ? samples25Lst[i].LabResist28days.Value : 0;
                                double prevVal = samples25Lst[i - 1].LabResist28days.HasValue ? samples25Lst[i - 1].LabResist28days.Value : 0;
                                double prev2Val = samples25Lst[i - 2].LabResist28days.HasValue ? samples25Lst[i - 2].LabResist28days.Value : 0;
                                samp.AverageThreeResults = Math.Round((currentVal + prev2Val + prevVal) / 3, 1);
                            }
                        }

                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("Samples30", samples25Lst));

                        repInfo.ClassificationId = clsItem.ClassificationId;
                        repInfo.DailyProductRating = clsItem.Factories.DailyProductRating;
                        repInfo.Description = clsItem.Factories.Description;
                        repInfo.FaxNo = clsItem.Factories.FaxNo;
                        repInfo.Location = clsItem.Factories.Location;
                        repInfo.Name = clsItem.Factories.Name;
                        repInfo.OwnerName = clsItem.Factories.OwnerName;
                        repInfo.Photo = clsItem.Factories.Photo;
                        repInfo.ProductEnergy = clsItem.Factories.ProductEnergy;
                        repInfo.ResponsibleManager = clsItem.Factories.ResponsibleManager;
                        repInfo.ResponsibleManagerCertified = clsItem.Factories.ResponsibleManagerCertified;
                        repInfo.TechManager = clsItem.Factories.TechManager;
                        repInfo.TechManagerCertified = clsItem.Factories.TechManagerCertified;
                        repInfo.TelephoneNo = clsItem.Factories.TelephoneNo;
                        repInfo.TimePeriodFrom = reportPeriod.StartDate.ToString("dd / MM / yyyy") + " م ";
                        repInfo.TimePeriodTo = reportPeriod.EndDate.ToString("dd / MM / yyyy") + " م ";
                        repInfo.TimePeriodName = reportPeriod.Title;
                        repInfo.MixStationsCount = clsItem.Cls_MixingStations.Count();

                        //repInfo.ClassificationGrade = clsItem.ClassificationGrade ?? 0;
                        //repInfo.ClassificationText = clsItem.ClassificationText;
                        if (clsItem.Cls_LabAdditionalReqInfo != null)
                        {
                            repInfo.CylindricalMolds = clsItem.Cls_LabAdditionalReqInfo.CylindricalMolds;
                            repInfo.Cubes = clsItem.Cls_LabAdditionalReqInfo.Cubes;
                            repInfo.ConcreteMachine = clsItem.Cls_LabAdditionalReqInfo.ConcreteMachine;
                            repInfo.ControlSpeedWay = clsItem.Cls_LabAdditionalReqInfo.ControlSpeedWay ?? 0;
                            repInfo.IsProcessingCheck1 = clsItem.Cls_LabAdditionalReqInfo.IsProcessingCheck1 ?? false;
                            repInfo.IsProcessingCheck2 = clsItem.Cls_LabAdditionalReqInfo.IsProcessingCheck2 ?? false;
                            repInfo.IsProcessingCheck3 = clsItem.Cls_LabAdditionalReqInfo.IsProcessingCheck3 ?? false;
                            repInfo.MachineType = clsItem.Cls_LabAdditionalReqInfo.MachineType;
                            repInfo.ManufacturingYear = clsItem.Cls_LabAdditionalReqInfo.ManufacturingYear;

                        }

                        if (clsItem.Cls_LabQCReqInfo != null)
                        {
                            repInfo.IsTechniqcalSheet = clsItem.Cls_LabQCReqInfo.IsTechniqcalSheet ?? false;
                            repInfo.TrialMixesEvalutePeriodicId = clsItem.Cls_LabQCReqInfo.TrialMixesEvalutePeriodicId ?? 0;
                            if (clsItem.Cls_LabQCReqInfo.StngPropPeriodics != null)
                            {
                                repInfo.TrialMixesEvalutePeriodicName = clsItem.Cls_LabQCReqInfo.StngPropPeriodics.Peroidic;

                            }

                            repInfo.CompressiveStrengthA = clsItem.Cls_LabQCReqInfo.CompressiveStrengthA ?? false;
                            repInfo.CompressiveStrengthB = clsItem.Cls_LabQCReqInfo.CompressiveStrengthB ?? false;
                            repInfo.CuringConcreteSamplesA = clsItem.Cls_LabQCReqInfo.CuringConcreteSamplesA ?? false;
                            repInfo.CuringConcreteSamplesB = clsItem.Cls_LabQCReqInfo.CuringConcreteSamplesB ?? false;
                            repInfo.IsProperProtection = clsItem.Cls_LabQCReqInfo.IsProperProtection ?? false;
                        }

                        Factories factoryItem = FactoriesRepository.GetFactoryById(clsItem.FactoryId, true);
                        ReportDataSource rdsFactoryEquipments = new ReportDataSource("FactoryEquipments", factoryItem.FactoryEquipments);

                        rvSiteMapping.LocalReport.EnableExternalImages = true;
                        rvSiteMapping.ShowParameterPrompts = true;
                        rvSiteMapping.LocalReport.DataSources.Add(rdsFactoryEquipments);

                        int classificationId = clsItem.ClassificationId;
                        List<ClassificationPropertyViewModel> factoryNotSelected = new List<ClassificationPropertyViewModel>();
                        //التجهيزات الاساسية
                        var ListProp1 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 1);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp1", ListProp1));
                        factoryNotSelected.AddRange(ListProp1.Where(a => a.IsSelected == false));

                        var ListProp2 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 2);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp2", ListProp2));
                        factoryNotSelected.AddRange(ListProp2.Where(a => a.IsSelected == false));

                        var ListProp3 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 3);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp3", ListProp3));
                        factoryNotSelected.AddRange(ListProp3.Where(a => a.IsSelected == false));

                        var ListProp4 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 4);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp4", ListProp4));
                        factoryNotSelected.AddRange(ListProp4.Where(a => a.IsSelected == false));

                        var ListProp5 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 5);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp5", ListProp5));
                        factoryNotSelected.AddRange(ListProp5.Where(a => a.IsSelected == false));

                        var ListProp6 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 6);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp6", ListProp6));
                        factoryNotSelected.AddRange(ListProp6.Where(a => a.IsSelected == false));

                        var ListProp7 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 7);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp7", ListProp7));
                        factoryNotSelected.AddRange(ListProp7.Where(a => a.IsSelected == false));

                        var ListProp8 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 8);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp8", ListProp8));
                        factoryNotSelected.AddRange(ListProp8.Where(a => a.IsSelected == false));

                        var ListProp9 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 9);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp9", ListProp9));
                        factoryNotSelected.AddRange(ListProp9.Where(a => a.IsSelected == false));

                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("FactoryBasicsNotSelected", factoryNotSelected));


                        //التجهيزات التكميلية
                        List<ClassificationPropertyViewModel> ListProp10 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 10);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp10", ListProp10));
                        List<ClassificationPropertyViewModel> ListProp11 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 11);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp11", ListProp11));
                        List<ClassificationPropertyViewModel> ListProp12 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 12);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp12", ListProp12));
                        List<ClassificationPropertyViewModel> ListProp15 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 15);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp15", ListProp15));
                        List<ClassificationPropertyViewModel> ListProp16 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 16);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp16", ListProp16));
                        List<ClassificationPropertyViewModel> ListProp17 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 17);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp17", ListProp17));


                        //المختبر
                        List<ClassificationPropertyViewModel> labBasicsNotSelected = new List<ClassificationPropertyViewModel>();

                        //المتطلبات الاساسية 
                        var ListProp18 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 18);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp18", ListProp18));
                        labBasicsNotSelected.AddRange(ListProp18.Where(a => a.IsSelected == false));

                        var ListProp19 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 19);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp19", ListProp19));
                        labBasicsNotSelected.AddRange(ListProp19.Where(a => a.IsSelected == false));

                        var ListProp33 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 33);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp33", ListProp33));

                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("LabBasicsNotSelected", labBasicsNotSelected));


                        //المتطلبات التكميلية 
                        List<ClassificationPropertyViewModel> ListProp20 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 20);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp20", ListProp20));
                        List<ClassificationPropertyViewModel> ListProp21 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 21);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp21", ListProp21));
                        List<ClassificationPropertyViewModel> ListProp22 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 22);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp22", ListProp22));


                        List<ClassificationLabPropertyViewModel> ListLabProp24 = FactoriyClassificationRepository.GetClassificationLabPropertyList(classificationId, 24);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListLabProp24", ListLabProp24));
                        List<ClassificationLabPropertyViewModel> ListLabProp25 = FactoriyClassificationRepository.GetClassificationLabPropertyList(classificationId, 25);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListLabProp25", ListLabProp25));
                        List<ClassificationLabPropertyViewModel> ListLabProp34 = FactoriyClassificationRepository.GetClassificationLabPropertyList(classificationId, 34);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListLabProp34", ListLabProp34));
                        //List<ClassificationPropertyViewModel> ListProp26 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 26);
                        //rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp26", ListProp26));

                        List<ClassificationLabPropertyViewModel> ListLabProp26 = FactoriyClassificationRepository.GetClassificationLabPropertyList(classificationId, 26);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListLabProp26", ListLabProp26));

                        List<ClassificationLabPropertyViewModel> ListLabProp28 = FactoriyClassificationRepository.GetClassificationLabPropertyList(classificationId, 28);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListLabProp28", ListLabProp28));

                        List<ClassificationPropertyViewModel> ListProp29 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 29);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp29", ListProp29));
                        List<ClassificationPropertyViewModel> ListProp30 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 30);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp30", ListProp30));
                        List<ClassificationPropertyViewModel> ListProp31 = FactoriyClassificationRepository.GetClassificationPropertyList(classificationId, 31);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ListProp31", ListProp31));


                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("TechnicalStuff1", factoryItem.FactoryTechnicalStuff.Where(a => a.JobDescriptionId == 1)));
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("TechnicalStuff2", factoryItem.FactoryTechnicalStuff.Where(a => a.JobDescriptionId == 2)));
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("TechnicalStuff3", factoryItem.FactoryTechnicalStuff.Where(a => a.JobDescriptionId == 3)));


                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("MixStations", clsItem.Cls_MixingStations));
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("FactoryMaterialSuppliers1", factoryItem.FactoryMaterialSuppliers.Where(a => a.MaterialTypeId == 1)));
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("FactoryMaterialSuppliers2", factoryItem.FactoryMaterialSuppliers.Where(a => a.MaterialTypeId == 2)));
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("FactoryMaterialSuppliers3", factoryItem.FactoryMaterialSuppliers.Where(a => a.MaterialTypeId == 3)));
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("FactoryMaterialSuppliers4", factoryItem.FactoryMaterialSuppliers.Where(a => a.MaterialTypeId == 4)));
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("FactoryMaterialSuppliers5", factoryItem.FactoryMaterialSuppliers.Where(a => a.MaterialTypeId == 5)));


                        //حساب المؤشرات 
                        #region  المؤشر الاول
                        //المؤشر الاول
                        List<ClsIndicatorList> ClsIndicatorList1MixStations = ClassificationHelper.GetMixiStationsWeightsList(clsItem.Cls_MixingStations.ToList());
                        List<ClsIndicatorList> ClsIndicatorList1A = ClassificationHelper.GetFirstIndicatorList(ListProp10, ListProp11, ListProp12, ListProp15, ListProp16, ListProp17, ClsIndicatorList1MixStations);

                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ClsIndicatorList1MixStations", ClsIndicatorList1MixStations));
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ClsIndicatorList1A", ClsIndicatorList1A));

                        #endregion


                        #region المؤشر الثاني
                        //المؤشر الثاني
                        List<ClsIndicatorList> ClsIndicatorList2A = ClassificationHelper.GetIndicator2A(ListProp20, ListProp21, ListProp22, clsItem.Cls_LabAdditionalReqInfo);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ClsIndicatorList2A", ClsIndicatorList2A));

                        List<ClsIndicatorList> ClsIndicatorList2B = ClassificationHelper.GetIndicator2B(ListLabProp24, ListLabProp25, ListLabProp26, ListLabProp28, ListLabProp34, ListProp29, ListProp30, ListProp31, clsItem.Cls_LabQCReqInfo);
                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ClsIndicatorList2B", ClsIndicatorList2B));

                        #endregion

                        #region المؤشر الثالث
                        //المؤشر الثالث
                        double tempraturePoints = ClassificationHelper.GetTempraturePoints(allSamplesLst);
                        double slumbPoints = ClassificationHelper.GetSlumbPoints(allSamplesLst);
                        double pressurePoints = ClassificationHelper.GetPressurePoints(allSamplesLst);
                        //standard Deviation for list of class 30
                        repInfo.SamplesSDeviation35 = ClassificationHelper.CalculateStandardDeviation(samples30Lst.Select(a => a.LabResist28days).ToList(), avg30);
                        //standard Deviation for list of class 25
                        repInfo.SamplesSDeviation30 = ClassificationHelper.CalculateStandardDeviation(samples25Lst.Select(a => a.LabResist28days).ToList(), avg25);
                        repInfo.ProductionStandardDeviation = ClassificationHelper.CalculateAverageStandardDeviation(repInfo.SamplesSDeviation35, repInfo.SamplesSDeviation30, samples30Lst.Count, samples25Lst.Count);
                        double sdProductionWeight = ClassificationHelper.GetProductionControlWeight(repInfo.ProductionStandardDeviation);


                        List<ClsIndicatorList> ClsIndicatorList3A = ClassificationHelper.GetThirdIndicatorList(tempraturePoints, slumbPoints, pressurePoints, repInfo.ProductionStandardDeviation);

                        rvSiteMapping.LocalReport.DataSources.Add(new ReportDataSource("ClsIndicatorList3A", ClsIndicatorList3A));


                        #endregion

                        repInfo.ClsIndicator1Val = ClassificationHelper.GetFirstIndicatorValue(ClsIndicatorList1A); // Math.Round(((ClsIndicatorList1A.Sum(a => a.Points) / ClsIndicatorList1A.Sum(a => a.MaxDegree)) * 100), 1);
                        repInfo.ClsIndicator2Val = ClassificationHelper.GetSecondIndicatorValue(ClsIndicatorList2A, ClsIndicatorList2B);
                        repInfo.ClsIndicator3Val = ClassificationHelper.GetThirdIndicatorValue(tempraturePoints, slumbPoints, pressurePoints, sdProductionWeight);

                        //الاعدادات النصية للتصنيف
                        NodeRepository nodeDI = new NodeRepository(1, new AmanaConcreteDBEntities1());
                        var introduction = nodeDI.SingleLocItem(2244);
                        var commentCompressiveStrength = nodeDI.SingleLocItem(2243);
                        var commentMaxTemprature = nodeDI.SingleLocItem(2240);
                        var commentMinTemprature = nodeDI.SingleLocItem(2241);
                        var commentSlumb = nodeDI.SingleLocItem(2242);

                        double temp32Percentage = allSamplesLst.Count > 0 ? Math.Round((((double)allSamplesLst.Count(a => a.ConcreteTemperture > 32) / allSamplesLst.Count)) * 100, 1) : 0;
                        double temp28Percentage = allSamplesLst.Count > 0 ? Math.Round((((double)allSamplesLst.Count(a => a.ConcreteTemperture > 28) / allSamplesLst.Count)) * 100, 1) : 0;
                        double slumb150_200Percentage = allSamplesLst.Count(a => a.ConcreteClass >= 25) > 0 ? Math.Round((((double)allSamplesLst.Count(a => a.ConcreteClass >= 25 && (a.SlumpValue < 150 || a.SlumpValue > 200)) / allSamplesLst.Count(a => a.ConcreteClass >= 25))) * 100, 1) : 0;
                        double compressiveVal = allSamplesLst.Count > 0 ? Math.Round((((double)allSamplesLst.Count(a => a.ConcreteClass - a.LabResist28days > 0.5) / allSamplesLst.Count)) * 100, 1) : 0;

                        if (commentCompressiveStrength != null)
                        {
                            repInfo.CommentCompressiveStrength = commentCompressiveStrength.Details.Replace("##Value##", compressiveVal.ToString());
                        }
                        if (commentMaxTemprature != null)
                        {
                            repInfo.CommentMaxTemprature = commentMaxTemprature.Details.Replace("##Value##", temp32Percentage.ToString());
                        }
                        if (commentMinTemprature != null)
                        {
                            repInfo.CommentMinTemprature = commentMinTemprature.Details.Replace("##Value##", temp28Percentage.ToString());
                        }
                        if (commentSlumb != null)
                        {
                            repInfo.CommentSlumb = commentSlumb.Details.Replace("##Value##", slumb150_200Percentage.ToString());
                        }
                        if (introduction != null)
                        {
                            repInfo.IntroductionText = introduction.Details;
                        }

                        repInfo.ClassificationText = ClassificationHelper.GetClassificationGradeText(repInfo.ClsIndicator1Val, repInfo.ClsIndicator2Val, repInfo.ClsIndicator3Val, reportPeriod.StartDate, reportPeriod.EndDate, clsItem.ClassificationText);
                        //var introPage=nodeDI.SingleLocItem(2235);
                        // if (introPage != null)
                        //     repInfo.Introduction = introPage.Details;

                        repInfoLst.Add(repInfo);
                        ReportDataSource rdc = new ReportDataSource("ReportInfo", repInfoLst);
                        rvSiteMapping.LocalReport.DataSources.Add(rdc);


                        rvSiteMapping.DataBind();
                        rvSiteMapping.LocalReport.Refresh();
                    }
                    else
                    {
                        rvSiteMapping.Visible = false;
                        lblError.Text = "لا يوجد بيانات";
                    }
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