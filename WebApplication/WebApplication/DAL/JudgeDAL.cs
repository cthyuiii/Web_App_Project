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
    public class JudgeDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        public JudgeDAL()
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
        public Judge LoginJudge(string email)
        {
            Judge judge = new Judge();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Judge WHERE EmailAddr = @inputEmail";

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
                    // Fill Judge object with values from the data reader
                    judge.JudgeID = reader.GetInt32(0);    //0: 1st column
                    judge.JudgeName = !reader.IsDBNull(1) ? reader.GetString(1) : null; //1: 2nd column
                    judge.Salutation = !reader.IsDBNull(2) ? reader.GetString(2) : null;     //2: 3rd column
                    judge.AreaInterestID = reader.GetInt32(3);      //3: 4th column
                    judge.EmailAddr = !reader.IsDBNull(4) ? reader.GetString(4) : null;       //4: 5th column
                    judge.Password = !reader.IsDBNull(5) ? reader.GetString(5) : null;       //5: 6th column
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();

            return judge;
        }
        public int Add(Judge judge)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated JudgeID after insertion
            cmd.CommandText = @"INSERT INTO Judge (JudgeName, Salutation, AreaInterestID, EmailAddr, Password)
                                OUTPUT INSERTED.JudgeID
                                VALUES(@name, @salut, @areaIntID, @email, @pw)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", judge.JudgeName);
            cmd.Parameters.AddWithValue("@salut", judge.Salutation);
            cmd.Parameters.AddWithValue("@areaIntID", judge.AreaInterestID);
            cmd.Parameters.AddWithValue("@email", judge.EmailAddr);
            cmd.Parameters.AddWithValue("@pw", judge.Password);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //CompetitorID after executing the INSERT SQL statement
            judge.JudgeID = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return judge.JudgeID;
        }
        public bool IsEmailExist(string email, int JudgeID)
        {
            bool emailFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a judge record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT JudgeID FROM Judge
                                WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);
            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != JudgeID)
                        //The email address is used by another judge
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
        public List<Judge> GetAllJudges()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SQL statement that select all branches
            cmd.CommandText = @"SELECT * FROM Judge";
            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a branch list
            List<Judge> judgeList = new List<Judge>();
            while (reader.Read()) {
                judgeList.Add(
                    new Judge {
                        JudgeID = reader.GetInt32(0),    //0: 1st column
                        JudgeName = !reader.IsDBNull(1) ? reader.GetString(1) : null, //1: 2nd column
                        Salutation = !reader.IsDBNull(2) ? reader.GetString(2) : null,     //2: 3rd column
                        AreaInterestID = reader.GetInt32(3),      //3: 4th column
                        EmailAddr = !reader.IsDBNull(4) ? reader.GetString(4) : null,       //4: 5th column
                    }
                    );  
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return judgeList;
        }
        public List<AreaInterest> GetAreaOfInterest()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM AreaInterest ORDER BY AreaInterestID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<AreaInterest> AreaInterestList = new List<AreaInterest>();
            while (reader.Read())
            {
                AreaInterestList.Add(
                    new AreaInterest
                    {
                        AreaInterestID = reader.GetInt32(0), //0: 1st Column
                        Name = !reader.IsDBNull(1) ? reader.GetString(1) : null, //1: 2nd Column
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return AreaInterestList;
        }
    }
}
