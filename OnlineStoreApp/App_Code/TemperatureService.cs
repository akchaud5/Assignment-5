using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace OnlineStoreApp.Services
{
    [ServiceContract(Namespace = "http://OnlineStoreApp")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TemperatureService
    {
        [OperationContract]
        public double FahrenheitToCelsius(double fahrenheit)
        {
            // Convert Fahrenheit to Celsius
            return (fahrenheit - 32) * 5 / 9;
        }

        [OperationContract]
        public double CelsiusToFahrenheit(double celsius)
        {
            // Convert Celsius to Fahrenheit
            return (celsius * 9 / 5) + 32;
        }
    }
}