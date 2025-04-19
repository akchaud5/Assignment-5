using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.IO;
using SecurityLib;

public partial class Pages_Staff : Page
{
    private const string ProductsXmlPath = "~/App_Data/Products.xml";
    private const string MembersXmlPath = "~/App_Data/Members.xml";
    private const string StaffXmlPath = "~/App_Data/Staff.xml";

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
        if (roleCookie == null || roleCookie.Value != "Staff")
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Pages/Login.aspx?ReturnUrl=" + Request.Url.PathAndQuery);
            return;
        }

        if (!IsPostBack)
        {
            // Display username
            lblUsername.Text = User.Identity.Name;

            // Load products and members
            LoadProducts();
            LoadMembers();
        }
    }

    private void LoadProducts()
    {
        string physicalPath = Server.MapPath(ProductsXmlPath);

        if (File.Exists(physicalPath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(physicalPath);

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Category", typeof(string));

            XmlNodeList products = doc.SelectNodes("//Product");
            foreach (XmlNode product in products)
            {
                dt.Rows.Add(
                    Convert.ToInt32(product.SelectSingleNode("Id").InnerText),
                    product.SelectSingleNode("Name").InnerText,
                    Convert.ToDecimal(product.SelectSingleNode("Price").InnerText),
                    product.SelectSingleNode("Category").InnerText
                );
            }

            gvProducts.DataSource = dt;
            gvProducts.DataBind();
        }
        else
        {
            // Create a DataTable with default products
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

            // Save to XML file
            SaveProductsToXml(dt);

            // Bind to GridView
            gvProducts.DataSource = dt;
            gvProducts.DataBind();
        }
    }

    private void LoadMembers()
    {
        string physicalPath = Server.MapPath(MembersXmlPath);

        if (File.Exists(physicalPath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(physicalPath);

            DataTable dt = new DataTable();
            dt.Columns.Add("Username", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("RegistrationDate", typeof(string));

            XmlNodeList members = doc.SelectNodes("//Member");
            foreach (XmlNode member in members)
            {
                dt.Rows.Add(
                    member.SelectSingleNode("Username").InnerText,
                    member.SelectSingleNode("Email").InnerText,
                    member.SelectSingleNode("RegistrationDate").InnerText
                );
            }

            gvMembers.DataSource = dt;
            gvMembers.DataBind();
        }
    }

    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        string name = txtProductName.Text.Trim();
        decimal price = Convert.ToDecimal(txtProductPrice.Text);
        string category = txtProductCategory.Text.Trim();

        string physicalPath = Server.MapPath(ProductsXmlPath);

        if (!File.Exists(physicalPath))
        {
            CreateProductsXmlFile(physicalPath);
        }

        XmlDocument doc = new XmlDocument();
        doc.Load(physicalPath);

        // Find the highest product ID
        int highestId = 0;
        XmlNodeList products = doc.SelectNodes("//Product");
        foreach (XmlNode product in products)
        {
            int id = Convert.ToInt32(product.SelectSingleNode("Id").InnerText);
            if (id > highestId)
            {
                highestId = id;
            }
        }

        // Create a new product node
        XmlElement newProduct = doc.CreateElement("Product");

        XmlElement idElement = doc.CreateElement("Id");
        idElement.InnerText = (highestId + 1).ToString();
        newProduct.AppendChild(idElement);

        XmlElement nameElement = doc.CreateElement("Name");
        nameElement.InnerText = name;
        newProduct.AppendChild(nameElement);

        XmlElement priceElement = doc.CreateElement("Price");
        priceElement.InnerText = price.ToString();
        newProduct.AppendChild(priceElement);

        XmlElement categoryElement = doc.CreateElement("Category");
        categoryElement.InnerText = category;
        newProduct.AppendChild(categoryElement);

        doc.DocumentElement.AppendChild(newProduct);
        doc.Save(physicalPath);

        // Clear form
        txtProductName.Text = "";
        txtProductPrice.Text = "";
        txtProductCategory.Text = "";

        // Show message
        lblProductMessage.Text = "Product added successfully!";

        // Reload products
        LoadProducts();
    }

    protected void btnAddStaff_Click(object sender, EventArgs e)
    {
        string username = txtStaffUsername.Text.Trim();
        string password = txtStaffPassword.Text;

        string physicalPath = Server.MapPath(StaffXmlPath);

        if (!File.Exists(physicalPath))
        {
            CreateStaffXmlFile(physicalPath);
        }

        // Hash password using DLL function
        string hashedPassword = SecurityLib.PasswordHasher.HashPassword(password);

        XmlDocument doc = new XmlDocument();
        doc.Load(physicalPath);

        // Check if username already exists
        XmlNode existingStaff = doc.SelectSingleNode($"//Staff[Username='{username}']");
        if (existingStaff != null)
        {
            lblStaffMessage.Text = "Username already exists. Please choose another.";
            return;
        }

        // Create a new staff node
        XmlElement newStaff = doc.CreateElement("Staff");

        XmlElement usernameElement = doc.CreateElement("Username");
        usernameElement.InnerText = username;
        newStaff.AppendChild(usernameElement);

        XmlElement passwordElement = doc.CreateElement("Password");
        passwordElement.InnerText = hashedPassword;
        newStaff.AppendChild(passwordElement);

        XmlElement dateElement = doc.CreateElement("CreationDate");
        dateElement.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        newStaff.AppendChild(dateElement);

        doc.DocumentElement.AppendChild(newStaff);
        doc.Save(physicalPath);

        // Clear form
        txtStaffUsername.Text = "";
        txtStaffPassword.Text = "";

        // Show message
        lblStaffMessage.Text = "Staff member added successfully!";
    }

    private void SaveProductsToXml(DataTable dt)
    {
        string physicalPath = Server.MapPath(ProductsXmlPath);
        CreateProductsXmlFile(physicalPath);

        XmlDocument doc = new XmlDocument();
        doc.Load(physicalPath);

        foreach (DataRow row in dt.Rows)
        {
            XmlElement product = doc.CreateElement("Product");

            XmlElement id = doc.CreateElement("Id");
            id.InnerText = row["Id"].ToString();
            product.AppendChild(id);

            XmlElement name = doc.CreateElement("Name");
            name.InnerText = row["Name"].ToString();
            product.AppendChild(name);

            XmlElement price = doc.CreateElement("Price");
            price.InnerText = row["Price"].ToString();
            product.AppendChild(price);

            XmlElement category = doc.CreateElement("Category");
            category.InnerText = row["Category"].ToString();
            product.AppendChild(category);

            doc.DocumentElement.AppendChild(product);
        }

        doc.Save(physicalPath);
    }

    private void CreateProductsXmlFile(string filePath)
    {
        XmlDocument doc = new XmlDocument();
        XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.AppendChild(xmlDeclaration);

        XmlElement root = doc.CreateElement("Products");
        doc.AppendChild(root);

        doc.Save(filePath);
    }

    private void CreateStaffXmlFile(string filePath)
    {
        XmlDocument doc = new XmlDocument();
        XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.AppendChild(xmlDeclaration);

        XmlElement root = doc.CreateElement("StaffMembers");
        doc.AppendChild(root);

        // Add TA user for testing
        XmlElement taStaff = doc.CreateElement("Staff");

        XmlElement taUsername = doc.CreateElement("Username");
        taUsername.InnerText = "TA";
        taStaff.AppendChild(taUsername);

        XmlElement taPassword = doc.CreateElement("Password");
        taPassword.InnerText = SecurityLib.PasswordHasher.HashPassword("Cse445!");
        taStaff.AppendChild(taPassword);

        XmlElement taDate = doc.CreateElement("CreationDate");
        taDate.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        taStaff.AppendChild(taDate);

        root.AppendChild(taStaff);

        doc.Save(filePath);
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
}