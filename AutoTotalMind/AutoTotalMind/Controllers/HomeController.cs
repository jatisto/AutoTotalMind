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
        #region ConnectDB

        DBContext _context = new DBContext();

        #endregion

        #region Index

        public ActionResult Index()
        {
            ViewBag.RandomPictures = _context.ImageFactory.TakeRandom(3); // рандомные картинки
            _context.ProductFactory.CountBy("BrandID", 1);
            return View(_context.SubpageFactory.Get(4)); // вывести subpage с ID 3
        }

        #endregion

        #region Product

        public ActionResult Products()
        {
            List<ProductVM> products = null;
            if (TempData["sortedProducts"] == null)
            {
                products = CreateListProductVm(_context.ProductFactory.GetAll());
                ViewBag.Title = "All Products";
            }
            else
            {
                products = CreateListProductVm(TempData["sortedProducts"] as List<Product>);
                ViewBag.Title = "Сортировка по: " + TempData["sortTitle"].ToString().ToUpper();
            }

            return View(products);
        }
        

        #endregion

        #region ShowProduct

        public ActionResult ShowProduct(int id = 0)
        {
            if (id == null || id > 0)
            {
                Product p = _context.ProductFactory.Get(id);
                p.Views++;
                _context.ProductFactory.Update(p);
                return View(CreateProductVM(_context.ProductFactory.Get(id)));
            }

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
        

        #endregion

        #region GetCreateListAndProductVM

        public List<ProductVM> CreateListProductVm(List<Product> productsToCreateForm)
        {
            List<ProductVM> allProductWithImages = new List<ProductVM>();

            foreach (Product product in productsToCreateForm)
            {
                ProductVM pvm = new ProductVM();
                pvm.Product = product;
                pvm.Color = _context.ColorFactory.Get(product.ColorID);
                pvm.Brand = _context.BrandFactory.Get(product.BrandID);
                pvm.Images = _context.ImageFactory.GetAllBy("ProductID", product.ID);

                allProductWithImages.Add(pvm);
            }

            return allProductWithImages;
        }

        public ProductVM CreateProductVM(Product productToCreateForm)
        {
            ProductVM pvm = new ProductVM();
            pvm.Product = productToCreateForm;
            pvm.Color = _context.ColorFactory.Get(productToCreateForm.ColorID);
            pvm.Brand = _context.BrandFactory.Get(productToCreateForm.BrandID);
            pvm.Images = _context.ImageFactory.GetAllBy("ProductID", productToCreateForm.ID);

            return pvm;
        }
        


        #endregion

        #region Sort Products

        public ActionResult SortProductBy()
        {
            List<Product> sortedProducts = _context.ProductFactory.GetAll();
            string sortBy = Request.QueryString["sortBy"].ToString();

            switch (sortBy.ToLower())
            {
                case "km":
                    sortedProducts = sortedProducts.OrderByDescending(x => x.Km).ToList();
                    break;
                case "price":
                    sortedProducts = sortedProducts.OrderByDescending(x => x.Price).ToList();
                    break;
                case "views":
                    sortedProducts = sortedProducts.OrderBy(x => x.Views).ToList();
                    break;
                case "hk":
                    sortedProducts = sortedProducts.OrderBy(x => x.BHP).ToList();
                    break;
                default:
                    break;
            }

            TempData["sortedProducts"] = sortedProducts.Take(5).ToList();
            TempData["sortTitle"] = sortBy;

            return RedirectToAction("Products");
        }


        public ActionResult SortByBrand(int id)
        {
            Brand brand = _context.BrandFactory.Get(id);
            TempData["sortedProducts"] = _context.ProductFactory.GetAllBy("BrandID", id);
            TempData["sortTitle"] = brand.Name;

            return RedirectToAction("Products");
        }

        #endregion

        #region Search

        public ActionResult Search(string searchQuery)
        {
            TempData["sortedProducts"] =
                _context.ProductFactory.SearchByJoin<Brand, Color>(searchQuery, "Model", "BHP", "Year", "Km", "Price", "Description");

            TempData["sortTitle"] = "Результат поиска: ";
            return RedirectToAction("Products");
        }

        #endregion

        #region UsedCarsList

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

            #endregion

        #region MostViewsCarsList

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

            #endregion

        #region MostExpensiveCar

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

            #endregion

        #region SortCars

        [ChildActionOnly]
        public ActionResult SortCars()
        {
            return PartialView();
        }

        #endregion

        #region Footer
        [ChildActionOnly]
        public ActionResult Footer()
        {
            return PartialView(_context.ContactFactory.Get(1));
        }

        #endregion
    }
}