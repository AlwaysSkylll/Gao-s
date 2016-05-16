using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXinStore.Models
{
    public class ProductListViewModel
    {
        public List<instruments> instrumentList { get; set; }
        public List<CartItem> cartItem { get; set; }
        public Nullable<double> priceCount { get; set; }
    }
}