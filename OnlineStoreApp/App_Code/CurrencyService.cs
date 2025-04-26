using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace OnlineStoreApp.Services
{
    [ServiceContract(Namespace = "http://OnlineStoreApp")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CurrencyService
    {
        // Fixed exchange rates for simplicity
        private const double USD_TO_EUR = 0.92;
        private const double USD_TO_GBP = 0.79;
        private const double USD_TO_JPY = 154.50;

        [OperationContract]
        public double ConvertCurrency(double amount, string fromCurrency, string toCurrency)
        {
            // Convert to USD as an intermediate step
            double amountInUSD = ConvertToUSD(amount, fromCurrency);
            
            // Convert from USD to target currency
            return ConvertFromUSD(amountInUSD, toCurrency);
        }

        private double ConvertToUSD(double amount, string currency)
        {
            switch (currency.ToUpper())
            {
                case "USD":
                    return amount;
                case "EUR":
                    return amount / USD_TO_EUR;
                case "GBP":
                    return amount / USD_TO_GBP;
                case "JPY":
                    return amount / USD_TO_JPY;
                default:
                    throw new ArgumentException("Unsupported currency: " + currency);
            }
        }

        private double ConvertFromUSD(double amountInUSD, string currency)
        {
            switch (currency.ToUpper())
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
                    throw new ArgumentException("Unsupported currency: " + currency);
            }
        }
    }
}