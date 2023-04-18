using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AkaProje
{
    public partial class kolonEkle : System.Web.UI.Page
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
                    //SqlHelper sqlHelper = new SqlHelper();
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
            CreateDynamicV2();
        }

        
        protected void btnKolonGetir_Click(object sender, EventArgs e)
        {            
            CreateDynamicV2();
            pnlControl.Visible = true;
            //btnKolonEkle.Visible = true;
        }

        protected void CreateDynamicV2()
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlConnection connection = sqlHelper.OpenConnection();
            SqlTransaction transaction = sqlHelper.BeginTrans(connection);
            try
            {
                pnlControl.Controls.Clear();

                Label lbl = new Label();
                lbl.ClientIDMode = ClientIDMode.Static;
                lbl.Text = "Kolon İsmi: ";
                lbl.CssClass = "mt-3 dynamic-control";

                TextBox txt = new TextBox();
                txt.ID = "txt22";
                txt.ClientIDMode = ClientIDMode.Static;
                txt.ViewStateMode = ViewStateMode.Inherit;
                txt.CssClass = "dynamic-control form-control";

                RequiredFieldValidator reqTxt = new RequiredFieldValidator();
                reqTxt.ForeColor = System.Drawing.Color.Red;
                reqTxt.Display = ValidatorDisplay.Dynamic;
                reqTxt.ControlToValidate = txt.ID;
                reqTxt.ErrorMessage = "Kolon İsmi Boş Bırakılamaz";

                Label lbl2 = new Label();
                lbl2.ClientIDMode = ClientIDMode.Static;
                lbl2.Text = "Data Tipi :";
                lbl2.CssClass = "dynamic-control";

                DropDownList ddl = new DropDownList();
                ddl.ID = "ddl1";
                ddl.ClientIDMode = ClientIDMode.Static;
                ddl.ViewStateMode = ViewStateMode.Inherit;
                ddl.CssClass = "dynamic-control";

                ddl.Items.Add(new ListItem("Seçiniz", "Seçiniz"));
                ddl.Items.Add(new ListItem("Kelime", "ntext"));
                ddl.Items.Add(new ListItem("Tarih", "datetime"));
                ddl.Items.Add(new ListItem("Sayı", "int"));
                ddl.Items.Add(new ListItem("Ondalıklı Sayı", "decimal(18,2)"));

                CheckBox chk = new CheckBox();
                chk.ClientIDMode = ClientIDMode.Static;
                chk.ID = "chk21";
                chk.Text = "Allow Nulls";
                chk.CssClass = "dynamic-control mb-4";

                string selectedTableName = ddlTablolar.SelectedItem.ToString();
                int rowCount = sqlHelper.ExecuteScalar(connection, $"SELECT COUNT(*) FROM {selectedTableName}",null,transaction);

                if (rowCount > 0)
                {
                    chk.Enabled = false;
                    chk.Checked = true;

                }

                Button btn = new Button();
                btn.ClientIDMode = ClientIDMode.Static;
                btn.ID = "btnKaydet";
                btn.Text = "Kaydet";
                btn.CssClass = "text-center btn btn-info";
                btn.Click += (s, e) => {

                    DropDownList ddlobj = (DropDownList)pnlControl.FindControl("ddl1");
                    string dataTipi = ddlobj.SelectedValue;
                    string kolonİsmi = txt.Text;
                    string nullable = string.Empty;

                    if (chk.Checked)
                        nullable = "NULL";

                    else
                        nullable = "NOT NULL";

                    connection.Open();
                    sqlHelper.ExecuteNonQuery(connection, $"ALTER TABLE {selectedTableName} ADD {kolonİsmi} {dataTipi} {nullable}");

                    

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
    "swal('Başarılı', 'Kolon başarıyla oluşturuldu.', 'success')", true);

                };

                

                pnlControl.Controls.Add(lbl);
                pnlControl.Controls.Add(reqTxt);
                pnlControl.Controls.Add(txt);
                pnlControl.Controls.Add(lbl2);
                pnlControl.Controls.Add(ddl);
                pnlControl.Controls.Add(chk);
                pnlControl.Controls.Add(btn);

                pnlControl.Visible = false;

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