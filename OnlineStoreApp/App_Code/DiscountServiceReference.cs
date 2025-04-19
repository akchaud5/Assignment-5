using System;
using System.ServiceModel;

namespace DiscountService
{
    [ServiceContract]
    public interface IDiscountService
    {
        [OperationContract]
        decimal CalculateDiscount(decimal price, int quantity);
    }

    public class DiscountServiceClient : ClientBase<IDiscountService>, IDiscountService
    {
        public decimal CalculateDiscount(decimal price, int quantity)
        {
            return Channel.CalculateDiscount(price, quantity);
        }
    }
}