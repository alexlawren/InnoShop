using InnoShop.Domain.Entities;
using InnoShop.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<PagedList<User>> GetAllAsync(int pageNumber, int pageSize);
        Task<User?> GetByIdAsync(Guid id); 
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}
