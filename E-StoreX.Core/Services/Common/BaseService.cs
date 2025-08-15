using AutoMapper;
using EStoreX.Core.RepositoryContracts.Common;
using System;

namespace EStoreX.Core.Services.Common
{
    public class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
