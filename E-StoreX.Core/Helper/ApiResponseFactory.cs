using EStoreX.Core.DTO.Common;

namespace EStoreX.Core.Helper
{
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
    }

}
