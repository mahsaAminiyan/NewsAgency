using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataBase
{
    public class NewsAgencyContext : DbContext
    {
        public NewsAgencyContext() : base("NewsAgency")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<News> AllNews { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }


    }
}
