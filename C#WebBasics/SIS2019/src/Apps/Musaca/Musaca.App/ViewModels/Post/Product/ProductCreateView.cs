
using SIS.MvcFramework.Attributes.Validation;

namespace Musaca.App.ViewModels.Post.Product
{
    public class ProductCreateView
    {
        [RequiredSis]
        [StringLengthSis(4,20, "Product name should be between 4 and 20 characters long")]
        public string Name { get; set; }

        [RequiredSis]
        public string Picture { get; set; }

        [RequiredSis]
        public decimal Price { get; set; }

        [RequiredSis]
        [RegexSis(@"[\d]{12}", "Barcode should be exactly 12 digits long")]
        public string Barcode { get; set; }
    }
}
