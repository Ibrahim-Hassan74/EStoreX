using EStoreX.Core.RepositoryContracts;
using EStoreX.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreX.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork   
    {
        private readonly ApplicationDbContext _context;
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IPhotoRepository PhotoRepository { get; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ProductRepository = new ProductRepository(_context);
            CategoryRepository = new CategoryRepository(_context);
            PhotoRepository = new PhotoRepository(_context);
        }
    }
}
