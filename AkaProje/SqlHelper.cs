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

        public SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public void CloseConnection(SqlConnection connection)
        {
            connection.Close();
        }

        public int ExecuteNonQuery(SqlConnection connection,string query, SqlParameter[] parameters = null, SqlTransaction transaction = null)
        {
                try
                {
                SqlCommand cmd = new SqlCommand(query, connection);
                    if(transaction != null)
                         cmd.Transaction = transaction;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        return cmd.ExecuteNonQuery();
                        
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }               
        }

        public int ExecuteScalar(SqlConnection connection, string query, SqlParameter[] parameters = null,SqlTransaction transaction = null)
        {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                    if(transaction != null)
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
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
               
        }

        public string ExecuteScalarString(SqlConnection connection, SqlTransaction transaction, string query, SqlParameter[] parameters = null)
        {
                try
                {
                        SqlCommand cmd = new SqlCommand(query, connection);
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
                        return result;
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }

        }

        public SqlDataReader ExecuteReader(SqlConnection connection  ,string query, SqlParameter[] parameters = null, SqlTransaction transaction = null)
        {
                try
                {

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Transaction = transaction;
                    
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                    SqlDataReader reader = cmd.ExecuteReader();
                     return reader;

                }
                catch (Exception ex)
                {
                throw ex;
                }
            
        }

        public DataTable ExecuteQuery(SqlConnection connection ,string query, SqlParameter[] parameters = null)
        {
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
                    throw ex;
                }
                        
        }

        public SqlTransaction BeginTrans(SqlConnection connection)
        {
            try
            {
                SqlTransaction transaction = connection.BeginTransaction();
                return transaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
                    
        }

        public void CommitTrans(SqlTransaction transaction)
        {
            try
            {
                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (transaction.Connection != null)
                {
                    transaction.Connection.Close();
                }
            }
        }

        public void RollbackTrans(SqlTransaction transaction)
        {
            try
            {
                transaction.Rollback();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (transaction.Connection != null)
                {
                    transaction.Connection.Close();
                }
            }
        }


    }
}