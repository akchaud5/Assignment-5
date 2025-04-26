# Online Store App - ASP.NET Web Application (Assignment-6)

## Overview
This is a simple e-commerce platform demonstrating various ASP.NET Web Forms features and components. This application fulfills all requirements for Assignment-6, including:

1. Member page with self-subscription and CAPTCHA verification
2. Staff page with admin-controlled access
3. Multiple web services integration
4. Forms authentication with XML-based data storage
5. Integration of all required components

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
- **Pages/Member.aspx**: Member-only page with shopping cart functionality
- **Pages/Staff.aspx**: Staff administration page
- **Pages/Login.aspx**: Authentication page

### Core Components
- **SecurityLib/PasswordHasher.cs**: Password hashing functionality (DLL library)
- **Controls/CaptchaControl.ascx**: CAPTCHA validation control (User control)
- **Services/DiscountService.svc**: Web service for calculating discounts
- **Services/TemperatureService.svc**: Web service for temperature conversion
- **Services/CurrencyService.svc**: Web service for currency conversion
- **Global.asax**: Application and session event handlers
- **Cookies**: User role tracking and authentication state

### Data Storage
The application uses XML files for data storage:
- **App_Data/Members.xml**: Member account information
- **App_Data/Staff.xml**: Staff account information
- **App_Data/Products.xml**: Product catalog data

## Test Credentials
- **Staff Login**: Username: "TA", Password: "Cse445!"