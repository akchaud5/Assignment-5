<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryIt.aspx.cs" Inherits="OnlineStoreApp.TryIt" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TryIt Page - Simple Online Store</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 0; padding: 20px; }
        .container { max-width: 1000px; margin: 0 auto; }
        .header { background-color: #f5f5f5; padding: 10px; margin-bottom: 20px; }
        .panel { margin-bottom: 20px; padding: 15px; border: 1px solid #ddd; border-radius: 5px; }
        .form-group { margin-bottom: 15px; }
        .form-group label { display: block; margin-bottom: 5px; }
        .form-group input { width: 100%; padding: 8px; box-sizing: border-box; }
        .btn { padding: 8px 12px; margin-right: 10px; background-color: #4CAF50; color: white; border: none; cursor: pointer; }
        .btn:hover { background-color: #45a049; }
        .result { margin-top: 15px; padding: 10px; background-color: #f9f9f9; border-radius: 3px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <h1>Service Testing Page</h1>
                <a href="Default.aspx">Back to Home</a>
            </div>

            <h2>Available Services</h2>

            <div class="panel">
                <h3>Discount Service</h3>
                <p>This service calculates a discount based on the order price and quantity.</p>
                
                <div class="form-group">
                    <label for="txtPrice">Price:</label>
                    <asp:TextBox ID="txtPrice" runat="server" TextMode="Number" Step="0.01"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPrice"
                        ErrorMessage="Price is required" Display="Dynamic" ForeColor="Red" ValidationGroup="DiscountGroup">
                    </asp:RequiredFieldValidator>
                </div>
                
                <div class="form-group">
                    <label for="txtQuantity">Quantity:</label>
                    <asp:TextBox ID="txtQuantity" runat="server" TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
                        ErrorMessage="Quantity is required" Display="Dynamic" ForeColor="Red" ValidationGroup="DiscountGroup">
                    </asp:RequiredFieldValidator>
                </div>
                
                <asp:Button ID="btnCalculateDiscount" runat="server" Text="Calculate Discount" CssClass="btn" 
                    OnClick="btnCalculateDiscount_Click" ValidationGroup="DiscountGroup" />
                
                <div class="result">
                    <strong>Result:</strong> <asp:Label ID="lblDiscountResult" runat="server"></asp:Label>
                </div>
            </div>

            <div class="panel">
                <h3>Password Hasher</h3>
                <p>This component securely hashes passwords using SHA-256.</p>
                
                <div class="form-group">
                    <label for="txtPassword">Password:</label>
                    <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="Password is required" Display="Dynamic" ForeColor="Red" ValidationGroup="HashGroup">
                    </asp:RequiredFieldValidator>
                </div>
                
                <asp:Button ID="btnHashPassword" runat="server" Text="Hash Password" CssClass="btn" 
                    OnClick="btnHashPassword_Click" ValidationGroup="HashGroup" />
                
                <div class="result">
                    <strong>Result:</strong> <asp:Label ID="lblHashResult" runat="server"></asp:Label>
                </div>
            </div>

            <div class="panel">
                <h3>CAPTCHA Control</h3>
                <p>This user control generates and validates CAPTCHAs for form verification.</p>
                
                <div>
                    <%@ Register Src="~/Controls/CaptchaControl.ascx" TagPrefix="uc" TagName="Captcha" %>
                    <uc:Captcha runat="server" ID="captchaControl" />
                </div>
                
                <asp:Button ID="btnVerifyCaptcha" runat="server" Text="Verify CAPTCHA" CssClass="btn" 
                    OnClick="btnVerifyCaptcha_Click" />
                
                <div class="result">
                    <strong>Result:</strong> <asp:Label ID="lblCaptchaResult" runat="server"></asp:Label>
                </div>
            </div>

            <div class="panel">
                <h3>Temperature Converter Service</h3>
                <p>This service converts between Fahrenheit and Celsius.</p>
                
                <div class="form-group">
                    <label for="txtTemperature">Temperature:</label>
                    <asp:TextBox ID="txtTemperature" runat="server" TextMode="Number" Step="0.01"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTemperature" runat="server" ControlToValidate="txtTemperature"
                        ErrorMessage="Temperature is required" Display="Dynamic" ForeColor="Red" ValidationGroup="TempGroup">
                    </asp:RequiredFieldValidator>
                </div>
                
                <div class="form-group">
                    <asp:RadioButton ID="rbFtoC" runat="server" GroupName="TempConversion" Text="Fahrenheit to Celsius" Checked="true" />
                    <asp:RadioButton ID="rbCtoF" runat="server" GroupName="TempConversion" Text="Celsius to Fahrenheit" />
                </div>
                
                <asp:Button ID="btnConvertTemp" runat="server" Text="Convert" CssClass="btn" 
                    OnClick="btnConvertTemp_Click" ValidationGroup="TempGroup" />
                
                <div class="result">
                    <strong>Result:</strong> <asp:Label ID="lblTempResult" runat="server"></asp:Label>
                </div>
            </div>

            <div class="panel">
                <h3>Currency Converter Service</h3>
                <p>This service converts between different currencies.</p>
                
                <div class="form-group">
                    <label for="txtAmount">Amount:</label>
                    <asp:TextBox ID="txtAmount" runat="server" TextMode="Number" Step="0.01"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount"
                        ErrorMessage="Amount is required" Display="Dynamic" ForeColor="Red" ValidationGroup="CurrencyGroup">
                    </asp:RequiredFieldValidator>
                </div>
                
                <div class="form-group">
                    <label for="ddlFromCurrency">From Currency:</label>
                    <asp:DropDownList ID="ddlFromCurrency" runat="server">
                        <asp:ListItem Text="USD - US Dollar" Value="USD" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="EUR - Euro" Value="EUR"></asp:ListItem>
                        <asp:ListItem Text="GBP - British Pound" Value="GBP"></asp:ListItem>
                        <asp:ListItem Text="JPY - Japanese Yen" Value="JPY"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label for="ddlToCurrency">To Currency:</label>
                    <asp:DropDownList ID="ddlToCurrency" runat="server">
                        <asp:ListItem Text="USD - US Dollar" Value="USD"></asp:ListItem>
                        <asp:ListItem Text="EUR - Euro" Value="EUR" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="GBP - British Pound" Value="GBP"></asp:ListItem>
                        <asp:ListItem Text="JPY - Japanese Yen" Value="JPY"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <asp:Button ID="btnConvertCurrency" runat="server" Text="Convert" CssClass="btn" 
                    OnClick="btnConvertCurrency_Click" ValidationGroup="CurrencyGroup" />
                
                <div class="result">
                    <strong>Result:</strong> <asp:Label ID="lblCurrencyResult" runat="server"></asp:Label>
                </div>
            </div>

            <div class="panel">
                <h3>Application State</h3>
                <p>This component demonstrates the use of Application state for visitor counting.</p>
                
                <div class="result">
                    <strong>Current Visitor Count:</strong> <asp:Label ID="lblVisitorCount" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </form>
</body>
</html>