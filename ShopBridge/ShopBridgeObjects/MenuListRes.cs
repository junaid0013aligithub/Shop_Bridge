using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridgeObjects
{
   public class MenuListRes
    {
        public int roleId { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Menu> Menu { get; set; }
    }

    public class Menu
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int? ParentId { get; set; }


        public static T GetComplexData<T>(string key)
        {

            if (key == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(key);
        }

    }
}
