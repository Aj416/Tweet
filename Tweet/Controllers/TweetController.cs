using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Tweet.Models;

namespace Tweet.Controllers
{
    public class TweetController : Controller
    {
        // GET: Tweet
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Home(TweetDateModel model)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = model.page.HasValue ? Convert.ToInt32(model.page) : 1;
            IPagedList<TweetViewModel> tweets = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://badapi.iqvia.io/api/v1/");

                var responseTask = client.GetAsync("Tweets?startDate=" + model.StartDate.ToString() + "&endDate=" + model.EndDate.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<TweetViewModel>>();
                    readTask.Wait();
                    var distinctList = readTask.Result.GroupBy(x => x.Text).Select(y => y.First());
                    tweets = distinctList.ToPagedList(pageIndex, pageSize);
                }
                else //web api sent error response 
                {
                    //log response status here..

                    //tweets = Enumerable.Empty<TweetViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                TempData["StartDate"] = model.StartDate.ToString();
                TempData["EndDate"] = model.EndDate.ToString();
            }
            return View(tweets);
        }

        [HttpGet]
        public ActionResult Home(int? page)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<TweetViewModel> tweets = null;

            using (var client = new HttpClient())
            {

                string startDate = DateTime.Now.ToString(), endDate = DateTime.Now.ToString();

                if (TempData["StartDate"] != null && TempData["EndDate"] != null)
                {
                    startDate = TempData["StartDate"] as string;
                    endDate = TempData["EndDate"] as string;
                }

                TempData.Keep();

                client.BaseAddress = new Uri("https://badapi.iqvia.io/api/v1/");
                //HTTP GET
                var responseTask = client.GetAsync("Tweets?startDate=" + startDate + "&endDate=" + endDate);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<TweetViewModel>>();
                    readTask.Wait();
                    var distinctList = readTask.Result.GroupBy(x => x.Text).Select(y => y.First());
                    tweets = distinctList.ToPagedList(pageIndex, pageSize);
                }
                else //web api sent error response 
                {
                    //log response status here..

                    //tweets = Enumerable.Empty<TweetViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(tweets);
        }

    }
}