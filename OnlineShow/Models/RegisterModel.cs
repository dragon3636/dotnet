using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShow.Models
{
    public class RegisterModel
    {
        public long ID { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên tài khoản")]
        public string UserName { get; set; }
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất  6,dài nhất 20 ký tu")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu ko đúng ")]
        public string ConfirmPassword { get; set; }

        [StringLength(50)]
        [Display(Name = "Họ tên")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [StringLength(50)]
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }
        [Display(Name ="Tỉnh/Thành phố")]
        public string ProvinceID { get; set; }
        [Display(Name ="Quận/Huyện-Phường/Xã")]
        public string DistrictID { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
    }
}