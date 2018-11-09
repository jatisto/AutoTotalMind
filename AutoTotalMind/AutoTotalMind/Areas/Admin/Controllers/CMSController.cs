using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoTotalMind.Factory;
using AutoTotalMind.Models;

namespace AutoTotalMind.Areas.Admin.Controllers
{
//    [Authorize]
    public class CMSController : Controller
    {
        #region ConnectDB

        DBContext _context = new DBContext();

        #endregion

        #region Login
        [AllowAnonymous]
        public ActionResult Login(string returnurl)
        {
            TempData["ReturnURL"] = returnurl;
            return View();
        }

        [AllowAnonymous, ValidateAntiForgeryToken, HttpPost]
        public ActionResult LoginSubmit(string email, string password, string rememberMe)
        {
            CMSUser user = _context.CMSUserFactory.Login(email, password);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(email, Convert.ToBoolean(rememberMe));
                Session["CMSUser"] = user;
                string returnurl = TempData["ReturnURL"]?.ToString();
                if (returnurl == null)
                {
                    returnurl = "/Admin/CMS/Index";
                }

                return Redirect(returnurl);
            }
            else
            {
                TempData["LoginError"] = "Wrong email or password";
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Remove("CMSUser");
            return RedirectToAction("Login");
        }
        #endregion

        #region Index

        public ActionResult Index()
        {

//            _context.CMSUserFactory.Login("login@test.ru", "123123");
            return View();
        }

        #endregion

        #region Products

        public ActionResult Products()
        {
            List<ProductVM> products = CreateListProductVm(_context.ProductFactory.GetAll());
            return View(products);
        }

        public ActionResult EditProduct(int id = 0)
        {
            ProductVM productVM = null;

            ViewBag.Brands = _context.BrandFactory.GetAll();
            ViewBag.Colors = _context.ColorFactory.GetAll();

            if (id == 0)
            {
                productVM = new ProductVM();
                productVM.Brand = new Brand();
                productVM.Color = new Color();
                productVM.Images = new List<Image>();
                productVM.Product = new Product();
                productVM.Images = _context.ImageFactory.GetAllBy("ProductID", 0);
                return View(productVM);
            }
            else
            {
                productVM = CreateProductVM(_context.ProductFactory.Get(id));
                productVM.Images.AddRange(_context.ImageFactory.GetAllBy("ProductID", 0));
                return View(productVM);
            }
        }

        [HttpPost]
        public ActionResult EditProductSubmit(Product product, List<int> imageIDs)
        {
            if (product.ID > 0)
            {
                _context.ProductFactory.Update(product);
                for (int i = 0; i < imageIDs.Count; i++)
                {
                    Image img = _context.ImageFactory.Get(imageIDs[i]);
                    img.ProductID = product.ID;
                    _context.ImageFactory.Update(img);
                }

                foreach (Image img in _context.ImageFactory.GetAllBy("ProductID", product.ID))
                {
                    if (imageIDs.Contains(img.ID))
                    {
                        continue;
                    }
                    img.ProductID = 0;
                    _context.ImageFactory.Update(img);
                }
                TempData["MSG"] = "The Product has been Edited";
            }
            else
            {
                _context.ProductFactory.Insert(product);
                for (int i = 0; i < imageIDs.Count; i++)
                {
                    Image img = _context.ImageFactory.Get(imageIDs[i]);
                    img.ProductID = _context.ProductFactory.GetLatest().ID;
                    _context.ImageFactory.Update(img);
                }

                TempData["MSG"] = "The Product has been Added";
            }

            return RedirectToAction("Products");
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

        #region EditAndCreateSubpage

        public ActionResult Subpages()
        {
            return View(_context.SubpageFactory.GetAll());
        }

        public ActionResult EditSubpage(int id = 0)
        {
            if (id > 0)
            {
                return View(_context.SubpageFactory.Get(id));
            }
            else
            {
                Subpage subpage = new Subpage();
                subpage.ID = 0;
                subpage.Title = "";
                subpage.Content = "";
                return View(subpage);
            }
        }

        [HttpPost]
        public ActionResult EditSubpageSubmit(Subpage subpage)
        {
            if (subpage.ID > 0)
            {
                _context.SubpageFactory.Update(subpage);

                TempData["MSG"] = "Subpage has been updated";
            }
            else
            {
                _context.SubpageFactory.Insert(subpage);
                TempData["MSG"] = "Subpage has been added";
            }

            return RedirectToAction("Subpages");
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