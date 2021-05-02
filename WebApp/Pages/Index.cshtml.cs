using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace WebApp.Pages
{
    public static class ApiConstants
    {
        public static string Href = "http://localhost:40783/api/Register";
    }
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IEnumerable<string> StoreNames { get; set; }
        public Store Store { get; set; }
        public IEnumerable<int> SectionCounts { get; set; }
        public double AlmostFullThreshhold { get; set; } = 0.8;

        public async Task<ActionResult> OnGetAsync(string store)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync($"{ApiConstants.Href}/Get");
                    var model = await response.Content.ReadAsStringAsync();
                    StoreNames = model
                        .Split(",")
                        .Select(w => Regex.Replace(w, @"[^a-zA-Z\s]+", ""));
                }
                catch (Exception error)
                {
                    return RedirectToPage("/ApiNotLoaded", new
                    {
                        Message = error.Message,
                        StackTrace = error.StackTrace,
                        Source = error.Source
                    });
                }
            }

            if (string.IsNullOrWhiteSpace(store))
                store = StoreNames.First();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ApiConstants.Href}/Get/{store}");
                var model = await response.Content.ReadAsStringAsync();
                Store = JsonConvert.DeserializeObject<Store>(model);
            }
            SectionCounts = CalculatePeopleInStore();
            return Page();
        }


        private IEnumerable<int> CalculatePeopleInStore()
        {
            var total = 0;
            foreach (var section in Store.Sections)
            {
                var enters = section.Enters.Count();
                var exits = section.Exits.Count();
                yield return enters - exits;
                total += (enters - exits);
            }
            yield return total;
        }

        private async Task StartTesting()
        {
            var test = new Testing(Store);
            await test.Start(10, Pressure.Average);
        }
    }
}
