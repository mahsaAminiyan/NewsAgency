using NewsAgency.Repositories;
using NewsAgency.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace NewsAgency.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        public HomeController(INewsAgencyService _service, INewsRepository _repository)
        {
            service = _service;
            news_repository = _repository;

        }


        public ActionResult Index(string category, int page = 1)
        {
            var model = service.GetNewsForHomePage(category, page, news_repository);
            model.Categories = service.GetAllCategories(news_repository);

            ViewBag.category = category;
            return View(model);
        }

        public ActionResult ReadNews(int Id)
        {
            var model = service.GetNewsWithId(Id, news_repository);
            ViewBag.Categories = service.GetAllCategories(news_repository);
            return View(model);
        }
    }
}