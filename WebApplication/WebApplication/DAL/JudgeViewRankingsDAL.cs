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
    public class JudgeViewRankingsDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        public JudgeViewRankingsDAL()
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
        public JudgeViewRankings GetRankingDetails(int competitionID, int competitorID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * 
                                FROM CompetitionSubmission
                                WHERE CompetitionID=@selectedCompetitionID
                                AND CompetitorID = @selectedCompetitorID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionID);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            JudgeViewRankings jvr = new JudgeViewRankings();
            while (reader.Read())
            {
                // Fill Competitor object with values from the data reader
                jvr.Appeal = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                jvr.VoteCount = !reader.IsDBNull(5) ? reader.GetInt32(5) : (int?)null;
                jvr.Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null;
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return jvr;
        }
        public List<JudgeViewRankings> scoresList(int competitionID, int competitorID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT *
                                FROM CompetitionScore
                                WHERE CompetitionID=@selectedCompetitionID
                                AND CompetitorID = @selectedCompetitorID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionID);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            JudgeViewRankings jvr = new JudgeViewRankings();
            while (reader.Read())
            {
                jvr.scoresList.Add(
                    new JudgeViewRankings
                    {   
                        CriteriaID = reader.GetInt32(0),
                        CompetitionID = competitionID,
                        CompetitorID = competitorID,
                        Scores = reader.GetInt32(3)
                    });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return jvr.scoresList;
        }
        public List<JudgeViewRankings> WeightageList(int competitionId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT *
                                FROM Criteria
                                WHERE CompetitionID=@selectedCompetitionID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            JudgeViewRankings jvr = new JudgeViewRankings();
            while (reader.Read())
            {
                jvr.weightageList.Add(
                    new JudgeViewRankings
                    {
                        CriteriaID = reader.GetInt32(0),
                        CriteriaName = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                        Weightage = reader.GetInt32(3)
                    });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return jvr.weightageList;
        }
        public void Update(JudgeViewRankings jvr)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the UPDATE SQL statement
            cmd.CommandText = @"UPDATE CompetitionSubmission 
                                SET Ranking = @rank
                                WHERE CompetitionID = @selectedCompetitionID
                                and CompetitorID = @selectedCompetitorID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “CompetitionID” and "CompetitorID.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", jvr.CompetitionID);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", jvr.CompetitorID);
            cmd.Parameters.AddWithValue("@rank", jvr.Ranking);
            //Open a database connection
            conn.Open();

            //Execute command to vote
            cmd.ExecuteNonQuery();

            //A connection should be closed after operations.
            conn.Close();
        }
    }
}
