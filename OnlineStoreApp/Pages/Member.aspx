<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Member.aspx.cs" Inherits="OnlineStoreApp.Pages.Pages_Member" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Member Area - Simple Online Store</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 0; padding: 20px; }
        .container { max-width: 1000px; margin: 0 auto; }
        .header { background-color: #f5f5f5; padding: 10px; margin-bottom: 20px; }
        .cart-item { border: 1px solid #ddd; padding: 10px; margin-bottom: 10px; }
        .product-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(200px, 1fr)); gap: 20px; }
        .product-card { border: 1px solid #ddd; padding: 15px; border-radius: 5px; }
        .btn { padding: 8px 12px; margin-right: 10px; background-color: #4CAF50; color: white; border: none; cursor: pointer; }
        .btn:hover { background-color: #45a049; }
        .profile-section { margin-bottom: 30px; padding: 20px; background-color: #f9f9f9; border-radius: 5px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <h1>Member Area</h1>
                <asp:Button ID="btnHome" runat="server" Text="Back to Home" CssClass="btn" OnClick="btnHome_Click" />
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn" OnClick="btnLogout_Click" />
            </div>

            <div class="profile-section">
                <h2>Welcome, <asp:Label ID="lblUsername" runat="server"></asp:Label>!</h2>
                <p>Last login: <asp:Label ID="lblLastLogin" runat="server"></asp:Label></p>
            </div>

            <h2>Shopping Cart</h2>
            <asp:Panel ID="pnlEmptyCart" runat="server" Visible="true">
                <p>Your cart is empty.</p>
            </asp:Panel>

            <asp:Panel ID="pnlCart" runat="server" Visible="false">
                <asp:Repeater ID="rptCart" runat="server" OnItemCommand="rptCart_ItemCommand">
                    <ItemTemplate>
                        <div class="cart-item">
                            <h3><%# Eval("Name") %></h3>
                            <p>Price: $<%# Eval("Price", "{0:0.00}") %></p>
                            <p>Quantity: <%# Eval("Quantity") %></p>
                            <p>Total: $<%# String.Format("{0:0.00}", Convert.ToDecimal(Eval("Price")) * Convert.ToInt32(Eval("Quantity"))) %></p>
                            <asp:Button ID="btnRemove" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%# Eval("Id") %>' CssClass="btn" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <p>Total: $<asp:Label ID="lblTotal" runat="server"></asp:Label></p>
                <asp:Button ID="btnCheckout" runat="server" Text="Checkout" CssClass="btn" OnClick="btnCheckout_Click" />
            </asp:Panel>

            <h2>Products</h2>
            <div class="product-grid">
                <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
                    <ItemTemplate>
                        <div class="product-card">
                            <h3><%# Eval("Name") %></h3>
                            <p>Price: $<%# Eval("Price", "{0:0.00}") %></p>
                            <p>Category: <%# Eval("Category") %></p>
                            <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CommandName="AddToCart" CommandArgument='<%# Eval("Id") %>' CssClass="btn" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div id="serviceTest" runat="server">
                <h2>Test Discount Service</h2>
                <p>This service calculates discounts based on order total and quantity.</p>
                <p>Price: <asp:TextBox ID="txtPrice" runat="server" TextMode="Number" Step="0.01"></asp:TextBox></p>
                <p>Quantity: <asp:TextBox ID="txtQuantity" runat="server" TextMode="Number"></asp:TextBox></p>
                <asp:Button ID="btnCalculate" runat="server" Text="Calculate Discount" OnClick="btnCalculate_Click" CssClass="btn" />
                <p>Discount Amount: $<asp:Label ID="lblDiscount" runat="server"></asp:Label></p>
            </div>
        </div>
    </form>
</body>
</html>