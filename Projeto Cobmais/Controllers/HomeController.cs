using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Projeto_Cobmais.Models;

namespace Projeto_Cobmais.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
