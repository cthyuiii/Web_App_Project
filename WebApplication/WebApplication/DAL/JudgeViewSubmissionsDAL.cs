using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;
using WebApplication.Models;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication.DAL
{
    public class JudgeViewSubmissionsDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        private IWebHostEnvironment Environment;
        public JudgeViewSubmissionsDAL(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        public JudgeViewSubmissionsDAL()
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
        public List<CompetitionCompetitorViewModel> GetAllCompetitor(int competitionId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT CompetitorID
                                FROM CompetitionSubmission
                                WHERE CompetitionID = @selectedCompetitionID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<CompetitionCompetitorViewModel> competitorList = new List<CompetitionCompetitorViewModel>();
            while (reader.Read())
            {
                competitorList.Add(
                    new CompetitionCompetitorViewModel
                    {
                        // Fill Competitor object with values from the data reader
                        CompetitorID = reader.GetInt32(0),    //0: 1st column
                        CompetitionID = competitionId
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return competitorList;
        }
        public List<JudgeViewSubmissions> GetScores(int competitionId, int competitorId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SQL statement that select all branches
            cmd.CommandText = @"SELECT cs.CompetitionID,cs.CompetitorID, cs.CriteriaID, c.CriteriaName, cs.Score, cc.ResultReleasedDate
                                FROM CompetitionScore cs 
                                INNER JOIN Criteria c
                                ON cs.CompetitionID = c.CompetitionID
                                INNER JOIN Competition cc
                                on cs.CompetitionID =cc.CompetitionID
                                WHERE cs.CriteriaID = c.CriteriaID 
                                AND cs.CompetitorID=@selectedCompetitorID
                                AND c.CompetitionID=@selectedCompetitionId
                                AND cc.CompetitionID=@selectedCompetitionId";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “competitionNo”.
            cmd.Parameters.AddWithValue("@selectedCompetitionId", competitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorId);
            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<JudgeViewSubmissions> scoreList = new List<JudgeViewSubmissions>();
            while (reader.Read())
            {
                scoreList.Add(
                new JudgeViewSubmissions
                {
                    CompetitionID = reader.GetInt32(0), //0: 1st column
                    CompetitorID = reader.GetInt32(1), //1: 2nd column
                    CriteriaID = reader.GetInt32(2), //3: 4th column
                    CriteriaName = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                    Score = reader.GetInt32(4),
                    ResultReleasedDate = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close database connection
            conn.Close();
            return scoreList;
        }
        public JudgeViewSubmissions GetCompetitorSubmission(int competitorId, int competitionId)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT *
                                FROM CompetitionSubmission
                                WHERE CompetitionID = @selectedCompetitionID
                                AND CompetitorID=@selectedCompetitorID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            JudgeViewSubmissions jvm = new JudgeViewSubmissions();
            while (reader.Read())
            {
                jvm.CompetitionID = reader.GetInt32(0);
                jvm.CompetitorID = reader.GetInt32(1);
                jvm.FileSubmitted = !reader.IsDBNull(2) ? reader.GetString(2) : null;
            }
            //explanation of above code
            //if (!reader.IsDBNull(2))
            //{
            //    jvm.FileSubmitted = reader.GetString(2);
            //}
            //else
            //{
            //    jvm.FileSubmitted = null;
            //}
            //Close DataReader
            reader.Close();
            //Close database connection
            conn.Close();
            return (jvm);
        }
        public List<JudgeViewSubmissions> GetCompetitionJudge(List<string> areaofinterestnameList,int judgeID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT *
                                FROM Competition c 
                                INNER JOIN CompetitionJudge cj
                                ON c.CompetitionID = cj.CompetitionID
                                WHERE cj.JudgeID=@judgeId";
            cmd.Parameters.AddWithValue("@judgeID", judgeID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a competition list
            List<JudgeViewSubmissions> competitionList = new List<JudgeViewSubmissions>();
            while (reader.Read())
            {
                competitionList.Add(
                    new JudgeViewSubmissions
                    {
                        CompetitionID = reader.GetInt32(0),         //0: 1st column
                        AreaInterest = areaofinterestnameList[reader.GetInt32(1)],
                        CompetitionName = !reader.IsDBNull(2) ? reader.GetString(2) : null,      //2: 3rd column
                        StartDate = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,         //3: 4th column
                        EndDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,           //4: 5th column
                        ResultReleasedDate = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null, //5: 6th column
                    });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return competitionList;
        }
        public List<JudgeViewSubmissions> GetAllCompetition(List<string> areaofinterestnameList)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Competition ORDER BY CompetitionID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a competition list
            List<JudgeViewSubmissions> competitionList = new List<JudgeViewSubmissions>();
            int i = 0;
            while (reader.Read())
            {
                competitionList.Add(
                    new JudgeViewSubmissions
                    {
                        CompetitionID = reader.GetInt32(0),         //0: 1st column
                        AreaInterest = areaofinterestnameList[i],
                        CompetitionName = !reader.IsDBNull(2) ? reader.GetString(2) : null,      //2: 3rd column
                        StartDate = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,         //3: 4th column
                        EndDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,           //4: 5th column
                        ResultReleasedDate = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null, //5: 6th column
                    });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return competitionList;
        }
        public int GetScore(JudgeViewSubmissions jvm)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated CriteriaID after insertion
            cmd.CommandText = @"SELECT Score
                                FROM CompetitionScore
                                WHERE CompetitionID=@competitionID AND CompetitorID=@competitorID AND CriteriaID=@critId";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@competitionID",jvm.CompetitionID );
            cmd.Parameters.AddWithValue("@competitorID",jvm.CompetitorID );
            cmd.Parameters.AddWithValue("@critId", jvm.CriteriaID);

            //A connection to database must be opened before any operations made.
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                jvm.Score = reader.GetInt32(0);
            }
            //A connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return jvm.Score;
        }
        public void Add(JudgeViewSubmissions jvm)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated CriteriaID after insertion
            cmd.CommandText = @"INSERT INTO CompetitionScore (CriteriaID, CompetitorID, CompetitionID, Score)
                                VALUES(@critID, @competitorID, @competitionID, @score)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@critID", jvm.CriteriaID);
            cmd.Parameters.AddWithValue("@competitorID", jvm.CompetitorID);
            cmd.Parameters.AddWithValue("@competitionID", jvm.CompetitionID);
            cmd.Parameters.AddWithValue("@score", jvm.Score);

            //A connection to database must be opened before any operations made.
            conn.Open();

            cmd.ExecuteNonQuery();
            //A connection should be closed after operations.
            conn.Close();
        }
        public List<JudgeViewSubmissions> VMCheckList(int CompetitionId, int CompetitorId)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT CriteriaID,CompetitionID,CompetitorID
                                From CompetitionScore
                                WHERE CompetitionID=@selectedCompetitionID AND CompetitorID = @selectedCompetitorID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", CompetitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", CompetitorId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            List<JudgeViewSubmissions> jVM = new List<JudgeViewSubmissions>();
            while (reader.Read())
            {
                jVM.Add(
                    new JudgeViewSubmissions
                    {
                        CriteriaID = reader.GetInt32(0),
                        CompetitionID = reader.GetInt32(1),
                        CompetitorID = reader.GetInt32(2),
                    });
            }
            //Close DataReader
            reader.Close();
            //Close database connection
            conn.Close();
            return (jVM);
        }
        public void Update(JudgeViewSubmissions jvm)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the UPDATE SQL statement
            cmd.CommandText = @"UPDATE CompetitionScore
                                SET Score = @score
                                WHERE CompetitionID = @selectedCompetitionId
                                and CompetitorID = @selectedCompetitorId
                                and CriteriaID = @selectedCritId";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “CompetitionID” and "CompetitorID.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", jvm.CompetitionID);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", jvm.CompetitorID);
            cmd.Parameters.AddWithValue("@selectedCritId", jvm.CriteriaID);
            cmd.Parameters.AddWithValue("@score", jvm.Score);
            //Open a database connection
            conn.Open();

            //Execute command to vote
            cmd.ExecuteNonQuery();

            //A connection should be closed after operations.
            conn.Close();
        }
    }
}
