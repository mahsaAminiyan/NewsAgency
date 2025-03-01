using DAL.DataBase;
using NewsAgency.Models;
using NewsAgency.Repositories;
using NewsAgency.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAgency.Services
{
    public class NewsAgencyService : INewsAgencyService
    {

        public bool Authenticate(string username, string password, INewsRepository repository)
        {


            var user = repository.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return false;
            }

            if (CryptographyHelper.Encrypt(password) != user.HashPassword)
            {
                return false;
            }

            return true;
        }


        public Models.ProfileViewModel GetProfileData(string username, INewsRepository repository)
        {
            var user = repository.Users.FirstOrDefault(x => x.UserName == username && x.Role == "reporter");
            if (user == null)
            {
                return null;
            }

            var confirmed_count = repository.AllAgencyNews.Count(x => x.UserId == user.Id && x.Staus == (int)NewsStatusEnum.Confirmed);
            var rejected_count = repository.AllAgencyNews.Count(x => x.UserId == user.Id && x.Staus == (int)NewsStatusEnum.Rejected);
            var waiting_count = repository.AllAgencyNews.Count(x => x.UserId == user.Id && x.Staus == (int)NewsStatusEnum.Waiting);

            return new Models.ProfileViewModel()
            {
                Name = user.Name,
                Family = user.Family,
                UserName = username,
                Confirmed_News_Count = confirmed_count,
                Rejected_News_Count = rejected_count,
                Waiting_News_Count = waiting_count,
                Confirm_percent = Math.Round(((float)confirmed_count / (confirmed_count + rejected_count)) * 100.0),
                Rejected_percent = Math.Round(((float)rejected_count / (confirmed_count + rejected_count)) * 100.0),
            };

        }


        public bool ChangePassword(string username, string oldPassword, string newPassword, INewsRepository repository)
        {
            var user = repository.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return false;
            }
            if (CryptographyHelper.Encrypt(oldPassword) != user.HashPassword)
            {
                return false;
            }
            user.HashPassword = CryptographyHelper.Encrypt(newPassword);
            repository.SaveChanges();
            return true;
        }


        public bool SaveNews(string username, string title, string summary, string mainContent, string category, HttpPostedFileBase NewsImage, INewsRepository repository)
        {
            var user = repository.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return false;
            }

            var category_of_news = repository.Categories.FirstOrDefault(x => x.Title == category);
            if (category_of_news == null)
            {
                return false;
            }

            var new_item = new DAL.DataBase.News
            {
                MainContent = mainContent,
                Summary = summary,
                Title = title,
                Staus = (int)NewsStatusEnum.Waiting,
                UserId = user.Id,
                CategoryId = category_of_news.Id,

            };

            new_item.ImageData = new byte[NewsImage.ContentLength];
            NewsImage.InputStream.Read(new_item.ImageData, 0, NewsImage.ContentLength);
            new_item.ImageMimeType = NewsImage.ContentType;
            repository.AddNews(new_item);
            repository.SaveChanges();

            return true;
        }


        public ShowNewsStatusViewModel GetNewsWithStatus(int page, NewsStatusEnum status, string username, INewsRepository repository)
        {

            var user = repository.Users.FirstOrDefault(x => x.UserName == username);

            if (!string.IsNullOrEmpty(username))
            {
                if (user == null)
                {
                    return null;
                }
            }


            int PageSize = 3;
            List<NewsModel> SelectedNews = new List<NewsModel>();
            var news = repository.AllAgencyNews
                .Where(x => x.Staus == (int)status)
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            if (!string.IsNullOrEmpty(username))
                news = news.Where(x => x.UserId == user.Id);

            var news_list = news.ToList();

            foreach (var item in news_list)
            {
                SelectedNews.Add(new NewsModel
                {
                    MainContent = item.MainContent,
                    Summary = item.Summary,
                    Title = item.Title,
                    Status = (NewsStatusEnum)item.Staus,
                    ReporterName = item.User.FullName,
                    Id = item.Id,
                    Category = item.Category.Title,
                });
            }


            ShowNewsStatusViewModel model = new ShowNewsStatusViewModel
            {
                News = SelectedNews,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = user == null ? repository.AllAgencyNews.Where(x => x.Staus == (int)status).Count() : repository.AllAgencyNews.Where(x => x.Staus == (int)status && x.UserId == user.Id).Count()
                }
            };

            return model;
        }


        public string GetRoleOfUser(string username, INewsRepository repository)
        {
            var user = repository.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return "unknown";
            }

            return user.Role;
        }


        public AdminPanelViewModel GetAdminProfileData(string username, INewsRepository repository)
        {
            var user = repository.Users.FirstOrDefault(x => x.UserName == username && x.Role == "admin");
            if (user == null)
            {
                return null;
            }
            var waiting_count = repository.AllAgencyNews.Count(x => x.Staus == (int)NewsStatusEnum.Waiting);

            return new AdminPanelViewModel
            {
                Family = user.Family,
                Name = user.Name,
                Waiting_News_Count = waiting_count,
            };

        }


        public bool SetNewsStatus(int id, NewsStatusEnum status, INewsRepository repository)
        {
            var news = repository.AllAgencyNews.FirstOrDefault(x => x.Id == id);
            if (news == null)
                return false;
            news.Staus = (int)status;
            repository.SaveChanges();
            return true;
        }




        public string[] GetAllNewsCategories(INewsRepository repository)
        {
            return repository.Categories.Select(x => x.Title).ToArray();
        }


        public NewsModel GetNewsWithId(int Id, INewsRepository repository)
        {
            var item = repository.AllAgencyNews.FirstOrDefault(x => x.Id == Id);
            if (item == null)
            {
                return null;
            }

            return new NewsModel
            {
                Category = item.Category.Title,
                Id = item.Id,
                ImageData = item.ImageData,
                ImageMimeType = item.ImageMimeType,
                MainContent = item.MainContent,
                ReporterName = item.User.FullName,
                Summary = item.Summary,
                Title = item.Title,
            };
        }


        public UserListViewModel GetUserList(int page, INewsRepository repository, string username = "")
        {
            int PageSize = 3;

            List<UserInfoModel> SelectedUsers = new List<UserInfoModel>();


            if (!string.IsNullOrEmpty(username))
            {
                var users_searched = repository.Users.Where(x => x.UserName.Contains(username))
                .OrderBy(p => p.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

                var user_list_searched = users_searched.ToList();

                foreach (var item in user_list_searched)
                {
                    SelectedUsers.Add(new UserInfoModel
                    {
                        Family = item.Family,
                        Name = item.Name,
                        Role = item.Role,
                        Username = item.UserName,
                        Id = item.Id,
                    });
                }

                UserListViewModel model_searched_user = new UserListViewModel
                {
                    Users = SelectedUsers,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = repository.Users.Where(x => x.UserName.Contains(username)).Count(),
                    }
                };

                return model_searched_user;
            }

            var users = repository.Users
                .OrderBy(p => p.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            var user_list = users.ToList();

            foreach (var item in user_list)
            {
                SelectedUsers.Add(new UserInfoModel
                {
                    Family = item.Family,
                    Name = item.Name,
                    Role = item.Role,
                    Username = item.UserName,
                    Id = item.Id,
                });
            }


            UserListViewModel model = new UserListViewModel
            {
                Users = SelectedUsers,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Users.Count(),
                }
            };

            return model;
        }


        public int AddUser(string username, string password, string name, string family, string role, INewsRepository repository)
        {
            try
            {
                var user = new User
                {
                    Family = family,
                    Name = name,
                    UserName = username,
                    Role = role,
                    HashPassword = CryptographyHelper.Encrypt(password),
                };
                repository.AddUser(user);

                repository.SaveChanges();
                return user.Id;
            }
            catch (Exception exp)
            {
                return -1;
            }

        }


        public bool EditUser(string username, string name, string family, string password, INewsRepository repository)
        {
            var user = repository.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return false;
            }

            user.Name = name;
            user.Family = family;
            user.HashPassword = CryptographyHelper.Encrypt(password);
            repository.SaveChanges();
            return true;
        }


        public bool ChangeNewsShowValue(int Id, INewsRepository repository)
        {
            var news = repository.AllAgencyNews.FirstOrDefault(x => x.Id == Id);
            if (news == null)
                return false;
            news.Show = !news.Show;
            repository.SaveChanges();
            return true;
        }


        public NewsListViewModel GetNewsList(int page, INewsRepository repository, string title)
        {
            int PageSize = 3;

            List<NewsModel> SelectedNews = new List<NewsModel>();


            if (!string.IsNullOrEmpty(title))
            {
                var news_searched = repository.AllAgencyNews.Where(x => x.Staus == (int)NewsStatusEnum.Confirmed && x.Title.Contains(title))
                .OrderBy(p => p.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

                var news_list_searched = news_searched.ToList();

                foreach (var item in news_list_searched)
                {
                    SelectedNews.Add(new NewsModel
                    {
                        Title = item.Title,
                        Summary = item.Summary,
                        Id = item.Id,
                        ReporterName = item.User.FullName,
                        show = item.Show,

                    });
                }

                NewsListViewModel model_searched_news = new NewsListViewModel
                {
                    NewsList = SelectedNews,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = repository.AllAgencyNews.Where(x => x.Staus == (int)NewsStatusEnum.Confirmed && x.Title.Contains(title)).Count(),
                    }
                };

                return model_searched_news;
            }

            var all_news = repository.AllAgencyNews.Where(x => x.Staus == (int)NewsStatusEnum.Confirmed)
                .OrderBy(p => p.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            var news_list = all_news.ToList();

            foreach (var item in news_list)
            {
                SelectedNews.Add(new NewsModel
                {
                    Title = item.Title,
                    Summary = item.Summary,
                    Id = item.Id,
                    ReporterName = item.User.FullName,
                    show = item.Show,

                });
            }


            NewsListViewModel model = new NewsListViewModel
            {

                NewsList = SelectedNews,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.AllAgencyNews.Where(x => x.Staus == (int)NewsStatusEnum.Confirmed).Count(),
                }
            };

            return model;
        }


        public HomePageModel GetNewsForHomePage(string category, int page, INewsRepository repository)
        {
            int PageSize = 3;
            List<NewsModel> SelectedNews = new List<NewsModel>();


            if (!string.IsNullOrEmpty(category))
            {
                var all_news_in_category = repository.AllAgencyNews.Where(x => x.Staus == (int)NewsStatusEnum.Confirmed && x.Show == true && x.Category != null && x.Category.Title == category)
                .OrderBy(p => p.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

                var news_list_in_category = all_news_in_category.ToList();
                foreach (var item in news_list_in_category)
                {
                    SelectedNews.Add(new NewsModel
                    {
                        Title = item.Title,
                        Summary = item.Summary,
                        Id = item.Id,
                        ReporterName = item.User.FullName,
                        show = item.Show,
                        ImageData = item.ImageData,
                        MainContent = item.MainContent,
                        Category = item.Category.Title,

                    });
                }


                HomePageModel model_for_category = new HomePageModel
                {

                    NewsList = SelectedNews,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = repository.AllAgencyNews.Where(x => x.Staus == (int)NewsStatusEnum.Confirmed && x.Show == true).Count(),
                    }
                };

                return model_for_category;
            }


            var all_news = repository.AllAgencyNews.Where(x => x.Staus == (int)NewsStatusEnum.Confirmed && x.Show == true)
                .OrderBy(p => p.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            var news_list = all_news.ToList();
            foreach (var item in news_list)
            {
                SelectedNews.Add(new NewsModel
                {
                    Title = item.Title,
                    Summary = item.Summary,
                    Id = item.Id,
                    ReporterName = item.User.FullName,
                    show = item.Show,
                    ImageData = item.ImageData,
                    MainContent = item.MainContent,
                    Category = item.Category.Title,

                });
            }


            HomePageModel model = new HomePageModel
            {

                NewsList = SelectedNews,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.AllAgencyNews.Where(x => x.Staus == (int)NewsStatusEnum.Confirmed && x.Show == true).Count(),
                }
            };

            return model;
        }


        public List<string> GetAllCategories(INewsRepository repository)
        {
            return repository.Categories.Select(x => x.Title).ToList();
        }
    }
}