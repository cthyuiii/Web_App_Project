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
    public class AreaInterestDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public AreaInterestDAL()
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
                        Name = reader.GetString(1), //1: 2nd Column
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return AreaInterestList;
        }
        public int Add(AreaInterest areaInterest)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated AreaUnterestID after insertion
            cmd.CommandText = @"INSERT INTO AreaInterest (Name)
                                OUTPUT INSERTED.AreaInterestID
                                VALUES(@name)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", areaInterest.Name);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            areaInterest.AreaInterestID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return areaInterest.AreaInterestID;
        }
        public AreaInterest GetDetails(int areaInterestID)
        {
            AreaInterest areaInterest = new AreaInterest();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a competition record.
            cmd.CommandText = @"SELECT * FROM AreaInterest
                        WHERE AreaInterestID = @selectedAreaInterestID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “competitionId”.
            cmd.Parameters.AddWithValue("@selectedAreaInterestID", areaInterestID);

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
                    areaInterest.AreaInterestID = areaInterestID;
                    areaInterest.Name = reader.GetString(1);
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();

            return areaInterest;
        }

        public int Delete(int areaInterestId)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a staff record specified by a Staff ID
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM AreaInterest
                                WHERE AreaInterestID = @selectAreaInterestID";
            cmd.Parameters.AddWithValue("@selectAreaInterestID", areaInterestId);
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

        public List<Competition> GetAreaInterestCompetition(int areaInterestID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SQL statement that select all branches
            cmd.CommandText = @"SELECT * FROM Competition WHERE AreaInterestID = @selectedAreaInterest";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “branchNo”.
            cmd.Parameters.AddWithValue("@selectedAreaInterest", areaInterestID);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            List<Competition> competitionList = new List<Competition>();
            while (reader.Read())
            {
                competitionList.Add(
                new Competition
                {
                    CompetitionID = reader.GetInt32(0), //0: 1st column
                    AreaInterestID = reader.GetInt32(1), //1: 2nd column
                                 //Get the first character of a string
                    CompetitionName = reader.GetString(2), //2: 3rd column
                    StartDate = !reader.IsDBNull(3) ? 
                                reader.GetDateTime(3) : (DateTime?)null, //3: 4th column
                    EndDate = !reader.IsDBNull(3) ?
                            reader.GetDateTime(3) : (DateTime?)null, //5: 6th column
                    ResultReleasedDate = !reader.IsDBNull(3) ?
                            reader.GetDateTime(3) : (DateTime?)null,
            }
                );
            }
            //Close DataReader
            reader.Close();
            //Close database connection
            conn.Close();
            return competitionList;
        }
    }  
}