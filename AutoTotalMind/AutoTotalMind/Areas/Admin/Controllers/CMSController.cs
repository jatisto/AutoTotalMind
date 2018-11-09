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

        #region ImageUpload

        public ActionResult Images()
        {
            return View(_context.ImageFactory.GetAll());
        }

        [HttpPost]
        public ActionResult CreateImages(List<HttpPostedFileBase> list)
        {
            foreach (HttpPostedFileBase file in list)
            {
                string fileName = "";
                if (Upload.Image(file, Request.PhysicalApplicationPath + @"/Content/Images/Product/", out fileName))
                {
                    Upload.Image(file, Request.PhysicalApplicationPath + @"/Content/Images/Product/", "tn_" + fileName, 400);

                    Image image = new Image();
                    image.ImageUrl = fileName;
                    image.ProductID = 0;
                    image.SubpageID = 0;
                    image.Alt = "";

                    _context.ImageFactory.Insert(image);
                }
            }

            TempData["MSG"] = "Images has been uploaded";

            return RedirectToAction("Images");
        }

        #endregion

        #region EditAndCreateColor

        public ActionResult Colors()
        {
            return View(_context.ColorFactory.GetAll());
        }

        public ActionResult EditColor(int id = 0)
        {
            ViewBag.AddColor = (id == 0);

            if (id == 0)
            {
                return View();
            }
            else
            {
                return View(_context.ColorFactory.Get(id));
            }
        }

        [HttpPost]
        public ActionResult EditColorSubmit(Color color)
        {
            if (color.ID > 0)
            {
                _context.ColorFactory.Update(color);

                TempData["MSG"] = "Color hass been updated.";
            }
            else
            {
                _context.ColorFactory.Insert(color);

                TempData["MSG"] = "Color hass been added.";
            }

            return RedirectToAction("Colors");
        }

        public ActionResult DeleteColor(int id)
        {
            _context.ColorFactory.Delete(id);
            return RedirectToAction("Colors");
        }

        #endregion

        #region EditAndCreateBrand

        public ActionResult Brands()
        {
            return View(_context.BrandFactory.GetAll());
        }

        public ActionResult EditBrand(int id = 0)
        {
            ViewBag.AddBrand = (id == 0);
            if (id == 0)
            {
                return View();
            }
            else
            {
                return View(_context.BrandFactory.Get(id));
            }
        }

        [HttpPost]
        public ActionResult EditBrandSubmit(Brand brand)
        {
            if (brand.ID > 0)
            {
                _context.BrandFactory.Update(brand);
            }
            else
            {
                _context.BrandFactory.Insert(brand);
            }
            return RedirectToAction("Brands");
        }

        public ActionResult DeleteBrand(int id)
        {
            _context.BrandFactory.Delete(id);
            TempData["MSG"] = "Brand has been deleted.";
            return RedirectToAction("Brands");
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