using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAgency.Models
{
    public class ShowNewsStatusViewModel
    {
        public IEnumerable<NewsModel> News { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}