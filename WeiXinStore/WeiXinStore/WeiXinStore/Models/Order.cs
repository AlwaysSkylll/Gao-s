//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeiXinStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class order
    {
        public int OrderId { get; set; }
        public Nullable<int> CartId { get; set; }
        public Nullable<double> OrderPrice { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<int> OrderStatus { get; set; }
        public int UserId { get; set; }
        public Nullable<int> AddressId { get; set; }
    }
}
