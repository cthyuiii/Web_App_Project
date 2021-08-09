using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;
using WebApplication.Models;

namespace WebApplication.DAL
{
    public class CommentDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CommentDAL()
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

        public List<Comment> GetAllComment(int competitionID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Comment
                                WHERE CompetitionID = @selectedCompetitionID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “CompetitionID”.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a comment list
            List<Comment> commentList = new List<Comment>();

            while (reader.Read())
            {
                commentList.Add(
                    new Comment
                    {
                        CommentID = reader.GetInt32(0),         //0: 1st column
                        CompetitionID = reader.GetInt32(1),     //1: 2nd column
                        Description = reader.GetString(2),      //2: 3rd column
                        DateTimePosted = reader.GetDateTime(3), //3: 4th column
                    });
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return commentList;
        }

        public int Add(Comment comment, int competitionID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement
            cmd.CommandText = @"INSERT INTO Comment (CompetitionID, Description, 
                                DateTimePosted) OUTPUT INSERTED.CommentID VALUES(@competitionID, 
                                @description, @datetimeposted)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from controller.
            cmd.Parameters.AddWithValue("@competitionID", competitionID);
            cmd.Parameters.AddWithValue("@description", comment.Description);
            cmd.Parameters.AddWithValue("@datetimeposted", DateTime.Now);

            //A connection to database must be opened before any operations made.
            conn.Open();
            //Execute Scalar is used to retrieve the auto-generated
            //CommentID after executing the INSERT SQL statement
            comment.CommentID = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return comment.CommentID;
        }
    }
}
