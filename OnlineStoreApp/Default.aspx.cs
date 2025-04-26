using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Xml;
using OnlineStoreApp.Services;
using SecurityLib;

namespace OnlineStoreApp
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Display visitor count from Application state
                lblVisitorCount.Text = Application["VisitorCount"].ToString();

                // Load products from XML
                LoadProducts();
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

            string productsXmlPath = Server.MapPath("~/App_Data/Products.xml");
            if (System.IO.File.Exists(productsXmlPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(productsXmlPath);

                XmlNodeList products = doc.SelectNodes("//Product");
                foreach (XmlNode product in products)
                {
                    // Try to get product name from either "Name" or "n" node
                    XmlNode nameNode = product.SelectSingleNode("Name") ?? product.SelectSingleNode("n");
                    string productName = nameNode != null ? nameNode.InnerText : "Unknown";
                    
                    dt.Rows.Add(
                        Convert.ToInt32(product.SelectSingleNode("Id").InnerText),
                        productName,
                        Convert.ToDecimal(product.SelectSingleNode("Price").InnerText),
                        product.SelectSingleNode("Category").InnerText
                    );
                }
            }
            else
            {
                // Fallback to sample products if XML doesn't exist
                dt.Rows.Add(1, "Laptop", 999.99, "Electronics");
                dt.Rows.Add(2, "Smartphone", 699.99, "Electronics");
                dt.Rows.Add(3, "Headphones", 149.99, "Accessories");
                dt.Rows.Add(4, "Desk Chair", 249.99, "Furniture");
                dt.Rows.Add(5, "Coffee Maker", 79.99, "Appliances");
            }

            // Bind to GridView
            gvProducts.DataSource = dt;
            gvProducts.DataBind();
        }

        protected void btnMember_Click(object sender, EventArgs e)
        {
            // Redirect to Member page
            Response.Redirect("~/Pages/Login.aspx?ReturnUrl=~/Pages/Member.aspx");
        }

        protected void btnStaff_Click(object sender, EventArgs e)
        {
            // Redirect to Staff page
            Response.Redirect("~/Pages/Login.aspx?ReturnUrl=~/Pages/Staff.aspx");
        }

        // Removed test methods as they're now handled by the TryIt.aspx page
    }
}