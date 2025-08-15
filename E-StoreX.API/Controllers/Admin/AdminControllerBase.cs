using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EStoreX.Core.Enums;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// Abstract base controller for admin-related operations.
    /// </summary>
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{nameof(UserTypeOptions.Admin)},{nameof(UserTypeOptions.SuperAdmin)}")]
    public class AdminControllerBase : ControllerBase
    {
    }
}
