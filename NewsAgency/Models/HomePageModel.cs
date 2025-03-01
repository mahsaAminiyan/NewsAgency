using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAgency.Models
{
    public class HomePageModel
    {

        public IEnumerable<NewsModel> NewsList { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public List<string> Categories { get; set; }

    }
}