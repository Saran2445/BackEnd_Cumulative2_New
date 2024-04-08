using Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cumulative1.Controllers
{
    public class TeacherController : Controller
    {
        // GET: localhost:xx/Teacher/Index ->
        public ActionResult Index()
        {
            //Navigates to Views/Teacher/Index.cshtml
            return View();
        }
        // GET: Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            List<Teacher> Teachers = controller.ListTeachers(SearchKey);
            return View(Teachers);
        }

        // GET: Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher pickedTeacher = controller.FindTeacher(id);

            return View(pickedTeacher);
        }
        // GET: Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher pickedTeacher = controller.FindTeacher(id);

            return View(pickedTeacher);
        }

        // POST: Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }
        //GET : Teacher/New
        public ActionResult New()
        {
            return View();
        }

        //POST : /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFName, string TeacherLName, string EmployeeNumber, DateTime HireDate, decimal Salary)
        {
            //Identify that this method is running
            //Identify the inputs provided from the form

            Debug.WriteLine("I have accessed the Create Method!");
            Debug.WriteLine(TeacherFName);
            Debug.WriteLine(TeacherLName);
            Debug.WriteLine(EmployeeNumber);

            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFName = TeacherFName;
            NewTeacher.TeacherLName = TeacherLName;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }
    }
}