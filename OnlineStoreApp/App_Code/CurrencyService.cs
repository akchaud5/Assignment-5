using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace OnlineStoreApp.Services
{
    [ServiceContract(Namespace = "http://OnlineStoreApp")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TaxCalculatorService
    {
        // Default tax rate of 7%
        private const double DEFAULT_TAX_RATE = 0.07;
        
        // State tax rates
        private static readonly System.Collections.Generic.Dictionary<string, double> StateTaxRates = 
            new System.Collections.Generic.Dictionary<string, double>
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

        [OperationContract]
        public decimal CalculateTax(decimal price)
        {
            // Calculate tax at the default rate of 7%
            return decimal.Round(price * (decimal)DEFAULT_TAX_RATE, 2);
        }
        
        [OperationContract]
        public decimal CalculateTaxByState(decimal price, string stateCode)
        {
            // Get the tax rate for the specified state
            double taxRate = DEFAULT_TAX_RATE;
            if (!string.IsNullOrEmpty(stateCode) && StateTaxRates.ContainsKey(stateCode.ToUpper()))
            {
                taxRate = StateTaxRates[stateCode.ToUpper()];
            }
            
            // Calculate and round to 2 decimal places
            return decimal.Round(price * (decimal)taxRate, 2);
        }
        
        [OperationContract]
        public decimal CalculateTotalWithTax(decimal price)
        {
            // Calculate the total price including default tax
            return price + CalculateTax(price);
        }
        
        [OperationContract]
        public decimal CalculateTotalWithTaxByState(decimal price, string stateCode)
        {
            // Calculate the total price including state-specific tax
            return price + CalculateTaxByState(price, stateCode);
        }
    }
}