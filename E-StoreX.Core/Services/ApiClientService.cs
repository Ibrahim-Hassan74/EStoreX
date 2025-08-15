using EStoreX.Core.Domain.Entities;
using EStoreX.Core.RepositoryContracts;
using ServiceContracts;

namespace EStoreX.Core.Services
{
    public class ApiClientService : IApiClientService
    {
        private readonly IApiClientRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public ApiClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repo = _unitOfWork.ApiClientRepository;
        }

        /// <inheritdoc/>
        public async Task<ApiClient> CreateClientAsync(string clientName)
        {
            string apiKey;

            do
            {
                apiKey = GenerateApiKey();
            }
            while (await _repo.ApiKeyExistsAsync(apiKey));

            var client = new ApiClient
            {
                Id = Guid.NewGuid(),
                ClientName = clientName,
                ApiKey = apiKey,
                IsActive = true
            };

            await _repo.AddAsync(client);

            await _unitOfWork.CompleteAsync();

            return client;
        }

        /// <inheritdoc/>
        public Task<ApiClient?> GetByApiKeyAsync(string apiKey)
            => _repo.GetByApiKeyAsync(apiKey);

        /// <summary>
        /// Generates a secure random API key.
        /// </summary>
        /// <param name="length">Length of the key.</param>
        /// <returns>Generated API key.</returns>
        private static string GenerateApiKey(int length = 64)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

}
