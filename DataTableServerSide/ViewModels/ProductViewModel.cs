﻿using DataTableServerSide.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataTableServerSide.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        public int Id { get; set; }
        [Orderable]
        [Required]
        public string Name { get; set; }
        [Orderable]
        [Required]
        public decimal Price { get; set; }
        [Orderable]
        [Required]
        public short Stock { get; set; }
        [Display(Name = "Ürün Türü")]
        public string Category { get; set; }
        [Display(Name = "Şirket")]
        public string Company { get; set; }
        [HiddenColumn]
        public int CategoryId { get; set; }
    }
}
