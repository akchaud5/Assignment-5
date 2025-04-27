using System;
using System.Collections.Generic;
using OnlineStoreApp.Services;
using SecurityLib;

namespace OnlineStoreApp
{
    // Product info class (to match the service)
    public class ProductInfo
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime ViewTime { get; set; }
    }
    
    public partial class TryIt : System.Web.UI.Page
    {
        // Cache for last viewed products (simulating the service)
        private static Dictionary<string, List<ProductInfo>> userViewHistory = new Dictionary<string, List<ProductInfo>>();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Display visitor count from Application state
                lblVisitorCount.Text = Application["VisitorCount"].ToString();
            }
        }
        
        protected void btnVerifyZipcode_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input value
                string zipcode = txtZipcode.Text.Trim();
                
                // Perform operation based on selected option
                if (rbVerify.Checked)
                {
                    bool isValid = VerifyZipcode(zipcode);
                    lblZipcodeResult.Text = isValid ? 
                        $"The zipcode '{zipcode}' is valid." : 
                        $"The zipcode '{zipcode}' is invalid. Format should be 5 digits or 5 digits-4 digits.";
                }
                else
                {
                    string stateRegion = GetZipcodeState(zipcode);
                    lblZipcodeResult.Text = $"Zipcode '{zipcode}' is in: {stateRegion}";
                }
            }
            catch (Exception ex)
            {
                lblZipcodeResult.Text = "Error: " + ex.Message;
            }
        }
        
        protected void btnCalculateTax_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input values
                decimal price = Convert.ToDecimal(txtTaxPrice.Text);
                string stateCode = ddlState.SelectedValue;
                decimal result;
                
                // Perform tax calculation based on selected options
                if (rbTaxOnly.Checked)
                {
                    if (string.IsNullOrEmpty(stateCode))
                    {
                        result = CalculateTax(price);
                        lblTaxResult.Text = $"Tax on ${price:0.00} at default rate (7%): ${result:0.00}";
                    }
                    else
                    {
                        result = CalculateTaxByState(price, stateCode);
                        lblTaxResult.Text = $"Tax on ${price:0.00} in {stateCode}: ${result:0.00}";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(stateCode))
                    {
                        result = CalculateTotalWithTax(price);
                        lblTaxResult.Text = $"Total with tax for ${price:0.00} at default rate (7%): ${result:0.00}";
                    }
                    else
                    {
                        result = CalculateTotalWithTaxByState(price, stateCode);
                        lblTaxResult.Text = $"Total with tax for ${price:0.00} in {stateCode}: ${result:0.00}";
                    }
                }
            }
            catch (Exception ex)
            {
                lblTaxResult.Text = "Error: " + ex.Message;
            }
        }
        
        // Age Verification Service
        protected void btnVerifyAge_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the age from the input
                int age = Convert.ToInt32(txtAge.Text);
                
                // Choose operation based on radio button
                if (rbVerifyAdult.Checked)
                {
                    bool isAdult = VerifyAdult(age);
                    string message = GetAgeVerificationMessage(age);
                    lblAgeResult.Text = message;
                }
                else
                {
                    int yearsUntil = GetYearsUntilAdult(age);
                    if (yearsUntil == 0)
                    {
                        lblAgeResult.Text = "You are already an adult.";
                    }
                    else if (yearsUntil < 0)
                    {
                        lblAgeResult.Text = "Invalid age entered.";
                    }
                    else
                    {
                        lblAgeResult.Text = $"You will be an adult in {yearsUntil} year{(yearsUntil == 1 ? "" : "s")}.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblAgeResult.Text = "Error: " + ex.Message;
            }
        }
        
        // Last Viewed Product Service
        protected void ddlOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Show/hide panels based on selected operation
            string operation = ddlOperation.SelectedValue;
            
            pnlRecordProduct.Visible = (operation == "record");
            pnlRecentProducts.Visible = (operation == "recent");
        }
        
        protected void btnProductOperation_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string operation = ddlOperation.SelectedValue;
                
                switch (operation)
                {
                    case "last":
                        // Get last viewed product
                        var product = GetLastViewedProduct(username);
                        if (product.ProductId == -1)
                        {
                            lblProductResult.Text = "No products viewed yet.";
                        }
                        else
                        {
                            lblProductResult.Text = $"Last viewed product: {product.ProductName} (ID: {product.ProductId}), viewed at {product.ViewTime}";
                        }
                        break;
                        
                    case "record":
                        // Record a product view
                        int productId = Convert.ToInt32(txtProductId.Text);
                        string productName = txtProductName.Text.Trim();
                        
                        RecordProductView(username, productId, productName);
                        lblProductResult.Text = $"Product view recorded: {productName} (ID: {productId})";
                        break;
                        
                    case "recent":
                        // Get recently viewed products
                        int count = Convert.ToInt32(txtCount.Text);
                        var products = GetRecentlyViewedProducts(username, count);
                        
                        if (products.Count == 0)
                        {
                            lblProductResult.Text = "No products viewed yet.";
                        }
                        else
                        {
                            lblProductResult.Text = $"Recently viewed products for {username}: <br/>";
                            for (int i = 0; i < products.Count; i++)
                            {
                                lblProductResult.Text += $"{i+1}. {products[i].ProductName} (ID: {products[i].ProductId}), viewed at {products[i].ViewTime}<br/>";
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                lblProductResult.Text = "Error: " + ex.Message;
            }
        }

        protected void btnCalculateDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input values
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int quantity = Convert.ToInt32(txtQuantity.Text);

                // Use local calculation instead of the service directly
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
        
        // Local implementation of zipcode verification (to avoid service reference issues)
        private bool VerifyZipcode(string zipcode)
        {
            // Basic US zipcode validation - 5 digits, optional dash, then optional 4 digits
            string pattern = @"^\d{5}(-\d{4})?$";
            
            // Check if zipcode matches the pattern
            return System.Text.RegularExpressions.Regex.IsMatch(zipcode, pattern);
        }
        
        private string GetZipcodeState(string zipcode)
        {
            // This is a simplified implementation that returns a state based on first digit
            // In a real application, this would use a more comprehensive database
            if (!VerifyZipcode(zipcode))
            {
                return "Invalid zipcode format";
            }
            
            // Extract the first digit
            char firstDigit = zipcode[0];
            
            // Return state based on first digit
            switch (firstDigit)
            {
                case '0': return "Northeast (CT, MA, ME, NH, NJ, PR, RI, VT)";
                case '1': return "Northeast (DE, NY, PA)";
                case '2': return "Southeast (DC, MD, NC, SC, VA, WV)";
                case '3': return "Southeast (AL, FL, GA, MS, TN)";
                case '4': return "Midwest (IN, KY, MI, OH)";
                case '5': return "Midwest (IA, MN, MT, ND, SD, WI)";
                case '6': return "Central (IL, KS, MO, NE)";
                case '7': return "Central/Southwest (AR, LA, OK, TX)";
                case '8': return "Western (AZ, CO, ID, NM, NV, UT, WY)";
                case '9': return "Western/Pacific (AK, CA, HI, OR, WA)";
                default: return "Unknown region";
            }
        }
        
        // Local implementation of tax calculation (to avoid service reference issues)
        private decimal CalculateTax(decimal price)
        {
            // Default tax rate of 7%
            const double DEFAULT_TAX_RATE = 0.07;
            
            // Calculate tax at the default rate
            return decimal.Round(price * (decimal)DEFAULT_TAX_RATE, 2);
        }
        
        private decimal CalculateTaxByState(decimal price, string stateCode)
        {
            // Default rate if state not found
            const double DEFAULT_TAX_RATE = 0.07;
            
            // State tax rates
            var StateTaxRates = new Dictionary<string, double>
            {
                {"AL", 0.04}, {"AK", 0.00}, {"AZ", 0.056}, {"AR", 0.065}, {"CA", 0.0725},
                {"CO", 0.029}, {"CT", 0.0635}, {"DE", 0.00}, {"FL", 0.06}, {"GA", 0.04},
                {"HI", 0.04}, {"ID", 0.06}, {"IL", 0.0625}, {"IN", 0.07}, {"IA", 0.06},
                {"KS", 0.065}, {"KY", 0.06}, {"LA", 0.0445}, {"ME", 0.055}, {"MD", 0.06},
                {"MA", 0.0625}, {"MI", 0.06}, {"MN", 0.06875}, {"MS", 0.07}, {"MO", 0.04225},
                {"MT", 0.00}, {"NE", 0.055}, {"NV", 0.0685}, {"NH", 0.00}, {"NJ", 0.06625},
                {"NM", 0.05125}, {"NY", 0.04}, {"NC", 0.0475}, {"ND", 0.05}, {"OH", 0.0575},
                {"OK", 0.045}, {"OR", 0.00}, {"PA", 0.06}, {"RI", 0.07}, {"SC", 0.06},
                {"SD", 0.045}, {"TN", 0.07}, {"TX", 0.0625}, {"UT", 0.061}, {"VT", 0.06},
                {"VA", 0.053}, {"WA", 0.065}, {"WV", 0.06}, {"WI", 0.05}, {"WY", 0.04}
            };
            
            // Get the tax rate for the specified state
            double taxRate = DEFAULT_TAX_RATE;
            if (!string.IsNullOrEmpty(stateCode) && StateTaxRates.ContainsKey(stateCode.ToUpper()))
            {
                taxRate = StateTaxRates[stateCode.ToUpper()];
            }
            
            // Calculate and round to 2 decimal places
            return decimal.Round(price * (decimal)taxRate, 2);
        }
        
        private decimal CalculateTotalWithTax(decimal price)
        {
            // Calculate the total price including default tax
            return price + CalculateTax(price);
        }
        
        private decimal CalculateTotalWithTaxByState(decimal price, string stateCode)
        {
            // Calculate the total price including state-specific tax
            return price + CalculateTaxByState(price, stateCode);
        }
        
        // Age verification service methods
        private bool VerifyAdult(int age)
        {
            // Simple adult verification - must be at least 18 years old
            const int ADULT_AGE = 18;
            return age >= ADULT_AGE;
        }
        
        private string GetAgeVerificationMessage(int age)
        {
            const int ADULT_AGE = 18;
            
            if (age < 0)
            {
                return "Invalid age provided.";
            }
            else if (age < ADULT_AGE)
            {
                return $"Age verification failed. Must be at least {ADULT_AGE} years old.";
            }
            else
            {
                return "Age verification successful. Access granted.";
            }
        }
        
        private int GetYearsUntilAdult(int currentAge)
        {
            const int ADULT_AGE = 18;
            
            if (currentAge >= ADULT_AGE)
            {
                return 0;
            }
            else if (currentAge < 0)
            {
                return -1; // Invalid age
            }
            else
            {
                return ADULT_AGE - currentAge;
            }
        }
        
        // Last viewed product service methods
        private void RecordProductView(string username, int productId, string productName)
        {
            // Create a timestamp for the view
            DateTime viewTime = DateTime.Now;
            
            // Create a new product info object
            ProductInfo productInfo = new ProductInfo
            {
                ProductId = productId,
                ProductName = productName,
                ViewTime = viewTime
            };
            
            // If user doesn't exist in dictionary, add them
            if (!userViewHistory.ContainsKey(username))
            {
                userViewHistory[username] = new List<ProductInfo>();
            }
            
            // Add the product to the user's history
            userViewHistory[username].Insert(0, productInfo);
            
            // Keep only the last 10 viewed products
            if (userViewHistory[username].Count > 10)
            {
                userViewHistory[username].RemoveAt(userViewHistory[username].Count - 1);
            }
        }
        
        private ProductInfo GetLastViewedProduct(string username)
        {
            // Check if the user exists and has view history
            if (userViewHistory.ContainsKey(username) && userViewHistory[username].Count > 0)
            {
                return userViewHistory[username][0];
            }
            
            // Return empty product info if no history exists
            return new ProductInfo
            {
                ProductId = -1,
                ProductName = "No products viewed",
                ViewTime = DateTime.MinValue
            };
        }
        
        private List<ProductInfo> GetRecentlyViewedProducts(string username, int count)
        {
            // Limit the count to avoid excessive data
            int maxCount = Math.Min(count, 10);
            
            // Check if the user exists
            if (userViewHistory.ContainsKey(username))
            {
                // Return the requested number of recent products
                if (userViewHistory[username].Count <= maxCount)
                {
                    return userViewHistory[username];
                }
                else
                {
                    return userViewHistory[username].GetRange(0, maxCount);
                }
            }
            
            // Return empty list if no history exists
            return new List<ProductInfo>();
        }
    }
}