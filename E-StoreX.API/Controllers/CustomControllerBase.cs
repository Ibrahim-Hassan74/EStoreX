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
        /// <summary>
        /// Constructor for CustomControllerBase, initializing the unit of work.
        /// </summary>

        public CustomControllerBase()
        {   
            // This constructor can be used to initialize common services or properties
            // that all derived controllers might need.
        }
    }
}
