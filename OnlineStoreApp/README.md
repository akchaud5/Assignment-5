# Online Store App - ASP.NET Web Application

## Testing the Application

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

### Option 3: Simple Testing on macOS
Since ASP.NET Web Forms is not directly supported on macOS, you can test individual components:

1. Open terminal and navigate to the project folder
2. Run a simple HTTP server to view the test page:
   ```
   cd /Users/akchaud5/Downloads/Assignment-5/OnlineStoreApp
   python3 -m http.server 8080
   ```
3. Open your browser and navigate to `http://localhost:8080/test.html`
4. This page simulates core components of the application

## Component Overview

### Web Pages
- **Default.aspx**: Public page with product catalog and service directory
- **Pages/Member.aspx**: Member-only page with shopping cart functionality
- **Pages/Staff.aspx**: Staff administration page
- **Pages/Login.aspx**: Authentication page

### Core Components
- **SecurityLib/PasswordHasher.cs**: Password hashing functionality
- **Controls/CaptchaControl.ascx**: CAPTCHA validation control
- **Services/DiscountService.svc**: Web service for calculating discounts
- **Global.asax**: Application and session event handlers

### Data Storage
The application uses XML files for data storage:
- **App_Data/Members.xml**: Member account information
- **App_Data/Staff.xml**: Staff account information
- **App_Data/Products.xml**: Product catalog data

## Test Credentials
- **Staff Login**: Username: "TA", Password: "Cse445!"