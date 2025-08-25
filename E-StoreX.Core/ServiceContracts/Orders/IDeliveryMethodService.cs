using EStoreX.Core.DTO.Orders.Requests;
using EStoreX.Core.DTO.Orders.Responses;

namespace EStoreX.Core.ServiceContracts.Orders
{
    public interface IDeliveryMethodService
    {
        /// <summary>
        /// Retrieves all delivery methods.
        /// </summary>
        /// <returns>A list of <see cref="DeliveryMethodResponse"/>.</returns>
        Task<IEnumerable<DeliveryMethodResponse>> GetAllAsync();

        /// <summary>
        /// Retrieves a delivery method by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the delivery method.</param>
        /// <returns>
        /// Returns the <see cref="DeliveryMethodResponse"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<DeliveryMethodResponse?> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates a new delivery method.
        /// </summary>
        /// <param name="request">The <see cref="DeliveryMethodRequest"/> containing delivery method details.</param>
        /// <returns>The created <see cref="DeliveryMethodResponse"/>.</returns>
        Task<DeliveryMethodResponse> CreateAsync(DeliveryMethodRequest request);

        /// <summary>
        /// Updates an existing delivery method.
        /// </summary>
        /// <param name="id">The unique identifier of the delivery method to update.</param>
        /// <param name="request">The <see cref="DeliveryMethodRequest"/> containing updated details.</param>
        /// <returns>
        /// Returns the updated <see cref="DeliveryMethodResponse"/> if the update succeeded; otherwise, <c>null</c> if not found.
        /// </returns>
        Task<DeliveryMethodResponse?> UpdateAsync(Guid id, DeliveryMethodRequest request);

        /// <summary>
        /// Deletes a delivery method by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the delivery method to delete.</param>
        /// <returns><c>true</c> if deletion succeeded; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteAsync(Guid id);
        /// <summary>
        /// Gets a delivery method by its name.
        /// </summary>
        /// <param name="name">delivery method name</param>
        Task<DeliveryMethodResponse?> GetByNameAsync(string name);
    }
}
