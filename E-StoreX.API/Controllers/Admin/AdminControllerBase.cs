using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// Abstract base controller for admin-related operations.
    /// </summary>
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminControllerBase : ControllerBase
    {
    }
}
