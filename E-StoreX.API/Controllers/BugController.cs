using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// Controller for handling bug-related operations.
    /// </summary>
    public class BugController : CustomControllerBase
    {
        /// <summary>
        /// Returns a 500 Internal Server Error response.
        /// </summary>
        /// <returns>A 500 status code with a message indicating an internal server error.</returns>
        [HttpGet("error")]
        public IActionResult GetError()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
        }

        /// <summary>
        /// Returns a 404 Not Found response.
        /// </summary>
        /// <returns>A 404 status code with a message indicating the resource was not found.</returns>
        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound("Resource not found");
        }
        ///  <summary>
        ///  Returns a 400 Bad Request response.
        ///  </summary>
        ///  <returns>A 400 status code with a message indicating the resource was bad-request</returns>
        [HttpGet("bad-request/{Id:guid}")]
        public IActionResult GetBadRequest(Guid Id)
        {
            return BadRequest($"Bad Request for Id: {Id}");
        }
        ///  <summary>
        ///  Returns a 400 Bad Request response.
        ///  </summary>
        ///  <returns>A 400 status code with a message indicating the resource was bad-request</returns>
        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return BadRequest();
        }
    }
}
