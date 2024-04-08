using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cumulative1.Models
{
    public class Teacher
    {
        //Attributes of Teacher table defined in Database
        public int TeacherId;
        public string TeacherFName;
        public string TeacherLName;
        public string EmployeeNumber;
        public DateTime HireDate;
        public decimal Salary;
        //To store the classes the Teacher is assigned with
        public List<Class> Classes;
    }
}