using Cumulative1.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Ajax.Utilities;

namespace Cumulative1.Controllers
{
    public class TeacherDataController : ApiController
    {
        /// <summary>
        ///  Initializing varaible for the School database context 
        /// </summary>
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>
        /// A list of Teacher objects.
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public List<Teacher> ListTeachers(string SearchKey=null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname,' ',teacherlname)like lower(@key))";
            cmd.Parameters.AddWithValue("key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();

                Teacher teacherObj = new Teacher();
                teacherObj.TeacherId = TeacherId;
                teacherObj.TeacherFName = TeacherFname;
                teacherObj.TeacherLName = TeacherLname;
                teacherObj.EmployeeNumber = EmployeeNumber;

                Teachers.Add(teacherObj);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            return Teachers;
        }


        /// <summary>
        /// Returns an individual teacher from the database by using the primary key teacherID
        /// </summary>
        /// <example>GET api/TeacherData/FindTeacher/1</example>
        /// <param name="id">the teacher's ID in the database</param>
        /// <returns>An Teacher object</returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            Teacher TeacherObj = new Teacher();
            List<Class> Classes = new List<Class>();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();
            //Open the connection between the web server and database
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
            //SQL Query
            cmd.CommandText = "SELECT * FROM teachers LEFT JOIN classes ON teachers.teacherid= classes.teacherid WHERE teachers.teacherid = " + id.ToString() + ";";
            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string TeacherEmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime TeacherHireDate = (DateTime)ResultSet["hiredate"];
                ///string formattedDateTime = TeacherHireDate.ToString("yyyy-MM-dd");
                decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);

                TeacherObj.TeacherId = TeacherId;
                TeacherObj.TeacherFName = TeacherFname;
                TeacherObj.TeacherLName = TeacherLname;
                TeacherObj.EmployeeNumber = TeacherEmployeeNumber;
                TeacherObj.HireDate = TeacherHireDate;
                TeacherObj.Salary = TeacherSalary;

                if (ResultSet["ClassId"] != DBNull.Value)
                {
                    int ClassId = Convert.ToInt32(ResultSet["classid"]);
                    string ClassCode = ResultSet["classcode"].ToString();
                    string ClassName = ResultSet["classname"].ToString();
                    DateTime ClassStartDate = (DateTime)ResultSet["startdate"];
                    DateTime ClassFinishDate = (DateTime)ResultSet["finishdate"];

                    Class NewClassObj = new Class();
                    NewClassObj.ClassId = ClassId;
                    NewClassObj.ClassCode = ClassCode;
                    NewClassObj.ClassName = ClassName;
                    NewClassObj.StartDate = ClassStartDate;
                    NewClassObj.FinishDate = ClassFinishDate;

                    //Add the Class details to the List
                    Classes.Add(NewClassObj);
                    TeacherObj.Classes = Classes;
                }
                else
                {
                    Class NewClassObj = new Class();
                    NewClassObj.ClassId = 0;
                    NewClassObj.ClassCode = "No Class Code Available";
                    NewClassObj.ClassName = "No Class Name Available";
                    NewClassObj.StartDate = DateTime.Now;
                    NewClassObj.FinishDate = DateTime.Now; 

                    //Add the Class details to the List
                    Classes.Add(NewClassObj);
                    TeacherObj.Classes = Classes;
                }
               
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();


            return TeacherObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <example> POST : /api/TeacherData/DeleteTeacher/3</example>
        /// <returns>An Teacher object</returns>
        [HttpPost]
        [Route("api/TeacherData/DeleteTeacher/{id}")]
        public void DeleteTeacher(int id)
        {
            //Create an instance of connection
            MySqlConnection Conn = School.AccessDatabase();
            //Open the connection between the web server and database
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query

            cmd.CommandText = "Delete from teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE classes SET teacherid = null WHERE teacherid = @id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            Conn.Close();
        }

        /// <summary>
        /// Adds a Teacher to the MySQL Database.
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the Teacher's table.</param>
        /// <example>
        /// POST api/TeacherData/AddTeacher 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFName":"Christine",
        ///	"TeacherLName":"Bittle",
        ///	"EmployeeNumber":"T404",
        ///	"HireDate":"01-01-2024",
        ///	"Salary":"78.89",
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherFName,@TeacherLName,@EmployeeNumber, @HireDate, @Salary)";
            cmd.Parameters.AddWithValue("@TeacherFName", NewTeacher.TeacherFName);
            cmd.Parameters.AddWithValue("@TeacherLName", NewTeacher.TeacherLName);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();



        }

        public void UpdateTeacher(int id, [FromBody] Teacher TeacherInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "update teachers set teacherfname=@TeacherFName, teacherlname=@TeacherLName, employeenumber=@EmployeeNumber, hiredate=@HireDate, salary=@Salary where teacherid=@TeacherId";
            cmd.Parameters.AddWithValue("@TeacherFName", TeacherInfo.TeacherFName);
            cmd.Parameters.AddWithValue("@TeacherLName", TeacherInfo.TeacherLName);
            cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", TeacherInfo.HireDate);
            cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@TeacherId", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }

    }
}
