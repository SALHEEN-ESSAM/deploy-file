<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassificationReport.aspx.cs" Inherits="Amana.ClassificationReportPage" EnableEventValidation="false" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   
    <link href="../Content/assets/css/Cairo.css" rel="stylesheet" />
    <style>
        body:nth-of-type(1) img[src*="Blank.gif"]
        {
            display: none;
        }
    </style>
    <script src="../Content/assets/js/jquery-3.4.1.min.js"></script>
     <script>

         $(document).ready(function () {
             
             $("#VisibleReportContentrvSiteMapping_ctl09").css("float", "right");
             $("#VisibleReportContentrvSiteMapping_ctl09").css("padding-right", "50px");
         });



     </script>
</head>

<body>
    <form id="form1" runat="server">
        <div  style="width:800px;text-align:center">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <%--   <rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>--%>
            
            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            
        <rsweb:ReportViewer   runat ="server"  Width="780px" Height="950px"  AsyncRendering="False" id="rvSiteMapping" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"  >
            <localreport reportpath="Reports\DailyVisitsSummary.rdlc">
            </localreport>
        </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
