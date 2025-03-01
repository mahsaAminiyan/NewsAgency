using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAgency.Models
{
    public class UserListViewModel
    {
        public IEnumerable<UserInfoModel> Users { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}