using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridgeObjects
{
  public   class GetProductByIDRes
    {
        public int productId { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public Product Product { get; set; }
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public byte[] Photo { get; set; }
    }
}
