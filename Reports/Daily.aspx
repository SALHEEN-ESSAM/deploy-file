<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Daily.aspx.cs" Inherits="Amana.DailyReportPage" EnableEventValidation="false" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body:nth-of-type(1) img[src*="Blank.gif"]
        {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
            
        <rsweb:ReportViewer  runat ="server"  Width="820px" Height="1250px"  AsyncRendering="False" id="rvSiteMapping" Font-Names="Verdana" Font-Size="8pt" style="margin-right: 0px" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" >
            <localreport reportpath="Reports\DailyVisitsSummary.rdlc">
            </localreport>
        </rsweb:ReportViewer> 
        <div  style="width:820px;text-align:center">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <%--   <rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>--%>
            
        </div>
            
    </form>
</body>
</html>
