
using RedisExample.API.Models;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExample.API.Repository
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private const string productKey = "productCaches";
      
        private readonly IProductRepository _productPepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;
        //service alamayız.Bizim işimiz burda db ile ilgili.Service de iş kodları yazar
        public ProductRepositoryWithCacheDecorator(IProductRepository productPepository, RedisService redisService)
        {
            _productPepository = productPepository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDb(2);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _productPepository.CreateAsync(product);
           
            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newProduct));
            }
            return newProduct;

        }

        public async Task<List<Product>> GetAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
            
                return await LoadToCacheFromDbAsync();
            var products= new List<Product>();

            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var item in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }
            return products;
            
        }

        public async Task<Product> GetByIDAsync(int id)
        {
            if (_cacheRepository.KeyExists(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }
            var products =await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x=>x.Id==id);


        }
        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products=await _productPepository.GetAsync();
            products.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });
            return products;
        }

       //redisstring-redislist-redisSet-redisstoredset-redishash
       //burada redishash kullanacağız.Key value tuttuğumuz için direkt arama yapabiliyoruz
    }
}
