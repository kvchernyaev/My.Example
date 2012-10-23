<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateProgress.ascx.cs" Inherits="My.Example.Web.Controls.UpdateProgress" %>

<script runat="server">
    public string AssociatedUpdatePanelID { get { return UpdateProgress1.AssociatedUpdatePanelID; } set { UpdateProgress1.AssociatedUpdatePanelID = value; } }
</script>

<asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="500" DynamicLayout="True">
    <ProgressTemplate>
        <div style="position: fixed; width: 100%; text-align: center; left: 0; top: 20%;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/progress4.gif" AlternateText="Update in progress..." />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
