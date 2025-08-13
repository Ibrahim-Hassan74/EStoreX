using EStoreX.Core.DTO;
namespace EStoreX.Core.Helper
{
    public static class AuthenticationResponseFactory
    {
        public static AuthenticationFailureResponse Failure(string message, int statusCode, params string[] errors)
        {
            return new AuthenticationFailureResponse
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Errors = errors.ToList()
            };
        }

        public static AuthenticationResponse Success(string message)
        {
            return new AuthenticationResponse
            {
                Success = true,
                Message = message,
                StatusCode = 200
            };
        }
    }

}
