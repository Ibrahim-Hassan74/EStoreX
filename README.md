## E-StoreX API Documentation

### Overview

EStoreX is a RESTful API for managing products, categories, and customer baskets in an e-commerce system. This documentation outlines the endpoints available for the client, along with request/response formats and expected behaviors.

---

# Authentication API – EStoreX

This API uses **JWT-based authentication** with support for user registration, login, email confirmation, and password reset.

---

## `POST /api/account/register`

**Description:**  
Register a new user. Sends a confirmation email with a token.

**Request Body:**

```json
{
  "userName": "string",
  "email": "user@example.com",
  "phone": "0123456789",
  "password": "string",
  "confirmPassword": "string"
}
```

### Password Requirements

- Minimum 8 characters
- At least 1 uppercase letter (A–Z)
- At least 1 lowercase letter (a–z)
- At least 1 digit (0–9)
- Special characters (e.g. ! @ # $ %) — optional but recommended

**Responses:**

- **200 OK:** Registration successful, email sent
- **400 Bad Request:** Validation error or missing fields
- **409 Conflict:** Email or username already in use

**Example response (200):**

```json
{
  "success": true,
  "message": "Registration successful. Please check your email to confirm your account.",
  "statusCode": 200
}
```

**Example response (400):**

```json
{
  "success": false,
  "message": "Validation failed.",
  "statusCode": 400,
  "errors": [
    "Email can't be blank",
    "Password should be at least 8 characters",
    "..."
  ]
}
```

**Example response (409):**

```json
{
  "success": false,
  "message": "Username or Email is already in use.",
  "statusCode": 409,
  "errors": ["The username or email is already taken.", "...."]
}
```

---

## `POST /api/account/login`

**Description:**  
Login using email and password. Returns JWT token on success.

**Request Body:**

```json
{
  "email": "user@example.com",
  "password": "string",
  "rememberMe": true
}
```

**Responses:**

- **200 OK:** Login successful with token
- **400 Bad Request:** Validation errors
- **401 Unauthorized:** Invalid credentials
- **403 Forbidden:** Email not confirmed
- **404 Not Found:** User not found
- **423 Locked:** Account locked due to failed attempts

**Example response (200):**

```json
{
  "success": true,
  "message": "Login successful.",
  "statusCode": 200,
  "userName": "string",
  "email": "string",
  "token": "JWT_TOKEN_HERE",
  "expiration": "2025-07-20T00:00:00Z",
  "refreshToken": "REFRESH_TOKEN_HERE",
  "refreshTokenExpirationDateTime": "2025-07-27T00:00:00Z"
}
```

**Example response (400):**

```json
{
  "success": false,
  "message": "Validation failed.",
  "statusCode": 400,
  "errors": ["Email can't be blank", "Password can't be blank"]
}
```

**Example response (401):**

```json
{
  "success": false,
  "message": "Invalid email or password.",
  "statusCode": 401,
  "errors": ["Incorrect email or password."]
}
```

**Example response (403):**

```json
{
  "success": false,
  "message": "You need to confirm your email before logging in.",
  "statusCode": 403,
  "errors": [
    "You must confirm your email before logging in.",
    "User is not allowed to login."
  ]
}
```

**Example response (404):**

```json
{
  "success": false,
  "message": "User with the provided email does not exist.",
  "statusCode": 404,
  "errors": ["No account found with this email."]
}
```

**Example response (423):**

```json
{
  "success": false,
  "message": "Account is locked due to multiple failed login attempts. Please try again later.",
  "statusCode": 423,
  "errors": [
    "Your account is temporarily locked due to multiple failed login attempts. Please try again later."
  ]
}
```

---

## `POST /api/account/forgot-password`

**Description:**  
Initiates the password reset process by sending a reset link to the user's email.

**Request Body:**

```json
{
  "email": "user@example.com"
}
```

**Responses:**

- **200 OK:** Password reset link sent successfully
- **400 Bad Request:** Email not found or not confirmed
- **429 Too Many Requests:** Password reset already requested recently

**Example response (200):**

```json
{
  "success": true,
  "message": "A password reset link has been sent to your email.",
  "statusCode": 200
}
```

**Example response (400):**

```json
{
  "success": false,
  "message": "Email is not confirmed.",
  "statusCode": 400,
  "errors": [
    "Email not found.",
    "Please confirm your email before resetting password."
  ]
}
```

**Example response (429):**

```json
{
  "success": false,
  "message": "A password reset email was already sent recently. Please wait before trying again.",
  "statusCode": 429,
  "errors": [
    "A password reset email was already sent recently. Please wait before trying again."
  ]
}
```

---

## `GET /api/account/reset-password/verify`

**Description:**  
Verifies the validity of a reset password token.

**Query Parameters:**

```
userId=<USER_ID>&token=<TOKEN>
```

**Responses:**

- **200 OK:** Token is valid
- **400 Bad Request:** Token or user is invalid
- **404 Not Found:** User not found

**Example response (200):**

```json
{
  "success": true,
  "message": "Token is valid.",
  "statusCode": 200
}
```

**Example response (400):**

```json
{
  "success": false,
  "message": "Invalid or expired token.",
  "statusCode": 400,
  "errors": ["Invalid verification request."]
}
```

---

## `POST /api/account/reset-password`

**Description:**  
Resets the user's password using a valid token.

**Request Body:**

```json
{
  "userId": "guid",
  "token": "RESET_TOKEN",
  "newPassword": "NewStrongPass1",
  "confirmPassword": "NewStrongPass1"
}
```

**Responses:**

- **200 OK:** Password reset successful
- **400 Bad Request:** Token invalid or password mismatch
- **404 Not Found:** User not found

**Example response (200):**

```json
{
  "success": true,
  "message": "Password reset successful.",
  "statusCode": 200
}
```

**Example response (400):**

```json
{
  "success": false,
  "message": "Passwords do not match or token is invalid.",
  "statusCode": 400,
  "errors": ["Invalid or expired token."]
}
```

### `POST /api/account/generate-new-jwt-token`

- **Description**:  
  Generates a **new access JWT token** using a valid **refresh token** when the old access token has expired.  
  This helps the user stay logged in without re-entering credentials, as long as the refresh token is valid.

---

### **Request Body**

```json
{
  "token": "EXPIRED_ACCESS_TOKEN",
  "refreshToken": "VALID_REFRESH_TOKEN"
}
```

---

### **Validation Rules**

- `token` (string): **Required**. Must be a valid JWT, even if expired.
- `refreshToken` (string): **Required**. Must match the one stored for the user and must not be expired.

---

### **Responses**

#### `200 OK` – **Token successfully refreshed**

```json
{
  "success": true,
  "message": "Token refreshed successfully.",
  "statusCode": 200,
  "userName": "string",
  "email": "user@example.com",
  "token": "NEW_JWT_ACCESS_TOKEN",
  "expiration": "2025-07-24T12:00:00Z",
  "refreshToken": "NEW_REFRESH_TOKEN",
  "refreshTokenExpirationDateTime": "2025-07-24T13:00:00Z"
}
```

#### `400 Bad Request` – **Invalid request**

```json
{
  "success": false,
  "message": "Invalid token or refresh token.",
  "statusCode": 400,
  "errors": ["Refresh token is invalid or expired."]
}
```

#### `401 Unauthorized` – **Token tampered or invalid**

```json
{
  "success": false,
  "message": "Invalid token signature or structure.",
  "statusCode": 401,
  "errors": ["Token tampered or invalid"]
}
```

#### `404 NotFound` – **User doesn't exist**

```json
{
  "success": false,
  "message": "User not found",
  "statusCode": 404,
  "errors": ["User does not exist."]
}
```

---

### **Notes**

- This endpoint should be called **only when the access token expires**.
- The new `refreshToken` returned in the response must replace the old one on the client side.
- If the refresh token is also expired, the user must **log in again**.

---

## Notes

- **JWT Token** is required for secured endpoints:  
  `Authorization: Bearer YOUR_JWT_TOKEN`
- **Email confirmation is mandatory** before login.
- Redirect URLs in emails:
  - `estorex://reset-password` for mobile apps
  - `https://localhost:4200/reset-password` for web
- Lockout policies apply after multiple failed login attempts.

## Endpoints

### Categories

#### `GET /api/categories`

- **Description**: Retrieve all categories.
- **Response**:

```json
[
  {
    "id": "guid",
    "name": "string",
    "description": "string"
  }
]
```

#### `GET /api/categories/{id}`

- **Description**: Get category by ID.
- **Response**:

```json
{
  "id": "guid",
  "name": "string",
  "description": "string"
}
```

#### `POST /api/categories`

- **Description**: Create a new category.
- **Request Body**:

```json
{
  "name": "string",
  "description": "string"
}
```

#### `PUT /api/categories/{id}`

- **Description**: Update a category by ID.
- **Request Body**:

```json
{
  "id": "guid",
  "name": "string",
  "description": "string"
}
```

#### `DELETE /api/categories/{id}`

- **Description**: Delete a category by ID.

---

### Products

#### `GET /api/products`

- **Description**: Retrieve products with optional filters.
- **Query Parameters**:

  - `searchBy`
  - `searchString`
  - `minPrice`
  - `maxPrice`
  - `categoryId`
  - `sortBy`
  - `sortOrder` (ASC/DESC)
  - `pageNumber`
  - `pageSize`

- **Response**:

```json
{
  "currentPage": 1,
  "pageSize": 10,
  "totalRecords": 100,
  "records": [
    {
      "id": "guid",
      "name": "string",
      "description": "string",
      "newPrice": 0,
      "oldPrice": 0,
      "categoryName": "string",
      "photos": [
        {
          "imageName": "string"
        }
      ]
    }
  ]
}
```

#### `GET /api/products/{id}`

- **Description**: Retrieve product by ID.

#### `POST /api/products`

- **Description**: Create a product.
- **Request Body** (multipart/form-data):

```json
{
  "name": "string",
  "description": "string",
  "newPrice": 0,
  "oldPrice": 0,
  "categoryId": "guid",
  "photos": ["file"]
}
```

- **Response**:

```json
{
  "id": "guid",
  "name": "string",
  "description": "string",
  "newPrice": 0,
  "oldPrice": 0,
  "categoryName": "string",
  "photos": [
    {
      "imageName": "string"
    }
  ]
}
```

#### `PUT /api/products/{id}`

- **Description**: Update a product.
- **Request Body** (multipart/form-data): same as `POST` but includes `id`.

#### `DELETE /api/products/{id}`

- **Description**: Delete a product by ID.

---

### Baskets

#### `GET /api/baskets/{id}`

- **Description**: Get a customer's basket by ID.
- **Response**:

```json
{
  "id": "string",
  "basketItems": [
    {
      "id": "guid",
      "name": "string",
      "description": "string",
      "qunatity": 1,
      "price": 0,
      "category": "string",
      "image": "url"
    }
  ]
}
```

#### `POST /api/baskets`

- **Description**: Add or update a customer basket.
- **Request Body**:

```json
{
  "id": "string",
  "basketItems": [
    {
      "id": "guid",
      "name": "string",
      "description": "string",
      "qunatity": 1,
      "price": 0,
      "category": "string",
      "image": "url"
    }
  ]
}
```

#### `DELETE /api/baskets/{id}`

- **Description**: Delete a customer basket by ID.

---

### Errors (Testing Only)

#### `GET /api/bug/error` - returns `500`

#### `GET /api/bug/not-found` - returns `404`

#### `GET /api/bug/bad-request` - returns `400`

---

## Notes

- IDs are represented as GUID strings.
- The basket ID is a plain string (not GUID) representing the customer.
- Validation errors will return status code `400`.
- Use proper `Content-Type: multipart/form-data` for file upload endpoints.
