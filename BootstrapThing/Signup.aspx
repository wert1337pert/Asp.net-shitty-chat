<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="BootstrapThing.Signup" %>
<!DOCTYPE html>
<html>
<head>
    <title>Signup Page</title>
    <link rel="stylesheet" type="text/css" href="styles.css" />
</head>
<body>
    <form runat="server">
        <div class="container">
            <h1>Signup</h1>
            <asp:Panel ID="pnlSignup" runat="server">
                <asp:TextBox ID="txtUsername" runat="server" placeholder="Username"></asp:TextBox>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                <asp:TextBox ID="txtEmail" runat="server" placeholder="Email"></asp:TextBox>
                <asp:Button ID="btnSignup" runat="server" Text="Sign Up" OnClick="btnSignup_Click" CssClass="button buttonCool" />
                <asp:Label ID="lblError" runat="server" ClientIDMode="AutoID" Enabled="False" ForeColor="Red" Text="Static" Visible="False"></asp:Label>
            </asp:Panel>
            <p>Already have an account? <a href="Default.aspx">Login here</a></p>
        </div>
    </form>
</body>
</html>