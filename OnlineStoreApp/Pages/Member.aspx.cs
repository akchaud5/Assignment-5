using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using OnlineStoreApp.Services;

namespace OnlineStoreApp.Pages
{
    public partial class Pages_Member : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to login page
                Response.Redirect("~/Pages/Login.aspx?ReturnUrl=" + Request.Url.PathAndQuery);
                return;
            }

            // Check user role from cookie
            HttpCookie roleCookie = Request.Cookies["UserRole"];
            if (roleCookie == null || roleCookie.Value != "Member")
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/Pages/Login.aspx?ReturnUrl=" + Request.Url.PathAndQuery);
                return;
            }

            if (!IsPostBack)
            {
                // Display username
                lblUsername.Text = User.Identity.Name;

                // Display last login time from session
                if (Session["LastVisit"] != null)
                {
                    lblLastLogin.Text = Session["LastVisit"].ToString();
                }
                else
                {
                    lblLastLogin.Text = "First visit";
                }

                // Update last login time
                Session["LastVisit"] = DateTime.Now.ToString();

                // Initialize shopping cart if not exists
                if (Session["Cart"] == null)
                {
                    Session["Cart"] = new List<CartItem>();
                }

                // Load products and cart
                LoadProducts();
                LoadCart();
            }
        }

        private void LoadProducts()
        {
            // Create a DataTable to hold products
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Category", typeof(string));

            // Add sample products
            dt.Rows.Add(1, "Laptop", 999.99, "Electronics");
            dt.Rows.Add(2, "Smartphone", 699.99, "Electronics");
            dt.Rows.Add(3, "Headphones", 149.99, "Accessories");
            dt.Rows.Add(4, "Desk Chair", 249.99, "Furniture");
            dt.Rows.Add(5, "Coffee Maker", 79.99, "Appliances");

            // Bind to Repeater
            rptProducts.DataSource = dt;
            rptProducts.DataBind();
        }

        private void LoadCart()
        {
            List<CartItem> cart = (List<CartItem>)Session["Cart"];

            if (cart.Count > 0)
            {
                rptCart.DataSource = cart;
                rptCart.DataBind();

                decimal total = 0;
                foreach (CartItem item in cart)
                {
                    total += item.Price * item.Quantity;
                }

                lblTotal.Text = total.ToString("0.00");

                pnlEmptyCart.Visible = false;
                pnlCart.Visible = true;
            }
            else
            {
                pnlEmptyCart.Visible = true;
                pnlCart.Visible = false;
            }
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                int productId = Convert.ToInt32(e.CommandArgument);

                // Get product details
                string productName = "";
                decimal productPrice = 0;
                string productCategory = "";

                // Find the product in the repeater
                foreach (RepeaterItem item in rptProducts.Items)
                {
                    Button btn = (Button)item.FindControl("btnAddToCart");
                    if (btn != null && btn.CommandArgument == productId.ToString())
                    {
                        // Extract product details
                        productName = ((System.Web.UI.HtmlControls.HtmlGenericControl)item.FindControl("Name")).InnerText;
                        productPrice = Convert.ToDecimal(((System.Web.UI.HtmlControls.HtmlGenericControl)item.FindControl("Price")).InnerText.Replace("$", ""));
                        productCategory = ((System.Web.UI.HtmlControls.HtmlGenericControl)item.FindControl("Category")).InnerText;
                        break;
                    }
                }

                // Get the cart from session
                List<CartItem> cart = (List<CartItem>)Session["Cart"];

                // Check if product already in cart
                CartItem existingItem = cart.Find(item => item.Id == productId);
                if (existingItem != null)
                {
                    // Update quantity
                    existingItem.Quantity++;
                }
                else
                {
                    // Add new item to cart
                    cart.Add(new CartItem
                    {
                        Id = productId,
                        Name = "Product " + productId, // Fallback if name not found
                        Price = GetProductPrice(productId), // Get price from product list
                        Quantity = 1
                    });
                }

                // Update session
                Session["Cart"] = cart;

                // Reload cart
                LoadCart();
            }
        }

        protected void rptCart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int productId = Convert.ToInt32(e.CommandArgument);

                // Get the cart from session
                List<CartItem> cart = (List<CartItem>)Session["Cart"];

                // Remove item from cart
                cart.RemoveAll(item => item.Id == productId);

                // Update session
                Session["Cart"] = cart;

                // Reload cart
                LoadCart();
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            // Implement checkout functionality
            // For now, just clear the cart
            Session["Cart"] = new List<CartItem>();
            LoadCart();

            // Display confirmation message
            Response.Write("<script>alert('Thank you for your order!');</script>");
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Sign out
            FormsAuthentication.SignOut();

            // Clear cookies
            if (Request.Cookies["UserRole"] != null)
            {
                HttpCookie roleCookie = new HttpCookie("UserRole");
                roleCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(roleCookie);
            }

            // Redirect to home page
            Response.Redirect("~/Default.aspx");
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input values
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int quantity = Convert.ToInt32(txtQuantity.Text);

                // Use local calculation instead of the service to avoid type conflicts
                decimal discount = CalculateDiscount(price, quantity);

                // Display result
                lblDiscount.Text = discount.ToString("0.00");
            }
            catch (Exception ex)
            {
                lblDiscount.Text = "Error: " + ex.Message;
            }
        }

        private decimal GetProductPrice(int productId)
        {
            // Hardcoded prices for demo
            switch (productId)
            {
                case 1: return 999.99m; // Laptop
                case 2: return 699.99m; // Smartphone
                case 3: return 149.99m; // Headphones
                case 4: return 249.99m; // Desk Chair
                case 5: return 79.99m; // Coffee Maker
                default: return 0m;
            }
        }
        
        // Local implementation of discount calculation to avoid type conflicts
        private decimal CalculateDiscount(decimal price, int quantity)
        {
            // Basic discount calculation logic
            decimal discount = 0;

            // Quantity-based discount
            if (quantity >= 10)
            {
                discount += price * quantity * 0.15m;  // 15% discount for 10+ items
            }
            else if (quantity >= 5)
            {
                discount += price * quantity * 0.10m;  // 10% discount for 5-9 items
            }
            else if (quantity >= 3)
            {
                discount += price * quantity * 0.05m;  // 5% discount for 3-4 items
            }

            // Price-based discount
            if (price * quantity >= 1000)
            {
                discount += 50;  // Additional $50 off for orders over $1000
            }
            else if (price * quantity >= 500)
            {
                discount += 25;  // Additional $25 off for orders over $500
            }

            return discount;
        }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}