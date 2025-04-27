# Online Store App - Summary Table

## Project Information
- **Project Name**: Online Store App (Assignment-6)
- **Development Platform**: ASP.NET Web Forms
- **Deployment URL**: [http://webstrar106.fulton.asu.edu/page3/](http://webstrar106.fulton.asu.edu/page3/)
- **GitHub**: Private repository

## Team Member Contributions

| Team Member      | Components                                         | Contribution % |
|------------------|---------------------------------------------------|----------------|
| Ayush Chaudhary  | Core application, Member/Staff pages, Authentication, DiscountService, CAPTCHA, Password Hasher, XML storage | 80% |
| Bryan Ambrose    | ZipcodeVerifierService, AgeVerificationService    | 10% |
| Nilesh Kumar     | TaxCalculatorService, LastViewedProductService    | 10% |

## Component Summary Table

| Component                 | Type           | Provider         | Description                                  | Test Method |
|---------------------------|----------------|------------------|----------------------------------------------|-------------|
| DiscountService           | WCF Service    | Ayush Chaudhary  | Calculates discounts based on order quantity and price | TryIt.aspx |
| ZipcodeVerifierService    | WCF Service    | Bryan Ambrose    | Validates US zipcode format and identifies state region | TryIt.aspx |
| TaxCalculatorService      | WCF Service    | Nilesh Kumar     | Calculates sales tax at default (7%) or state rates | TryIt.aspx |
| AgeVerificationService    | WCF Service    | Bryan Ambrose    | Verifies if a user is an adult (18+) | TryIt.aspx |
| LastViewedProductService  | WCF Service    | Nilesh Kumar     | Tracks and retrieves recently viewed products | TryIt.aspx |
| PasswordHasher            | DLL Library    | Ayush Chaudhary  | Securely hashes passwords with SHA-256 | TryIt.aspx |
| CaptchaControl            | User Control   | Ayush Chaudhary  | CAPTCHA verification for forms | TryIt.aspx, Login.aspx |
| Default.aspx              | Web Page       | Ayush Chaudhary  | Public landing page with product catalog | Direct access |
| Member.aspx               | Web Page       | Ayush Chaudhary  | Member area with shopping cart | Login required |
| Staff.aspx                | Web Page       | Ayush Chaudhary  | Staff administration area | Login required |
| Login.aspx                | Web Page       | Ayush Chaudhary  | Authentication for members and staff | Direct access |
| TryIt.aspx                | Web Page       | Ayush Chaudhary  | Testing interface for all services | Direct access |
| Products.xml              | XML Storage    | Ayush Chaudhary  | Product catalog data | View on Default.aspx |
| Members.xml               | XML Storage    | Ayush Chaudhary  | Member account storage | View on Member.aspx |
| Staff.xml                 | XML Storage    | Ayush Chaudhary  | Staff account storage | View on Staff.aspx |
| Global.asax               | System File    | Ayush Chaudhary  | Application events, visitor counting | N/A |
| Web.config                | Config File    | Ayush Chaudhary  | Application configuration, service endpoints | N/A |

## Technical Overview

### Authentication System
- Forms Authentication with cookie persistence
- Role-based authorization (Member vs Staff)
- Secure password hashing with SHA-256
- CAPTCHA verification for new registrations
- XML-based user account storage

### Web Services Implementation
Each service is implemented using WCF (Windows Communication Foundation) with proper error handling and validation:

1. **DiscountService**:
   - Method: `CalculateDiscount(price, quantity)`
   - Returns a discount amount based on order quantity and price
   - Implements tiered discounting (5%/10%/15%) based on quantity

2. **ZipcodeVerifierService**:
   - Methods: `VerifyZipcode(zipcode)`, `GetZipcodeState(zipcode)`
   - Validates US postal code format (5 digits or 5+4)
   - Returns region/state information based on first digit

3. **TaxCalculatorService**:
   - Methods: `CalculateTax(price)`, `CalculateTaxByState(price, stateCode)`
   - Calculates sales tax at default rate (7%) or by state-specific rates
   - Includes all 50 US states with accurate tax rates

4. **AgeVerificationService**:
   - Methods: `VerifyAdult(age)`, `GetYearsUntilAdult(currentAge)`
   - Verifies if a user is 18+ years old
   - Calculates years remaining until adulthood

5. **LastViewedProductService**:
   - Methods: `RecordProductView(username, productId, productName)`, `GetLastViewedProduct(username)`, `GetRecentlyViewedProducts(username, count)`
   - Tracks product view history for users
   - Returns most recent or multiple recent product views

### Data Storage
- **Products.xml**: Stores product catalog with ID, name, price, and category
- **Members.xml**: Stores member accounts with hashed passwords
- **Staff.xml**: Stores staff accounts with higher privilege

### State Management
- Session state for shopping cart and user preferences
- Application state for visitor counting
- Cookies for authentication persistence

## Detailed Testing Instructions

### Access Testing Page
1. Navigate to the application URL: [http://webstrar106.fulton.asu.edu/page3/](http://webstrar106.fulton.asu.edu/page3/)
2. Click on any "Try It" button in the services table, or click on "TryIt.aspx" directly

### 1. Discount Service Testing
1. **Navigate** to the "Discount Service" section on TryIt.aspx
2. **Enter values**:
   - Price: 100
   - Quantity: 5
3. **Click** "Calculate Discount"
4. **Expected Result**: $10.00 (10% discount for 5 items)
5. **Try Other Test Cases**:
   - Price: 100, Quantity: 3 → Expected: $5.00 (5% discount)
   - Price: 100, Quantity: 10 → Expected: $15.00 (15% discount)
   - Price: 200, Quantity: 5 → Expected: $20.00 + $25.00 = $45.00 (10% + $25 for orders over $500)

### 2. Zipcode Verifier Service Testing
1. **Navigate** to the "Zipcode Verifier Service" section
2. **Test Verification**:
   - Enter Zipcode: 85281
   - Select "Verify Format"
   - Click "Process"
   - Expected Result: "The zipcode '85281' is valid."
3. **Test State Lookup**:
   - Enter Zipcode: 85281
   - Select "Get State Region"
   - Click "Process"
   - Expected Result: "Zipcode '85281' is in: Western (AZ, CO, ID, NM, NV, UT, WY)"
4. **Try Invalid Zipcode**:
   - Enter Zipcode: ABC123
   - Select either option
   - Click "Process"
   - Expected Result: Error or invalid format message

### 3. Tax Calculator Service Testing
1. **Navigate** to the "Tax Calculator Service" section
2. **Test Default Tax Rate**:
   - Enter Price: 100
   - Leave State as "Default (7%)"
   - Select "Calculate Tax Only"
   - Click "Calculate"
   - Expected Result: "Tax on $100.00 at default rate (7%): $7.00"
3. **Test State-Specific Tax**:
   - Enter Price: 100
   - Select State: "California (7.25%)"
   - Select "Calculate Tax Only"
   - Click "Calculate"
   - Expected Result: "Tax on $100.00 in CA: $7.25"
4. **Test Total with Tax**:
   - Enter Price: 100
   - Select State: "New York (4%)"
   - Select "Calculate Total with Tax"
   - Click "Calculate"
   - Expected Result: "Total with tax for $100.00 in NY: $104.00"

### 4. Age Verification Service Testing
1. **Navigate** to the "Age Verification Service" section
2. **Test Adult Verification**:
   - Enter Age: 21
   - Select "Verify Adult Status"
   - Click "Verify"
   - Expected Result: "Age verification successful. Access granted."
3. **Test Minor Verification**:
   - Enter Age: 16
   - Select "Verify Adult Status"
   - Click "Verify"
   - Expected Result: "Age verification failed. Must be at least 18 years old."
4. **Test Years Until Adult**:
   - Enter Age: 16
   - Select "Years Until Adult"
   - Click "Verify"
   - Expected Result: "You will be an adult in 2 years."
5. **Try Edge Cases**:
   - Enter Age: 18 with "Verify Adult Status" → Expected: "Age verification successful."
   - Enter Age: 18 with "Years Until Adult" → Expected: "You are already an adult."
   - Enter Age: 0 with "Years Until Adult" → Expected: "You will be an adult in 18 years."

### 5. Last Viewed Product Service Testing
1. **Navigate** to the "Last Viewed Product Service" section
2. **Record Product Views First**:
   - Enter Username: "testuser"
   - Select Operation: "Record Product View"
   - Enter Product ID: 1
   - Enter Product Name: "Laptop"
   - Click "Submit"
   - Expected Result: "Product view recorded: Laptop (ID: 1)"
3. **Record More Products** (repeat step 2 with different products):
   - Product ID: 2, Name: "Smartphone"
   - Product ID: 3, Name: "Headphones"
4. **View Last Product**:
   - Enter Username: "testuser"
   - Select Operation: "View Last Product"
   - Click "Submit"
   - Expected Result: "Last viewed product: Headphones (ID: 3), viewed at [timestamp]"
5. **View Recent Products**:
   - Enter Username: "testuser"
   - Select Operation: "Get Recent Products"
   - Enter Number of Products: 3
   - Click "Submit"
   - Expected Result: List of 3 recently viewed products with Headphones first, followed by Smartphone, then Laptop

### 6. Password Hasher Testing
1. **Navigate** to the "Password Hasher" section
2. **Test Password Hashing**:
   - Enter Password: "Test123!"
   - Click "Hash Password"
   - Expected Result: A hashed string (SHA-256)
3. **Try Different Passwords**:
   - Simple: "password"
   - Complex: "P@ssw0rd123!"
   - Observe how the hashed values differ completely

### 7. CAPTCHA Testing
1. **Navigate** to the "CAPTCHA Control" section
2. **Test Valid CAPTCHA**:
   - View the displayed CAPTCHA image
   - Enter the characters exactly as shown
   - Click "Verify CAPTCHA"
   - Expected Result: "CAPTCHA verification successful!"
3. **Test Invalid CAPTCHA**:
   - Enter incorrect characters
   - Click "Verify CAPTCHA"
   - Expected Result: "CAPTCHA verification failed. Please try again."

### 8. Authentication Testing
1. **Staff Login**:
   - Username: "TA"
   - Password: "Cse445!"
   - Expected Result: Access to Staff.aspx
2. **Member Registration and Login**:
   - Navigate to Member.aspx
   - Click "Register" on the login page
   - Create a new account with valid CAPTCHA
   - Login with new credentials
   - Expected Result: Access to Member.aspx with shopping cart

## Deployment Notes
- Deployed on WebStrar server
- URL: [http://webstrar106.fulton.asu.edu/page3/](http://webstrar106.fulton.asu.edu/page3/)
- Default debugging disabled for production
- App_Data folder permissions set for XML writing
- Forms authentication configured for cookie timeout of 30 minutes