using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoTotalMind.Factory;
using AutoTotalMind.Models;

namespace AutoTotalMind.Areas.Admin.Controllers
{
    public class CMSController : Controller
    {
        #region ConnectDB

        DBContext _context = new DBContext();

        #endregion

        #region Index

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Products

        public ActionResult Products()
        {
            List<ProductVM> products = CreateListProductVm(_context.ProductFactory.GetAll());
            return View(products);
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
    }
}