﻿using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private RedisService _redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
             
            db.StringSet("name","SS");
            db.StringSet("ziyaretci",100);
                return View();
        }
        public IActionResult Show() 
        {
           var value= db.StringGetRange("name",0,3);
         //   db.StringIncrement("ziyaretci",1);
          //var count=  db.StringDecrementAsync("ziyaretci",1).Result;
            db.StringDecrementAsync("ziyaretci", 10).Wait();
            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
            }
        return View();
        }
    }
}
