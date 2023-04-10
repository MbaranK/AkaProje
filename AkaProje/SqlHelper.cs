using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AkaProje
{
    public class SqlHelper
    {
        private string connectionString;

        public SqlHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString;
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Transaction = transaction;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        transaction.Commit();
                        return cmd.ExecuteNonQuery();
                        
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                
            }
        }

        public int ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Transaction = transaction;

                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        if (query.ToLower().StartsWith("exec ") || query.Trim().IndexOf(' ') == -1)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                        }

                        int result = Convert.ToInt32(cmd.ExecuteScalar());
                        transaction.Commit();
                        return result;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
               
            }
        }

        public string ExecuteScalarString(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Transaction = transaction;

                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        if (query.ToLower().StartsWith("exec ") || query.Trim().IndexOf(' ') == -1)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                        }

                        string result = (cmd.ExecuteScalar()).ToString();
                        transaction.Commit();
                        return result;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        public SqlDataReader ExecuteReader(string query, SqlParameter[] parameters = null)
        {
            SqlConnection connection = new SqlConnection(connectionString);           
                connection.Open();
                try
                {

                SqlCommand cmd = new SqlCommand(query, connection);
                    
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                    SqlDataReader reader = cmd.ExecuteReader();
                     return reader;

                }
                catch (Exception ex)
                {
                throw;
                }
            
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        if (parameters != null)
                        {
                            adapter.SelectCommand.Parameters.AddRange(parameters);
                        }

                        adapter.Fill(dataTable);                                             
                    }
                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw;
                }
               
            }
        }

    }
}