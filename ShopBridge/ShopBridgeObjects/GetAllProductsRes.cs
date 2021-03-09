using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridgeObjects
{
   public class GetAllProductsRes
    {
            public int productId { get; set; }
            public string Code { get; set; }
            public string Message { get; set; }
            public List<Product> Product { get; set; }
        
    }
}
