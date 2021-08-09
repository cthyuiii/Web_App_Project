using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using WebApplication.Models;

namespace WebApplication.DAL
{
    public class CompetitionDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CompetitionDAL()
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

        public List<string> GetAreaOfInterest()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();            
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT Name
                                FROM AreaInterest a, Competition c
                                WHERE a.AreaInterestID = c.AreaInterestID";            
            //Open a database connection
            conn.Open();             
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> areaofinterestnameList = new List<string>();
            while (reader.Read())
            {
                areaofinterestnameList.Add(reader.GetString(0));
            }
            //Close DataReader
            reader.Close();            
            //Close the database connection
            conn.Close();

            return areaofinterestnameList;
        }

        public string GetAreaInterestJoined(int competitionId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT Name
                                FROM AreaInterest a, Competition c
                                WHERE a.AreaInterestID = c.AreaInterestID AND c.CompetitionID = @selectedCompetitionId";

            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            string aoiName = "";

            while (reader.Read())
            {
                aoiName = reader.GetString(0);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return aoiName;
        }

        public List<Competition> GetCompetitions()
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
            List<Competition> competitionList = new List<Competition>();
            while (reader.Read())
            {
                competitionList.Add(
                    new Competition
                    {
                        CompetitionID = reader.GetInt32(0),         //0: 1st column
                        AreaInterestID = reader.GetInt32(1),        //0: 2nd column
                        CompetitionName = reader.GetString(2),      //2: 3rd column
                        StartDate = reader.GetDateTime(3),          //3: 4th column
                        EndDate = reader.GetDateTime(4),            //4: 5th column
                        ResultReleasedDate = reader.GetDateTime(5), //5: 6th column
                    });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return competitionList;
        }

        public List<CompetitionViewModel> GetAllCompetition(List<string> areaofinterestnameList)
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
            List<CompetitionViewModel> competitionList = new List<CompetitionViewModel>();
            int i = 0;
            while (reader.Read())
            {
                competitionList.Add(
                    new CompetitionViewModel
                    {
                        CompetitionID = reader.GetInt32(0),         //0: 1st column
                        AreaInterest = areaofinterestnameList[i],
                        CompetitionName = reader.GetString(2),      //2: 3rd column
                        StartDate = reader.GetDateTime(3),          //3: 4th column
                        EndDate = reader.GetDateTime(4),            //4: 5th column
                        ResultReleasedDate = reader.GetDateTime(5), //5: 6th column
                    });
                i++;
            }
            //Close DataReader
            reader.Close();            
            //Close the database connection
            conn.Close();

            return competitionList;
        }

        public CompetitionViewModel GetJoinedCompetition(string areaofinterestName, int competitionId)
        {
            CompetitionViewModel compVM = new CompetitionViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Competition WHERE CompetitionID = @selectedCompetitionID ORDER BY CompetitionID";

            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    compVM.CompetitionID = reader.GetInt32(0);         //0: 1st column
                    compVM.AreaInterest = areaofinterestName;
                    compVM.CompetitionName = reader.GetString(2);      //2: 3rd column
                    compVM.StartDate = reader.GetDateTime(3);          //3: 4th column
                    compVM.EndDate = reader.GetDateTime(4);            //4: 5th column
                    compVM.ResultReleasedDate = reader.GetDateTime(5); //5: 6th column
                }
            }
      
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return compVM;
        }

        public int Add(Competition competition)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated CompetitionID after insertion
            cmd.CommandText = @"INSERT INTO Competition (AreaInterestID,CompetitionName,StartDate,EndDate,ResultReleasedDate)
                                OUTPUT INSERTED.CompetitionID
                                VALUES(@areainterestid,@competitionname,@startdate,@enddate,@resultreleaseddate)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@areainterestname", competition.AreaInterestID);
            cmd.Parameters.AddWithValue("@competitionname", competition.CompetitionName);
            cmd.Parameters.AddWithValue("@startdate", competition.StartDate);
            cmd.Parameters.AddWithValue("@enddate", competition.EndDate);
            cmd.Parameters.AddWithValue("@resultreleaseddate", competition.ResultReleasedDate);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            competition.CompetitionID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return competition.CompetitionID;
        }

        public Competition GetDetails(int competitionId)
        {
            Competition competition = new Competition();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a competition record.
            cmd.CommandText = @"SELECT * FROM Competition
                        WHERE CompetitionID = @selectedCompetitionID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “competitionId”.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill competition object with values from the data reader
                    competition.CompetitionID = competitionId;
                    competition.AreaInterestID = reader.GetInt32(1);
                    competition.CompetitionName = reader.GetString(2);
                    competition.StartDate = !reader.IsDBNull(3) ?
                                reader.GetDateTime(3) : (DateTime?)null;
                    competition.EndDate = !reader.IsDBNull(4) ?
                                reader.GetDateTime(4) : (DateTime?)null;
                    competition.ResultReleasedDate = !reader.IsDBNull(5) ?
                                reader.GetDateTime(5) : (DateTime?)null;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();

            return competition;
        }
        // Return number of row updated
        public int Update(Competition competition, Judge judge)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Staff SET AreaInterest=@areainterest, CompetitionName=@competitionName,
                                StartDate=@startDate, EndDate = @endDate, ResultReleasedDate=@resultsReleasedDate
                                JudgeID=@judgeID
                                WHERE CompetitionID = @selectedCompetitionID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@areainterest", competition.AreaInterestID);
            cmd.Parameters.AddWithValue("@competitionName", competition.CompetitionName);
            cmd.Parameters.AddWithValue("@startDate", competition.StartDate);
            cmd.Parameters.AddWithValue("@endDate", competition.EndDate);
            cmd.Parameters.AddWithValue("@resultsReleasedDate", competition.ResultReleasedDate);

            if (judge.JudgeID != 0)
                // A branch is assigned
                cmd.Parameters.AddWithValue("@judgeID", judge.JudgeID);
            else // No branch is assigned
                cmd.Parameters.AddWithValue("@judgeID", DBNull.Value);
                cmd.Parameters.AddWithValue("@selectedCompetitionID", competition.CompetitionID);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }
        public int Delete(int competitionId)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a staff record specified by a Staff ID
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Competition
                                WHERE CompetitionID = @selectCompetitionID";
            cmd.Parameters.AddWithValue("@selectCompetitionID", competitionId);
            //Open a database connection
            conn.Open();
            int rowAffected = 0;
            //Execute the DELETE SQL to remove the staff record
            rowAffected += cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
             //Return number of row of staff record updated or deleted
            return rowAffected;
        }
        
        public List<CompetitionSubmission> Rankings(List<string> competitornameList, int competitionId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT top 3 * FROM CompetitionSubmission 
                                WHERE Ranking IS NOT NULL AND CompetitionID = @selectedCompetitionID
                                ORDER BY Ranking";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “CompetitionID”.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a CompetitorSubmission list
            List<CompetitionSubmission> competitorSubmissionList =
                new List<CompetitionSubmission>();
            int i = 0;
            while (reader.Read())
            {
                competitorSubmissionList.Add(
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

            return competitorSubmissionList;
        }
        public List<CompetitionViewModel> GetAssignedCompetition(List<string> areaofinterestnameList,int judgeID)
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
            List<CompetitionViewModel> competitionList = new List<CompetitionViewModel>();
            while (reader.Read())
            {
                competitionList.Add(
                    new CompetitionViewModel
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
    }
}