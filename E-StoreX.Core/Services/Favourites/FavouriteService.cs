using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.Domain.Entities.Favourites;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Products.Responses;
using EStoreX.Core.Helper;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.RepositoryContracts.Favourites;
using EStoreX.Core.ServiceContracts.Favourites;
using EStoreX.Core.Services.Common;

namespace EStoreX.Core.Services.Favourites
{
    public class FavouriteService : BaseService, IFavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;

        public FavouriteService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _favouriteRepository = _unitOfWork.FavouriteRepository;
        }
        /// <inheritdoc/>
        public async Task<ApiResponse> AddToFavouriteAsync(Guid userId, Guid productId)
        {
            var favourite = new Favourite
            {
                UserId = userId,
                ProductId = productId
            };

            if (await _favouriteRepository.IsFavouriteAsync(favourite))
                return ApiResponseFactory.Conflict("Product already in favourites");

            await _favouriteRepository.AddToFavouriteAsync(favourite);
            return ApiResponseFactory.Success("Product added to favourites successfully");
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> RemoveFromFavouriteAsync(Guid userId, Guid productId)
        {
            var favourite = new Favourite
            {
                UserId = userId,
                ProductId = productId
            };

            if (!await _favouriteRepository.IsFavouriteAsync(favourite))
                return ApiResponseFactory.NotFound("Product not found in favourites");

            var removed = await _favouriteRepository.RemoveFromFavouriteAsync(favourite);

            if (!removed)
                return ApiResponseFactory.InternalServerError("Failed to remove product from favourites");

            return ApiResponseFactory.Success("Product removed from favourites successfully");
        }

        /// <inheritdoc/>
        public async Task<List<ProductResponse>> GetUserFavouritesAsync(Guid userId)
        {
            var favourites = await _favouriteRepository.GetUserFavouritesAsync(userId);
            return _mapper.Map<List<ProductResponse>>(favourites);
        }
    }
}
