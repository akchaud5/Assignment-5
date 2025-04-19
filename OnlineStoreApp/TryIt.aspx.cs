using System;
using OnlineStoreApp.Services;
using SecurityLib;

namespace OnlineStoreApp
{
    public partial class TryIt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Display visitor count from Application state
                lblVisitorCount.Text = Application["VisitorCount"].ToString();
            }
        }

        protected void btnCalculateDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input values
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int quantity = Convert.ToInt32(txtQuantity.Text);

                // Call discount service
                OnlineStoreApp.Services.DiscountService discountService = new OnlineStoreApp.Services.DiscountService();
                decimal discount = discountService.CalculateDiscount(price, quantity);

                // Display result
                lblDiscountResult.Text = string.Format("${0:0.00}", discount);
            }
            catch (Exception ex)
            {
                lblDiscountResult.Text = "Error: " + ex.Message;
            }
        }

        protected void btnHashPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input value
                string password = txtPassword.Text;

                // Call hash function from DLL
                string hashedPassword = SecurityLib.PasswordHasher.HashPassword(password);

                // Display result (truncated for security)
                if (hashedPassword.Length > 20)
                {
                    lblHashResult.Text = hashedPassword.Substring(0, 20) + "...";
                }
                else
                {
                    lblHashResult.Text = hashedPassword;
                }
            }
            catch (Exception ex)
            {
                lblHashResult.Text = "Error: " + ex.Message;
            }
        }

        protected void btnVerifyCaptcha_Click(object sender, EventArgs e)
        {
            // Verify captcha
            if (captchaControl.IsValid)
            {
                lblCaptchaResult.Text = "CAPTCHA verification successful!";
            }
            else
            {
                lblCaptchaResult.Text = "CAPTCHA verification failed. Please try again.";
            }
        }
    }
}