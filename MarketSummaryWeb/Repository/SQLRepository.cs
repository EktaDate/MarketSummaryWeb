using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MarketSummaryWeb.Models;

namespace MarketSummaryWeb.Repository
{
    public class SQLRepository
    {
        private SqlConnection connection = null;
        private void initialiseConnection()
        {
            if (connection == null )
            {
                SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder();
                connBuilder.DataSource = "masteksql.database.windows.net";
                connBuilder.UserID = "yashpal";
                connBuilder.Password = "Welcome498$";
                connBuilder.InitialCatalog = "ProspectData";
                connection = new SqlConnection(connBuilder.ConnectionString);             
            }            
        }

        public List<ProspectSearchCriteria> GetProspectData(string WhereClause ="")
        {
            try
            {
                initialiseConnection();
                connection.Open();
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("SELECT * from  ProspectDataSearchCriteria");
                if (!string.IsNullOrEmpty(WhereClause))
                {
                    strBuilder.Append(WhereClause);
                }             
                string cmdText = strBuilder.ToString();
                DataTable dataTableProspects = new DataTable();
                using (SqlCommand sqlCmd = new SqlCommand(cmdText, connection))
                {
                    SqlDataAdapter sqlda = new SqlDataAdapter(sqlCmd);
                    sqlda.Fill(dataTableProspects);
                }
                List<ProspectSearchCriteria> prospectList = new List<ProspectSearchCriteria>();
                foreach (DataRow dataRow in dataTableProspects.Rows)
                {

                    prospectList.Add(
                        new ProspectSearchCriteria
                        {

                            Id = Convert.ToString(dataRow["Id"]),
                            ProspectName = Convert.ToString(dataRow["ProspectName"]),
                            SearchString = Convert.ToString(dataRow["SearchString"]),
                            TwitterUpdates = Convert.ToBoolean(dataRow["TwitterUpdates"]),
                            FacebookUpdates = Convert.ToBoolean(dataRow["FacebookUpdates"]),
                            LinkedinUpdates = Convert.ToBoolean(dataRow["LinkedinUpdates"]),
                            EmailUpdates = Convert.ToBoolean(dataRow["EmailUpdates"]),
                            BingSearchUpdates = Convert.ToBoolean(dataRow["BingSearchUpdates"]),  
                        }
                        );


                }

                return prospectList;                
            }
            catch (SqlException e)
            {
                throw;
            }
            
        }

        public bool InsertProspects(ProspectSearchCriteria prospectSearchCriteria)
        {

            initialiseConnection();
            SqlCommand com = new SqlCommand("Insert into ProspectDataSearchCriteria(ProspectName,SearchString,TwitterUpdates,FacebookUpdates,LinkedinUpdates,EmailUpdates,BingSearchUpdates) values(@ProspectName,@SearchString,@TwitterUpdates,@FacebookUpdates,@LinkedinUpdates,@EmailUpdates,@BingSearchUpdates)", connection);
            com.CommandType = CommandType.Text;
            com.Parameters.AddWithValue("@ProspectName", prospectSearchCriteria.ProspectName);
            com.Parameters.AddWithValue("@SearchString", prospectSearchCriteria.SearchString);
            com.Parameters.AddWithValue("@TwitterUpdates", prospectSearchCriteria.TwitterUpdates);
            com.Parameters.AddWithValue("@FacebookUpdates", prospectSearchCriteria.FacebookUpdates);
            com.Parameters.AddWithValue("@LinkedinUpdates", prospectSearchCriteria.LinkedinUpdates);
            com.Parameters.AddWithValue("@EmailUpdates", prospectSearchCriteria.EmailUpdates);
            com.Parameters.AddWithValue("@BingSearchUpdates", prospectSearchCriteria.BingSearchUpdates);

            connection.Open();
            int rows = com.ExecuteNonQuery();
            connection.Close();
            return rows >= 1 ? true : false;            
        }

        public bool UpdateProspects(ProspectSearchCriteria prospectSearchCriteria)
        {

            initialiseConnection();
            SqlCommand com = new SqlCommand("Update ProspectDataSearchCriteria set ProspectName=@ProspectName,SearchString=@SearchString,TwitterUpdates=@TwitterUpdates,FacebookUpdates=@FacebookUpdates,LinkedinUpdates=@LinkedinUpdates,EmailUpdates=@EmailUpdates,BingSearchUpdates=@BingSearchUpdates Where Id=@Id", connection);

            com.CommandType = CommandType.Text;
            com.Parameters.AddWithValue("@Id", prospectSearchCriteria.Id);
            com.Parameters.AddWithValue("@ProspectName", prospectSearchCriteria.ProspectName);
            com.Parameters.AddWithValue("@SearchString", prospectSearchCriteria.SearchString);
            com.Parameters.AddWithValue("@TwitterUpdates", prospectSearchCriteria.TwitterUpdates);
            com.Parameters.AddWithValue("@FacebookUpdates", prospectSearchCriteria.FacebookUpdates);
            com.Parameters.AddWithValue("@LinkedinUpdates", prospectSearchCriteria.LinkedinUpdates);
            com.Parameters.AddWithValue("@EmailUpdates", prospectSearchCriteria.EmailUpdates);
            com.Parameters.AddWithValue("@BingSearchUpdates", prospectSearchCriteria.BingSearchUpdates);

            connection.Open();
            int rows = com.ExecuteNonQuery();
            connection.Close();
            return rows >= 1 ? true : false;
        }
        //To delete Employee details    
        public bool DeleteProspects(string Id)
        {

            initialiseConnection();
            SqlCommand com = new SqlCommand("delete from ProspectDataSearchCriteria where id=@id", connection);

            com.CommandType = CommandType.Text;
            com.Parameters.AddWithValue("@id", Id);

            connection.Open();
            int rows = com.ExecuteNonQuery();
            connection.Close();
            return rows >= 1 ? true : false;
        }
    }
}