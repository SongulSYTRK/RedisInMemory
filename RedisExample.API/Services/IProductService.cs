using RedisExample.API.Models;

namespace RedisExample.API.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetByIDAsync(int id);
        Task<Product> CreateAsync(Product product);
    }
}
