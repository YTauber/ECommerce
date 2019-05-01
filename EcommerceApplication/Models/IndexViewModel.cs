using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager;

namespace EcommerceApplication.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Categoties> Categoties { get; set; }
        public IEnumerable<Products> Products { get; set; }
        public int CurrentCategory { get; set; }
    }

    public class ItemViewModel
    {
        public Products Product { get; set; }
    }

    public class CartViewModel
    {
        public IEnumerable<CartItems> CartItems { get; set; }
    }



}
