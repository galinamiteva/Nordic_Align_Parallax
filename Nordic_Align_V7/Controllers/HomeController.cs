using System.Diagnostics;
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

        // Other actions...
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
