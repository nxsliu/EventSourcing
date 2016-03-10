using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminWeb.Repositories;

namespace AdminWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var applicationRepo = new ApplicationRepository();            

            return View(applicationRepo.GetApplications());
        }
    }
}