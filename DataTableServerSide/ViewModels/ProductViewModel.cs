using DataTableServerSide.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataTableServerSide.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public short Stock { get; set; }
        [Display(Name = "Ürün Türü")]
        public string Category { get; set; }
        [Display(Name = "Şirket")]
        public string Company { get; set; }
        [HiddenColumn]
        public int CategoryId { get; set; }
    }
}
