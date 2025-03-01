using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsAgency.Models
{
    public class UserInfoModel
    {

        [Required(ErrorMessage = "لطفا نام  را وارد کنید")]
        [Display(Name = "نام ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لطفا نام خانوادگی را وارد کنید")]
        [Display(Name = "نام خانوادگی ")]
        public string Family { get; set; }

        [Required(ErrorMessage = "لطفا نام کاربری را وارد کنید")]
        [MinLength(8, ErrorMessage = "نام کاربری حداقل باید هشت کاراکتر باشد")]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Required(ErrorMessage = "لطفا رمز  را وارد کنید")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "رمز باید حداقل 8 رقم، و دارای حداقل یک حرف، یک عدد و یک کاراکتر ویژه باشد")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا تکرار رمز را وارد کنید")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "تکرار رمز جدید باید حداقل 8 رقم، و دارای حداقل یک حرف، یک عدد و یک کاراکتر ویژه باشد")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور ")]
        [Compare("Password", ErrorMessage = "رمز  و تکرار آن یکسان نیستند")]
        public string RepeatePassword { get; set; }
        public int Id { get; set; }
        public string Role { get; set; }
    }
}