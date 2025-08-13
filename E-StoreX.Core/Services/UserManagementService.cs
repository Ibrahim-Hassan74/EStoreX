using AutoMapper;
using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO;
using EStoreX.Core.Enums;
using EStoreX.Core.Helper;
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
                return AuthenticationResponseFactory.Failure("Failed to create admin.", 400, result.Errors.Select(x => x.Description).ToArray());

            if (!await _roleManager.RoleExistsAsync(UserTypeOptions.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() });
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            return AuthenticationResponseFactory.Success("Admin created successfully.");
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> AssignRoleToUserAsync(UpdateUserRoleDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return AuthenticationResponseFactory.Failure("User not found.", 404, "User not found.");

            if (!await _roleManager.RoleExistsAsync(dto.Role.ToString()))
                await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() });

            var result = await _userManager.AddToRoleAsync(user, dto.Role.ToString());

            return result.Succeeded
                ? AuthenticationResponseFactory.Success("Role assigned successfully.")
                : AuthenticationResponseFactory.Failure("Failed to assign role.",400, result.Errors.Select(error => error.Description).ToArray());
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> RemoveRoleFromUserAsync(UpdateUserRoleDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return AuthenticationResponseFactory.Failure("User not found.", 404, "User not found.");


            var result = await _userManager.RemoveFromRoleAsync(user, dto.Role.ToString());

            return result.Succeeded
                ? AuthenticationResponseFactory.Success("Role removed successfully.")
                : AuthenticationResponseFactory.Failure("Failed to remove role.",400, result.Errors.Select(error => error.Description).ToArray());
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> ActivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return  AuthenticationResponseFactory.Failure("User not found.", 404, "User not found.");


            user.LockoutEnd = null;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
                ? AuthenticationResponseFactory.Success("User activated successfully.")
                : AuthenticationResponseFactory.Failure("Failed to activate user.",400, result.Errors.Select(error => error.Description).ToArray());
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> DeactivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return AuthenticationResponseFactory.Failure("User not found.", 404, "User not found.");


            user.LockoutEnd = DateTimeOffset.MaxValue;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
                ? AuthenticationResponseFactory.Success("User deactivated successfully.")
                : AuthenticationResponseFactory.Failure("Failed to deactivate user.",400, result.Errors.Select(error => error.Description).ToArray());
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> DeleteUserAsync(string targetUserId, string currentUserId)
        {
            var targetUser = await _userManager.FindByIdAsync(targetUserId);
            if (targetUser == null)
                return AuthenticationResponseFactory.Failure("User not found.", 404, "User not found.");

            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            if (currentUser == null)
                return AuthenticationResponseFactory.Failure("Unauthorized.",404);

            var targetRoles = await _userManager.GetRolesAsync(targetUser);
            var currentRoles = await _userManager.GetRolesAsync(currentUser);

            bool isCurrentSuperAdmin = currentRoles.Contains("SuperAdmin");
            bool isCurrentAdmin = currentRoles.Contains("Admin");

            bool isTargetAdmin = targetRoles.Contains("Admin");
            bool isTargetSuperAdmin = targetRoles.Contains("SuperAdmin");

            if (isTargetSuperAdmin && !isCurrentSuperAdmin)
                return AuthenticationResponseFactory.Failure("Only a Super Admin can delete another Super Admin.", 400);

            if (isTargetAdmin && !isCurrentSuperAdmin)
                return AuthenticationResponseFactory.Failure("Only a Super Admin can delete an Admin.", 400);

            if (!isTargetAdmin && !isTargetSuperAdmin && !(isCurrentAdmin || isCurrentSuperAdmin))
                return AuthenticationResponseFactory.Failure("Only Admin or Super Admin can delete a user.", 400);

            var result = await _userManager.DeleteAsync(targetUser);

            return result.Succeeded
                ? AuthenticationResponseFactory.Success("User deleted successfully.")
                : AuthenticationResponseFactory.Failure("Failed to delete user.",400, result.Errors.Select(e => e.Description).ToArray());
        }



        /// <inheritdoc/>
        public async Task<AuthenticationResponse> DeleteAdminAsync(string adminUserId)
        {
            var user = await _userManager.FindByIdAsync(adminUserId);
            if (user == null)
                return AuthenticationResponseFactory.Failure("User not found.", 404, "User not found.");

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded
                ? AuthenticationResponseFactory.Success("Admin deleted successfully.")
                : AuthenticationResponseFactory.Failure("Failed to delete admin.", 400, result.Errors.Select(e => e.Description).ToArray());
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
    }
}