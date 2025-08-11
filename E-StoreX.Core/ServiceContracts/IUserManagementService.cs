using EStoreX.Core.DTO;

namespace EStoreX.Core.ServiceContracts
{
    /// <summary>
    /// Provides administrative operations for managing users and admins.
    /// </summary>
    public interface IUserManagementService
    {
        /// <summary>
        /// Adds a new administrator account to the system.
        /// </summary>
        /// <param name="dto">
        /// The data required to create a new admin, including username, email, password, and any role-specific settings.
        /// </param>
        /// <returns>
        /// An <see cref="AuthenticationResponse"/> indicating whether the creation was successful (StatusCode 200)
        /// or failed with details (e.g., StatusCode 400 for validation errors or StatusCode 409 if email/username already exists).
        /// </returns>
        Task<AuthenticationResponse> AddAdminAsync(CreateAdminDTO dto);

        /// <summary>
        /// Permanently deletes an administrator account by their unique user ID.
        /// </summary>
        /// <param name="adminUserId">
        /// The unique identifier of the admin to remove.
        /// </param>
        /// <returns>
        /// An <see cref="AuthenticationResponse"/> indicating whether the deletion was successful (StatusCode 200),
        /// or failed due to invalid ID or insufficient permissions (StatusCode 400/403/404).
        /// </returns>
        Task<AuthenticationResponse> DeleteAdminAsync(string adminUserId);

        /// <summary>
        /// Permanently deletes a regular (non-admin) user account by their unique user ID.
        /// </summary>
        /// <param name="targetUserId">
        /// The unique identifier of the user to remove.
        /// </param>
        /// <param name="currentUserId">
        /// The unique identifier of the user want to remove another.
        /// </param>
        /// <returns>
        /// An <see cref="AuthenticationResponse"/> indicating whether the deletion was successful (StatusCode 200),
        /// or failed due to invalid ID or insufficient permissions (StatusCode 400/403/404).
        /// </returns>
        Task<AuthenticationResponse> DeleteUserAsync(string targetUserId, string currentUserId);

        /// <summary>
        /// Temporarily disables a user account, preventing them from logging in until reactivated.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user to deactivate.
        /// </param>
        /// <returns>
        /// An <see cref="AuthenticationResponse"/> indicating whether the deactivation was successful (StatusCode 200),
        /// or failed due to invalid ID or insufficient permissions (StatusCode 400/403/404).
        /// </returns>
        Task<AuthenticationResponse> DeactivateUserAsync(string userId);

        /// <summary>
        /// Reactivates a previously deactivated user account, restoring their login access.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user to reactivate.
        /// </param>
        /// <returns>
        /// An <see cref="AuthenticationResponse"/> indicating whether the activation was successful (StatusCode 200),
        /// or failed due to invalid ID or insufficient permissions (StatusCode 400/403/404).
        /// </returns>
        Task<AuthenticationResponse> ActivateUserAsync(string userId);

        /// <summary>
        /// Retrieves a list of all user accounts in the system, including basic profile details.
        /// </summary>
        /// <returns>
        /// A list of <see cref="ApplicationUserResponse"/> objects containing user details.
        /// If no users are found, an empty list is returned.
        /// </returns>
        Task<List<ApplicationUserResponse>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves detailed information for a specific user by their unique ID.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user to retrieve.
        /// </param>
        /// <returns>
        /// An <see cref="ApplicationUserResponse"/> containing user details if found,
        /// or <c>null</c> if the user does not exist.
        /// </returns>
        Task<ApplicationUserResponse?> GetUserByIdAsync(string userId);

        /// <summary>
        /// Assigns a specific role to an existing user.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user to update.
        /// </param>
        /// <param name="roleName">
        /// The name of the role to assign (e.g., "Admin", "Manager").
        /// </param>
        /// <returns>— An<see cref = "AuthenticationResponse" /> indicating whether the role assignment
        /// was successful (StatusCode 200) or failed due to invalid user, role, or insufficient 
        /// permissions (StatusCode 400/403/404).
        /// </returns>
        Task<AuthenticationResponse> AssignRoleToUserAsync(UpdateUserRoleDTO updateUserRoleDTO);

        /// <summary>
        /// Removes a specific role from an existing user without deleting the account.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user to update.
        /// </param>
        /// <param name="roleName">
        /// The name of the role to remove (e.g., "Admin", "Manager").
        /// </param>
        /// <returns>— An<see cref = "AuthenticationResponse" /> indicating whether the role removal
        /// was successful (StatusCode 200) or failed due to invalid user, role, or insufficient 
        /// permissions (StatusCode 400/403/404).
        /// </returns>
        Task<AuthenticationResponse> RemoveRoleFromUserAsync(UpdateUserRoleDTO updateUserRoleDTO);

    }

}
