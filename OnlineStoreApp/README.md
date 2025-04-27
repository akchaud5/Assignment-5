# Online Store App - ASP.NET Web Application (Assignment-6)

## Overview
This is a simple e-commerce platform demonstrating various ASP.NET Web Forms features and components. This application fulfills all requirements for Assignment-6, including:

1. Member page with self-subscription and CAPTCHA verification
2. Staff page with admin-controlled access
3. Multiple web services integration
4. Forms authentication with XML-based data storage
5. Integration of all required components

## Features

### Authentication & Authorization
- Forms Authentication with secure password hashing
- Role-based access (Staff vs Member)
- CAPTCHA verification for registration
- XML-based user data storage
- Cookie-based role management

### Web Services
- **DiscountService** (WCF): Calculates discounts based on order quantity and price
- **ZipcodeVerifierService** (WCF): Validates US zipcode format and identifies state region
- **TaxCalculatorService** (WCF): Calculates sales tax at default rate (7%) or by state
- **AgeVerificationService** (WCF): Verifies if a user is an adult (18+) or calculates years until adulthood
- **LastViewedProductService** (Cookie-based): Tracks and retrieves recently viewed products using browser cookies

### Member Features
- Shopping cart functionality
- Personalized product browsing
- Discount calculator
- Session-based user tracking
- Last login time tracking

### Staff Features
- Product management
- Member account management
- System statistics
- Authentication oversight

### Other Components
- XML data storage and manipulation
- Session state management
- Application state for visitor counting
- User controls (CAPTCHA)
- Security library (password hashing)

## Deployment Instructions

### Option 1: Using Visual Studio on Windows
If you have Visual Studio installed on a Windows machine:
1. Open the `OnlineStoreApp.sln` solution file in Visual Studio
2. Press F5 to build and run the application
3. The application should open in your default web browser

### Option 2: Using IIS Express on Windows
1. Install IIS Express
2. Navigate to the project folder in Command Prompt
3. Run: `"C:\Program Files\IIS Express\iisexpress.exe" /path:full-path-to-project /port:8080`
4. Open your browser and navigate to `http://localhost:8080/Default.aspx`

### Option 3: WebStrar Deployment (Required for Assignment-6)
1. Build the solution in Visual Studio
2. Deploy to WebStrar server using Web Deploy:
   - Right-click on the project in Solution Explorer
   - Select "Publish"
   - Configure publish profile for WebStrar
   - Click "Publish"
3. After deployment, test all functionality to ensure everything works properly
4. Verify that XML files have appropriate permissions

## Component Overview

### Web Pages
- **Default.aspx**: Public page with product catalog and service directory
- **TryIt.aspx**: Testing page for all services and components
- **Pages/Member.aspx**: Member-only page with shopping cart functionality
- **Pages/Staff.aspx**: Staff administration page
- **Pages/Login.aspx**: Authentication page for both member and staff access

### Web Services (WCF)
- **DiscountService.svc**: Calculates discount based on quantity and price
- **ZipcodeVerifierService.svc**: Validates zipcode format and identifies state region
- **TaxCalculatorService.svc**: Calculates sales tax at default or state-specific rates
- **AgeVerificationService.svc**: Verifies adult status and calculates years until adulthood
- **LastViewedProductService.svc**: Cookie-based service that tracks and retrieves product viewing history

### Core Components
- **SecurityLib/PasswordHasher.cs**: Password hashing functionality (DLL library)
- **Controls/CaptchaControl.ascx**: CAPTCHA validation control (User control)
- **Global.asax**: Application and session event handlers, error logging
- **Cookies**: User role tracking and authentication state

### Data Storage
The application uses XML files for data storage:
- **App_Data/Members.xml**: Member account information
- **App_Data/Staff.xml**: Staff account information
- **App_Data/Products.xml**: Product catalog data

## Service Testing Guide

### Testing Services with TryIt.aspx
1. Navigate to the TryIt.aspx page from the Default page
2. Each service has its own dedicated testing section
3. Enter the required parameters and click the corresponding action button
4. View the results displayed beneath each service panel

#### Specific Testing Examples:

**Discount Service**:
- Enter a price (e.g., 100) and quantity (e.g., 5)
- Click "Calculate Discount"
- View the calculated discount amount

**Zipcode Verifier Service**:
- Enter a zipcode (e.g., "90210")
- Choose either "Verify Format" or "Get State Region"
- Click "Process"
- View the validation result or state information

**Tax Calculator Service**:
- Enter a price (e.g., 50)
- Optionally select a state from the dropdown
- Choose "Calculate Tax Only" or "Calculate Total with Tax"
- Click "Calculate"
- View the calculated tax or total amount

**Age Verification Service**:
- Enter an age (e.g., 16)
- Choose "Verify Adult Status" or "Years Until Adult"
- Click "Verify"
- View the verification result or years until adulthood

**Last Viewed Product Service**:
- Enter a username (e.g., "testuser")
- Select an operation (View Last Product, Record Product View, Get Recent Products)
- Fill in any additional fields that appear based on the selected operation
- Click "Submit"
- View the result of the operation

### Password Hasher and CAPTCHA
You can also test the password hashing functionality and CAPTCHA verification directly from the TryIt page.

## Test Credentials
- **Staff Login**: Username: "TA", Password: "Cse445!"

## Troubleshooting
- If XML files aren't being accessed, check file permissions in App_Data folder
- For deployment issues, ensure Web.config is properly configured
- If services aren't working, verify that the .svc endpoints are accessible
- For authentication problems, clear browser cookies and try again
- Check Global.asax for error logs if unexplained errors occur

## Contributors
- Ayush Chaudhary: Primary developer
- Bryan Ambrose: Contributed to ZipcodeVerifierService and AgeVerificationService
- Nilay Kumar: Contributed to TaxCalculatorService and LastViewedProductService