using DataTableServerSide.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataTableServerSide.ViewModels
{
    public class SupplierViewModel : ViewModelBase
    {
        [Display(Name = "Şirket No")]
        public int Id { get; set; }
        [Display(Name = "Firma Adı")]
        public string Name { get; set; }
        [Display(Name = "Yönetici")]
        public string Manager { get; set; }
        [Display(Name = "Şehir")]
        public string City { get; set; }
        [Display(Name = "Ülke")]
        public string Country { get; set; }
        [HiddenColumn]
        public IEnumerable<string> Products { get; set; }
        [Display(Name = "Ürünler")]
        public string FormattedProducts { get; set; }
    }
}
