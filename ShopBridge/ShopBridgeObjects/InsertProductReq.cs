using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridgeObjects
{
  public   class InsertProductReq
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public byte[] Photo { get; set; }
    }
}
