using EStoreX.Core.DTO.Common;

namespace EStoreX.Core.Helper
{
    /// <summary>
    /// Provides factory methods to create standardized API error responses with common HTTP status codes.
    /// </summary>
    public static class ApiResponseFactory
    {
        public static ApiErrorResponse Failure(string message, int statusCode, params string[] errors)
        {
            return new ApiErrorResponse
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Errors = errors.ToList()
            };
        }

        public static ApiResponse Success(string message)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                StatusCode = 200
            };
        }

        /// <summary>
        /// Creates a 400 Bad Request error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 400.</returns>
        public static ApiErrorResponse BadRequest(string message = "Bad Request", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 400,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }

        /// <summary>
        /// Creates a 401 Unauthorized error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 401.</returns>
        public static ApiErrorResponse Unauthorized(string message = "Unauthorized", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 401,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }

        /// <summary>
        /// Creates a 403 Forbidden error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 403.</returns>
        public static ApiErrorResponse Forbidden(string message = "Forbidden", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 403,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }

        /// <summary>
        /// Creates a 404 Not Found error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 404.</returns>
        public static ApiErrorResponse NotFound(string message = "Not Found", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 404,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }

        /// <summary>
        /// Creates a 409 Conflict error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 409.</returns>
        public static ApiErrorResponse Conflict(string message = "Conflict", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 409,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }

        /// <summary>
        /// Creates a 410 Gone error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 410.</returns>
        public static ApiErrorResponse Gone(string message = "Gone", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 410,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }

        /// <summary>
        /// Creates a 415 Unsupported Media Type error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 415.</returns>
        public static ApiErrorResponse UnsupportedMediaType(string message = "Unsupported Media Type", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 415,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }

        /// <summary>
        /// Creates a 429 Too Many Requests error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 429.</returns>
        public static ApiErrorResponse TooManyRequests(string message = "Too Many Requests", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 429,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }

        /// <summary>
        /// Creates a 500 Internal Server Error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 500.</returns>
        public static ApiErrorResponse InternalServerError(string message = "Internal Server Error", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 500,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }

        /// <summary>
        /// Creates a 503 Service Unavailable error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 503.</returns>
        public static ApiErrorResponse ServiceUnavailable(string message = "Service Unavailable", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 503,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }

        /// <summary>
        /// Creates a 501 Not Implemented error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 501.</returns>
        public static ApiErrorResponse NotImplemented(string message = "Not Implemented", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 501,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }

        /// <summary>
        /// Creates a 408 Request Timeout error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional detailed error list.</param>
        /// <returns>An <see cref="ApiErrorResponse"/> with status 408.</returns>
        public static ApiErrorResponse RequestTimeout(string message = "Request Timeout", List<string>? errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = 408,
                Message = message,
                Errors = errors ?? new List<string>(),
                Success = false
            };
        }
    }

}
