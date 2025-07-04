﻿using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Repositories.UserRepository;
using SWD392_backend.Models.Requests;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Services.UserService
{
    public interface IUserService
    {
        Task<List<user>> GetAllUserAsync();
        Task<user?> GetUserByIdAsync(int id);
        Task<int> GetTotalUserAsync();

        Task AddUserAsync(UserRequest request);

        Task<int> GetTotalUsersByMonth(int month, int year);

        Task<int> GetUserCountByExactDay(DateTime day);
    }
}