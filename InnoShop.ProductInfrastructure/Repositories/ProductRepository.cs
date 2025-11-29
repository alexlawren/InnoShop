using InnoShop.ProductApplication.Contracts.Persistence;
using InnoShop.ProductDomain.Entities;
using InnoShop.ProductInfrastructure.Persistence;
using InnoShop.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.ProductInfrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedList<Product>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Products
                .AsNoTracking()              
                .Where(p => p.IsAvailable == true)   
                .OrderByDescending(p => p.CreatedAt); 

            return await PagedList<Product>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<PagedList<Product>> SearchAsync(string? searchText, decimal? minPrice, decimal? maxPrice, bool? isAvailable, int pageNumber, int pageSize)
        {
            var query = _context.Products.AsNoTracking().AsQueryable();

            if (!isAvailable.HasValue)
            {
                query = query.Where(p => p.IsAvailable == true);
            }
            else 
            {
                query = query.Where(p => p.IsAvailable == isAvailable.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchText.ToLower()) ||
                    p.Description.ToLower().Contains(searchText.ToLower()));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            query = query.OrderByDescending(p => p.CreatedAt);

            return await PagedList<Product>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task SetAvailabilityForUserProductsAsync(Guid userId, bool isAvailable)
        {
            await _context.Products
                .Where(p => p.UserId == userId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsAvailable, isAvailable));
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        
    }
}
