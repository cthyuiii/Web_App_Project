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
    public class CompetitionScoreDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CompetitionScoreDAL()
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

        public List<CompetitionScore> GetScoresOfJoinedComp(int competitionId, int competitorId)
        {
            List<CompetitionScore> compScoreList = new List<CompetitionScore>();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM CompetitionScore WHERE CompetitionID = @selectedCompetitionId AND CompetitorID = @selectedCompetitorId";

            //Define the parameters used in SQL statement
            cmd.Parameters.AddWithValue("@selectedCompetitionId", competitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorId", competitorId);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read the record from database
            while (reader.Read())
            {
                compScoreList.Add(
                    new CompetitionScore
                    {
                        CriteriaID = reader.GetInt32(0),        //0: 1st column
                        CompetitorID = competitorId,        //0: 2nd column
                        CompetitionID = competitionId,      //2: 3rd column
                        Score = reader.GetInt32(3),          //3: 4th column     
                    }
                );

            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return compScoreList;
        }
    }
}
