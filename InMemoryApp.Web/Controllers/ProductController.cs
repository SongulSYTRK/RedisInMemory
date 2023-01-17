using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            /* /////1.yol
             if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
             {
                 _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
             }*/
            //get:data almak için set:data kaydetmek için 
            /////2.yol
            ////// if (_memoryCache.TryGetValue("zaman", out string zamancache))

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
           // options.SlidingExpiration= TimeSpan.FromSeconds(10);
           
            options.Priority = CacheItemPriority.High;
            options.RegisterPostEvictionCallback((key, value, reason, state) => 
            {
            _memoryCache.Set("callback",$"{key}->{value}=> sebep: {reason}");
                
            });
            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            //AbsoluteExpiration : belirtilen zamandan sonra refresh atılınca veri silindiği için ulaşamazsn(durmadan refresh atsan da belirtilen süre dolunca veri gelmez)
            //SlidingExpiration: belirtilen zaman kadar refresh atılmazsa atıldığında yeri veri gelmez.(biraz bekle refresh at )
            //cachepriority: datanın önemliliğini belirleriz. (high , loww . neverremove )
            //neverremove derseniz sürekli hiç silemediği için bir zaman sonra exception fırlatır 

            //registerpostevictioncallback: memoryden gitme sebebi 


            Product p = new Product { Id=1,Name="Kalem", Price=200};
            _memoryCache.Set<Product>("product=1",p);
            return View();
        }
        public IActionResult Show()
        {
            /*  _memoryCache.Remove("zaman");
              _memoryCache.GetOrCreate<string>("zaman", entry =>
              {
                  return DateTime.Now.ToString();
              });*/
            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callback);
            

            //  ViewBag.zaman = _memoryCache.Get<string>("zaman");
            ViewBag.zaman = zamancache;
            ViewBag.callback = callback;
            ViewBag.product = _memoryCache.Get<Product>("product=1");
            return View();
        }
    }
}
