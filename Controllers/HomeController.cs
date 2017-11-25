using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using crypto_converter.Models;

namespace crypto_converter.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("<p>Crypto Converter backend</p>","text/html");
        }        
    }
}
