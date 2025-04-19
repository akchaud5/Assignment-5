<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Staff.aspx.cs" Inherits="Pages_Staff" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Staff Area - Simple Online Store</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 0; padding: 20px; }
        .container { max-width: 1000px; margin: 0 auto; }
        .header { background-color: #f5f5f5; padding: 10px; margin-bottom: 20px; }
        table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }
        th, td { padding: 10px; border: 1px solid #ddd; }
        th { background-color: #f5f5f5; }
        .btn { padding: 8px 12px; margin-right: 10px; background-color: #4CAF50; color: white; border: none; cursor: pointer; }
        .btn:hover { background-color: #45a049; }
        .panel { margin-bottom: 20px; padding: 15px; border: 1px solid #ddd; }
        .form-group { margin-bottom: 15px; }
        .form-group label { display: block; margin-bottom: 5px; }
        .form-group input, .form-group select { width: 100%; padding: 8px; box-sizing: border-box; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <h1>Staff Management Area</h1>
                <asp:Button ID="btnHome" runat="server" Text="Back to Home" CssClass="btn" OnClick="btnHome_Click" />
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn" OnClick="btnLogout_Click" />
            </div>

            <h2>Welcome, <asp:Label ID="lblUsername" runat="server"></asp:Label>!</h2>

            <div class="panel">
                <h3>Product Management</h3>
                <div class="form-group">
                    <label for="txtProductName">Product Name:</label>
                    <asp:TextBox ID="txtProductName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ControlToValidate="txtProductName"
                        ErrorMessage="Product name is required" Display="Dynamic" ForeColor="Red" ValidationGroup="ProductGroup">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <label for="txtProductPrice">Price:</label>
                    <asp:TextBox ID="txtProductPrice" runat="server" TextMode="Number" Step="0.01"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvProductPrice" runat="server" ControlToValidate="txtProductPrice"
                        ErrorMessage="Price is required" Display="Dynamic" ForeColor="Red" ValidationGroup="ProductGroup">
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvProductPrice" runat="server" ControlToValidate="txtProductPrice"
                        ErrorMessage="Price must be greater than 0" MinimumValue="0.01" MaximumValue="10000"
                        Type="Double" Display="Dynamic" ForeColor="Red" ValidationGroup="ProductGroup">
                    </asp:RangeValidator>
                </div>
                <div class="form-group">
                    <label for="txtProductCategory">Category:</label>
                    <asp:TextBox ID="txtProductCategory" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvProductCategory" runat="server" ControlToValidate="txtProductCategory"
                        ErrorMessage="Category is required" Display="Dynamic" ForeColor="Red" ValidationGroup="ProductGroup">
                    </asp:RequiredFieldValidator>
                </div>
                <asp:Button ID="btnAddProduct" runat="server" Text="Add Product" CssClass="btn" OnClick="btnAddProduct_Click" ValidationGroup="ProductGroup" />
                <asp:Label ID="lblProductMessage" runat="server" ForeColor="Red"></asp:Label>
            </div>

            <h3>Current Products</h3>
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

            <div class="panel">
                <h3>Member Management</h3>
                <asp:GridView ID="gvMembers" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="Username" HeaderText="Username" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="RegistrationDate" HeaderText="Registration Date" />
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
            </div>

            <div class="panel">
                <h3>Add Staff Member</h3>
                <div class="form-group">
                    <label for="txtStaffUsername">Username:</label>
                    <asp:TextBox ID="txtStaffUsername" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvStaffUsername" runat="server" ControlToValidate="txtStaffUsername"
                        ErrorMessage="Username is required" Display="Dynamic" ForeColor="Red" ValidationGroup="StaffGroup">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <label for="txtStaffPassword">Password:</label>
                    <asp:TextBox ID="txtStaffPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvStaffPassword" runat="server" ControlToValidate="txtStaffPassword"
                        ErrorMessage="Password is required" Display="Dynamic" ForeColor="Red" ValidationGroup="StaffGroup">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revStaffPassword" runat="server" ControlToValidate="txtStaffPassword"
                        ErrorMessage="Password must be at least 8 characters with uppercase, lowercase, digit, and special character"
                        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$"
                        Display="Dynamic" ForeColor="Red" ValidationGroup="StaffGroup">
                    </asp:RegularExpressionValidator>
                </div>
                <asp:Button ID="btnAddStaff" runat="server" Text="Add Staff" CssClass="btn" OnClick="btnAddStaff_Click" ValidationGroup="StaffGroup" />
                <asp:Label ID="lblStaffMessage" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>