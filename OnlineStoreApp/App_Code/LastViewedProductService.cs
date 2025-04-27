using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace OnlineStoreApp.Services
{
    [ServiceContract(Namespace = "http://OnlineStoreApp")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LastViewedProductService
    {
        // Dictionary to store last viewed products by user
        private static Dictionary<string, List<ProductInfo>> userViewHistory = new Dictionary<string, List<ProductInfo>>();
        
        [OperationContract]
        public void RecordProductView(string username, int productId, string productName)
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
        
        [OperationContract]
        public ProductInfo GetLastViewedProduct(string username)
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
        
        [OperationContract]
        public List<ProductInfo> GetRecentlyViewedProducts(string username, int count)
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