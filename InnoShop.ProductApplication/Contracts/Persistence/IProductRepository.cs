using InnoShop.ProductDomain.Entities;
using InnoShop.Shared.Models;

namespace InnoShop.ProductApplication.Contracts.Persistence
{
    public interface IProductRepository
    {
        Task AddAsync (Product product );
        Task<PagedList<Product>> GetAllAsync(int pageNumber, int pageSize);
        Task UpdateAsync( Product product );
        Task<Product?> GetByIdAsync(Guid id);
        Task DeleteAsync(Product product);
        Task<PagedList<Product>> SearchAsync(string? searchText, decimal? minPrice, decimal? maxPrice, bool? isAvailable, int pageNumber, int pageSize);
        Task SetAvailabilityForUserProductsAsync(Guid userId, bool isAvailable);
    }
}
