using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using X.PagedList;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ITestCount _testCount;

        private readonly IConfiguration Configuration;

        public PaginatedList<MovieDto>? movie { get; set; }
         
        private readonly IHttpClientFactory _httpClientFactory;

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string PriceSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public IndexModel(/*ILogger<IndexModel> logger, ITestCount testCount,*/ IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            // _logger = logger;
            // _testCount = testCount;
            _httpClientFactory = httpClientFactory;
            Configuration = configuration;
        }

        //public async Task<List<MovieDto>> GetData()
        //{
        //    HttpClient httpClient = _httpClientFactory.CreateClient("data");

        //    var movieDtos = await httpClient.GetFromJsonAsync<List<MovieDto>>(
        //        "/Movie");
        //    movie = movieDtos;

        //    return movie;
        //}

        //public async Task<List<MovieDto>> Get()
        //{
        //    HttpClient httpClient = _httpClientFactory.CreateClient("data");

        //    var movieDtos = await httpClient.GetFromJsonAsync<List<MovieDto>>(
        //        "/Movie");

        //    movie = movieDtos;

        //    //NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    //DateSort = sortOrder == "Date" ? "date_desc" : "Date";

        //    //IQueryable<MovieDto> studentsIQ = (IQueryable<MovieDto>)(from s in movieDtos select s);

        //    //switch (sortOrder)
        //    //{
        //    //    case "name_desc":
        //    //        studentsIQ = studentsIQ.OrderByDescending(s => s.Title);
        //    //        break;
        //    //    case "Date":
        //    //        studentsIQ = studentsIQ.OrderBy(s => s.Date);
        //    //        break;
        //    //    case "date_desc":
        //    //        studentsIQ = studentsIQ.OrderByDescending(s => s.Price);
        //    //        break;
        //    //    default:
        //    //        studentsIQ = studentsIQ.OrderBy(s => s.Title);
        //    //        break;
        //    //}

        //    //movieDtos = await studentsIQ.AsNoTracking().ToListAsync();

        //    return movie;
        //}

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            PriceSort = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            HttpClient httpClient = _httpClientFactory.CreateClient("data");

            var movieDtos = await httpClient.GetFromJsonAsync<List<MovieDto>>(
                "/Movie");

            if (!String.IsNullOrEmpty(searchString))
            {
                movieDtos = movieDtos.Where(m => m.Title.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    movieDtos = movieDtos.OrderByDescending(s => s.Title).ToList();
                    break;
                case "Date":
                    movieDtos = movieDtos.OrderBy(s => s.Date).ToList();
                    break;
                case "date_desc":
                    movieDtos = movieDtos.OrderByDescending(s => s.Date).ToList();
                    break;
                case "Price":
                    movieDtos = movieDtos.OrderBy(s => s.Price).ToList();
                    break;
                case "price_desc":
                    movieDtos = movieDtos.OrderByDescending(s => s.Price).ToList();
                    break;
                default:
                    movieDtos = movieDtos.OrderBy(s => s.Title).ToList();
                    break;
            }

            //int pageSize = 2;
            //pageNumber = (pageNumber ?? 1);
            //movie = movieDtos.ToPagedList((int)pageNumber, pageSize);
            var pageSize = Configuration.GetValue("PageSize", 4);
            movie = await PaginatedList<MovieDto>.CreateAsync(
                movieDtos, pageIndex ?? 1, pageSize);
        }
    }
}