using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Api.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp.Pages
{
    public static class ApiConstants
    {
        public static string Href = "http://localhost:40783/api/Register";
    }
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public IEnumerable<string> StoreNames { get; set; }

        [BindProperty]
        public Store Store { get; set; }

        [BindProperty]
        public IEnumerable<Section> Sections { get; set; }

        public async Task<ActionResult> OnGetAsync(string store)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ApiConstants.Href}/Get");
                var model = await response.Content.ReadAsStringAsync();
            }
            if (string.IsNullOrWhiteSpace(store))
            {
                return Page();
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ApiConstants.Href}/Get/{store}");
                var model = await response.Content.ReadAsStringAsync();
                Store = JsonConvert.DeserializeObject<Store>(model);
                Sections = Store.Sections;
            }

            return Page();
        }
    }
}
