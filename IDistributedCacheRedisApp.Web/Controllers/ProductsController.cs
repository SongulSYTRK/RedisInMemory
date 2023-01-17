using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            /*  _distributedCache.SetString("name","Songül", distributedCacheEntryOptions);
             await _distributedCache.SetStringAsync("surname","Songül", distributedCacheEntryOptions);*/
            Product product = new Product { Id=1,Name="Kalem",Price=100};
            string jsonproduct = JsonConvert.SerializeObject(product);

            Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct);
            _distributedCache.Set("product:1",byteproduct);
         // await _distributedCache.SetStringAsync("product:1", jsonproduct,distributedCacheEntryOptions);
            return View();
        }
        public IActionResult Show()
        {
            Byte[] byteproduct = _distributedCache.Get("product:1");


            string jsonproduct = Encoding.UTF8.GetString(byteproduct);

           // string jsonproduct = _distributedCache.GetString("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(jsonproduct);
            ViewBag.product = p;
          /*  string name = _distributedCache.GetString("name");
            ViewBag.name = name;*/
            return View();
        }
        public IActionResult Remove()
        {
           _distributedCache.Remove("name");
            return View();
        }
        public IActionResult ImageUrl()
        {
            byte[] resimbyte = _distributedCache.Get("resim");
            return File(resimbyte,"image/jpeg");
        }
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images/meleksuaktas_hayvanyuvası.jpeg");
            byte[] imageByte=System.IO.File.ReadAllBytes(path); 
            _distributedCache.Set("resim",imageByte);
            return View();
        }

    }
}
