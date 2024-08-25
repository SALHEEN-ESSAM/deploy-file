<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonthlyWarnings.aspx.cs" Inherits="Amana.MonthlyWarningsPage" EnableEventValidation="false" %>
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
        <div  style="width:820px;text-align:center">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <%--   <rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>--%>
            
        <rsweb:ReportViewer  runat ="server"  Width="820px" Height="1250px" ShowPrintButton="true"  AsyncRendering="false" id="rvSiteMapping" >  
        </rsweb:ReportViewer> 
        </div>
    </form>
</body>
</html>
