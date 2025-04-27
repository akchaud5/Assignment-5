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
                <h3>Zipcode Verifier Service</h3>
                <p>This service verifies zipcode format and identifies the state region.</p>
                
                <div class="form-group">
                    <label for="txtZipcode">Zipcode:</label>
                    <asp:TextBox ID="txtZipcode" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvZipcode" runat="server" ControlToValidate="txtZipcode"
                        ErrorMessage="Zipcode is required" Display="Dynamic" ForeColor="Red" ValidationGroup="ZipcodeGroup">
                    </asp:RequiredFieldValidator>
                </div>
                
                <div class="form-group">
                    <asp:RadioButton ID="rbVerify" runat="server" GroupName="ZipcodeOperation" Text="Verify Format" Checked="true" />
                    <asp:RadioButton ID="rbGetState" runat="server" GroupName="ZipcodeOperation" Text="Get State Region" />
                </div>
                
                <asp:Button ID="btnVerifyZipcode" runat="server" Text="Process" CssClass="btn" 
                    OnClick="btnVerifyZipcode_Click" ValidationGroup="ZipcodeGroup" />
                
                <div class="result">
                    <strong>Result:</strong> <asp:Label ID="lblZipcodeResult" runat="server"></asp:Label>
                </div>
            </div>

            <div class="panel">
                <h3>Tax Calculator Service</h3>
                <p>This service calculates sales tax at a default rate of 7% or by state.</p>
                
                <div class="form-group">
                    <label for="txtPrice">Price:</label>
                    <asp:TextBox ID="txtTaxPrice" runat="server" TextMode="Number" Step="0.01"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTaxPrice" runat="server" ControlToValidate="txtTaxPrice"
                        ErrorMessage="Price is required" Display="Dynamic" ForeColor="Red" ValidationGroup="TaxGroup">
                    </asp:RequiredFieldValidator>
                </div>
                
                <div class="form-group">
                    <label for="ddlState">State (optional):</label>
                    <asp:DropDownList ID="ddlState" runat="server">
                        <asp:ListItem Text="Default (7%)" Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Alabama (4%)" Value="AL"></asp:ListItem>
                        <asp:ListItem Text="Alaska (0%)" Value="AK"></asp:ListItem>
                        <asp:ListItem Text="Arizona (5.6%)" Value="AZ"></asp:ListItem>
                        <asp:ListItem Text="California (7.25%)" Value="CA"></asp:ListItem>
                        <asp:ListItem Text="New York (4%)" Value="NY"></asp:ListItem>
                        <asp:ListItem Text="Texas (6.25%)" Value="TX"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <asp:RadioButton ID="rbTaxOnly" runat="server" GroupName="TaxOperation" Text="Calculate Tax Only" Checked="true" />
                    <asp:RadioButton ID="rbTotalWithTax" runat="server" GroupName="TaxOperation" Text="Calculate Total with Tax" />
                </div>
                
                <asp:Button ID="btnCalculateTax" runat="server" Text="Calculate" CssClass="btn" 
                    OnClick="btnCalculateTax_Click" ValidationGroup="TaxGroup" />
                
                <div class="result">
                    <strong>Result:</strong> <asp:Label ID="lblTaxResult" runat="server"></asp:Label>
                </div>
            </div>

            <div class="panel">
                <h3>Age Verification Service</h3>
                <p>This service verifies if a user is an adult (18 years or older).</p>
                
                <div class="form-group">
                    <label for="txtAge">Age:</label>
                    <asp:TextBox ID="txtAge" runat="server" TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAge" runat="server" ControlToValidate="txtAge"
                        ErrorMessage="Age is required" Display="Dynamic" ForeColor="Red" ValidationGroup="AgeGroup">
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvAge" runat="server" ControlToValidate="txtAge"
                        ErrorMessage="Age must be between 0 and 120" MinimumValue="0" MaximumValue="120"
                        Type="Integer" Display="Dynamic" ForeColor="Red" ValidationGroup="AgeGroup">
                    </asp:RangeValidator>
                </div>
                
                <div class="form-group">
                    <asp:RadioButton ID="rbVerifyAdult" runat="server" GroupName="AgeOperation" Text="Verify Adult Status" Checked="true" />
                    <asp:RadioButton ID="rbYearsUntil" runat="server" GroupName="AgeOperation" Text="Years Until Adult" />
                </div>
                
                <asp:Button ID="btnVerifyAge" runat="server" Text="Verify" CssClass="btn" 
                    OnClick="btnVerifyAge_Click" ValidationGroup="AgeGroup" />
                
                <div class="result">
                    <strong>Result:</strong> <asp:Label ID="lblAgeResult" runat="server"></asp:Label>
                </div>
            </div>
            
            <div class="panel">
                <h3>Last Viewed Product Service</h3>
                <p>This service tracks and retrieves the last products viewed by a user.</p>
                
                <div class="form-group">
                    <label for="txtUsername">Username:</label>
                    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                        ErrorMessage="Username is required" Display="Dynamic" ForeColor="Red" ValidationGroup="ProductGroup">
                    </asp:RequiredFieldValidator>
                </div>
                
                <div class="form-group">
                    <label for="ddlOperation">Operation:</label>
                    <asp:DropDownList ID="ddlOperation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOperation_SelectedIndexChanged">
                        <asp:ListItem Text="View Last Product" Value="last" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Record Product View" Value="record"></asp:ListItem>
                        <asp:ListItem Text="Get Recent Products" Value="recent"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <asp:Panel ID="pnlRecordProduct" runat="server" Visible="false">
                    <div class="form-group">
                        <label for="txtProductId">Product ID:</label>
                        <asp:TextBox ID="txtProductId" runat="server" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvProductId" runat="server" ControlToValidate="txtProductId"
                            ErrorMessage="Product ID is required" Display="Dynamic" ForeColor="Red" ValidationGroup="ProductGroup">
                        </asp:RequiredFieldValidator>
                    </div>
                    
                    <div class="form-group">
                        <label for="txtProductName">Product Name:</label>
                        <asp:TextBox ID="txtProductName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ControlToValidate="txtProductName"
                            ErrorMessage="Product Name is required" Display="Dynamic" ForeColor="Red" ValidationGroup="ProductGroup">
                        </asp:RequiredFieldValidator>
                    </div>
                </asp:Panel>
                
                <asp:Panel ID="pnlRecentProducts" runat="server" Visible="false">
                    <div class="form-group">
                        <label for="txtCount">Number of Products:</label>
                        <asp:TextBox ID="txtCount" runat="server" TextMode="Number" Text="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCount" runat="server" ControlToValidate="txtCount"
                            ErrorMessage="Count is required" Display="Dynamic" ForeColor="Red" ValidationGroup="ProductGroup">
                        </asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvCount" runat="server" ControlToValidate="txtCount"
                            ErrorMessage="Count must be between 1 and 10" MinimumValue="1" MaximumValue="10"
                            Type="Integer" Display="Dynamic" ForeColor="Red" ValidationGroup="ProductGroup">
                        </asp:RangeValidator>
                    </div>
                </asp:Panel>
                
                <asp:Button ID="btnProductOperation" runat="server" Text="Submit" CssClass="btn" 
                    OnClick="btnProductOperation_Click" ValidationGroup="ProductGroup" />
                
                <div class="result">
                    <strong>Result:</strong> <asp:Label ID="lblProductResult" runat="server"></asp:Label>
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