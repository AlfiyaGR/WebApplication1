using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private HttpClient httpClient;

        [BindProperty]
        public MovieDto newMovie { get; set; } = default;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            httpClient = _httpClientFactory.CreateClient("create");
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                using var response = await httpClient.PostAsJsonAsync("https://localhost:7068/Movie/Create", newMovie);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MovieDto? movieDto = await response.Content.ReadFromJsonAsync<MovieDto>();
                    await HttpContext.Response.WriteAsync("Created");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("error");
                }
            }
            return Page();
        }
    }
}
