using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text.RegularExpressions;

namespace OnlineStoreApp.Services
{
    [ServiceContract(Namespace = "http://OnlineStoreApp")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ZipcodeVerifierService
    {
        [OperationContract]
        public bool VerifyZipcode(string zipcode)
        {
            // Basic US zipcode validation - 5 digits, optional dash, then optional 4 digits
            string pattern = @"^\d{5}(-\d{4})?$";
            
            // Check if zipcode matches the pattern
            return Regex.IsMatch(zipcode, pattern);
        }
        
        [OperationContract]
        public string GetZipcodeState(string zipcode)
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
    }
}