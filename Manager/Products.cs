using System;
using System.Collections.Generic;
using System.Text;

namespace Manager
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategotyId { get; set; }
    }

    public class Categoties
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Carts
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }

    public class CartItems
    {
        public int ItemId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }

        public string Name { get; set; }
        public string PictureName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategotyId { get; set; }
    }
}
