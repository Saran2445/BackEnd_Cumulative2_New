using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cumulative1.Controllers
{
    public class StudentDataController : ApiController
    {
        //Initializing varaible for the School database context 
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns the list of students in the database
        /// </summary>
        /// <example>GET api/StudentData/ListStudents</example>
        /// <returns>
        /// A list of Student objects.
        /// </returns>
        [HttpGet]
        [Route("api/StudentData/ListStudents/{SearchKey?}")]
        public List <Student> ListStudents(string SearchKey = null)
        {
            MySqlConnection Conn = School.AccessDatabase();
            //Open the connection between the web server and database
            Conn.Open();
            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();
            //SQL Query
            cmd.CommandText = "Select * from Students where lower(studentfname) like lower(@key) or lower(studentlname) like lower(@key) or lower(concat(studentfname,' ',studentlname)like lower(@key))";
            cmd.Parameters.AddWithValue("key", "%" + SearchKey + "%");
            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Student> Students = new List<Student> { };
            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFullName = ResultSet["studentfname"].ToString() + " " + ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                DateTime StudentEnrolDate = (DateTime)ResultSet["enroldate"];

                Student StudentObj = new Student();
                StudentObj.StudentId = StudentId;
                StudentObj.StudentFullName = StudentFullName;
                StudentObj.StudentNumber = StudentNumber;
                StudentObj.EnrollDate = StudentEnrolDate;

                //Adds the Student Name to the List
                Students.Add(StudentObj);
            }

            //Close the connection 
            Conn.Close();

            //Return the final list of Student names
            return Students;
        }

        /// <summary>
        /// Returns an specific student from the database by using the primary key studentID
        /// </summary>
        /// <example>GET api/StudentData/FindStudent/1</example>
        /// <param name="id">Which is the Student's ID in the database</param>
        /// <returns>An Student object</returns>
        [HttpGet]
        [Route("api/StudentData/FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            Student StudentObj = new Student();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();
            //Open the connection between the web server and database
            Conn.Open();
            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM students WHERE studentid = " + id.ToString() + ";";
            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFullName = ResultSet["studentfname"].ToString() + " " + ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                DateTime StudentEnrolDate = (DateTime)ResultSet["enroldate"];

                StudentObj.StudentId = StudentId;
                StudentObj.StudentFullName = StudentFullName;
                StudentObj.StudentNumber = StudentNumber;
                StudentObj.EnrollDate = StudentEnrolDate;
            }
            //Close the connection 
            Conn.Close();

            return StudentObj;
        }
    }
}
