﻿using RedisExample.API.Models;
using RedisExample.API.Repository;

namespace RedisExample.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> CreateAsync(Product product)
        {
           return  await _productRepository.CreateAsync(product);
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _productRepository.GetAsync();
        }

        public async Task<Product> GetByIDAsync(int id)
        {
            return await _productRepository.GetByIDAsync(id);
        }
    }
}
