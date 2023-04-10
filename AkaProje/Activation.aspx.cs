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
            if(!IsPostBack)
            {
                string activationCode = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();
                SqlHelper sqlHelper = new SqlHelper();

                //ID'yi Aldığım yer
                SqlParameter[] parameters =
                {
                    new SqlParameter("@p1", activationCode)
                };

                int status = sqlHelper.ExecuteScalar("SELECT Id FROM tbl_ActivationUser where ActivationCode=@p1", parameters);

                //Aktifi Set ettiğim yer
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@p2", status)
                };
                sqlHelper.ExecuteNonQuery("UPDATE tbl_User SET Aktif = 1 WHERE Id=@p2", parameters2);


                //Önceden oluşturulan aktivasyon kodunun silindiği yer
                SqlParameter[] parameters3 =
                {
                    new SqlParameter("@p3", activationCode)
                };
                int rowsAffected = sqlHelper.ExecuteNonQuery("DELETE FROM tbl_ActivationUser WHERE ActivationCode = @p3", parameters3);

                if(rowsAffected == 1)
                {
                    ltMessage.Text = "Aktivasyon Tamamlandı !";
                    System.Threading.Thread.Sleep(100000);
                    Response.Redirect("http://localhost:49743/Login.aspx");
                    
                }
                else
                {
                    ltMessage.Text = "Aktivasyon Kodu Geçerli Değil !";
                }
            }
        }
    }
}