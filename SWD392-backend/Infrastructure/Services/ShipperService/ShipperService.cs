using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Repositories.ShipperRepository;
using SWD392_backend.Models.Request;

namespace SWD392_backend.Infrastructure.Services.ShipperService
{
    public class ShipperService : IShipperService
    {
        private readonly IShipperRepository _shipperRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShipperService(IShipperRepository shipperRepository, IUnitOfWork unitOfWork)
        {
            _shipperRepository = shipperRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AssignAreaAsync(int userId, AssignAreaRequest request)
        {
            var shipper = await _shipperRepository.GetShipperByUserIdAsync(userId);
            if (shipper == null)
                return false;

            var areaCode = $"{request.ProvinceCode}_{request.DistrictCode}_{request.WardCode}";

            shipper.AreaCode = areaCode;

            await _shipperRepository.AssignAreaAsync(shipper);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<shipper> GetShipperByUserIdAsync(int userId)
        {
            return await _shipperRepository.GetShipperByUserIdAsync(userId);
        }
    }
}
