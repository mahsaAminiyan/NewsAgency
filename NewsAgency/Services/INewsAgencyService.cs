using NewsAgency.Repositories;
using NewsAgency.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NewsAgency.Services
{
    public interface INewsAgencyService
    {
        bool Authenticate(string username, string password, INewsRepository repository);
        ProfileViewModel GetProfileData(string username, INewsRepository repository);
        bool ChangePassword(string username, string oldPassword, string newPassword, INewsRepository repository);
        bool SaveNews(string username, string title, string summary, string mainContent, string category, HttpPostedFileBase NewsImage, INewsRepository repository);

        ShowNewsStatusViewModel GetNewsWithStatus(int page, NewsStatusEnum status, string username, INewsRepository repository);

        string GetRoleOfUser(string username, INewsRepository repository);

        AdminPanelViewModel GetAdminProfileData(string username, INewsRepository repository);

        bool SetNewsStatus(int id, NewsStatusEnum status, INewsRepository repository);
        string[] GetAllNewsCategories(INewsRepository repository);

        NewsModel GetNewsWithId(int Id, INewsRepository repository);

        UserListViewModel GetUserList(int page, INewsRepository repository, string username);

        int AddUser(string username, string password, string name, string family, string role, INewsRepository repository);

        bool EditUser(string username, string name, string family, string password, INewsRepository repository);

        bool ChangeNewsShowValue(int Id, INewsRepository repository);

        NewsListViewModel GetNewsList(int page, INewsRepository repository, string title);

        HomePageModel GetNewsForHomePage(string category, int page, INewsRepository repository);

        List<string> GetAllCategories(INewsRepository repository);
    }
}
