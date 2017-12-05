<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CallWebService.aspx.cs" Inherits="CallWebService" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <br />
        <asp:Button ID="Call1" runat="server" Text="叫一下字串" OnClick="Call1_Click"  />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <br />
        <asp:Button ID="Call2" runat="server" Text="叫一下XML" OnClick="Call2_Click"  />
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <br />
        <asp:Button ID="Call3" runat="server" Text="叫一下Bat1" OnClick="Call3_Click"  />
        <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <br />
        <asp:Button ID="Call4" runat="server" Text="叫一下Bat2" OnClick="Call4_Click"   />
        <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
        <br />
    
    </div>
    </form>
</body>
</html>
