using AutoMapper;
using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO;
using EStoreX.Core.Enums;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace EStoreX.Core.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public UserManagementService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        /// <inheritdoc/>
        public async Task<AuthenticationResponse> AddAdminAsync(CreateAdminDTO dto)
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
            {
                return FailureResponse("Failed to create admin.", result.Errors);
            }

            if (!await _roleManager.RoleExistsAsync(UserTypeOptions.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() });
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            return new AuthenticationResponse
            {
                Success = true,
                StatusCode = 201,
                Message = "Admin created successfully."
            };
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> AssignRoleToUserAsync(UpdateUserRoleDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return NotFoundResponse("User not found.");

            if (!await _roleManager.RoleExistsAsync(dto.Role.ToString()))
                await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() });

            var result = await _userManager.AddToRoleAsync(user, dto.Role.ToString());

            return result.Succeeded
                ? SuccessResponse("Role assigned successfully.")
                : FailureResponse("Failed to assign role.", result.Errors);
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> RemoveRoleFromUserAsync(UpdateUserRoleDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return NotFoundResponse("User not found.");

            var result = await _userManager.RemoveFromRoleAsync(user, dto.Role.ToString());

            return result.Succeeded
                ? SuccessResponse("Role removed successfully.")
                : FailureResponse("Failed to remove role.", result.Errors);
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> ActivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFoundResponse("User not found.");

            user.LockoutEnd = null;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
                ? SuccessResponse("User activated successfully.")
                : FailureResponse("Failed to activate user.", result.Errors);
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> DeactivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFoundResponse("User not found.");

            user.LockoutEnd = DateTimeOffset.MaxValue;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
                ? SuccessResponse("User deactivated successfully.")
                : FailureResponse("Failed to deactivate user.", result.Errors);
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> DeleteUserAsync(string targetUserId, string currentUserId)
        {
            var targetUser = await _userManager.FindByIdAsync(targetUserId);
            if (targetUser == null)
                return NotFoundResponse("User not found.");

            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            if (currentUser == null)
                return FailureResponse("Unauthorized.", Enumerable.Empty<IdentityError>());

            var targetRoles = await _userManager.GetRolesAsync(targetUser);
            var currentRoles = await _userManager.GetRolesAsync(currentUser);

            bool isCurrentSuperAdmin = currentRoles.Contains("SuperAdmin");
            bool isCurrentAdmin = currentRoles.Contains("Admin");

            bool isTargetAdmin = targetRoles.Contains("Admin");
            bool isTargetSuperAdmin = targetRoles.Contains("SuperAdmin");

            if (isTargetSuperAdmin && !isCurrentSuperAdmin)
                return FailureResponse("Only a Super Admin can delete another Super Admin.", Enumerable.Empty<IdentityError>());

            if (isTargetAdmin && !isCurrentSuperAdmin)
                return FailureResponse("Only a Super Admin can delete an Admin.", Enumerable.Empty<IdentityError>());

            if (!isTargetAdmin && !isTargetSuperAdmin && !(isCurrentAdmin || isCurrentSuperAdmin))
                return FailureResponse("Only Admin or Super Admin can delete a user.", Enumerable.Empty<IdentityError>());

            var result = await _userManager.DeleteAsync(targetUser);

            return result.Succeeded
                ? SuccessResponse("User deleted successfully.")
                : FailureResponse("Failed to delete user.", result.Errors ?? Enumerable.Empty<IdentityError>());
        }



        /// <inheritdoc/>
        public async Task<AuthenticationResponse> DeleteAdminAsync(string adminUserId)
        {
            var user = await _userManager.FindByIdAsync(adminUserId);
            if (user == null)
                return NotFoundResponse("Admin not found.");

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded
                ? SuccessResponse("Admin deleted successfully.")
                : FailureResponse("Failed to delete admin.", result.Errors);
        }

        /// <inheritdoc/>
        public async Task<List<ApplicationUserResponse>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var responses = _mapper.Map<List<ApplicationUserResponse>>(users);

            foreach (var response in responses)
            {
                var user = users.First(u => u.Id.ToString() == response.Id);
                response.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            }

            return responses;
        }

        /// <inheritdoc/>
        public async Task<ApplicationUserResponse?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var response = _mapper.Map<ApplicationUserResponse>(user);
            response.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            return response;
        }



        #region ---------- Helper methods ----------
        private AuthenticationResponse SuccessResponse(string message) =>
            new AuthenticationResponse
            {
                Success = true,
                StatusCode = 200,
                Message = message
            };

        private AuthenticationResponse NotFoundResponse(string message) =>
            new AuthenticationResponse
            {
                Success = false,
                StatusCode = 404,
                Message = message
            };

        private AuthenticationResponse FailureResponse(string message, IEnumerable<IdentityError> errors) =>
            new AuthenticationFailureResponse
            {
                Success = false,
                StatusCode = 400,
                Message = message,
                Errors = errors.Select(e => e.Description).ToList()
            };
        #endregion
    }
}