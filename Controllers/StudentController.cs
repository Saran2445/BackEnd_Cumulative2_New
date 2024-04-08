using Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cumulative1.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        // GET: Student/List
        public ActionResult List(string SearchKey = null)
        {
            StudentDataController controller = new StudentDataController();
            List <Student> Students = controller.ListStudents(SearchKey);
            return View(Students);
        }

        // GET: Student/Show/{id}
        public ActionResult Show(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student pickedStudent = controller.FindStudent(id);
            return View(pickedStudent);
        }
    }
}