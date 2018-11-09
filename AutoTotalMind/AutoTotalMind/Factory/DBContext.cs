using AutoTotalMind.Models;

namespace AutoTotalMind.Factory
{
    public class DBContext
    {
        public AutoFactory<Brand> brandFactory;
        public AutoFactory<Color> colorFactory;
        public AutoFactory<Image> imageFactory;
        public AutoFactory<Product> productFactory;
        public AutoFactory<Subpage> subpageFactory;
        public ContactFactory contactFactory;
        public CMSUserFactory cmsUserFactory;

        public AutoFactory<Brand> BrandFactory => brandFactory ?? (brandFactory = new AutoFactory<Brand>());

        public AutoFactory<Color> ColorFactory => colorFactory ?? (colorFactory = new AutoFactory<Color>());

        public AutoFactory<Image> ImageFactory => imageFactory ?? (imageFactory = new AutoFactory<Image>());

        public AutoFactory<Product> ProductFactory => productFactory ?? (productFactory = new AutoFactory<Product>());

        public AutoFactory<Subpage> SubpageFactory => subpageFactory ?? (subpageFactory = new AutoFactory<Subpage>());
        public ContactFactory ContactFactory => contactFactory ?? (contactFactory = new ContactFactory());
        public CMSUserFactory CMSUserFactory => cmsUserFactory ?? (cmsUserFactory = new CMSUserFactory());
    }
}