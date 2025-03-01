using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAgency.Models
{
    public class NewsListViewModel
    {
        public IEnumerable<NewsModel> NewsList { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}