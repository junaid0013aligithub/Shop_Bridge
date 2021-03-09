using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridgeObjects
{
   public class LoginRes
    {
        public string userName { get; set; }
        public int? roleId { get; set; }
        public string token { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

    }
}
