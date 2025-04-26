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
        
        protected void btnConvertTemp_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input value
                double temperature = Convert.ToDouble(txtTemperature.Text);
                double result;
                
                // Perform conversion based on selected option
                if (rbFtoC.Checked)
                {
                    result = FahrenheitToCelsius(temperature);
                    lblTempResult.Text = string.Format("{0:0.00}°F = {1:0.00}°C", temperature, result);
                }
                else
                {
                    result = CelsiusToFahrenheit(temperature);
                    lblTempResult.Text = string.Format("{0:0.00}°C = {1:0.00}°F", temperature, result);
                }
            }
            catch (Exception ex)
            {
                lblTempResult.Text = "Error: " + ex.Message;
            }
        }
        
        protected void btnConvertCurrency_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input values
                double amount = Convert.ToDouble(txtAmount.Text);
                string fromCurrency = ddlFromCurrency.SelectedValue;
                string toCurrency = ddlToCurrency.SelectedValue;
                
                // Perform currency conversion
                double result = ConvertCurrency(amount, fromCurrency, toCurrency);
                
                // Display formatted result
                lblCurrencyResult.Text = string.Format("{0:0.00} {1} = {2:0.00} {3}", 
                    amount, fromCurrency, result, toCurrency);
            }
            catch (Exception ex)
            {
                lblCurrencyResult.Text = "Error: " + ex.Message;
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

        // Use DLL library for password hashing
        private string HashPassword(string password)
        {
            return PasswordHasher.HashPassword(password);
        }
        
        // Local implementation of temperature conversion (to avoid service reference issues)
        private double FahrenheitToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }
        
        private double CelsiusToFahrenheit(double celsius)
        {
            return (celsius * 9 / 5) + 32;
        }
        
        // Local implementation of currency conversion (to avoid service reference issues)
        private double ConvertCurrency(double amount, string fromCurrency, string toCurrency)
        {
            // Fixed exchange rates for simplicity
            const double USD_TO_EUR = 0.92;
            const double USD_TO_GBP = 0.79;
            const double USD_TO_JPY = 154.50;
            
            // Convert to USD as an intermediate step
            double amountInUSD;
            
            switch (fromCurrency.ToUpper())
            {
                case "USD":
                    amountInUSD = amount;
                    break;
                case "EUR":
                    amountInUSD = amount / USD_TO_EUR;
                    break;
                case "GBP":
                    amountInUSD = amount / USD_TO_GBP;
                    break;
                case "JPY":
                    amountInUSD = amount / USD_TO_JPY;
                    break;
                default:
                    throw new ArgumentException("Unsupported currency: " + fromCurrency);
            }
            
            // Convert from USD to target currency
            switch (toCurrency.ToUpper())
            {
                case "USD":
                    return amountInUSD;
                case "EUR":
                    return amountInUSD * USD_TO_EUR;
                case "GBP":
                    return amountInUSD * USD_TO_GBP;
                case "JPY":
                    return amountInUSD * USD_TO_JPY;
                default:
                    throw new ArgumentException("Unsupported currency: " + toCurrency);
            }
        }
    }
}