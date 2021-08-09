using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using WebApplication.Models;


namespace WebApplication.DAL
{
    public class CompetitorDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CompetitorDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJPConnectionString");

            //Instantiate a SqlConnection object with the              
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<Competitor> GetAllCompetitor()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Competitor ORDER BY CompetitorID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<Competitor> competitorList = new List<Competitor>();
            while (reader.Read())
            {
                competitorList.Add(
                    new Competitor
                    {
                        // Fill Competitor object with values from the data reader
                        CompetitorID = reader.GetInt32(0),    //0: 1st column
                        CompetitorName = reader.GetString(1), //1: 2nd column
                        Salutation = reader.GetString(2),     //2: 3rd column
                        EmailAddr = reader.GetString(3),      //3: 4th column
                        Password = reader.GetString(4),       //4: 5th column
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return competitorList;
        }

        public Competitor LoginCompetitor(string email)
        {
            Competitor competitor = new Competitor();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Competitor WHERE EmailAddr = @inputEmail";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “email”.
            cmd.Parameters.AddWithValue("@inputEmail", email);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill Competitor object with values from the data reader
                    competitor.CompetitorID = reader.GetInt32(0);    //0: 1st column
                    competitor.CompetitorName = reader.GetString(1); //1: 2nd column
                    competitor.Salutation = reader.GetString(2);     //2: 3rd column
                    competitor.EmailAddr = reader.GetString(3);      //3: 4th column
                    competitor.Password = reader.GetString(4);       //4: 5th column
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();

            return competitor;
        }

        public int Add(Competitor competitor)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated CompetitorID after insertion
            cmd.CommandText = @"INSERT INTO Competitor (CompetitorName, Salutation, EmailAddr, Password)
                                OUTPUT INSERTED.CompetitorID
                                VALUES(@name, @salut, @email, @pw)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", competitor.CompetitorName);
            cmd.Parameters.AddWithValue("@salut", competitor.Salutation);
            cmd.Parameters.AddWithValue("@email", competitor.EmailAddr);
            cmd.Parameters.AddWithValue("@pw", competitor.Password);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //CompetitorID after executing the INSERT SQL statement
            competitor.CompetitorID = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return competitor.CompetitorID;
        }

        public bool IsEmailExist(string email, int competitorId)
        {
            bool emailFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a competitor record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CompetitorID FROM Competitor
                                WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);
            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != competitorId)
                        //The email address is used by another competitor
                        emailFound = true;
                }
            }
            else
            { //No record
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return emailFound;
        }
    }
}
