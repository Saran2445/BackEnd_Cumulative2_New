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
    public class ClassDataController : ApiController
    {
        //Initializing varaible for the School database context 
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns the list of Classes in the database
        /// </summary>
        /// <example>GET api/ClassData/ListClasses</example>
        /// <returns>
        /// A list of Class objects.
        /// </returns>
        [HttpGet]
        [Route("api/ClassData/ListClasses/{SearchKey?}")]
        public List <Class> ListClasses(string SearchKey = null)
        {
            MySqlConnection Conn = School.AccessDatabase();
            //Open the connection between the web server and database
            Conn.Open();
            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM classes where lower(classname) like lower(@key)";
            cmd.Parameters.AddWithValue("key", "%" + SearchKey + "%");

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Class> Classes = new List<Class> { };
            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassCode = ResultSet["classcode"].ToString();
                string ClassName = ResultSet["classname"].ToString();
                DateTime ClassStartDate = (DateTime)ResultSet["startdate"];
                DateTime ClassFinishDate = (DateTime)ResultSet["finishdate"];
                if(ResultSet["TeacherId"] != DBNull.Value)
                {
                    int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                    Class ClassObj = new Class();
                    ClassObj.ClassId = ClassId;
                    ClassObj.ClassCode = ClassCode;
                    ClassObj.ClassName = ClassName;
                    ClassObj.StartDate = ClassStartDate;
                    ClassObj.FinishDate = ClassFinishDate;
                    ClassObj.TeacherId = TeacherId;
                    //Add the Class to the List
                    Classes.Add(ClassObj);
                }
                else
                {
                    Class ClassObj = new Class();
                    ClassObj.ClassId = ClassId;
                    ClassObj.ClassCode = ClassCode;
                    ClassObj.ClassName = ClassName;
                    ClassObj.StartDate = ClassStartDate;
                    ClassObj.FinishDate = ClassFinishDate;
                    ClassObj.TeacherId = 0;
                    //Add the Class to the List
                    Classes.Add(ClassObj);
                }
                

            
            }

            //Close the connection 
            Conn.Close();



            //Return the final list of Class names
            return Classes;
        }

        /// <summary>
        /// Returns an specific Class from the database by using the primary key classID
        /// </summary>
        /// <example>GET api/ClassData/FindClass/1</example>
        /// <param name="id">Which is the class's ID in the database</param>
        /// <returns>An Class object</returns>
        [HttpGet]
        [Route("api/ClassData/FindClass/{id}")]
        public Class FindClass(int id)
        {
            Class ClassObj = new Class();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
            //SQL Query
            cmd.CommandText = "SELECT * FROM classes WHERE classid = " + id.ToString() + ";";
            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassCode = ResultSet["classcode"].ToString();
                string ClassName = ResultSet["classname"].ToString();
                DateTime ClassStartDate = (DateTime)ResultSet["startdate"];
                DateTime ClassFinishDate = (DateTime)ResultSet["finishdate"];
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);

                ClassObj.ClassId = ClassId;
                ClassObj.ClassCode = ClassCode;
                ClassObj.ClassName = ClassName;
                ClassObj.StartDate = ClassStartDate;
                ClassObj.FinishDate = ClassFinishDate;
                ClassObj.TeacherId = TeacherId;
            }
            //Close the connection
            Conn.Close();

            return ClassObj;
        }
    }
}
