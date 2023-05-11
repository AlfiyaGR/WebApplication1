using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using WebApplication1.Models;
using X.PagedList;
using X.PagedList.Mvc.Core;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private HttpClient httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            httpClient = _httpClientFactory.CreateClient("data");
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            HttpClient httpClient = _httpClientFactory.CreateClient("data");

            var movies = await httpClient.GetFromJsonAsync<List<MovieDto>>(
                "/Movie");

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    movies = movies.OrderByDescending(x => x.Title).ToList();
                    break;
                case "Date":
                    movies = movies.OrderBy(x => x.Date).ToList();
                    break;
                case "Date_desc":
                    movies = movies.OrderByDescending(x => x.Date).ToList();
                    break;
                case "Price":
                    movies = movies.OrderBy(x => x.Price).ToList();
                    break;
                case "Price_desc":
                    movies = movies.OrderByDescending(x => x.Price).ToList();
                    break;
                default:
                    movies = movies.OrderBy(x => x.Title).ToList();
                    break;
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            ViewBag.movie = movies.ToPagedList(pageNumber, pageSize);
            return View(movies.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public async Task<ActionResult> EditAsync(MovieDto movie, int? id)
        {
            if (ModelState.IsValid)
            {
                using var response = await httpClient.PutAsJsonAsync($"/Movie/Details/{id}", movie);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("error");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MovieDto? movieDTO = await response.Content.ReadFromJsonAsync<MovieDto>();
                }

                await HttpContext.Response.WriteAsync("Saved");
            }
            return RedirectToAction("Index");
        }
    }
}
