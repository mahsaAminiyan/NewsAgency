using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.DataBase;
using System.Threading.Tasks;

namespace NewsAgency.Repositories
{
    public interface INewsRepository
    {
        IEnumerable<News> AllAgencyNews { get; }
        IEnumerable<User> Users { get; }
        IEnumerable<NewsCategory> Categories { get; }

        void SaveChanges();

        void AddNews(News item);
        void AddUser(User item);
    }
}
