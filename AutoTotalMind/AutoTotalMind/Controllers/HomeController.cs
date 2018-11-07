using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoTotalMind.Factory;
using AutoTotalMind.Models;

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

        [ChildActionOnly]
        public ActionResult UsedCarsList()
        {
            ViewBag.TotalProductCount = _context.ProductFactory.Count(); //Передаём колличество подерженных авто

            List<BrandVM> brandWhitCount = new List<BrandVM>();
            foreach (Brand item in _context.BrandFactory.GetAll())
            {
                BrandVM brandVm = new BrandVM();
                brandVm.Brand = item;
                brandVm.ProductCount = _context.ProductFactory.CountBy("BrandID", item.ID);

                brandWhitCount.Add(brandVm);
            }

            return PartialView(brandWhitCount);
        }

        [ChildActionOnly]
        public ActionResult MostViewsCarsList()
        {
            List<Product> top3MostViewed = _context.ProductFactory
                .GetAll()
                .OrderByDescending(x => x.Views)
                .Take(3)
                .ToList();
            return PartialView(top3MostViewed);
        }
    }
}