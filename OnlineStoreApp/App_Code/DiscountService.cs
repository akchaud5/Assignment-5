using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace OnlineStoreApp.Services
{
    [ServiceContract(Namespace = "http://OnlineStoreApp")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DiscountService
    {
        [OperationContract]
        public decimal CalculateDiscount(decimal price, int quantity)
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
    }
}