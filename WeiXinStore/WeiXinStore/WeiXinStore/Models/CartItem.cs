using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXinStore.Models
{
    public class CartItem
    {
        public string product { get; set; }
        public int productId { get; set; }
        public int num { get; set; }
        public Nullable<double> price { get; set; }
        public Nullable<double> totalPrice { get; set; }
    }
}