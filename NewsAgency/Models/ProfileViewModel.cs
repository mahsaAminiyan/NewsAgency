using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAgency.Models
{
    public class ProfileViewModel
    {

        public string Name { get; set; }

        public string Family { get; set; }

        public string UserName { get; set; }

        public int Confirmed_News_Count { get; set; }
        public int Rejected_News_Count { get; set; }

        public int Waiting_News_Count { get; set; }


        public double Confirm_percent { get; set; }
        public double Rejected_percent { get; set; }

    }
}