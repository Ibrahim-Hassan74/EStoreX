## EStoreX API Documentation

### Overview

EStoreX is a RESTful API for managing products, categories, and customer baskets in an e-commerce system. This documentation outlines the endpoints available for the client, along with request/response formats and expected behaviors.

---

### Authentication

This API uses **JWT-based authentication** with support for user registration and login.

#### `POST /api/account/register`

- **Description**: Register a new user. Sends a confirmation email with a token.
- **Request Body**:

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

To ensure strong account security, passwords must meet the following criteria:

- Minimum 8 characters
- At least 1 uppercase letter (A–Z)
- At least 1 lowercase letter (a–z)
- At least 1 digit (0–9)
- Special characters (e.g. ! @ # $ %) — optional but recommended

Example of a valid password:  
StrongPass1

If any of these conditions are not met, the API will return:

- 400 Bad Request
- With detailed error messages in the `errors.password[]` field of the response

- **Responses**:
  - `200`: Registration successful, email sent
  - `400`: Validation error or missing fields
  - `409`: Email or username already in use

_Example response (200):_

```json
{
  "success": true,
  "message": "Registration successful. Please check your email to confirm your account.",
  "statusCode": 200
}
```

_Example response (400):_

```json
{
  "success": false,
  "message": "Validation failed.",
  "statusCode": 400,
  "errors": {
    "email": ["Email can't be blank"],
    "password": ["Password should be at least 7 characters"]
  }
}
```

_Example response (409):_

```json
{
  "success": false,
  "message": "Email is already in use.",
  "statusCode": 409
}
```

---

#### `POST /api/account/login`

- **Description**: Login using email and password. Returns JWT token on success.
- **Request Body**:

```json
{
  "email": "user@example.com",
  "password": "string",
  "rememberMe": true
}
```

- **Responses**:
  - `200`: Login successful with token
  - `401`: Invalid credentials
  - `403`: Email not confirmed or not allowed
  - `404`: User not found
  - `423`: Account locked due to failed attempts

_Example response (200):_

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

_Example response (400):_

```json
{
  "success": false,
  "message": "Validation failed.",
  "statusCode": 400,
  "errors": {
    "email": ["Email can't be blank"],
    "password": ["Password should be at least 7 characters"]
  }
}
```

_Example response (401):_

```json
{
  "success": false,
  "message": "Invalid email or password.",
  "statusCode": 401
}
```

_Example response (403):_

```json
{
  "success": false,
  "message": "You need to confirm your email before logging in.",
  "statusCode": 403
}
```

_Example response (404):_

```json
{
  "success": false,
  "message": "User with the provided email does not exist.",
  "statusCode": 404
}
```

_Example response (423):_

```json
{
  "success": false,
  "message": "Account is locked due to multiple failed login attempts. Please try again later.",
  "statusCode": 423
}
```

---

### Notes

- JWT Token is required for secured endpoints  
  Add this header:  
  `Authorization: Bearer YOUR_JWT_TOKEN`
- Email confirmation is **mandatory** before login
- A redirect URL is included in the confirmation email:
  - `estorex://account-verified` for mobile apps
  - `https://localhost:4200/active` for web
- Lockout policies apply after multiple failed login attempts

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
