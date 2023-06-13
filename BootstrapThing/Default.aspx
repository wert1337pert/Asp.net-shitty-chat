<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BootstrapThing.Default" %>
<!DOCTYPE html>
<html>
<head>
    <title>Login Page</title>
    <link rel="stylesheet" type="text/css" href="styles.css" />
</head>
<body>
    <form runat="server">
        <div class="container">
            <h1>Login</h1>
            <asp:Panel ID="pnlLogin" runat="server">
                <asp:TextBox ID="txtUsername" runat="server" placeholder="Username"></asp:TextBox>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                <asp:Label ID="lblError" runat="server" Text="Static" ClientIDMode="AutoID" Font-Size="Medium" ForeColor="#CC0000" Visible="False"></asp:Label>
                <% if (lblError.Visible == true) { %>
                <br />
                <br />
                <% } %>
                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
            </asp:Panel>
            <p>Don't have an account? <a href="signup.aspx">Sign up here</a></p>
        </div>
    </form>
</body>
</html>