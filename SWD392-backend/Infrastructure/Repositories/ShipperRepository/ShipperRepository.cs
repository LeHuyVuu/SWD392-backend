using Microsoft.EntityFrameworkCore;
using SWD392_backend.Context;
using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Repositories.ShipperRepository
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly MyDbContext _context;

        public ShipperRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AssignAreaAsync(shipper shipper)
        {
            _context.shipper.Update(shipper);
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<shipper?> GetShipperByUserIdAsync(int userId)
        {
            return await _context.shipper.FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}
