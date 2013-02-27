<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="My.Example.Web.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ucc:CountChooser runat="server" ID="cc" Counts="10,20,200,500" Title="колво" DefaultValue="20" />
        <asp:Button runat="server" Text="test" OnClick="OnClick" />
        <asp:Button runat="server" Text="test2" />

     

    </div>
    </form>
</body>
</html>
