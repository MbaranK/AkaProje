using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Web.Configuration;

namespace AkaProje
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                HttpCookie cerezal = Request.Cookies["cerezler"];
                if(cerezal!= null)
                {
                    txtKullanici.Text = cerezal["kullaniciad"];
                }
            }
        }

        protected void btnGiris_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                SqlHelper sqlHelper = new SqlHelper();
                ////Bağlantıyı aç
                SqlConnection connection = sqlHelper.OpenConnection();
                try
                {
                    string sifre = txtSifre.Text;
                    string hashedsifre = Encryption.GetHasedPassword(sifre);
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@p1",txtKullanici.Text),
                        new SqlParameter("@p2",sifre)
                    };
                                                         
                    SqlDataReader dr = sqlHelper.ExecuteReader(connection,"Select * from tbl_User where KullaniciAd=@p1 and Sifre=@p2", parameters);

                    if (dr.Read())
                    {
                        int aktif = Convert.ToInt32(dr["Aktif"]);
                        if (aktif == 1)
                        {
                            if(chkRemember.Checked)
                            {
                                string cookieValue = Request.Cookies["cerezler"].Value;
                                Session["kullaniciadi"] = cookieValue;
                                Configuration con = WebConfigurationManager.OpenWebConfiguration("~/web.config/");
                                SessionStateSection section = (SessionStateSection)con.GetSection("system.web/sessionState");
                            }
                            else
                            {
                                Session["kullaniciadi"] = txtKullanici.Text;
                                Configuration con = WebConfigurationManager.OpenWebConfiguration("~/web.config/");
                                SessionStateSection section = (SessionStateSection)con.GetSection("system.web/sessionState");
                            }
                            //sqlHelper.CommitTrans(transaction);
                            Response.Redirect("https://localhost:44370/Default.aspx");
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                        "swal('Hata!', 'Hesabınız pasif durumda', 'error')", true);
                        }
                        
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                        "swal('Hata!', 'Kullanıcı Adı veya Şifre Hatalı', 'error')", true);
                    }
                    dr.Close();
                }
                catch(Exception)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                        "swal('Hata!', '"+ "Beklenmedik bir hata oluştu, yönetici ile iletişime geçiniz." + "', 'error')", true);
                }
                finally
                {
                    sqlHelper.CloseConnection(connection);
                }
            }
        }

        protected void chkRemember_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HttpCookie cerez = new HttpCookie("cerezler");
                cerez["kullaniciad"] = txtKullanici.Text;
                cerez.Expires = DateTime.Now.AddHours(8);
                Response.Cookies.Add(cerez);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
    }
