using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewBPMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DevController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}