using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cumulative1.Models
{
    public class SchoolDbContext
    {

        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "school"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "8889"; } }

        // ConnectionString is a series of credentials used to connect to the database.
        protected static string ConnectionString
        {
            get
            {

                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
            }
        }
        public MySqlConnection AccessDatabase()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
    
}
