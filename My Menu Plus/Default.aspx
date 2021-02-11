<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="My_Menu_Plus._Default" %>
<html>
    <head runat="server">
        <title></title>
    </head>
    <body>
        <form id="form" runat="server">
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <br/>
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />         
        </form>

         <asp:Label ID="Label1" runat="server" Text="Name:"></asp:Label>


    </body>
</html>