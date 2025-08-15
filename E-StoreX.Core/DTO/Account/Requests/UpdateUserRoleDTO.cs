using EStoreX.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Account.Requests
{
    public class UpdateUserRoleDTO
    {
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; } = string.Empty;
        [Required(ErrorMessage = "Role is required.")]
        public UserTypeOptions Role { get; set; }
    }
}
