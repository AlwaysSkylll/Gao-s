using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXinStore.Models
{
    public class orderManage
    {
        public int OrderId { get; set; }
        public Nullable<double> OrderPrice { get; set; }
        public string CreateTime { get; set; }
        public string Name { get; set; }
        public string AddressDetail { get; set; }
        public string Phone { get; set; }
        public Nullable<int> OrderStatus { get; set; }
        //public string CartJson { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}