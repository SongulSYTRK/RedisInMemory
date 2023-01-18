﻿using RedisExample.API.Models;

namespace RedisExample.API.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetByIDAsync(int id);
        Task<Product> CreateAsync(Product product);
    }
}
