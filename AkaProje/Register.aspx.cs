using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using Serilog;

namespace AkaProje
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Error testi(log ve errorpage için)
            //throw new Exception("Testing error handling");
        }

        protected void btnKayit_Click(object sender, EventArgs e)
        { 
            
            string sifre = txtSifre.Text;
            string hashedsifre = Encryption.GetHasedPassword(sifre);

            SqlHelper sqlHelper = new SqlHelper();
            SqlParameter[] parameters =
            {
                new SqlParameter("@Ad", txtAd.Text),
                new SqlParameter("@Soyad", txtSoyad.Text),
                new SqlParameter("@Email", txtEmail.Text),
                new SqlParameter("@KullaniciAd", txtKullaniciAdi.Text),
                new SqlParameter("@Sifre", hashedsifre)
            };

            int userId = sqlHelper.ExecuteScalar("Insert_User", parameters);
                            
                switch(userId)
                {
                    case -1:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Hata!', 'Kullanıcı Adı zaten kayıtlı', 'error')", true);
                        break;
                    case -2:
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Hata!', 'Email adresi zaten kayıtlı', 'error')", true);
                        break;
                    default:
                        ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "Kayıt başarılı, Email adresinize gelen Aktivasyon linkine tıklayınız');", true);
                        SendActivationEmail(userId);
                        Response.Redirect("https://localhost:44370/Login.aspx");
                        break;
                }     
            
        }
        public void SendActivationEmail(int userId)
        {
            string activationCode = Guid.NewGuid().ToString();
            SqlHelper sqlHelper = new SqlHelper();
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@ActivationCode", activationCode)
            };
            sqlHelper.ExecuteNonQuery("INSERT INTO tbl_ActivationUser VALUES (@UserId, @ActivationCode)", parameters);

            using (MailMessage mm = new MailMessage("baran231905@outlook.com", txtEmail.Text))
            {
                mm.Subject = "Account Activation";
                string body = "Merhaba " + txtAd.Text + ",";
                body += "<br /><br />Hesabınızı aktif etmek için lütfen bir sonraki cümleye tıklayınız.";
                body += "<br /><a href = '" + Request.Url.AbsoluteUri.Replace("Register.aspx", "Activation.aspx?ActivationCode=" + activationCode) + "'>buraya tıklayınız.</a>";
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.office365.com";
                smtp.EnableSsl = true;
                NetworkCredential network = new NetworkCredential("baran231905@outlook.com", "Baran180516");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = network;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }
}