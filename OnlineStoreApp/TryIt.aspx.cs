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

                // Create a local implementation instead of using the service directly
                // This avoids the type conflict between App_Code and compiled DLL
                decimal discount = CalculateDiscount(price, quantity);

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

                // Use a local hash implementation to avoid type conflict
                string hashedPassword = HashPassword(password);

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

    // Local implementation of password hashing to avoid type conflicts
    private string HashPassword(string password)
    {
        // Use SHA256 for hashing
        using (System.Security.Cryptography.SHA256 sha256Hash = System.Security.Cryptography.SHA256.Create())
        {
            // Convert the input string to a byte array and compute the hash
            byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            // Convert byte array to a string
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}