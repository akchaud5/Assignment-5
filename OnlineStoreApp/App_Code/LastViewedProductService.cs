using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Script.Serialization;

namespace OnlineStoreApp.Services
{
    [ServiceContract(Namespace = "http://OnlineStoreApp")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LastViewedProductService
    {
        // Cookie name constant
        private const string COOKIE_NAME_PREFIX = "LastViewedProducts_";
        private const int COOKIE_EXPIRY_DAYS = 30;
        
        [OperationContract]
        public void RecordProductView(string username, int productId, string productName)
        {
            if (string.IsNullOrEmpty(username) || HttpContext.Current == null)
                return;
                
            // Create a timestamp for the view
            DateTime viewTime = DateTime.Now;
            
            // Create a new product info object
            ProductInfo productInfo = new ProductInfo
            {
                ProductId = productId,
                ProductName = productName,
                ViewTime = viewTime
            };
            
            // Get existing product history from cookie
            List<ProductInfo> productHistory = GetProductHistoryFromCookie(username);
            
            // Add the product to the beginning of the history
            productHistory.Insert(0, productInfo);
            
            // Keep only the last 10 viewed products
            if (productHistory.Count > 10)
            {
                productHistory.RemoveAt(productHistory.Count - 1);
            }
            
            // Save updated history back to cookie
            SaveProductHistoryToCookie(username, productHistory);
        }
        
        [OperationContract]
        public ProductInfo GetLastViewedProduct(string username)
        {
            if (string.IsNullOrEmpty(username) || HttpContext.Current == null)
            {
                return new ProductInfo
                {
                    ProductId = -1,
                    ProductName = "No products viewed",
                    ViewTime = DateTime.MinValue
                };
            }
            
            // Get product history from cookie
            List<ProductInfo> productHistory = GetProductHistoryFromCookie(username);
            
            // Check if there is any history
            if (productHistory.Count > 0)
            {
                return productHistory[0];
            }
            
            // Return empty product info if no history exists
            return new ProductInfo
            {
                ProductId = -1,
                ProductName = "No products viewed",
                ViewTime = DateTime.MinValue
            };
        }
        
        [OperationContract]
        public List<ProductInfo> GetRecentlyViewedProducts(string username, int count)
        {
            if (string.IsNullOrEmpty(username) || HttpContext.Current == null)
                return new List<ProductInfo>();
                
            // Limit the count to avoid excessive data
            int maxCount = Math.Min(count, 10);
            
            // Get product history from cookie
            List<ProductInfo> productHistory = GetProductHistoryFromCookie(username);
            
            // Return the requested number of recent products
            if (productHistory.Count <= maxCount)
            {
                return productHistory;
            }
            else
            {
                return productHistory.GetRange(0, maxCount);
            }
        }
        
        // Helper method to get product history from cookie
        private List<ProductInfo> GetProductHistoryFromCookie(string username)
        {
            List<ProductInfo> productHistory = new List<ProductInfo>();
            
            try
            {
                // Get the cookie with the user's product history
                HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIE_NAME_PREFIX + username];
                
                if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                {
                    // Deserialize the cookie value to product history
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    productHistory = serializer.Deserialize<List<ProductInfo>>(cookie.Value);
                }
            }
            catch (Exception ex)
            {
                // Log the error but return an empty list
                System.Diagnostics.Debug.WriteLine("Error getting product history from cookie: " + ex.Message);
            }
            
            return productHistory;
        }
        
        // Helper method to save product history to cookie
        private void SaveProductHistoryToCookie(string username, List<ProductInfo> productHistory)
        {
            try
            {
                // Serialize the product history to JSON
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string historyJson = serializer.Serialize(productHistory);
                
                // Create a new cookie or get the existing one
                HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIE_NAME_PREFIX + username];
                if (cookie == null)
                {
                    cookie = new HttpCookie(COOKIE_NAME_PREFIX + username);
                }
                
                // Set the cookie value and expiration
                cookie.Value = historyJson;
                cookie.Expires = DateTime.Now.AddDays(COOKIE_EXPIRY_DAYS);
                
                // Add the cookie to the response
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine("Error saving product history to cookie: " + ex.Message);
            }
        }
    }
    
    // Data contract for product information
    [System.Runtime.Serialization.DataContract]
    public class ProductInfo
    {
        [System.Runtime.Serialization.DataMember]
        public int ProductId { get; set; }
        
        [System.Runtime.Serialization.DataMember]
        public string ProductName { get; set; }
        
        [System.Runtime.Serialization.DataMember]
        public DateTime ViewTime { get; set; }
    }
}