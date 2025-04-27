<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OnlineStoreApp.Default" enableViewStateMac="false" %>
<%@ Register Src="~/Controls/CaptchaControl.ascx" TagPrefix="uc" TagName="Captcha" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>Simple Online Store</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 0; padding: 20px; }
        .container { max-width: 1000px; margin: 0 auto; }
        .header { background-color: #f5f5f5; padding: 10px; margin-bottom: 20px; }
        table { width: 100%; border-collapse: collapse; }
        th, td { padding: 10px; border: 1px solid #ddd; }
        th { background-color: #f5f5f5; }
        .btn { padding: 8px 12px; margin-right: 10px; background-color: #4CAF50; color: white; border: none; cursor: pointer; }
        .btn:hover { background-color: #45a049; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <h1>Simple Online Store</h1>
                <asp:Button ID="btnMember" runat="server" Text="Member Area" CssClass="btn" OnClick="btnMember_Click" />
                <asp:Button ID="btnStaff" runat="server" Text="Staff Area" CssClass="btn" OnClick="btnStaff_Click" />
            </div>

            <h2>Welcome to our Online Store!</h2>
            <p>This application demonstrates a simple e-commerce platform with the following features:</p>
            <ul>
                <li>Browse products</li>
                <li>Member registration with secure authentication</li>
                <li>Shopping cart functionality</li>
                <li>Staff management interface</li>
            </ul>

            <h3>Test Instructions</h3>
            <p>To test this application, follow these steps:</p>
            <ol>
                <li>Browse the public products below</li>
                <li>Click "Member Area" to register or login</li>
                <li>After login, you can add products to your cart</li>
                <li>For staff access, use username: "TA" and password: "Cse445!"</li>
            </ol>

            <h2>Product Catalog</h2>
            <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="ID" />
                    <asp:BoundField DataField="Name" HeaderText="Product Name" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Category" HeaderText="Category" />
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>

            <h2>Application and Components Summary Table</h2>
            <p>Percentage of overall contribution: Your Name: 100%</p>
            <table>
                <tr>
                    <th>Provider</th>
                    <th>Component Type</th>
                    <th>Operation Name</th>
                    <th>Parameters</th>
                    <th>Return Type</th>
                    <th>Description</th>
                    <th>Test</th>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>WSDL Service</td>
                    <td>CalculateDiscount</td>
                    <td>price (decimal), quantity (int)</td>
                    <td>decimal</td>
                    <td>Calculates discount based on quantity</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>WSDL Service</td>
                    <td>ZipcodeVerifierService</td>
                    <td>zipcode (string)</td>
                    <td>bool, string</td>
                    <td>Verifies zipcode format and identifies state region</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>WSDL Service</td>
                    <td>TaxCalculatorService</td>
                    <td>price (decimal), stateCode (string)</td>
                    <td>decimal</td>
                    <td>Calculates sales tax at rate of 0.07 or by state</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>WSDL Service</td>
                    <td>AgeVerificationService</td>
                    <td>age (int), birthdate (DateTime)</td>
                    <td>bool, string, int</td>
                    <td>Verifies if a user is an adult (18+)</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>WSDL Service</td>
                    <td>LastViewedProductService</td>
                    <td>username (string), productId (int), count (int)</td>
                    <td>ProductInfo, List</td>
                    <td>Tracks and retrieves recently viewed products</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>DLL Function</td>
                    <td>HashPassword</td>
                    <td>password (string)</td>
                    <td>string</td>
                    <td>Securely hashes passwords</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>User Control</td>
                    <td>Captcha</td>
                    <td>N/A</td>
                    <td>N/A</td>
                    <td>Validates human users</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>Global.asax</td>
                    <td>Session_Start</td>
                    <td>N/A</td>
                    <td>N/A</td>
                    <td>Visitor counter implementation</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>Cookie</td>
                    <td>UserRole</td>
                    <td>N/A</td>
                    <td>N/A</td>
                    <td>Stores user role for authentication</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>XML Storage</td>
                    <td>Products.xml</td>
                    <td>N/A</td>
                    <td>N/A</td>
                    <td>Stores product information</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>XML Storage</td>
                    <td>Members.xml</td>
                    <td>N/A</td>
                    <td>N/A</td>
                    <td>Stores member accounts</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
                <tr>
                    <td>Your Name</td>
                    <td>XML Storage</td>
                    <td>Staff.xml</td>
                    <td>N/A</td>
                    <td>N/A</td>
                    <td>Stores staff accounts</td>
                    <td><a href="TryIt.aspx" class="btn">Try It</a></td>
                </tr>
            </table>

            <!-- Test components moved to TryIt.aspx page -->

            <p>Visitor count: <asp:Label ID="lblVisitorCount" runat="server"></asp:Label></p>
        </div>
    </form>
</body>
</html>