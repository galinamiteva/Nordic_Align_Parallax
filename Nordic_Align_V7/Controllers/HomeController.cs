using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Nordic_Align_V7.Models;
using Nordic_Align_V7.Services;
using Nordic_Align_V7.ViewModels;

namespace Nordic_Align_V7.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mailService;
        public HomeController(ILogger<HomeController> logger, IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        

        // POST: /Home/Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var body = $"From: {model.Name}<br/>Email: {model.Email}<br/>Message: {model.Message}";
                await _mailService.SendEmailAsync("galinamiteva69@gmail.com", "New message from website Nordic Align", body);
                return RedirectToAction("Index"); 
            }
            return View(model); 
        }


        [Route("/error")]
        public IActionResult Error404(int statusCode) => View();


        


        // Other actions...
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




        public IActionResult ChangeLanguage(string lang)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                lang = "en";
            }
            Response.Cookies.Append("Language", lang);
            return Redirect(Request.GetTypedHeaders().Referer!.ToString());
        }
    }
}
