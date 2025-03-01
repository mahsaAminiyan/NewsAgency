using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataBase
{
    public class News
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Summary { get; set; }
        [MaxLength(2000)]
        public string MainContent { get; set; }

        public int Staus { get; set; }

        public int UserId { get; set; }

        public int CategoryId { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        public bool Show { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("CategoryId")]
        public virtual NewsCategory Category { get; set; }

    }
}
