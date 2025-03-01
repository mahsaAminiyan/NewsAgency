using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsAgency.Models
{
    public class NewsModel
    {
        [Required(ErrorMessage = "لطفا عنوان خبر را وارد کنید")]
        [MaxLength(100, ErrorMessage = "عنوان باید حداکثر 100 حرف باشد")]
        [MinLength(10, ErrorMessage = "عنوان خبر باید حداقل 10 حرف باشد")]
        [Display(Name = "عنوان خبر")]
        public string Title { get; set; }

        [Required(ErrorMessage = "لطفا خلاصه خبر را وارد کنید")]
        [MaxLength(100, ErrorMessage = "خلاصه خبر باید حداکثر 100 حرف باشد")]
        [MinLength(20, ErrorMessage = "خلاصه خبر باید حداقل 20 حرف باشد")]
        [Display(Name = "خلاصه خبر")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "لطفا متن خبر را وارد کنید")]
        [MinLength(100, ErrorMessage = "متن خبر باید حداقل 100 حرف باشد")]
        [Display(Name = "متن اصلی خبر")]
        public string MainContent { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public NewsStatusEnum Status { get; set; }

        public string Category { get; set; }
        public string ReporterName { get; set; }

        public int Id { get; set; }
        public bool show { get; set; }
        public SelectList Categories { get; set; }

    }
}