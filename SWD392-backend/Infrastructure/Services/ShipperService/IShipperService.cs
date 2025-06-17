using SWD392_backend.Entities;
using SWD392_backend.Models.Request;

namespace SWD392_backend.Infrastructure.Services.ShipperService
{
    public interface IShipperService
    {
        Task<bool> AssignAreaAsync(int userId, AssignAreaRequest request);

        Task<shipper> GetShipperByUserIdAsync(int userId);
    }
}
