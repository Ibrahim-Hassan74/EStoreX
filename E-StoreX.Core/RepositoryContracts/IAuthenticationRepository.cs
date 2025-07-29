using Domain.Entities;

namespace EStoreX.Core.RepositoryContracts
{
    public interface IAuthenticationRepository
    {
        /// <summary>
        /// Updates the address information for the user with the specified user ID.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user whose address is being updated.
        /// </param>
        /// <param name="address">
        /// The new <see cref="Address"/> details to be associated with the user.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <c>true</c>
        /// if the address was updated successfully; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> UpdateAddress(Guid userId, Address address);

        /// <summary>
        /// Retrieves the address associated with a specific user.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user whose address is to be retrieved.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the <see cref="Address"/> of the specified user.
        /// </returns>
        Task<Address?> GetAddress(Guid userId);


    }
}
