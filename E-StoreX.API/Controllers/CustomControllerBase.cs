using EStoreX.Core.RepositoryContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// Base controller for the E-StoreX API, providing common functionality for all controllers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Constructor for CustomControllerBase, initializing the unit of work.
        /// </summary>
        /// <param name="unitOfWork"></param>

        public CustomControllerBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
