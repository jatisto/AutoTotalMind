using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoTotalMind.Factory;

namespace AutoTotalMind.Controllers
{
    public class HomeController : Controller
    {
        DBContext _context = new DBContext();

        public ActionResult Index()
        {
            ViewBag.RandomPictures = _context.ImageFactory.TakeRandom(3); // рандомные картинки
            _context.ProductFactory.CountBy("BrandID", 1);
            return View(_context.SubpageFactory.Get(3)); // вывести subpage с ID 3
        }
    }
}