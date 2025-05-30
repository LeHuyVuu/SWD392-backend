namespace SWD392_backend.Infrastructure.Repositories.OrderRepository;

// IOrderRepository.cs
using SWD392_backend.Entities;
using System;
using System.Threading.Tasks;

public interface IOrderRepository
{
    Task AddAsync(order entity);
    // Thêm các method cần thiết khác nếu muốn, ví dụ GetById, Update...
    IQueryable<order> GetAll();

}