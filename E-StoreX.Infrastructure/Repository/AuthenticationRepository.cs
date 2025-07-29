using Domain.Entities;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EStoreX.Infrastructure.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address?> GetAddress(Guid userId)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.ApplicationUserId == userId);
            return address;
        }

        public async Task<bool> UpdateAddress(Guid userId, Address address)
        {
            var userAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.ApplicationUserId == userId);
            if (userAddress == null)
            {
                address.ApplicationUserId = userId;
                await _context.Addresses.AddAsync(address);
            }
            else
            {
                userAddress.Id = address.Id;
                _context.Addresses.Update(userAddress);
            }
            int res = await _context.SaveChangesAsync();
            return res > 0;
        }
    }
}
