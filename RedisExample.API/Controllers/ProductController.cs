using Microsoft.AspNetCore.Mvc;
using RedisExample.API.Models;
using RedisExample.API.Repository;
using RedisExampleApp.Cache;

namespace RedisExample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        //kontroller üzerine tüm datalar redisten gelsin istiyoruz. Kontroller ve repository tarafında herhangi bir değişiklik
        //yapmadan bu işlemleri yapmalıyız
            //DecoratorDesignPattern:Bir uygulamnın kodlarını değiştirmeden o uygulamaya yeni davranışlara imkan verir.
            //controller hangi interface'i biliyorsa onun repositorysi ile işlem yapacağız

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepository.GetAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productRepository.GetByIDAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            return Created(string.Empty, await _productRepository.CreateAsync(product));
        }
    }
}
