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
    public class CriteriaDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CriteriaDAL()
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
        public List<Criteria> GetAllCriteria()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Criteria ORDER BY CriteriaID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a competition list
            List<Criteria> criteriaList = new List<Criteria>();
            while (reader.Read())
            {
                criteriaList.Add(
                    new Criteria
                    {
                        CriteriaID = reader.GetInt32(0), //0: 1st column
                        CompetitionID = reader.GetInt32(1), //1: 2nd column
                        CriteriaName = !reader.IsDBNull(2) ? reader.GetString(2) : null, //2: 3rd column
                        Weightage = reader.GetInt32(3), //3: 4th column
                    });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return criteriaList;
        }
        public List<Criteria> GetCompetitionCriteria(int competitionID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SQL statement that select all branches
            cmd.CommandText = @"SELECT * FROM Criteria WHERE CompetitionID = @selectedCompetition";

            //Define the parameter used in SQL statement, value for the
            cmd.Parameters.AddWithValue("@selectedCompetition", competitionID);

            //Open a database connection
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<Criteria> criteriaList = new List<Criteria>();
            while (reader.Read())
            {
                criteriaList.Add(
                new Criteria
                {
                    CriteriaID = reader.GetInt32(0), //0: 1st column
                    CompetitionID = competitionID, //1: 2nd column
                    CriteriaName = !reader.IsDBNull(2) ? reader.GetString(2) : null, //2: 3rd column
                    Weightage = reader.GetInt32(3), //3: 4th column
                }
                );
            }
            reader.Close();
            conn.Close();
            return criteriaList;
        }
        public int Add(Criteria criteria)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated CriteriaID after insertion
            cmd.CommandText = @"INSERT INTO Criteria (CompetitionID, CriteriaName, Weightage)
                                OUTPUT INSERTED.CriteriaID
                                VALUES(@compID, @critName, @Weigh)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@compID", criteria.CompetitionID);
            cmd.Parameters.AddWithValue("@critName", criteria.CriteriaName);
            cmd.Parameters.AddWithValue("@Weigh", criteria.Weightage);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //CompetitorID after executing the INSERT SQL statement
            criteria.CriteriaID = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return criteria.CriteriaID;
        }
        public void Update(Criteria criteria)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated CriteriaID after insertion
            cmd.CommandText = @"UPDATE Criteria
                                SET CriteriaName =@critName,
                                Weightage=@weigh
                                WHERE CompetitionID = @compID and CriteriaID = @selectedCritID)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@compID", criteria.CompetitionID);
            cmd.Parameters.AddWithValue("@critName", criteria.CriteriaName);
            cmd.Parameters.AddWithValue("@Weigh", criteria.Weightage);
            cmd.Parameters.AddWithValue("selectedCritID", criteria.CriteriaID);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            cmd.ExecuteNonQuery();
            //A connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
        }
        public int Delete(Criteria criteria)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a criteria specified by a CriteriaID
            int rowAffected = 0;

            SqlCommand cmd6 = conn.CreateCommand();
            cmd6.CommandText = @"UPDATE CompetitionScore SET CriteriaID=NULL
                                    WHERE CriteriaID = @selectedCriteriaID";
            cmd6.Parameters.AddWithValue("@selectedCriteriaID", criteria.CriteriaID);

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Criteria
                                WHERE CriteriaID = @selectedCriteriaID";
            cmd.Parameters.AddWithValue("@selectedCriteriaID", criteria.CriteriaID);

            //Open a database connection
            conn.Open();
            //Execute the DELETE SQL to remove the staff record
            rowAffected += cmd6.ExecuteNonQuery();
            rowAffected += cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
            //Return number of row of staff record updated or deleted
            return rowAffected;
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
                        CompetitionName = !reader.IsDBNull(2) ? reader.GetString(2) : null,      //2: 3rd column
                        StartDate = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,         //3: 4th column
                        EndDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,          //4: 5th column
                        ResultReleasedDate = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null, //5: 6th column
                    });
                i++;
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return competitionList;
        }
        public string CompetitionName(int competitionId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT CompetitionName
                                FROM Competition c
                                WHERE CompetitionID = @selectedCompetitionID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            string CompName = "";
            while (reader.Read())
            {
                CompName = !reader.IsDBNull(0) ? reader.GetString(0) : null;
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return CompName;
        }

        public string GetCritName(int criteriaId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT CriteriaName
                                FROM Criteria WHERE CriteriaID = @selectedCriteriaId";

            cmd.Parameters.AddWithValue("@selectedCriteriaId", criteriaId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            string critName = "";

            while (reader.Read())
            {
                critName = reader.GetString(0);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return critName;
        }
        public List<Criteria> GetCritDetails (int CompetitionID,int CritID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SQL statement that select all branches
            cmd.CommandText = @"SELECT * FROM Criteria WHERE CompetitionID = @selectedCompetition AND CriteriaID=@critID";

            //Define the parameter used in SQL statement, value for the
            cmd.Parameters.AddWithValue("@selectedCompetition", CompetitionID);
            cmd.Parameters.AddWithValue("@critID", CritID);
            //Open a database connection
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<Criteria> criteriaList = new List<Criteria>();
            while (reader.Read())
            {
                criteriaList.Add(
                new Criteria
                {
                    CriteriaID = reader.GetInt32(0), //0: 1st column
                    CompetitionID = CompetitionID, //1: 2nd column
                    CriteriaName = !reader.IsDBNull(2) ? reader.GetString(2) : null, //2: 3rd column
                    Weightage = reader.GetInt32(3), //3: 4th column
                }
                );
            }
            reader.Close();
            conn.Close();
            return criteriaList;
        }
    }
}
