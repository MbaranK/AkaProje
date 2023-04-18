using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace AkaProje
{
    public partial class Activation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlConnection connection = sqlHelper.OpenConnection();
            SqlTransaction transaction = sqlHelper.BeginTrans(connection);
            if (!IsPostBack)
            {
                try
                {
                    string activationCode = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();

                    //ID'yi Aldığım yer
                    SqlParameter[] parameters =
                    {
                    new SqlParameter("@p1", activationCode)
                };

                    int status = sqlHelper.ExecuteScalar(connection, "SELECT Id FROM tbl_ActivationUser where ActivationCode=@p1", parameters,transaction);

                    //Aktifi Set ettiğim yer
                    SqlParameter[] parameters2 =
                    {
                    new SqlParameter("@p2", status)
                };
                    sqlHelper.ExecuteNonQuery(connection, "UPDATE tbl_User SET Aktif = 1 WHERE Id=@p2", parameters2,transaction);


                    //Önceden oluşturulan aktivasyon kodunun silindiği yer
                    SqlParameter[] parameters3 =
                    {
                    new SqlParameter("@p3", activationCode)
                };
                    int rowsAffected = sqlHelper.ExecuteNonQuery(connection, "DELETE FROM tbl_ActivationUser WHERE ActivationCode = @p3", parameters3,transaction);

                    if (rowsAffected == 1)
                    {
                        ltMessage.Text = "Aktivasyon Tamamlandı !";
                        System.Threading.Thread.Sleep(100000);
                        Response.Redirect("http://localhost:49743/Login.aspx");

                    }
                    else
                    {
                        ltMessage.Text = "Aktivasyon Kodu Geçerli Değil !";
                    }
                    sqlHelper.CommitTrans(transaction);
                }
                catch (Exception)
                {
                    sqlHelper.RollbackTrans(transaction);
                    throw;
                }
               finally
                {
                    sqlHelper.CloseConnection(connection);
                }
            }
        }
    }
}