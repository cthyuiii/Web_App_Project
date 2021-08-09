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
    public class CompetitionSubmissionDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CompetitionSubmissionDAL()
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

        public List<string> GetCompetitorName(int competitionID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT CompetitorName
                                FROM Competitor c, CompetitionSubmission a
                                WHERE c.CompetitorID = a.CompetitorID 
                                AND a.CompetitionID = @selectedCompetitionID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “CompetitionID”.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> competitornameList = new List<string>();
            while (reader.Read())
            {
                // Add competitor's name in list for each data
                competitornameList.Add(reader.GetString(0));
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return competitornameList;
        }

        public List<CompetitionSubmission> GetAllCompetitionSubmission(
            List<string> competitornameList, int competitionID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission 
                                WHERE CompetitionID = @selectedCompetitionID
                                ORDER BY CompetitorID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “CompetitionID”.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a competitor list
            List<CompetitionSubmission> competitorList =
                new List<CompetitionSubmission>();
            int i = 0;
            while (reader.Read())
            {
                competitorList.Add(
                    new CompetitionSubmission
                    {
                        CompetitionID = reader.GetInt32(0),         //0: 1st column
                        CompetitorID = reader.GetInt32(1),          //1: 2nd column
                        CompetitorName = competitornameList[i],
                        //2: 3rd column
                        FileSubmitted =
                        !reader.IsDBNull(2) ? reader.GetString(2) : null,
                        //3: 4th column
                        DateTimeFileUpload =
                        !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,
                        //4: 5th column
                        Appeal =
                        !reader.IsDBNull(4) ? reader.GetString(4) : null,
                        VoteCount = reader.GetInt32(5),             //5: 6th column
                        //6: 7th column
                        Ranking =
                        !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null,
                    });
                i++;
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return competitorList;
        }

        public void Vote(int competitionId, int competitorId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the UPDATE SQL statement
            cmd.CommandText = @"UPDATE CompetitionSubmission 
                                SET VoteCount = VoteCount + 1
                                WHERE CompetitionID = @selectedCompetitionID 
                                and CompetitorID = @selectedCompetitorID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “CompetitionID” and "CompetitorID.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorId);
            //Open a database connection
            conn.Open();

            //Execute command to vote
            cmd.ExecuteNonQuery();

            //A connection should be closed after operations.
            conn.Close();
        }

        public void Add(int competitionId, int competitorId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //insert the values into a new competition submission record
            cmd.CommandText = @"INSERT INTO CompetitionSubmission (CompetitionID, CompetitorID, FileSubmitted, DateTimeFileUpload, Appeal, VoteCount, Ranking)
                                VALUES(@competitionId, @competitorId, @file, @fileUploadDT, @appeal, @voteCount, @rank)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@competitionId", competitionId);
            cmd.Parameters.AddWithValue("@competitorId", competitorId);
            cmd.Parameters.AddWithValue("@file", DBNull.Value);
            cmd.Parameters.AddWithValue("@fileUploadDT", DBNull.Value);
            cmd.Parameters.AddWithValue("@appeal", DBNull.Value);
            cmd.Parameters.AddWithValue("@voteCount", 0);
            cmd.Parameters.AddWithValue("@rank", DBNull.Value);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //Execute command to add competition submission record
            cmd.ExecuteNonQuery();

            //A connection should be closed after operations.
            conn.Close();
        }

        public CompetitionSubmission IsCompetitorInCompetition(int competitionId, int competitorId)
        {
            CompetitionSubmission competitionSubmission = new CompetitionSubmission();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission 
                                WHERE CompetitionID = @selectedCompetitionID AND CompetitorID = @selectedCompetitorID";

            //Define the parameters used in SQL statement
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorId);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill competitionsubmission object with values from the data reader
                    competitionSubmission.CompetitionID = competitionId;
                    competitionSubmission.CompetitorID = reader.GetInt32(1); //1: 2nd column
                    competitionSubmission.FileSubmitted = !reader.IsDBNull(2) ? reader.GetString(2) : null; //2: 3rd column
                    competitionSubmission.DateTimeFileUpload = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null; //3: 4th column
                    competitionSubmission.Appeal = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                    competitionSubmission.VoteCount = reader.GetInt32(5); //5: 6th column
                    competitionSubmission.Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null; //6: 7th column
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();

            return competitionSubmission;
        }

        public List<int> competitionsJoinedByCompetitor(int competitorId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission 
                                WHERE CompetitorID = @selectedCompetitorID";

            //Define the parameters used in SQL statement
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorId);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<int> competitionIdJoinedList = new List<int>();
            while (reader.Read())
            {
                competitionIdJoinedList.Add(reader.GetInt32(0));
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return competitionIdJoinedList;
        }

        public CompetitionSubmission GetDetails(int competitionId, int competitorId)
        {
            CompetitionSubmission competitionSubmission = new CompetitionSubmission();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a competition submission record.
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission
                        WHERE CompetitionID = @selectedCompetitionId AND CompetitorID = @selectedCompetitorId";

            //Define the parameters used in SQL statement
            cmd.Parameters.AddWithValue("@selectedCompetitionId", competitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorId", competitorId);

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
                    competitionSubmission.CompetitionID = competitionId;
                    competitionSubmission.CompetitorID = reader.GetInt32(1); //1: 2nd column
                    competitionSubmission.FileSubmitted = !reader.IsDBNull(2) ? reader.GetString(2) : null; //2: 3rd column
                    competitionSubmission.DateTimeFileUpload = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null; //3: 4th column
                    competitionSubmission.Appeal = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                    competitionSubmission.VoteCount = reader.GetInt32(5); //5: 6th column
                    competitionSubmission.Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null; //6: 7th column
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();

            return competitionSubmission;
        }

        public void UpdateFileNameAndDT(CompetitionSubmissionViewModel compSubVM)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE CompetitionSubmission SET FileSubmitted = @fileSubmitted, DateTimeFileUpload = @fileUploadDT
                                WHERE CompetitionID = @selectedCompetitionID AND CompetitorID = @selectedCompetitorID";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@fileSubmitted", compSubVM.FileSubmitted);
            cmd.Parameters.AddWithValue("@fileUploadDT", compSubVM.DateTimeFileUpload);
            cmd.Parameters.AddWithValue("@selectedCompetitionID", compSubVM.CompetitionID);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", compSubVM.CompetitorID);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
        }

        public void UpdateAppealRecord(CompetitionSubmissionViewModel compSubVM)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the UPDATE SQL statement
            cmd.CommandText = @"UPDATE CompetitionSubmission 
                                SET Appeal = @selectedAppeal
                                WHERE CompetitionID = @selectedCompetitionID 
                                and CompetitorID = @selectedCompetitorID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “CompetitionID” and "CompetitorID.
            cmd.Parameters.AddWithValue("@selectedAppeal", compSubVM.Appeal);
            cmd.Parameters.AddWithValue("@selectedCompetitionID", compSubVM.CompetitionID);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", compSubVM.CompetitorID);
            //Open a database connection
            conn.Open();

            //Execute command to vote
            cmd.ExecuteNonQuery();

            //A connection should be closed after operations.
            conn.Close();
        }
    }
}
