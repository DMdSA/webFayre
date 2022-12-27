using System.ComponentModel.DataAnnotations;

namespace WebFayre.Models
{
    public class StandShoppingCart
    {
        [Display(Name = "Feira ID")]
        public int FeiraId { get; set; }

        [Display(Name = "Stand ID")]
        public int StandId { get; set; }

        [Display(Name = "Products")]
        public List<ProductInfo> Products { get; set; } = new List<ProductInfo>();

    }

    public class ProductInfo
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Quantity")]
        public float Quantity { get; set; }

        [Display(Name = "Final Price")]
        public float FinalPrice { get; set; }

        [Display(Name = "IVA")]
        public float Iva { get; set; }
    
    }
}
