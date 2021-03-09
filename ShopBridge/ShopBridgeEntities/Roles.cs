using System;
using System.Collections.Generic;

namespace ShopBridgeEntities
{
    public partial class Roles
    {
        public Roles()
        {
            Menus = new HashSet<Menus>();
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public ICollection<Menus> Menus { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
