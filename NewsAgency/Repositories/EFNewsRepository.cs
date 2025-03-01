using DAL.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAgency.Repositories
{
    public class EFNewsRepository
    : INewsRepository
    {
        private NewsAgencyContext context = new NewsAgencyContext();
        public IEnumerable<DAL.DataBase.News> AllAgencyNews
        {
            get { return context.AllNews.Include("Category"); }
        }
        public IEnumerable<User> Users
        {
            get { return context.Users; }

        }

        public IEnumerable<NewsCategory> Categories
        {
            get { return context.NewsCategories; }
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void AddNews(News item)
        {
            context.AllNews.Add(item);

        }

        public void AddUser(User item)
        {
            context.Users.Add(item);

        }
    }
}