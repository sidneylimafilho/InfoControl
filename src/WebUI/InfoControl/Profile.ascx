<%@ Control Language="C#" AutoEventWireup="true" Inherits="Company_GeneralProfile"
    CodeBehind="Profile.ascx.cs" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="Profile_LegalEntity.ascx" TagName="CompanyProfile" TagPrefix="uc1" %>
<%@ Register Src="Profile_NaturalPerson.ascx" TagName="Profile" TagPrefix="uc2" %>
<%@ Register Src="../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<div>
    <asp:RadioButton ID="rdbCNPJ" runat="server" GroupName="Choose" Text="Pessoa Jurídica"
        AutoPostBack="True" OnCheckedChanged="rdbCNPJ_CheckedChanged" Checked="True" />&nbsp;&nbsp;&nbsp;
    <asp:RadioButton ID="rdbCPF" runat="server" GroupName="Choose" Text="Pessoa Física"
        AutoPostBack="True" OnCheckedChanged="rdbCPF_CheckedChanged" />
    
</div>
<uc1:CompanyProfile ID="ucCompanyProfile" runat="server" Visible="false" />
<uc2:Profile ID="ucProfile" runat="server" Visible="False" />
