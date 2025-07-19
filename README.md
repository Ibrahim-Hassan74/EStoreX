## EStoreX API Documentation

### Overview

EStoreX is a RESTful API for managing products, categories, and customer baskets in an e-commerce system. This documentation outlines the endpoints available for the client, along with request/response formats and expected behaviors.

---

### Authentication

Currently, this API does **not require authentication**.

---

## Endpoints

### Categories

#### `GET /api/categories`

* **Description**: Retrieve all categories.
* **Response**:

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

* **Description**: Get category by ID.
* **Response**:

```json
{
  "id": "guid",
  "name": "string",
  "description": "string"
}
```

#### `POST /api/categories`

* **Description**: Create a new category.
* **Request Body**:

```json
{
  "name": "string",
  "description": "string"
}
```

#### `PUT /api/categories/{id}`

* **Description**: Update a category by ID.
* **Request Body**:

```json
{
  "id": "guid",
  "name": "string",
  "description": "string"
}
```

#### `DELETE /api/categories/{id}`

* **Description**: Delete a category by ID.

---

### Products

#### `GET /api/products`

* **Description**: Retrieve products with optional filters.
* **Query Parameters**:

  * `searchBy`
  * `searchString`
  * `minPrice`
  * `maxPrice`
  * `categoryId`
  * `sortBy`
  * `sortOrder` (ASC/DESC)
  * `pageNumber`
  * `pageSize`
* **Response**:

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

* **Description**: Retrieve product by ID.

#### `POST /api/products`

* **Description**: Create a product.
* **Request Body** (multipart/form-data):

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

* **Response**:

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

* **Description**: Update a product.
* **Request Body** (multipart/form-data): same as `POST` but includes `id`.

#### `DELETE /api/products/{id}`

* **Description**: Delete a product by ID.

---

### Baskets

#### `GET /api/baskets/{id}`

* **Description**: Get a customer's basket by ID.
* **Response**:

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

* **Description**: Add or update a customer basket.
* **Request Body**:

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

* **Description**: Delete a customer basket by ID.

---

### Errors (Testing Only)

#### `GET /api/bug/error` - returns `500`

#### `GET /api/bug/not-found` - returns `404`

#### `GET /api/bug/bad-request` - returns `400`

---

## Notes

* IDs are represented as GUID strings.
* The basket ID is a plain string (not GUID) representing the customer.
* Validation errors will return status code `400`.
* Use proper `Content-Type: multipart/form-data` for file upload endpoints.

