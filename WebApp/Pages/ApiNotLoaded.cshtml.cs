using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    [BindProperties]
    public class ApiNotLoadedModel : PageModel
    {
        public string Message { get; set; }
        public IList<string> StackTrace { get; set; }
        public string Source { get; set; }
        public void OnGet()
        {
            Message = Request.Query["Message"];
            StackTrace = Regex.Split(Request.Query["StackTrace"], @"(?:at\s)");
            Source = Request.Query["Source"];
        }
    }
}
