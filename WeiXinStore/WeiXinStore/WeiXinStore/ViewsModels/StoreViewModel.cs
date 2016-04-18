using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXinStore.ViewsModels
{
    public class StoreViewModel
    {
        public List<InstrumentView> InstrumentsView { get; set; }
        public UserViewModel UserView { get; set; }
    }
}