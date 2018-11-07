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

        public ActionResult Products()
        {
            //Создание списка ProductVM, который содержит продукт, бренд и его изображения
            List<ProductVM> allProductWithImages = new List<ProductVM>();

            // Делаем foreach на всех продуктах в таблице Product собственный
            // _context.ProductFactory.GetAll()
            foreach (Product product in _context.ProductFactory.GetAll())
            {
                ProductVM pvm = new ProductVM();
                pvm.Product = product;
                pvm.Brand = _context.BrandFactory.Get(product.BrandID);
                pvm.Images = _context.ImageFactory.GetAllBy("ProductID", product.ID);

                allProductWithImages.Add(pvm);
            }
            return View(allProductWithImages);
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

            List<ProductVM> productVmList = new List<ProductVM>();

            foreach (Product product in top3MostViewed)
            {
                ProductVM productVm = new ProductVM();
                productVm.Product = product;
                productVm.Brand = _context.BrandFactory.Get(product.BrandID);
                productVm.Images = _context.ImageFactory.GetAllBy("ProductID", product.ID);

                productVmList.Add(productVm);

            }

            return PartialView(productVmList);
        }

        [ChildActionOnly]
        public ActionResult MostExpensiveCar()
        {
            Product theChosenChineseOrWasItJapanes = _context.ProductFactory.GetAll()
                .OrderByDescending(x => x.Price).FirstOrDefault();

            ProductVM productVm = new ProductVM();
            productVm.Product = theChosenChineseOrWasItJapanes;
            productVm.Brand = _context.BrandFactory.Get(theChosenChineseOrWasItJapanes.BrandID);
            productVm.Images = _context.ImageFactory.GetAllBy("ProductID", theChosenChineseOrWasItJapanes.ID);

            return PartialView(productVm);
        }
    }
}