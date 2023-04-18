using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AkaProje
{
    public partial class tabloDüzenle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlHelper sqlHelper = new SqlHelper();
                SqlConnection connection = sqlHelper.OpenConnection();
                try
                {
                    string kullanici = Session["kullaniciadi"].ToString();
                    SqlDataReader dr = sqlHelper.ExecuteReader(connection,$"SELECT name FROM sys.tables WHERE name LIKE '%_{kullanici}'");

                    List<string> tablolar = new List<string>();

                    while (dr.Read())
                    {
                        string tableName = (string)dr["name"];
                        tablolar.Add(tableName);
                    }
                    dr.Close();

                    ddlTablolar.DataSource = tablolar;
                    ddlTablolar.DataBind();
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlHelper.CloseConnection(connection);
                }

            }
            CreateDynamicControls();
        }

            protected void btnTabloDüzenle_Click(object sender, EventArgs e)
            {
                CreateDynamicControls();
                pnlControl.Visible = true;
            }

        protected void CreateDynamicControls()
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlConnection connection = sqlHelper.OpenConnection();
            SqlTransaction transaction = sqlHelper.BeginTrans(connection);
            try
            {
                string selectedTableName = ddlTablolar.SelectedItem.ToString();
                string query = "SELECT c.name AS ColumnName " + "FROM sys.columns c " + "INNER JOIN sys.objects o ON c.object_id = o.object_id " + "WHERE o.type = 'U' AND o.name = @TableName " + "ORDER BY c.column_id";
                SqlParameter[] parameters =
                    {
                    new SqlParameter("@TableName", selectedTableName)
                };
                
                List<string> kolonİsimleri = new List<string>();
                SqlDataReader dr = sqlHelper.ExecuteReader(connection,query, parameters,transaction);
                while (dr.Read())
                {
                    kolonİsimleri.Add(dr.GetString(0));
                }
                dr.Close();

                pnlControl.Controls.Clear();

                int kolonSayi = sqlHelper.ExecuteScalar(connection,"SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID('" + selectedTableName + "')",null,transaction);

                for (int i = 0; i < kolonSayi; i++)
                {
                    Label lbl = new Label();
                    lbl.ClientIDMode = ClientIDMode.Static;
                    lbl.Text = "Kolon " + (i + 1).ToString() + ":";
                    lbl.CssClass = "mt-3 dynamic-control";

                    TextBox txt = new TextBox();
                    txt.ClientIDMode = ClientIDMode.Static;
                    txt.ID = "txt" + (i + 1).ToString();
                    txt.Text = kolonİsimleri[i];
                    txt.ViewStateMode = ViewStateMode.Inherit;
                    txt.CssClass = "dynamic-control form-control";

                    Label lbl3 = new Label();
                    lbl3.ClientIDMode = ClientIDMode.Static;
                    lbl3.Text = kolonİsimleri[i];
                    lbl3.Visible = false;

                    Label lbl2 = new Label();
                    lbl2.ClientIDMode = ClientIDMode.Static;
                    lbl2.Text = "Data Tipi " + (i + 1).ToString() + ":";
                    lbl2.CssClass = "dynamic-control";

                    RequiredFieldValidator reqTxt = new RequiredFieldValidator();
                    reqTxt.ID = "reqTxt" + (i + 1).ToString();
                    reqTxt.ForeColor = System.Drawing.Color.Red;
                    reqTxt.Display = ValidatorDisplay.Dynamic;
                    reqTxt.ControlToValidate = txt.ID;
                    reqTxt.ErrorMessage = "Kolon İsmi Boş Bırakılamaz";                 

                    string query2 = ($"SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{selectedTableName}' AND COLUMN_NAME = '{kolonİsimleri[i]}'");
                    string dataTipi = sqlHelper.ExecuteScalarString(connection,transaction,query2).ToString();

                    DropDownList ddl = new DropDownList();
                    ddl.ClientIDMode = ClientIDMode.Static;
                    ddl.ID = "ddl" + (i + 1).ToString();

                    ddl.Items.Add(new ListItem("Seçiniz", "Seçiniz"));
                    ddl.Items.Add(new ListItem("Kelime", "ntext"));
                    ddl.Items.Add(new ListItem("Tarih", "datetime"));
                    ddl.Items.Add(new ListItem("Sayı", "int"));
                    ddl.Items.Add(new ListItem("Ondalıklı Sayı", "decimal"));

                    if (dataTipi == "ntext")
                    {
                        ddl.Items.Add("Kelime");
                    }
                    else if (dataTipi == "datetime")
                    {
                        ddl.Items.Add("Tarih");
                    }
                    else if (dataTipi == "int")
                    {
                        ddl.Items.Add("Sayı");
                    }
                    else
                    {
                        ddl.Items.Add("Ondalıklı Sayı");
                    }
                    ddl.SelectedValue = dataTipi;
                    ddl.ViewStateMode = ViewStateMode.Inherit;
                    ddl.CssClass = "dynamic-control";

                    CheckBox chk = new CheckBox();
                    chk.ClientIDMode = ClientIDMode.Static;
                    chk.ID = "chk" + (i + 1).ToString();
                    chk.Text = "Allow Nulls";
                    chk.CssClass = "dynamic-control";

                    string query3 = ($"SELECT IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{selectedTableName}' AND COLUMN_NAME = '{kolonİsimleri[i]}'");
                    string isNullable = sqlHelper.ExecuteScalarString(connection,transaction,query3);

                    if (isNullable == "YES")
                    {
                        chk.Checked = true;
                    }
                    else
                    {
                        chk.Checked = false;
                    }

                    Button btn = new Button();
                    btn.ClientIDMode = ClientIDMode.Static;
                    btn.ID = "btn" + (i + 1).ToString();
                    btn.Text = "Sil";
                    btn.CssClass = "inline btn btn-danger";
                    btn.Click += (s, e) => {                       
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                       "swal({title: 'Emin misiniz?',text: 'İşlem gerçekleşirse, kolon kalıcı olarak veritabanından silinecektir.',icon: 'warning',buttons: true,dangerMode: true,}).then((willDelete) => {if (willDelete){swal('Kolon veritabanından silindi!', {icon: 'success',}); }else{swal('Silme işlemi iptal edildi.');}});", true);
                        string kolon = txt.Text;
                        connection.Open();
                        sqlHelper.ExecuteNonQuery(connection,$"ALTER TABLE {selectedTableName} DROP COLUMN {kolon}",null,transaction);
                    };


                    Button btn2 = new Button();
                    btn2.ClientIDMode = ClientIDMode.Static;
                    btn2.ID = "btn2" + (i + 1).ToString();
                    btn2.Text = "Güncelle";
                    btn2.CssClass = "inline btn btn-warning";
                    btn2.Click += (s, e) => {
                        connection.Open();
                        string kolon = txt.Text;
                            string kolonData = lbl3.Text;

                            string veriTabanıData = dataTipi.ToString();
                            string seciliData = ddl.SelectedValue;

                        if (seciliData == "decimal")
                            seciliData = "decimal(18,2)";

                            if (kolonData != kolon)
                            {
                                sqlHelper.ExecuteNonQuery(connection,$"EXEC sp_rename '{selectedTableName}.{kolonData}', '{kolon}'",null,transaction);
                            }


                            if (seciliData == "int" && veriTabanıData.Contains("decimal") || seciliData.Contains("decimal") && veriTabanıData == "int" || seciliData.Contains("ntext") && veriTabanıData == "int" || seciliData.Contains("ntext") && veriTabanıData == "datetime" || seciliData.Contains("ntext") && veriTabanıData.Contains("ntext") || seciliData.Contains("int") && veriTabanıData == "int" || seciliData.Contains("datetime") && veriTabanıData == "datetime" || seciliData.Contains("decimal") && veriTabanıData.Contains("decimal"))
                            {
                                sqlHelper.ExecuteNonQuery(connection,$"ALTER TABLE {selectedTableName} ALTER COLUMN {kolon} {seciliData}",null, transaction);
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                            "swal('Hata!', 'Geçersiz dönüşüm isteği', 'error')", true);
                            }

                            if (chk.Checked)
                            {
                                sqlHelper.ExecuteNonQuery(connection,$"ALTER TABLE {selectedTableName} ALTER COLUMN {kolon} {seciliData} NULL",null,transaction);
                            }
                            else
                            {
                                sqlHelper.ExecuteNonQuery(connection,$"ALTER TABLE {selectedTableName} ALTER COLUMN {kolon} {seciliData} NOT NULL",null,transaction);
                            }

                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                           "swal('Başarılı', 'Tablonuzu güncellendi.', 'success');", true);
                        
                    };

                    pnlControl.Controls.Add(lbl);
                    pnlControl.Controls.Add(reqTxt);
                    pnlControl.Controls.Add(txt);
                    pnlControl.Controls.Add(lbl2);
                    pnlControl.Controls.Add(ddl);
                    pnlControl.Controls.Add(chk);
                    pnlControl.Controls.Add(btn);
                    pnlControl.Controls.Add(btn2);
                    pnlControl.Controls.Add(lbl3);

                    

                }
                sqlHelper.CommitTrans(transaction);
            }
            catch (Exception)
            {
                sqlHelper.RollbackTrans(transaction);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                        "swal('Hata!', '" + "Beklenmedik bir hata oluştu, yönetici ile iletişime geçiniz." + "', 'error')", true);
            }
            finally
            {
                sqlHelper.CloseConnection(connection);
            }
        }
    }
}