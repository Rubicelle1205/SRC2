using Microsoft.AspNetCore.Mvc;
using WebPccuClub.Models;
using System.Diagnostics;

namespace WebPccuClub.Controllers
{
    public class ClubListController : FBaseController
    {
        private readonly ILogger<ClubListController> _logger;

        public ClubListController(ILogger<ClubListController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {


            return View();
        }

    }
}