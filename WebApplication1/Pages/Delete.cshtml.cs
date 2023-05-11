using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private HttpClient httpClient;

        [BindProperty]
        public MovieDto movieDel { get; set; } = default;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            httpClient = _httpClientFactory.CreateClient("delete");
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            movieDel = await httpClient.GetFromJsonAsync<MovieDto>($"https://localhost:7068/Movie/Delete/{id}");

            if (movieDel == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();
            var movie = await httpClient.GetFromJsonAsync<MovieDto>($"https://localhost:7068/Movie/Delete/{id}");

            if (movie != null)
            {
                movieDel = movie;
                var response = await httpClient.PostAsJsonAsync<MovieDto>($"https://localhost:7068/Movie/Delete/{id}", movieDel);
            }

            return RedirectToPage("./Index");
        }
    }
}
