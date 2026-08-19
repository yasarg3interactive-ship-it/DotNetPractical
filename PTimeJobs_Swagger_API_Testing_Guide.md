# PTimeJobs Swagger API Testing Guide

Source project: `D:\Practical Project\PTimeJobs`

Target API style: ASP.NET Core Web API with Swagger/OpenAPI.

## 1. Important Current Status

This project exposes many marketplace APIs through controllers, but there is no implemented `AuthController` in the current source. That means these endpoints are not currently available in Swagger:

- `POST /api/v1/auth/register`
- `POST /api/v1/auth/login`
- `POST /api/v1/auth/refresh-token`
- `POST /api/v1/auth/logout`

JWT bearer authentication is configured in `Program.cs`, and Swagger has the Bearer authorization button, but the login/register endpoints still need to be created before a tester can generate tokens from the API.

This document includes:

- How to start Swagger.
- How to test the implemented APIs.
- How to use a JWT token in Swagger if you already have one.
- Sample register/login request bodies for the future auth endpoints.
- Recommended testing order with sample JSON payloads.
- Common errors and how to debug them.

## 2. Start the API

Open a terminal:

```bash
cd D:\Practical Project\PTimeJobs\src\PTimeJobs.Api
dotnet run
```

Development launch URLs from `launchSettings.json`:

```text
HTTP:  http://localhost:5084
HTTPS: https://localhost:7053
```

Swagger URLs:

```text
http://localhost:5084/swagger
https://localhost:7053/swagger
```

If the browser opens automatically, it should open Swagger because `launchUrl` is configured as `swagger`.

## 3. Database Configuration Before Testing

Development database connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=PTimeJobDB;Username=postgres;Password=1234"
  }
}
```

File:

```text
PTimeJobs/src/PTimeJobs.Api/appsettings.Development.json
```

Before testing write APIs, confirm:

- PostgreSQL is running.
- Database `PTimeJobDB` exists.
- SQL scripts in `database_migrations/` were executed in numeric order.
- Optional extensions `postgis` and `vector` are installed if the migration scripts require them.

Migration execution order:

```text
00_extensions_enums.sql
01_identity_access.sql
02_location.sql
03_worker_profile.sql
04_job_marketplace.sql
05_matching_ai.sql
06_accommodation.sql
07_food_service.sql
08_communication.sql
09_payments.sql
10_reviews_admin_analytics.sql
98_table_comments.sql
99_indexes.sql
```

## 4. Swagger Basic Testing Steps

For every endpoint:

1. Open Swagger.
2. Expand the controller group.
3. Select an endpoint.
4. Click `Try it out`.
5. Fill route parameters, query parameters, or request body.
6. Click `Execute`.
7. Check status code, response body, and response headers.
8. Copy generated IDs from successful responses for the next endpoint.

Expected response wrapper:

```json
{
  "status": "success",
  "message": "Operation completed successfully.",
  "data": {}
}
```

Expected error wrapper:

```json
{
  "status": "error",
  "message": "Validation failed.",
  "data": null,
  "errors": {}
}
```

## 5. Testing JWT Authorization in Swagger

Swagger has Bearer token support.

If you already have a JWT token:

1. Open Swagger.
2. Click `Authorize`.
3. In the value box, enter only the token if Swagger uses HTTP bearer scheme.
4. If that does not work, enter:

```text
Bearer <your-jwt-token>
```

5. Click `Authorize`.
6. Close the dialog.
7. Test protected endpoints.

Currently visible protected endpoint:

```http
GET /api/v1/Users/{userId}
```

It requires:

```text
Role: admin
```

Without a valid admin token, expect:

- `401 Unauthorized` when no valid token is provided.
- `403 Forbidden` when token is valid but the user lacks `admin` role.

## 6. Auth API Samples for Future Implementation

These endpoints are not currently implemented. Add an `AuthController` before testing them.

### Register Worker User

Expected endpoint:

```http
POST /api/v1/Auth/register
```

Sample request:

```json
{
  "email": "worker1@example.com",
  "mobileNumber": "+919876543210",
  "password": "Test@12345",
  "confirmPassword": "Test@12345",
  "roles": ["worker"],
  "profile": {
    "firstName": "Asha",
    "lastName": "Raman",
    "preferredLanguage": "en",
    "timezone": "Asia/Kolkata"
  }
}
```

Expected success:

```json
{
  "status": "success",
  "message": "User registered.",
  "data": {
    "userId": "11111111-1111-1111-1111-111111111111",
    "email": "worker1@example.com",
    "mobileNumber": "+919876543210",
    "status": "Pending",
    "roles": ["worker"]
  }
}
```

Validation tests:

- Empty email and empty mobile number should fail.
- Weak password should fail.
- Password and confirm password mismatch should fail.
- Duplicate email/mobile should fail.

### Register Employer User

Expected endpoint:

```http
POST /api/v1/Auth/register
```

Sample request:

```json
{
  "email": "employer1@example.com",
  "mobileNumber": "+919812345670",
  "password": "Test@12345",
  "confirmPassword": "Test@12345",
  "roles": ["employer"],
  "profile": {
    "firstName": "Nikhil",
    "lastName": "Menon",
    "preferredLanguage": "en",
    "timezone": "Asia/Kolkata"
  }
}
```

### Login

Expected endpoint:

```http
POST /api/v1/Auth/login
```

Sample request:

```json
{
  "emailOrMobile": "employer1@example.com",
  "password": "Test@12345"
}
```

Expected success:

```json
{
  "status": "success",
  "message": "Login successful.",
  "data": {
    "accessToken": "<jwt-access-token>",
    "refreshToken": "<refresh-token>",
    "expiresAt": "2026-08-18T12:00:00Z",
    "user": {
      "userId": "22222222-2222-2222-2222-222222222222",
      "email": "employer1@example.com",
      "roles": ["employer"]
    }
  }
}
```

Login failure tests:

- Wrong password should return `401`.
- Unknown email/mobile should return `401`.
- Suspended user should return `403`.
- Locked user should return `423` or a domain-specific error response.

### Refresh Token

Expected endpoint:

```http
POST /api/v1/Auth/refresh-token
```

Sample request:

```json
{
  "refreshToken": "<refresh-token>"
}
```

### Logout

Expected endpoint:

```http
POST /api/v1/Auth/logout
```

Sample request:

```json
{
  "refreshToken": "<refresh-token>"
}
```

## 7. Recommended End-to-End Swagger Test Order

Because many entities depend on earlier IDs, test in this order:

1. Health check.
2. Country.
3. State.
4. City.
5. Area.
6. Location.
7. Permission and role setup.
8. User role assignment, if test users already exist in database.
9. Worker profile.
10. Employer profile.
11. Skill.
12. Job category.
13. Job.
14. Job location, schedule, and required skill.
15. Job application.
16. Shortlist and application status.
17. Contract.
18. Matching score.
19. Accommodation provider, property, room, availability, booking.
20. Food provider, item, plan, subscription.
21. Conversation and message.
22. Notification.
23. Invoice, payment, transaction.
24. Review, complaint, report.
25. Analytics, search history, recommendation, preferences.

Use a notepad while testing and save IDs returned from each successful `POST`.

Example ID placeholders used below:

```text
{userId}                  11111111-1111-1111-1111-111111111111
{employerUserId}          22222222-2222-2222-2222-222222222222
{workerProfileId}         33333333-3333-3333-3333-333333333333
{employerProfileId}       44444444-4444-4444-4444-444444444444
{countryId}               55555555-5555-5555-5555-555555555555
{stateId}                 66666666-6666-6666-6666-666666666666
{cityId}                  77777777-7777-7777-7777-777777777777
{areaId}                  88888888-8888-8888-8888-888888888888
{locationId}              99999999-9999-9999-9999-999999999999
{jobId}                   aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa
{skillId}                 bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb
```

Replace placeholders with real IDs from Swagger responses.

## 8. Health API

### GET Health

Swagger group:

```text
Health
```

Endpoint:

```http
GET /api/v1/Health
```

Expected status:

- `200 OK` when API and database are healthy.
- `503 Service Unavailable` when database connection fails.

Expected success:

```json
{
  "status": "success",
  "message": "Service is healthy.",
  "data": {
    "service": "PTimeJobs API",
    "databaseConnected": true,
    "checkedAt": "2026-08-18T06:00:00Z"
  }
}
```

## 9. Location APIs

### Create Country

```http
POST /api/v1/Countries
```

```json
{
  "iso2": "IN",
  "iso3": "IND",
  "countryName": "India",
  "phoneCode": "+91"
}
```

Then test:

```http
GET /api/v1/Countries
GET /api/v1/Countries/{countryId}
```

### Create State

```http
POST /api/v1/States
```

```json
{
  "countryId": "{countryId}",
  "stateName": "Kerala",
  "stateCode": "KL"
}
```

### Create City

```http
POST /api/v1/Cities
```

```json
{
  "stateId": "{stateId}",
  "cityName": "Kochi"
}
```

### Create Area

```http
POST /api/v1/Areas
```

```json
{
  "cityId": "{cityId}",
  "areaName": "Kakkanad",
  "postalCode": "682030"
}
```

### Create Location

```http
POST /api/v1/Locations
```

```json
{
  "countryId": "{countryId}",
  "stateId": "{stateId}",
  "cityId": "{cityId}",
  "areaId": "{areaId}",
  "addressLine1": "Infopark Road",
  "addressLine2": "Near Phase 1",
  "landmark": "Infopark",
  "latitude": 10.0159,
  "longitude": 76.3419,
  "googlePlaceId": "sample-place-id"
}
```

## 10. RBAC and User APIs

### Create Permission

```http
POST /api/v1/Permissions
```

```json
{
  "permissionCode": "jobs.create",
  "moduleName": "Jobs",
  "description": "Create job postings"
}
```

### Get Roles

```http
GET /api/v1/Roles
```

### Add Permission to Role

```http
POST /api/v1/Roles/{roleId}/permissions/{permissionId}
```

No body required.

### Get User by ID

```http
GET /api/v1/Users/{userId}
```

Requires admin token.

Expected success fields:

```json
{
  "userId": "{userId}",
  "email": "admin@example.com",
  "mobileNumber": "+919999999999",
  "status": "Active",
  "isEmailVerified": true,
  "isMobileVerified": true,
  "roles": ["admin"],
  "createdAt": "2026-08-18T06:00:00Z"
}
```

### Assign Role to User

```http
POST /api/v1/Users/{userId}/roles/{roleId}
```

```json
{
  "assignedBy": "{adminUserId}"
}
```

### Remove Role from User

```http
DELETE /api/v1/Users/{userId}/roles/{roleId}
```

## 11. Verification APIs

### Create Verification

```http
POST /api/v1/Verifications
```

```json
{
  "userId": "{userId}",
  "channel": "Email",
  "targetValue": "worker1@example.com",
  "tokenHash": "hashed-token-value",
  "expiresAt": "2026-08-19T00:00:00Z"
}
```

Then test:

```http
GET /api/v1/Verifications/{verificationId}
GET /api/v1/Verifications/by-user/{userId}
PATCH /api/v1/Verifications/{verificationId}/verify
PATCH /api/v1/Verifications/{verificationId}/fail
PATCH /api/v1/Verifications/{verificationId}/revoke
```

## 12. Worker APIs

### Create Worker Profile

```http
POST /api/v1/WorkerProfiles
```

```json
{
  "userId": "{userId}"
}
```

### Update Headline

```http
PATCH /api/v1/WorkerProfiles/{workerProfileId}/headline
```

```json
{
  "headline": "Part-time cafe assistant with evening availability"
}
```

### Update Expected Salary

```http
PATCH /api/v1/WorkerProfiles/{workerProfileId}/expected-salary
```

```json
{
  "min": 150,
  "max": 220,
  "salaryModel": "Hourly"
}
```

### Add Worker Skill

```http
POST /api/v1/WorkerProfiles/{workerProfileId}/skills
```

```json
{
  "skillId": "{skillId}",
  "proficiencyLevel": 4,
  "yearsExperience": 1.5,
  "isPrimary": true
}
```

### Add Experience

```http
POST /api/v1/WorkerProfiles/{workerProfileId}/experience
```

```json
{
  "jobTitle": "Cafe Assistant",
  "companyName": "City Cafe",
  "employmentType": "PartTime",
  "startDate": "2025-01-01",
  "endDate": "2025-12-31",
  "description": "Handled counter service and inventory support."
}
```

### Add Education

```http
POST /api/v1/WorkerProfiles/{workerProfileId}/education
```

```json
{
  "institutionName": "Kochi College",
  "degree": "B.Com",
  "fieldOfStudy": "Commerce",
  "startYear": 2022,
  "endYear": 2025,
  "isCurrent": false
}
```

## 13. Employer APIs

### Create Employer Profile

```http
POST /api/v1/EmployerProfiles
```

```json
{
  "userId": "{employerUserId}",
  "companyName": "Kochi Retail Mart",
  "businessType": "Retail",
  "registrationNumber": "KL-REG-1001",
  "locationId": "{locationId}"
}
```

Then test:

```http
GET /api/v1/EmployerProfiles/{employerProfileId}
GET /api/v1/EmployerProfiles/by-user/{userId}
```

## 14. Skill and Job Category APIs

### Create Skill

```http
POST /api/v1/Skills
```

```json
{
  "skillName": "Customer Service",
  "skillCategory": "Retail"
}
```

### Create Job Category

```http
POST /api/v1/JobCategories
```

```json
{
  "categoryName": "Retail Assistant",
  "categorySlug": "retail-assistant",
  "parentCategoryId": null
}
```

## 15. Job APIs

### Create Job

```http
POST /api/v1/Jobs
```

```json
{
  "employerProfileId": "{employerProfileId}",
  "title": "Evening Store Assistant",
  "description": "Assist customers, arrange shelves, and support billing counter during evening hours.",
  "employmentType": "PartTime",
  "salaryModel": "Hourly",
  "salaryMin": 150,
  "salaryMax": 220,
  "openingsCount": 3,
  "minExperienceMonths": 0,
  "jobCategoryId": "{jobCategoryId}"
}
```

### Search Jobs

```http
GET /api/v1/Jobs?status=Open&employmentType=PartTime&page=1&pageSize=20
```

Also test without filters:

```http
GET /api/v1/Jobs
```

### Publish Job

```http
PATCH /api/v1/Jobs/{jobId}/publish
```

No body required.

### Close Job

```http
PATCH /api/v1/Jobs/{jobId}/close
```

No body required.

### Add Job Location

```http
POST /api/v1/Jobs/{jobId}/locations
```

```json
{
  "locationId": "{locationId}",
  "latitude": 10.0159,
  "longitude": 76.3419,
  "isRemoteAllowed": false
}
```

### Add Job Schedule

```http
POST /api/v1/Jobs/{jobId}/schedules
```

```json
{
  "dayOfWeek": 1,
  "startTime": "18:00:00",
  "endTime": "22:00:00",
  "startDate": "2026-08-20",
  "endDate": "2026-12-31",
  "shiftLabel": "Evening",
  "requiredWorkers": 3
}
```

### Add Job Skill

```http
POST /api/v1/Jobs/{jobId}/skills
```

```json
{
  "skillId": "{skillId}",
  "requiredLevel": 3,
  "isMandatory": true
}
```

## 16. Job Application APIs

### Apply for Job

```http
POST /api/v1/JobApplications
```

```json
{
  "jobId": "{jobId}",
  "workerProfileId": "{workerProfileId}",
  "coverNote": "I am available for evening shifts and have customer service experience.",
  "expectedSalary": 180
}
```

### Search Applications

```http
GET /api/v1/JobApplications?jobId={jobId}&workerProfileId={workerProfileId}&status=Submitted&page=1&pageSize=20
```

### Update Application Status

```http
PATCH /api/v1/JobApplications/{applicationId}/status
```

```json
{
  "status": "Shortlisted"
}
```

Suggested status test values:

```text
Submitted
Reviewing
Shortlisted
Interview
Offered
Hired
Rejected
Withdrawn
```

### Add Shortlist

```http
POST /api/v1/JobApplications/{applicationId}/shortlist
```

```json
{
  "shortlistedBy": "{employerUserId}",
  "notes": "Good match for evening shift."
}
```

## 17. Contract APIs

### Create Contract

```http
POST /api/v1/Contracts
```

```json
{
  "jobId": "{jobId}",
  "workerProfileId": "{workerProfileId}",
  "employerProfileId": "{employerProfileId}",
  "startDate": "2026-09-01",
  "applicationId": "{applicationId}",
  "agreedSalary": 180,
  "salaryModel": "Hourly",
  "termsUrl": "https://example.com/contracts/contract-001.pdf"
}
```

Then test:

```http
PATCH /api/v1/Contracts/{contractId}/activate
PATCH /api/v1/Contracts/{contractId}/complete
PATCH /api/v1/Contracts/{contractId}/cancel
PATCH /api/v1/Contracts/{contractId}/terminate
```

For complete/cancel/terminate endpoints, use:

```json
{
  "endDate": "2026-12-31"
}
```

## 18. Matching API

### Create Matching Score

```http
POST /api/v1/MatchingScores
```

```json
{
  "workerProfileId": "{workerProfileId}",
  "jobId": "{jobId}",
  "modelVersion": "match-v1",
  "overallScore": 92.5,
  "skillScore": 90,
  "distanceScore": 85,
  "availabilityScore": 95,
  "experienceScore": 80,
  "salaryScore": 88,
  "ratingScore": 75
}
```

Then test:

```http
GET /api/v1/MatchingScores/by-job/{jobId}
GET /api/v1/MatchingScores/by-worker/{workerProfileId}
```

## 19. Accommodation APIs

### Create Accommodation Provider

```http
POST /api/v1/AccommodationProviders
```

```json
{
  "userId": "{userId}",
  "businessName": "Kochi Working Hostel",
  "contactNumber": "+919800000001"
}
```

### Verify Accommodation Provider

```http
PATCH /api/v1/AccommodationProviders/{accommodationProviderId}/verify
```

### Create Facility

```http
POST /api/v1/Facilities
```

```json
{
  "facilityName": "WiFi",
  "facilityCategory": "Basic"
}
```

### Create Room Type

```http
POST /api/v1/RoomTypes
```

```json
{
  "typeName": "Shared Room",
  "description": "Shared room for workers"
}
```

### Create Property

```http
POST /api/v1/Properties
```

```json
{
  "accommodationProviderId": "{accommodationProviderId}",
  "propertyName": "Infopark Stay",
  "propertyType": "Hostel",
  "locationId": "{locationId}",
  "latitude": 10.0159,
  "longitude": 76.3419,
  "addressText": "Near Infopark, Kakkanad",
  "description": "Affordable worker accommodation near job clusters."
}
```

### Add Property Image

```http
POST /api/v1/Properties/{propertyId}/images
```

```json
{
  "imageUrl": "https://example.com/images/property-1.jpg",
  "sortOrder": 1,
  "isPrimary": true
}
```

### Add Property Facility

```http
POST /api/v1/Properties/{propertyId}/facilities
```

```json
{
  "facilityId": "{facilityId}",
  "details": "High-speed shared WiFi"
}
```

### Create Room

```http
POST /api/v1/Rooms
```

```json
{
  "propertyId": "{propertyId}",
  "capacity": 4,
  "monthlyPrice": 4500,
  "roomTypeId": "{roomTypeId}",
  "roomNumber": "A-101",
  "floorNumber": "1",
  "securityDeposit": 2000
}
```

### Add Room Availability

```http
POST /api/v1/Rooms/{roomId}/availability
```

```json
{
  "availableFrom": "2026-09-01",
  "availableBeds": 2,
  "availableTo": "2026-12-31",
  "priceOverride": 4300
}
```

### Create Accommodation Booking

```http
POST /api/v1/AccommodationBookings
```

```json
{
  "roomId": "{roomId}",
  "workerProfileId": "{workerProfileId}",
  "checkInDate": "2026-09-01",
  "totalAmount": 4500
}
```

Then test:

```http
PATCH /api/v1/AccommodationBookings/{bookingId}/confirm
PATCH /api/v1/AccommodationBookings/{bookingId}/check-in
PATCH /api/v1/AccommodationBookings/{bookingId}/complete
PATCH /api/v1/AccommodationBookings/{bookingId}/cancel
PATCH /api/v1/AccommodationBookings/{bookingId}/reject
```

Complete request:

```json
{
  "checkOutDate": "2026-09-30"
}
```

## 20. Food APIs

### Create Food Provider

```http
POST /api/v1/FoodProviders
```

```json
{
  "userId": "{userId}",
  "businessName": "Daily Meals Kochi",
  "providerType": "TiffinService",
  "locationId": "{locationId}"
}
```

### Create Delivery Area

```http
POST /api/v1/FoodProviders/{foodProviderId}/delivery-areas
```

```json
{
  "areaId": "{areaId}",
  "radiusKm": 5,
  "deliveryFee": 30
}
```

### Create Food Item

```http
POST /api/v1/FoodItems
```

```json
{
  "foodProviderId": "{foodProviderId}",
  "itemName": "Veg Meals",
  "price": 90,
  "description": "Rice, curry, vegetables, and pickle",
  "foodType": "Veg"
}
```

### Create Food Plan

```http
POST /api/v1/FoodPlans
```

```json
{
  "foodProviderId": "{foodProviderId}",
  "planName": "Monthly Lunch Plan",
  "durationDays": 30,
  "price": 2500,
  "mealsPerDay": 1,
  "description": "Lunch delivered every working day."
}
```

### Add Food Plan Item

```http
POST /api/v1/FoodPlans/{foodPlanId}/items
```

```json
{
  "foodItemId": "{foodItemId}",
  "mealSlot": "Lunch"
}
```

### Create Food Subscription

```http
POST /api/v1/FoodSubscriptions
```

```json
{
  "foodPlanId": "{foodPlanId}",
  "userId": "{userId}",
  "startDate": "2026-09-01",
  "deliveryLocationId": "{locationId}"
}
```

Then test:

```http
PATCH /api/v1/FoodSubscriptions/{foodSubscriptionId}/activate
PATCH /api/v1/FoodSubscriptions/{foodSubscriptionId}/cancel
```

Cancel request:

```json
{
  "endDate": "2026-09-30"
}
```

## 21. Messaging APIs

### Create Conversation

```http
POST /api/v1/Conversations
```

```json
{
  "conversationType": "WorkerEmployer",
  "participantUserIds": ["{userId}", "{employerUserId}"],
  "subject": "Job discussion",
  "relatedEntityType": "Job",
  "relatedEntityId": "{jobId}",
  "createdBy": "{userId}"
}
```

### Add Participant

```http
POST /api/v1/Conversations/{conversationId}/participants/{userId}
```

No body required.

### Send Message

```http
POST /api/v1/Messages/conversation/{conversationId}
```

```json
{
  "senderUserId": "{userId}",
  "body": "Hello, I am interested in this evening job.",
  "attachments": [
    {
      "fileUrl": "https://example.com/files/resume.pdf",
      "fileName": "resume.pdf",
      "mimeType": "application/pdf",
      "fileSizeBytes": 250000
    }
  ]
}
```

### Edit Message

```http
PATCH /api/v1/Messages/{messageId}
```

```json
{
  "body": "Hello, I am available for the evening job."
}
```

## 22. Notification APIs

### Create Notification

```http
POST /api/v1/Notifications
```

```json
{
  "userId": "{userId}",
  "notificationType": "JobUpdate",
  "title": "Application shortlisted",
  "body": "Your application was shortlisted.",
  "entityType": "JobApplication",
  "entityId": "{applicationId}"
}
```

Then test:

```http
PATCH /api/v1/Notifications/{notificationId}/mark-sent
PATCH /api/v1/Notifications/{notificationId}/mark-read
GET /api/v1/Notifications/by-user/{userId}
```

## 23. Billing APIs

### Create Invoice

```http
POST /api/v1/Invoices
```

```json
{
  "userId": "{userId}",
  "invoiceNumber": "INV-2026-0001",
  "subtotalAmount": 1000,
  "taxAmount": 180,
  "currency": "INR",
  "dueAt": "2026-09-15T00:00:00Z"
}
```

### Create Payment

```http
POST /api/v1/Payments
```

```json
{
  "userId": "{userId}",
  "payableEntityType": "Invoice",
  "payableEntityId": "{invoiceId}",
  "amount": 1180,
  "invoiceId": "{invoiceId}",
  "currency": "INR",
  "paymentMethod": "UPI",
  "providerName": "TestProvider",
  "providerPaymentId": "pay_test_001"
}
```

### Add Transaction

```http
POST /api/v1/Payments/{paymentId}/transactions
```

```json
{
  "transactionType": "capture",
  "amount": 1180,
  "status": "Paid",
  "providerTransactionId": "txn_test_001"
}
```

Then test:

```http
PATCH /api/v1/Payments/{paymentId}/mark-paid
PATCH /api/v1/Payments/{paymentId}/mark-failed
PATCH /api/v1/Invoices/{invoiceId}/mark-paid
GET /api/v1/Payments/by-user/{userId}
GET /api/v1/Invoices/by-user/{userId}
```

### Create Billing Subscription

```http
POST /api/v1/BillingSubscriptions
```

```json
{
  "userId": "{userId}",
  "planCode": "premium-employer-monthly",
  "startsAt": "2026-09-01T00:00:00Z",
  "providerName": "TestProvider",
  "providerSubscriptionId": "sub_test_001"
}
```

## 24. Review, Complaint, Report APIs

### Create Review

```http
POST /api/v1/Reviews
```

```json
{
  "reviewerUserId": "{userId}",
  "targetEntityType": "EmployerProfile",
  "targetEntityId": "{employerProfileId}",
  "rating": 5,
  "reviewText": "Good communication and timely payment.",
  "relatedEntityType": "Contract",
  "relatedEntityId": "{contractId}"
}
```

Then test:

```http
GET /api/v1/Reviews/for-entity?targetEntityType=EmployerProfile&targetEntityId={employerProfileId}
PATCH /api/v1/Reviews/{reviewId}/flag
PATCH /api/v1/Reviews/{reviewId}/hide
```

### Create Complaint

```http
POST /api/v1/Complaints
```

```json
{
  "complainantUserId": "{userId}",
  "targetEntityType": "Job",
  "targetEntityId": "{jobId}",
  "complaintCategory": "MisleadingInformation",
  "description": "Job details did not match the actual work."
}
```

Resolve request:

```http
PATCH /api/v1/Complaints/{complaintId}/resolve
```

```json
{
  "resolutionNotes": "Employer updated the job description."
}
```

### Create Report

```http
POST /api/v1/Reports
```

```json
{
  "reportType": "DailyMarketplaceSummary",
  "generatedBy": "{adminUserId}",
  "parameters": "{\"date\":\"2026-08-18\"}"
}
```

Complete report:

```http
PATCH /api/v1/Reports/{reportId}/complete
```

```json
{
  "reportUrl": "https://example.com/reports/daily-summary.pdf"
}
```

## 25. Analytics and Personalization APIs

### Create Analytics Event

```http
POST /api/v1/AnalyticsEvents
```

```json
{
  "eventName": "job_viewed",
  "userId": "{userId}",
  "anonymousId": null,
  "source": "web",
  "sessionId": null,
  "entityType": "Job",
  "entityId": "{jobId}"
}
```

### Create User Behavior Event

```http
POST /api/v1/UserBehaviorEvents
```

```json
{
  "eventName": "job_apply_clicked",
  "userId": "{userId}",
  "entityType": "Job",
  "entityId": "{jobId}"
}
```

### Create Search History

```http
POST /api/v1/SearchHistory
```

```json
{
  "searchScope": "Jobs",
  "userId": "{userId}",
  "queryText": "evening retail jobs",
  "resultCount": 12,
  "locationId": "{locationId}"
}
```

### Create Recommendation

```http
POST /api/v1/Recommendations
```

```json
{
  "userId": "{userId}",
  "recommendationType": "Job",
  "targetEntityType": "Job",
  "targetEntityId": "{jobId}",
  "score": 92.5,
  "modelVersion": "recommendation-v1"
}
```

Then test:

```http
PATCH /api/v1/Recommendations/{recommendationId}/click
PATCH /api/v1/Recommendations/{recommendationId}/dismiss
PATCH /api/v1/Recommendations/{recommendationId}/convert
```

### Upsert User Preference

```http
PUT /api/v1/UserPreferences/by-user/{userId}
```

```json
{
  "preferenceScope": "job_search",
  "preferences": "{\"preferredShift\":\"Evening\",\"maxDistanceKm\":8}"
}
```

## 26. Endpoint Catalog

Health:

- `GET /api/v1/Health`

Identity/RBAC:

- `GET /api/v1/Users/{userId}`
- `GET /api/v1/Users/{userId}/roles`
- `POST /api/v1/Users/{userId}/roles/{roleId}`
- `DELETE /api/v1/Users/{userId}/roles/{roleId}`
- `GET /api/v1/Roles`
- `GET /api/v1/Roles/{roleId}`
- `GET /api/v1/Roles/{roleId}/permissions`
- `POST /api/v1/Roles/{roleId}/permissions/{permissionId}`
- `GET /api/v1/Permissions`
- `GET /api/v1/Permissions/{permissionId}`
- `POST /api/v1/Permissions`
- `GET /api/v1/Verifications/{verificationId}`
- `GET /api/v1/Verifications/by-user/{userId}`
- `POST /api/v1/Verifications`
- `PATCH /api/v1/Verifications/{verificationId}/verify`
- `PATCH /api/v1/Verifications/{verificationId}/fail`
- `PATCH /api/v1/Verifications/{verificationId}/revoke`

Locations:

- `GET/POST /api/v1/Countries`
- `GET /api/v1/Countries/{countryId}`
- `GET/POST /api/v1/States`
- `GET /api/v1/States/{stateId}`
- `GET/POST /api/v1/Cities`
- `GET /api/v1/Cities/{cityId}`
- `GET/POST /api/v1/Areas`
- `GET /api/v1/Areas/{areaId}`
- `GET /api/v1/Locations/{locationId}`
- `POST /api/v1/Locations`

Jobs and workers:

- `GET/POST /api/v1/WorkerProfiles`
- `GET /api/v1/WorkerProfiles/{workerProfileId}`
- `GET /api/v1/WorkerProfiles/by-user/{userId}`
- `PATCH /api/v1/WorkerProfiles/{workerProfileId}/headline`
- `PATCH /api/v1/WorkerProfiles/{workerProfileId}/expected-salary`
- `POST /api/v1/WorkerProfiles/{workerProfileId}/skills`
- `POST /api/v1/WorkerProfiles/{workerProfileId}/experience`
- `POST /api/v1/WorkerProfiles/{workerProfileId}/education`
- `GET/POST /api/v1/EmployerProfiles`
- `GET /api/v1/EmployerProfiles/{employerProfileId}`
- `GET /api/v1/EmployerProfiles/by-user/{userId}`
- `GET/POST /api/v1/Skills`
- `GET /api/v1/Skills/{skillId}`
- `GET/POST /api/v1/JobCategories`
- `GET /api/v1/JobCategories/{jobCategoryId}`
- `GET/POST /api/v1/Jobs`
- `GET /api/v1/Jobs/{jobId}`
- `PATCH /api/v1/Jobs/{jobId}/publish`
- `PATCH /api/v1/Jobs/{jobId}/close`
- `GET/POST /api/v1/Jobs/{jobId}/locations`
- `GET/POST /api/v1/Jobs/{jobId}/schedules`
- `GET/POST /api/v1/Jobs/{jobId}/skills`
- `GET/POST /api/v1/JobApplications`
- `GET /api/v1/JobApplications/{applicationId}`
- `PATCH /api/v1/JobApplications/{applicationId}/status`
- `GET /api/v1/JobApplications/{applicationId}/history`
- `GET/POST /api/v1/JobApplications/{applicationId}/shortlist`
- `GET /api/v1/Contracts/{contractId}`
- `POST /api/v1/Contracts`
- `PATCH /api/v1/Contracts/{contractId}/activate`
- `PATCH /api/v1/Contracts/{contractId}/complete`
- `PATCH /api/v1/Contracts/{contractId}/cancel`
- `PATCH /api/v1/Contracts/{contractId}/terminate`
- `GET /api/v1/MatchingScores/by-job/{jobId}`
- `GET /api/v1/MatchingScores/by-worker/{workerProfileId}`
- `POST /api/v1/MatchingScores`

Accommodation:

- `GET/POST /api/v1/AccommodationProviders`
- `GET /api/v1/AccommodationProviders/{accommodationProviderId}`
- `PATCH /api/v1/AccommodationProviders/{accommodationProviderId}/verify`
- `GET/POST /api/v1/Facilities`
- `GET/POST /api/v1/RoomTypes`
- `GET /api/v1/Properties/{propertyId}`
- `GET /api/v1/Properties/by-provider/{accommodationProviderId}`
- `POST /api/v1/Properties`
- `PATCH /api/v1/Properties/{propertyId}/deactivate`
- `POST /api/v1/Properties/{propertyId}/images`
- `POST /api/v1/Properties/{propertyId}/facilities`
- `GET /api/v1/Rooms/{roomId}`
- `GET /api/v1/Rooms/by-property/{propertyId}`
- `POST /api/v1/Rooms`
- `GET/POST /api/v1/Rooms/{roomId}/availability`
- `GET /api/v1/AccommodationBookings/{bookingId}`
- `GET /api/v1/AccommodationBookings/by-worker/{workerProfileId}`
- `POST /api/v1/AccommodationBookings`
- `PATCH /api/v1/AccommodationBookings/{bookingId}/confirm`
- `PATCH /api/v1/AccommodationBookings/{bookingId}/check-in`
- `PATCH /api/v1/AccommodationBookings/{bookingId}/complete`
- `PATCH /api/v1/AccommodationBookings/{bookingId}/cancel`
- `PATCH /api/v1/AccommodationBookings/{bookingId}/reject`

Food:

- `GET/POST /api/v1/FoodProviders`
- `GET /api/v1/FoodProviders/{foodProviderId}`
- `PATCH /api/v1/FoodProviders/{foodProviderId}/verify`
- `GET/POST /api/v1/FoodProviders/{foodProviderId}/delivery-areas`
- `GET /api/v1/FoodItems/{foodItemId}`
- `GET /api/v1/FoodItems/by-provider/{foodProviderId}`
- `POST /api/v1/FoodItems`
- `GET /api/v1/FoodPlans/{foodPlanId}`
- `GET /api/v1/FoodPlans/by-provider/{foodProviderId}`
- `POST /api/v1/FoodPlans`
- `POST /api/v1/FoodPlans/{foodPlanId}/items`
- `GET /api/v1/FoodSubscriptions/{foodSubscriptionId}`
- `GET /api/v1/FoodSubscriptions/by-user/{userId}`
- `POST /api/v1/FoodSubscriptions`
- `PATCH /api/v1/FoodSubscriptions/{foodSubscriptionId}/activate`
- `PATCH /api/v1/FoodSubscriptions/{foodSubscriptionId}/cancel`

Communication:

- `GET /api/v1/Conversations/{conversationId}`
- `GET /api/v1/Conversations/by-user/{userId}`
- `POST /api/v1/Conversations`
- `POST /api/v1/Conversations/{conversationId}/participants/{userId}`
- `PATCH /api/v1/Conversations/{conversationId}/participants/{userId}/mark-read`
- `GET /api/v1/Messages/by-conversation/{conversationId}`
- `POST /api/v1/Messages/conversation/{conversationId}`
- `PATCH /api/v1/Messages/{messageId}`
- `DELETE /api/v1/Messages/{messageId}`
- `GET /api/v1/Notifications/{notificationId}`
- `GET /api/v1/Notifications/by-user/{userId}`
- `POST /api/v1/Notifications`
- `PATCH /api/v1/Notifications/{notificationId}/mark-sent`
- `PATCH /api/v1/Notifications/{notificationId}/mark-read`

Billing:

- `GET /api/v1/BillingSubscriptions/{billingSubscriptionId}`
- `GET /api/v1/BillingSubscriptions/by-user/{userId}`
- `POST /api/v1/BillingSubscriptions`
- `PATCH /api/v1/BillingSubscriptions/{billingSubscriptionId}/activate`
- `PATCH /api/v1/BillingSubscriptions/{billingSubscriptionId}/cancel`
- `GET /api/v1/Invoices/{invoiceId}`
- `GET /api/v1/Invoices/by-user/{userId}`
- `POST /api/v1/Invoices`
- `PATCH /api/v1/Invoices/{invoiceId}/mark-paid`
- `GET /api/v1/Payments/{paymentId}`
- `GET /api/v1/Payments/by-user/{userId}`
- `POST /api/v1/Payments`
- `PATCH /api/v1/Payments/{paymentId}/mark-paid`
- `PATCH /api/v1/Payments/{paymentId}/mark-failed`
- `POST /api/v1/Payments/{paymentId}/transactions`

Trust, reports, and analytics:

- `GET /api/v1/Reviews/{reviewId}`
- `GET /api/v1/Reviews/for-entity`
- `POST /api/v1/Reviews`
- `PATCH /api/v1/Reviews/{reviewId}/flag`
- `PATCH /api/v1/Reviews/{reviewId}/hide`
- `GET/POST /api/v1/Complaints`
- `GET /api/v1/Complaints/{complaintId}`
- `PATCH /api/v1/Complaints/{complaintId}/assign/{assignedTo}`
- `PATCH /api/v1/Complaints/{complaintId}/resolve`
- `PATCH /api/v1/Complaints/{complaintId}/reject`
- `PATCH /api/v1/Complaints/{complaintId}/escalate`
- `GET/POST /api/v1/Reports`
- `GET /api/v1/Reports/{reportId}`
- `PATCH /api/v1/Reports/{reportId}/complete`
- `PATCH /api/v1/Reports/{reportId}/fail`
- `GET /api/v1/AuditLogs/by-user/{actorUserId}`
- `POST /api/v1/AuditLogs`
- `GET /api/v1/AnalyticsEvents/by-user/{userId}`
- `POST /api/v1/AnalyticsEvents`
- `GET /api/v1/UserBehaviorEvents/by-user/{userId}`
- `POST /api/v1/UserBehaviorEvents`
- `GET /api/v1/SearchHistory/by-user/{userId}`
- `POST /api/v1/SearchHistory`
- `GET /api/v1/Recommendations/by-user/{userId}`
- `POST /api/v1/Recommendations`
- `PATCH /api/v1/Recommendations/{recommendationId}/click`
- `PATCH /api/v1/Recommendations/{recommendationId}/dismiss`
- `PATCH /api/v1/Recommendations/{recommendationId}/convert`
- `GET /api/v1/UserPreferences/by-user/{userId}`
- `PUT /api/v1/UserPreferences/by-user/{userId}`

## 27. Common Swagger Test Failures

### 404 Not Found

Likely causes:

- Wrong route casing or wrong controller name.
- ID does not exist.
- Dependent entity was not created before testing.

Fix:

- Copy the route directly from Swagger.
- Use IDs returned from previous successful `POST` calls.

### 400 Bad Request

Likely causes:

- Missing required JSON property.
- Invalid enum string.
- Invalid date/time format.
- Required numeric value is zero or negative.
- Salary min is greater than salary max.

Fix:

- Use ISO date format for `DateOnly`: `2026-09-01`.
- Use time format for `TimeOnly`: `18:00:00`.
- Use ISO datetime format for `DateTimeOffset`: `2026-09-01T00:00:00Z`.

### 401 Unauthorized

Likely causes:

- Protected endpoint called without JWT.
- JWT expired.
- JWT signing key/issuer/audience mismatch.

Fix:

- Click Swagger `Authorize`.
- Add a valid bearer token.
- Confirm `Jwt:Issuer`, `Jwt:Audience`, and `Jwt:Key` match token generation settings.

### 403 Forbidden

Likely causes:

- Token is valid but role is not allowed.
- Admin endpoint called by non-admin user.

Fix:

- Add required role claim to token.
- Assign role in database or through role assignment endpoint.

### 503 Service Unavailable

Likely cause:

- Database is not reachable.

Fix:

- Start PostgreSQL.
- Check `appsettings.Development.json`.
- Verify database name, username, password, and port.

## 28. Minimum Manual Test Checklist

Use this checklist before saying Swagger testing is complete:

- Health endpoint returns `200`.
- Swagger loads without application startup errors.
- At least one `POST`, `GET`, and `PATCH` endpoint works.
- Location chain works: country -> state -> city -> area -> location.
- Worker profile can be created and fetched.
- Employer profile can be created and fetched.
- Skill and job category can be created.
- Job can be created, published, fetched, and searched.
- Job application can be submitted and status updated.
- Accommodation provider, property, room, and booking flow works.
- Food provider, item, plan, and subscription flow works.
- Conversation and message flow works.
- Invoice, payment, and transaction flow works.
- Review, complaint, and report flow works.
- Analytics event, search history, recommendation, and preference flows work.
- Protected endpoint returns `401` without token.
- Protected endpoint returns success with a valid admin token.
- Invalid request bodies return `400`.
- Random non-existing IDs return `404`.

## 29. What Should Be Added Next for Complete Auth Testing

To fully test register and login from Swagger, implement:

- `AuthController`
- `RegisterRequest`
- `LoginRequest`
- `AuthResponse`
- Password hashing service
- JWT token generator
- Refresh token/session persistence
- Register worker/employer/admin role assignment flow
- Login failed-attempt lockout integration with `User.RecordFailedLogin()`
- Login success integration with `User.RecordSuccessfulLogin()`
- Tests for register, login, refresh, logout, lockout, and role claims

Recommended future endpoints:

```http
POST /api/v1/Auth/register
POST /api/v1/Auth/login
POST /api/v1/Auth/refresh-token
POST /api/v1/Auth/logout
POST /api/v1/Auth/forgot-password
POST /api/v1/Auth/reset-password
```

Once these endpoints exist, the Swagger test order should begin with register -> login -> authorize -> protected workflows.
