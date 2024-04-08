using Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cumulative1.Controllers
{
    public class ClassController : Controller
    {
        // GET: localhost:xx/Class/Index ->
        public ActionResult Index()
        {
            //Navigates to Views/Class/Index.cshtml
            return View();
        }
        // GET: Class/List
        public ActionResult List(string SearchKey = null)
        {
            ClassDataController controller = new ClassDataController();
            List <Class> Classes = controller.ListClasses(SearchKey);
            return View(Classes);
        }

        // GET: Class/Show/{id}
        public ActionResult Show(int id)
        {
            ClassDataController controller = new ClassDataController();
            Class pickedClass = controller.FindClass(id);
            return View(pickedClass);
        }
    }
}