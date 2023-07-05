﻿namespace Savoy.Models
{
    public class Color : BaseEntity
    {
        public string Name { get;set; }
        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }

    }
}