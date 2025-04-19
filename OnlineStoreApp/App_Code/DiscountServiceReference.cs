using System;
using System.ServiceModel;

namespace OnlineStoreApp.Services
{
    [ServiceContract]
    public interface IDiscountService
    {
        [OperationContract]
        decimal CalculateDiscount(decimal price, int quantity);
    }

    // Direct implementation of the service client as a simple class
    // in ASP.NET Website projects, we can use the service directly without a proxy
    // The ClientBase approach would be used in a deployed application
}