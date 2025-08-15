using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// Abstract base controller for admin-related operations.
    /// </summary>
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminControllerBase : ControllerBase
    {
    }
}
