using AutoTotalMind.Models;

namespace AutoTotalMind.Factory
{
    public class DBContext
    {
        private AutoFactory<Brand> brandFactory;
        private AutoFactory<Color> colorFactory;
        private AutoFactory<Image> imageFactory;
        private AutoFactory<Product> productFactory;
        private AutoFactory<Subpage> subpageFactory;
        private ContactFactory contactFactory;
        private CMSUserFactory cmsUserFactory;

        public AutoFactory<Brand> BrandFactory => brandFactory ?? (brandFactory = new AutoFactory<Brand>());

        public AutoFactory<Color> ColorFactory => colorFactory ?? (colorFactory = new AutoFactory<Color>());

        public AutoFactory<Image> ImageFactory => imageFactory ?? (imageFactory = new AutoFactory<Image>());

        public AutoFactory<Product> ProductFactory => productFactory ?? (productFactory = new AutoFactory<Product>());

        public AutoFactory<Subpage> SubpageFactory => subpageFactory ?? (subpageFactory = new AutoFactory<Subpage>());
        public ContactFactory ContactFactory => contactFactory ?? (contactFactory = new ContactFactory());

        public CMSUserFactory CMSUserFactory
        {
            get
            {
                if (cmsUserFactory == null)
                {
                    cmsUserFactory = new CMSUserFactory();
                }

                return cmsUserFactory;
            }
        }
    }
}