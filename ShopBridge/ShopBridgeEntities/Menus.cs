using System;
using System.Collections.Generic;

namespace ShopBridgeEntities
{
    public partial class Menus
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public bool? IsActive { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int? ParentId { get; set; }
        public int? RoleId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public Roles Role { get; set; }
    }
}
