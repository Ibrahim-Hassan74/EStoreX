using E_StoreX.API.Helper;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Public
{
    /// <summary>
    /// 
    /// </summary>
    [Route("errors/{statusCode}")]
    public class ErrorController : CustomControllerBase
    {
        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult(new ResponseAPI(statusCode));
        }
    }
}
