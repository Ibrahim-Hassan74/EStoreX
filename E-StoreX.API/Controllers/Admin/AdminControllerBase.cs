using Asp.Versioning;
using EStoreX.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// Abstract base controller for admin-related operations.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    [Authorize(Roles = $"{nameof(UserTypeOptions.Admin)},{nameof(UserTypeOptions.SuperAdmin)}")]
    public class AdminControllerBase : ControllerBase
    {
    }
}
