using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace OnlineStoreApp.Services
{
    [ServiceContract(Namespace = "http://OnlineStoreApp")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AgeVerificationService
    {
        // Minimum age for adult verification
        private const int ADULT_AGE = 18;
        
        [OperationContract]
        public bool VerifyAdult(int age)
        {
            // Simple adult verification - must be at least 18 years old
            return age >= ADULT_AGE;
        }
        
        [OperationContract]
        public bool VerifyAdultByBirthdate(DateTime birthdate)
        {
            // Calculate age based on birthdate
            int age = CalculateAge(birthdate);
            
            // Verify if age meets adult requirement
            return age >= ADULT_AGE;
        }
        
        [OperationContract]
        public int CalculateAge(DateTime birthdate)
        {
            // Get current date
            DateTime today = DateTime.Today;
            
            // Calculate age
            int age = today.Year - birthdate.Year;
            
            // Adjust age if birthday hasn't occurred yet this year
            if (birthdate.Date > today.AddYears(-age))
            {
                age--;
            }
            
            return age;
        }
        
        [OperationContract]
        public string GetAgeVerificationMessage(int age)
        {
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
        
        [OperationContract]
        public int GetYearsUntilAdult(int currentAge)
        {
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
    }
}