<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Pages_Login" %>
<%@ Register Src="~/Controls/CaptchaControl.ascx" TagPrefix="uc" TagName="Captcha" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - Simple Online Store</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 0; padding: 20px; }
        .container { max-width: 600px; margin: 0 auto; }
        .header { background-color: #f5f5f5; padding: 10px; margin-bottom: 20px; }
        .form-group { margin-bottom: 15px; }
        .form-group label { display: block; margin-bottom: 5px; }
        .form-group input { width: 100%; padding: 8px; box-sizing: border-box; }
        .btn { padding: 8px 12px; margin-right: 10px; background-color: #4CAF50; color: white; border: none; cursor: pointer; }
        .btn:hover { background-color: #45a049; }
        .error { color: red; margin-bottom: 15px; }
        .tabs { display: flex; margin-bottom: 20px; }
        .tab { padding: 10px 15px; background-color: #ddd; cursor: pointer; }
        .tab.active { background-color: #f5f5f5; }
        .tab-content { display: none; }
        .tab-content.active { display: block; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <h1>Login / Register</h1>
                <a href="/OnlineStoreApp/Default.aspx">Back to Home</a>
            </div>

            <div class="tabs">
                <div id="loginTab" class="tab active" onclick="showTab('login')">Login</div>
                <div id="registerTab" class="tab" onclick="showTab('register')">Register</div>
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>

            <!-- Login Form -->
            <div id="loginContent" class="tab-content active">
                <div class="form-group">
                    <label for="txtLoginUsername">Username:</label>
                    <asp:TextBox ID="txtLoginUsername" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvLoginUsername" runat="server" ControlToValidate="txtLoginUsername"
                        ErrorMessage="Username is required" Display="Dynamic" ForeColor="Red" ValidationGroup="LoginGroup">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <label for="txtLoginPassword">Password:</label>
                    <asp:TextBox ID="txtLoginPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvLoginPassword" runat="server" ControlToValidate="txtLoginPassword"
                        ErrorMessage="Password is required" Display="Dynamic" ForeColor="Red" ValidationGroup="LoginGroup">
                    </asp:RequiredFieldValidator>
                </div>
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" ValidationGroup="LoginGroup" />
            </div>

            <!-- Register Form -->
            <div id="registerContent" class="tab-content">
                <div class="form-group">
                    <label for="txtRegisterUsername">Username:</label>
                    <asp:TextBox ID="txtRegisterUsername" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvRegisterUsername" runat="server" ControlToValidate="txtRegisterUsername"
                        ErrorMessage="Username is required" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterGroup">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <label for="txtRegisterPassword">Password:</label>
                    <asp:TextBox ID="txtRegisterPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvRegisterPassword" runat="server" ControlToValidate="txtRegisterPassword"
                        ErrorMessage="Password is required" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterGroup">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtRegisterPassword"
                        ErrorMessage="Password must be at least 8 characters with uppercase, lowercase, digit, and special character"
                        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$"
                        Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterGroup">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="form-group">
                    <label for="txtConfirmPassword">Confirm Password:</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                        ErrorMessage="Confirm Password is required" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterGroup">
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword"
                        ControlToCompare="txtRegisterPassword" ErrorMessage="Passwords do not match"
                        Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterGroup">
                    </asp:CompareValidator>
                </div>
                <div class="form-group">
                    <label for="txtEmail">Email:</label>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Email is required" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterGroup">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Invalid email format" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                        Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterGroup">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="form-group">
                    <uc:Captcha runat="server" ID="registerCaptcha" />
                </div>
                <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn" OnClick="btnRegister_Click" ValidationGroup="RegisterGroup" />
            </div>
        </div>

        <script type="text/javascript">
            function showTab(tabName) {
                // Hide all tabs and contents
                document.getElementById('loginTab').className = 'tab';
                document.getElementById('registerTab').className = 'tab';
                document.getElementById('loginContent').className = 'tab-content';
                document.getElementById('registerContent').className = 'tab-content';

                // Show selected tab and content
                if (tabName === 'login') {
                    document.getElementById('loginTab').className = 'tab active';
                    document.getElementById('loginContent').className = 'tab-content active';
                } else {
                    document.getElementById('registerTab').className = 'tab active';
                    document.getElementById('registerContent').className = 'tab-content active';
                }
            }
        </script>
    </form>
</body>
</html>