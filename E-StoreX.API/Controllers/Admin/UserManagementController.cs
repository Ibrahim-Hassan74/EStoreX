using EStoreX.Core.DTO;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// user management controller for administrative operations
    /// </summary>
    public class UserManagementController : AdminControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        /// <summary>
        /// User management controller constructor
        /// </summary>
        /// <param name="userManagementService">Service responsible for management</param>
        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        /// <summary>
        /// Get all users (Admin or SuperAdmin).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagementService.GetAllUsersAsync();
            return Ok(users);
        }
        /// <summary>
        /// Get a user by id (Admin or SuperAdmin).
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManagementService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        /// <summary>
        /// Create an Admin (SuperAdmin only).
        /// </summary>
        [HttpPost("add-admin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddAdmin([FromBody] CreateAdminDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userManagementService.AddAdminAsync(dto);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Delete an admin permanently (SuperAdmin only).
        /// </summary>
        [HttpDelete("admin/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            var result = await _userManagementService.DeleteAdminAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Delete a user (rules depend on roles: SuperAdmin/Admin).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized("User not authenticated.");

            var result = await _userManagementService.DeleteUserAsync(id, currentUserId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Deactivate a user (Admin or SuperAdmin).
        /// </summary>
        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(string id)
        {
            var result = await _userManagementService.DeactivateUserAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Activate a user (Admin or SuperAdmin).
        /// </summary>
        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var result = await _userManagementService.ActivateUserAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Assign a specified role to a user. Accessible only by Super Admin.
        /// </summary>
        [HttpPost("assign-role")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AssignRole([FromBody] UpdateUserRoleDTO dto)
        {
            var result = await _userManagementService.AssignRoleToUserAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Remove a specified role from a user. Accessible only by Super Admin.
        /// </summary>
        [HttpPost("remove-role")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RemoveRole([FromBody] UpdateUserRoleDTO dto)
        {
            var result = await _userManagementService.RemoveRoleFromUserAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

    }
}
