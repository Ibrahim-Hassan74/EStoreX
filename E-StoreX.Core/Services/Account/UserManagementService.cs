using AutoMapper;
using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO.Account.Requests;
using EStoreX.Core.DTO.Account.Responses;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.Enums;
using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Account;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EStoreX.Core.Services.Account
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserManagementService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <inheritdoc/>
        public async Task<ApiResponse> AddAdminAsync(CreateAdminDTO dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                EmailConfirmed = true,
                PhoneNumber = dto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return ApiResponseFactory.Failure("Failed to create admin.", 400, result.Errors.Select(x => x.Description).ToArray());

            if (!await _roleManager.RoleExistsAsync(UserTypeOptions.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() });
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            return ApiResponseFactory.Success("Admin created successfully.");
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> AssignRoleToUserAsync(UpdateUserRoleDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return ApiResponseFactory.Failure("User not found.", 404, "User not found.");

            if (!await _roleManager.RoleExistsAsync(dto.Role.ToString()))
                await _roleManager.CreateAsync(new ApplicationRole() { Name = dto.Role.ToString() });

            var result = await _userManager.AddToRoleAsync(user, dto.Role.ToString());

            return result.Succeeded
                ? ApiResponseFactory.Success("Role assigned successfully.")
                : ApiResponseFactory.Failure("Failed to assign role.", 400, result.Errors.Select(error => error.Description).ToArray());
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> RemoveRoleFromUserAsync(UpdateUserRoleDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return ApiResponseFactory.Failure("User not found.", 404, "User not found.");


            var result = await _userManager.RemoveFromRoleAsync(user, dto.Role.ToString());

            return result.Succeeded
                ? ApiResponseFactory.Success("Role removed successfully.")
                : ApiResponseFactory.Failure("Failed to remove role.", 400, result.Errors.Select(error => error.Description).ToArray());
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> ActivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponseFactory.Failure("User not found.", 404, "User not found.");

            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            var currentRoles = await _userManager.GetRolesAsync(currentUser!);

            var targetRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Contains("Admin"))
            {
                if (targetRoles.Contains("Admin") || targetRoles.Contains("SuperAdmin"))
                    return ApiResponseFactory.Failure("Admins cannot activate Admins or SuperAdmins.", 403);
            }

            if (currentRoles.Contains("SuperAdmin"))
            {
                if (targetRoles.Contains("SuperAdmin"))
                    return ApiResponseFactory.Failure("SuperAdmins cannot activate other SuperAdmins.", 403);
            }

            user.LockoutEnd = null;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
                ? ApiResponseFactory.Success("User activated successfully.")
                : ApiResponseFactory.Failure("Failed to activate user.", 400, result.Errors.Select(error => error.Description).ToArray());
        }


        /// <inheritdoc/>
        public async Task<ApiResponse> DeactivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponseFactory.Failure("User not found.", 404, "User not found.");

            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            var currentRoles = await _userManager.GetRolesAsync(currentUser!);

            // Get target user roles
            var targetRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Contains("Admin"))
            {
                if (targetRoles.Contains("Admin") || targetRoles.Contains("SuperAdmin"))
                    return ApiResponseFactory.Failure("Admins cannot deactivate Admins or SuperAdmins.", 403);
            }

            if (currentRoles.Contains("SuperAdmin"))
            {
                if (targetRoles.Contains("SuperAdmin"))
                    return ApiResponseFactory.Failure("SuperAdmins cannot deactivate other SuperAdmins.", 403);
            }

            user.LockoutEnd = DateTimeOffset.MaxValue;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
                ? ApiResponseFactory.Success("User deactivated successfully.")
                : ApiResponseFactory.Failure("Failed to deactivate user.", 400, result.Errors.Select(error => error.Description).ToArray());
        }


        /// <inheritdoc/>
        public async Task<ApiResponse> DeleteUserAsync(string targetUserId, string currentUserId)
        {
            var targetUser = await _userManager.FindByIdAsync(targetUserId);
            if (targetUser == null)
                return ApiResponseFactory.Failure("User not found.", 404, "User not found.");

            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            if (currentUser == null)
                return ApiResponseFactory.Failure("Unauthorized.", 404);

            var targetRoles = await _userManager.GetRolesAsync(targetUser);
            var currentRoles = await _userManager.GetRolesAsync(currentUser);

            bool isCurrentSuperAdmin = currentRoles.Contains("SuperAdmin");
            bool isCurrentAdmin = currentRoles.Contains("Admin");

            bool isTargetAdmin = targetRoles.Contains("Admin");
            bool isTargetSuperAdmin = targetRoles.Contains("SuperAdmin");

            if (isTargetSuperAdmin && !isCurrentSuperAdmin)
                return ApiResponseFactory.Failure("Only a Super Admin can delete another Super Admin.", 400);

            if (isTargetAdmin && !isCurrentSuperAdmin)
                return ApiResponseFactory.Failure("Only a Super Admin can delete an Admin.", 400);

            if (!isTargetAdmin && !isTargetSuperAdmin && !(isCurrentAdmin || isCurrentSuperAdmin))
                return ApiResponseFactory.Failure("Only Admin or Super Admin can delete a user.", 400);

            var result = await _userManager.DeleteAsync(targetUser);

            return result.Succeeded
                ? ApiResponseFactory.Success("User deleted successfully.")
                : ApiResponseFactory.Failure("Failed to delete user.", 400, result.Errors.Select(e => e.Description).ToArray());
        }



        /// <inheritdoc/>
        public async Task<ApiResponse> DeleteAdminAsync(string adminUserId)
        {
            var user = await _userManager.FindByIdAsync(adminUserId);
            if (user == null)
                return ApiResponseFactory.Failure("User not found.", 404, "User not found.");

            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            var currentRoles = await _userManager.GetRolesAsync(currentUser!);

            var targetRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Contains("Admin"))
            {
                return ApiResponseFactory.Failure("Admins cannot delete Admins or SuperAdmins.", 403);
            }

            if (currentRoles.Contains("SuperAdmin"))
            {
                if (targetRoles.Contains("SuperAdmin"))
                    return ApiResponseFactory.Failure("SuperAdmins cannot delete other SuperAdmins.", 403);
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded
                ? ApiResponseFactory.Success("Admin deleted successfully.")
                : ApiResponseFactory.Failure("Failed to delete admin.", 400, result.Errors.Select(e => e.Description).ToArray());
        }


        /// <inheritdoc/>
        public async Task<List<ApplicationUserResponse>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                .Include(u => u.Photo) 
                .ToListAsync();

            var filteredUsers = new List<ApplicationUser>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(nameof(UserTypeOptions.User)))
                {
                    filteredUsers.Add(user);
                }
            }

            var responses = _mapper.Map<List<ApplicationUserResponse>>(filteredUsers);

            foreach (var response in responses)
            {
                var user = filteredUsers.First(u => u.Id.ToString() == response.Id);
                response.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            }

            return responses;
        }



        /// <inheritdoc/>
        public async Task<ApplicationUserResponse?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.Users.Include(u => u.Photo).FirstOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null) return null;

            var response = _mapper.Map<ApplicationUserResponse>(user);
            response.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            return response;
        }
        /// <inheritdoc/>
        public async Task<List<ApplicationUserResponse>> GetAdminsAsync()
        {
            var admins = await _userManager.GetUsersInRoleAsync(nameof(UserTypeOptions.Admin));
            var superAdmins = await _userManager.GetUsersInRoleAsync(nameof(UserTypeOptions.SuperAdmin));
            var res = admins.Concat(superAdmins).Distinct().ToList();
            return _mapper.Map<List<ApplicationUserResponse>>(res);
        }
    }
}