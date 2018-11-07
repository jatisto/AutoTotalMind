using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTotalMind.Models
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public Brand Brand { get; set; }
        public List<Image> Images { get; set; }

        public Image GetFirstImages()
        {
            if (Images?.Count > 0)
            {
                return Images[0];
            }

            return new Image(){ImageUrl = "no-images.png", Alt = Product.Model};
        }
    }
}