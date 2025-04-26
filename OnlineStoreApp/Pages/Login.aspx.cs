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

            // Use local hash implementation to avoid type conflicts
            string hashedPassword = HashPassword(password);

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
            try
            {
                string physicalPath = Server.MapPath(MembersXmlPath);

                // Check if members.xml file exists
                if (!File.Exists(physicalPath))
                {
                    // Create an empty members file if it doesn't exist
                    CreateMembersXmlFile(physicalPath);
                    return false;
                }

                try
                {
                    // Try to load the file with better error handling
                    string xmlContent = File.ReadAllText(physicalPath);
                    
                    // Remove any potential problematic comment tags
                    xmlContent = System.Text.RegularExpressions.Regex.Replace(
                        xmlContent, 
                        @"<\\!--.*?-->", 
                        string.Empty);
                    
                    using (System.IO.StringReader reader = new System.IO.StringReader(xmlContent))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(reader);

                        XmlNodeList members = doc.SelectNodes("//Member");

                        if (members != null)
                        {
                            foreach (XmlNode member in members)
                            {
                                XmlNode usernameNode = member.SelectSingleNode("Username");
                                XmlNode passwordNode = member.SelectSingleNode("Password");
                                
                                if (usernameNode != null && passwordNode != null)
                                {
                                    string storedUsername = usernameNode.InnerText;
                                    string storedHashedPassword = passwordNode.InnerText;

                                    if (storedUsername == username)
                                    {
                                        // Use the DLL library for hashing
                                        string hashedPassword = HashPassword(password);
                                        if (storedHashedPassword == hashedPassword)
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Diagnostics.Debug.WriteLine("Error authenticating member: " + ex.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log any other errors
                System.Diagnostics.Debug.WriteLine("General error in AuthenticateMember: " + ex.Message);
                return false;
            }

            return false;
        }

        private bool AuthenticateStaff(string username, string password)
        {
            // Special case for TA testing - always allow this hardcoded account
            if (username == "TA" && password == "Cse445!")
            {
                return true;
            }

            try
            {
                string physicalPath = Server.MapPath(StaffXmlPath);

                // Check if staff.xml file exists
                if (!File.Exists(physicalPath))
                {
                    // Create staff file with TA account if it doesn't exist
                    CreateStaffXmlFile(physicalPath);
                    
                    // Since we just created the file, we've only added the TA account
                    // If the authentication request was for TA, we already returned true above
                    // Otherwise, the account doesn't exist, return false
                    return false;
                }

                try 
                {
                    // Try to load the file with XDocument which handles comments better
                    string xmlContent = File.ReadAllText(physicalPath);
                    
                    // Remove malformed XML comment tags that could cause parsing issues
                    xmlContent = System.Text.RegularExpressions.Regex.Replace(
                        xmlContent, 
                        @"<\\!--.*?-->", 
                        string.Empty);
                        
                    using (System.IO.StringReader reader = new System.IO.StringReader(xmlContent))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(reader);

                        XmlNodeList staffMembers = doc.SelectNodes("//Staff");

                        if (staffMembers != null)
                        {
                            foreach (XmlNode staff in staffMembers)
                            {
                                XmlNode usernameNode = staff.SelectSingleNode("Username");
                                XmlNode passwordNode = staff.SelectSingleNode("Password");
                                
                                if (usernameNode != null && passwordNode != null)
                                {
                                    string storedUsername = usernameNode.InnerText;
                                    string storedHashedPassword = passwordNode.InnerText;

                                    if (storedUsername == username)
                                    {
                                        // Use the DLL library to hash the password
                                        string hashedPassword = HashPassword(password);
                                        if (storedHashedPassword == hashedPassword)
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Diagnostics.Debug.WriteLine("Error authenticating staff: " + ex.Message);
                    
                    // If there's an XML parsing error, fall back to just the TA credentials
                    // We already checked for TA above, so return false here
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log any other errors
                System.Diagnostics.Debug.WriteLine("General error in AuthenticateStaff: " + ex.Message);
                return false;
            }

            return false;
        }
        
        private void CreateStaffXmlFile(string filePath)
        {
            try
            {
                string staffXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<StaffMembers>
  <Staff>
    <Username>TA</Username>
    <Password>" + HashPassword("Cse445!") + @"</Password>
    <CreationDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"</CreationDate>
  </Staff>
</StaffMembers>";

                File.WriteAllText(filePath, staffXml);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error creating Staff.xml: " + ex.Message);
            }
        }

        private bool UsernameExists(string username)
        {
            try
            {
                string physicalPath = Server.MapPath(MembersXmlPath);

                if (!File.Exists(physicalPath))
                {
                    return false;
                }

                try
                {
                    // Read the file with better error handling
                    string xmlContent = File.ReadAllText(physicalPath);
                    
                    // Remove any problematic comment tags
                    xmlContent = System.Text.RegularExpressions.Regex.Replace(
                        xmlContent, 
                        @"<\\!--.*?-->", 
                        string.Empty);
                    
                    using (System.IO.StringReader reader = new System.IO.StringReader(xmlContent))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(reader);

                        // Use a safer approach to find the user node
                        XmlNodeList members = doc.SelectNodes("//Member");
                        
                        if (members != null)
                        {
                            foreach (XmlNode member in members)
                            {
                                XmlNode usernameNode = member.SelectSingleNode("Username");
                                if (usernameNode != null && usernameNode.InnerText == username)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Diagnostics.Debug.WriteLine("Error checking if username exists: " + ex.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log any other errors
                System.Diagnostics.Debug.WriteLine("General error in UsernameExists: " + ex.Message);
                return false;
            }

            return false;
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
    
        // Use DLL library for password hashing
        private string HashPassword(string password)
        {
            return PasswordHasher.HashPassword(password);
        }
    }
}