using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using WebApplication1.Models;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;
using System;
using System.Net;
using System.Net.WebSockets;
using Azure.Messaging;
using Microsoft.Extensions.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace WebApplication1.Pages
{
    public class ChangeModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private HttpClient httpClient;

        [BindProperty]
        public MovieDto movie { get; set; } = default;

        public ChangeModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            httpClient = _httpClientFactory.CreateClient("data");
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            movie = await httpClient.GetFromJsonAsync<MovieDto>($"/Movie/Details/{id}");

            if (movie == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {

            if (ModelState.IsValid)
            {
                if (id != null)
                    movie.Id = (int)id;

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
            return Page();
        }
    }
}
