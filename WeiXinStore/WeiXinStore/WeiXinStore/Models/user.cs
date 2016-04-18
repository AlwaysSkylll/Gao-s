using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WeiXinStore.Models
{
    public class user
    {
        //用户ID
        [Key] public int UserId { get; set; }
        //用户姓名
        public string Name { get; set; }
        //用户邮箱
        public string Mail { get; set; }
        //用户密码
        public string Password { get; set; }
        //用户手机号
        public string Phone { get; set; }
        //创建时间
        public DateTime CreateTime { get; set; }
        //创建时间
        public string Sex { get; set; }
    }
}