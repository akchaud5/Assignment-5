using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml;
using System.IO;
using SecurityLib;

namespace OnlineStoreApp.Pages
{
    public partial class Pages_Login : Page
    {
        private const string MembersXmlPath = "~/App_Data/Members.xml";
        private const string StaffXmlPath = "~/App_Data/Staff.xml";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if return URL is provided
            if (!IsPostBack && Request.QueryString["ReturnUrl"] != null)
            {
                // Store return URL in ViewState
                ViewState["ReturnUrl"] = Request.QueryString["ReturnUrl"];

                // Check if return URL is for Staff page
                if (Request.QueryString["ReturnUrl"].ToString().Contains("Staff.aspx"))
                {
                    // Hide register tab for staff login
                    ClientScript.RegisterStartupScript(this.GetType(), "HideRegister", 
                        "document.getElementById('registerTab').style.display = 'none';", true);
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtLoginUsername.Text.Trim();
            string password = txtLoginPassword.Text;

            // Check if return URL is for Staff page
            if (ViewState["ReturnUrl"] != null && ViewState["ReturnUrl"].ToString().Contains("Staff.aspx"))
            {
                // Authenticate as staff
                if (AuthenticateStaff(username, password))
                {
                    // Create authentication ticket
                    FormsAuthentication.SetAuthCookie(username, false);

                    // Create a role cookie
                    HttpCookie roleCookie = new HttpCookie("UserRole", "Staff");
                    Response.Cookies.Add(roleCookie);

                    // Redirect to return URL
                    Response.Redirect(ViewState["ReturnUrl"].ToString());
                }
                else
                {
                    lblMessage.Text = "Invalid staff credentials.";
                }
            }
            else
            {
                // Authenticate as member
                if (AuthenticateMember(username, password))
                {
                    // Create authentication ticket
                    FormsAuthentication.SetAuthCookie(username, false);

                    // Create a role cookie
                    HttpCookie roleCookie = new HttpCookie("UserRole", "Member");
                    Response.Cookies.Add(roleCookie);

                    // Redirect to return URL or default page
                    if (ViewState["ReturnUrl"] != null)
                    {
                        Response.Redirect(ViewState["ReturnUrl"].ToString());
                    }
                    else
                    {
                        Response.Redirect("~/Pages/Member.aspx");
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid member credentials.";
                }
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // Verify captcha
            if (!registerCaptcha.IsValid)
            {
                lblMessage.Text = "Captcha verification failed. Please try again.";
                return;
            }

            string username = txtRegisterUsername.Text.Trim();
            string password = txtRegisterPassword.Text;
            string email = txtEmail.Text.Trim();

            // Check if username already exists
            if (UsernameExists(username))
            {
                lblMessage.Text = "Username already exists. Please choose another.";
                return;
            }

            // Create member XML file if it doesn't exist
            string physicalPath = Server.MapPath(MembersXmlPath);
            if (!File.Exists(physicalPath))
            {
                CreateMembersXmlFile(physicalPath);
            }

            // Hash password using DLL function
            string hashedPassword = SecurityLib.PasswordHasher.HashPassword(password);

            // Add new member to XML file
            XmlDocument doc = new XmlDocument();
            doc.Load(physicalPath);

            XmlElement newMember = doc.CreateElement("Member");

            XmlElement usernameElement = doc.CreateElement("Username");
            usernameElement.InnerText = username;
            newMember.AppendChild(usernameElement);

            XmlElement passwordElement = doc.CreateElement("Password");
            passwordElement.InnerText = hashedPassword;
            newMember.AppendChild(passwordElement);

            XmlElement emailElement = doc.CreateElement("Email");
            emailElement.InnerText = email;
            newMember.AppendChild(emailElement);

            XmlElement dateElement = doc.CreateElement("RegistrationDate");
            dateElement.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            newMember.AppendChild(dateElement);

            doc.DocumentElement.AppendChild(newMember);
            doc.Save(physicalPath);

            // Create authentication ticket
            FormsAuthentication.SetAuthCookie(username, false);

            // Create a role cookie
            HttpCookie roleCookie = new HttpCookie("UserRole", "Member");
            Response.Cookies.Add(roleCookie);

            // Redirect to member page
            Response.Redirect("~/Pages/Member.aspx");
        }

        private bool AuthenticateMember(string username, string password)
        {
            string physicalPath = Server.MapPath(MembersXmlPath);

            // Check if members.xml file exists
            if (!File.Exists(physicalPath))
            {
                return false;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(physicalPath);

            XmlNodeList members = doc.SelectNodes("//Member");

            foreach (XmlNode member in members)
            {
                string storedUsername = member.SelectSingleNode("Username").InnerText;
                string storedHashedPassword = member.SelectSingleNode("Password").InnerText;

                if (storedUsername == username)
                {
                    // Hash input password and compare
                    string hashedPassword = SecurityLib.PasswordHasher.HashPassword(password);
                    if (storedHashedPassword == hashedPassword)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool AuthenticateStaff(string username, string password)
        {
            // Special case for TA testing
            if (username == "TA" && password == "Cse445!")
            {
                return true;
            }

            string physicalPath = Server.MapPath(StaffXmlPath);

            // Check if staff.xml file exists
            if (!File.Exists(physicalPath))
            {
                return false;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(physicalPath);

            XmlNodeList staffMembers = doc.SelectNodes("//Staff");

            foreach (XmlNode staff in staffMembers)
            {
                string storedUsername = staff.SelectSingleNode("Username").InnerText;
                string storedHashedPassword = staff.SelectSingleNode("Password").InnerText;

                if (storedUsername == username)
                {
                    // Hash input password and compare
                    string hashedPassword = SecurityLib.PasswordHasher.HashPassword(password);
                    if (storedHashedPassword == hashedPassword)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool UsernameExists(string username)
        {
            string physicalPath = Server.MapPath(MembersXmlPath);

            if (!File.Exists(physicalPath))
            {
                return false;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(physicalPath);

            XmlNode existingUser = doc.SelectSingleNode($"//Member[Username='{username}']");

            return existingUser != null;
        }

        private void CreateMembersXmlFile(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);

            XmlElement root = doc.CreateElement("Members");
            doc.AppendChild(root);

            doc.Save(filePath);
        }
    }
}