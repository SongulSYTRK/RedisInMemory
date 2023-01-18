using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "names";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            List<string> nameslist = new List<string>();
            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x=>{
                nameslist.Add(x.ToString());
                });
            }
            return View();
        }
        [HttpPost]
        public IActionResult Add(string name)
        { 
        //db.ListRightPush(listKey,name);
        db.ListLeftPush(listKey,name);
        return RedirectToAction("Index");   
        }
        [HttpDelete]
        public IActionResult DeleteItem(string name)
        {
            db.ListRemoveAsync(listKey,name).Wait();
            return View();
        }
        public IActionResult DeleteFirstItem()
        {
            db.ListRightPop(listKey);
            return RedirectToAction("Index");

        }
    }
}
