using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXinStore.ViewsModels
{
    
    public class InstrumentView
    {
        public int InstrumentId { get; set; }
        public int InstrumentNum { get; set; }
        public string InstrumentName { get; set; }
        public string InstrumentDescribe { get; set; }
        public string Instrumentprice { get; set; }
    }
}