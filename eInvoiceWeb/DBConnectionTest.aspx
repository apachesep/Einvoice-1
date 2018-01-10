<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DBConnectionTest.aspx.cs" Inherits="DBConnectionTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
</body>
</html>
<asp:gridview runat="server"></asp:gridview>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Print_No,Seq_No" DataSourceID="SqlDataSource1" EmptyDataText="沒有資料錄可顯示。">
        <Columns>
            <asp:BoundField DataField="Print_No" HeaderText="Print_No" SortExpression="Print_No" />
            <asp:BoundField DataField="VAT_No" HeaderText="VAT_No" SortExpression="VAT_No" />
            <asp:BoundField DataField="Seq_No" HeaderText="Seq_No" SortExpression="Seq_No" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:TestFINConnectionString1 %>" DeleteCommand="DELETE FROM [Rinnai$VAT Print Number] WHERE [Print No] = @Print_No AND [Seq No] = @Seq_No" InsertCommand="INSERT INTO [Rinnai$VAT Print Number] ([Print No], [VAT No], [Seq No]) VALUES (@Print_No, @VAT_No, @Seq_No)" ProviderName="<%$ ConnectionStrings:TestFINConnectionString1.ProviderName %>" SelectCommand="SELECT [timestamp], [Print No] AS Print_No, [VAT No] AS VAT_No, [Seq No] AS Seq_No FROM [Rinnai$VAT Print Number]" UpdateCommand="UPDATE [Rinnai$VAT Print Number] SET [VAT No] = @VAT_No WHERE [Print No] = @Print_No AND [Seq No] = @Seq_No">
        <DeleteParameters>
            <asp:Parameter Name="Print_No" Type="Int32" />
            <asp:Parameter Name="Seq_No" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Print_No" Type="Int32" />
            <asp:Parameter Name="VAT_No" Type="String" />
            <asp:Parameter Name="Seq_No" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="VAT_No" Type="String" />
            <asp:Parameter Name="Print_No" Type="Int32" />
            <asp:Parameter Name="Seq_No" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    </form>

