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
                string kullanici = Session["kullaniciadi"].ToString();
                SqlHelper sqlHelper = new SqlHelper();
                SqlDataReader dr = sqlHelper.ExecuteReader($"SELECT name FROM sys.tables WHERE name LIKE '%_{kullanici}'");

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
        }

            protected void btnTabloDüzenle_Click(object sender, EventArgs e)
            {
                CreateDynamicControls();
                pnlControl.Visible = true;
            }

        protected void CreateDynamicControls()
        {
            try
            {
                string selectedTableName = ddlTablolar.SelectedItem.ToString();
                string query = "SELECT c.name AS ColumnName " + "FROM sys.columns c " + "INNER JOIN sys.objects o ON c.object_id = o.object_id " + "WHERE o.type = 'U' AND o.name = @TableName " + "ORDER BY c.column_id";
                SqlParameter[] parameters =
                    {
                    new SqlParameter("@TableName", selectedTableName)
                };
                SqlHelper sqlHelper = new SqlHelper();
                List<string> kolonİsimleri = new List<string>();
                SqlDataReader dr = sqlHelper.ExecuteReader(query, parameters);
                while (dr.Read())
                {
                    kolonİsimleri.Add(dr.GetString(0));
                }
                dr.Close();

                pnlControl.Controls.Clear();

                int kolonSayi = sqlHelper.ExecuteScalar("SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID('" + selectedTableName + "')");

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

                    Label lbl2 = new Label();
                    lbl2.ClientIDMode = ClientIDMode.Static;
                    lbl2.Text = "Data Tipi " + (i + 1).ToString() + ":";
                    lbl2.CssClass = "dynamic-control";

                    //RequiredFieldValidator reqTxt = new RequiredFieldValidator();
                    //reqTxt.ID = "reqTxt" + (i + 1).ToString();
                    //reqTxt.ForeColor = System.Drawing.Color.Red;
                    //reqTxt.Display = ValidatorDisplay.Dynamic;
                    //reqTxt.ControlToValidate = txt.ID;
                    //reqTxt.ErrorMessage = "Kolon İsmi Boş Bırakılamaz";

                    string query2 = ($"SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{selectedTableName}' AND COLUMN_NAME = '{kolonİsimleri[i]}'");
                    string dataTipi = sqlHelper.ExecuteScalarString(query2).ToString();

                    DropDownList ddl = new DropDownList();
                    ddl.ClientIDMode = ClientIDMode.Static;
                    ddl.ID = "ddl" + (i + 1).ToString();

                    ddl.Items.Add(new ListItem("Seçiniz", "Seçiniz"));
                    ddl.Items.Add(new ListItem("Kelime", "varchar"));
                    ddl.Items.Add(new ListItem("Tarih", "datetime"));
                    ddl.Items.Add(new ListItem("Sayı", "int"));
                    ddl.Items.Add(new ListItem("Ondalıklı Sayı", "decimal(18,2)"));

                    if (dataTipi == "varchar")
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
                    string isNullable = sqlHelper.ExecuteScalarString(query3);

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
                    btn.CssClass = "btn btn-danger";
                    btn.Click += new EventHandler(test);


                    pnlControl.Controls.Add(lbl);
                    //pnlControl.Controls.Add(reqTxt);
                    pnlControl.Controls.Add(txt);
                    pnlControl.Controls.Add(lbl2);
                    pnlControl.Controls.Add(ddl);
                    pnlControl.Controls.Add(chk);
                    pnlControl.Controls.Add(btn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        //protected void btn_Click(object sender, EventArgs e, string btnID)
        //{
        //    string columnID = btnID.Replace("btn", "");

        //    string selectedTableName = ddlTablolar.SelectedValue;

        //    string columnName = ((TextBox)pnlControl.FindControl("txt" + columnID)).Text;

        //    string query = $"ALTER TABLE {selectedTableName} DROP COLUMN {columnName}";

        //    SqlHelper sqlHelper = new SqlHelper();
        //    sqlHelper.ExecuteNonQuery(query);
        //}

        protected void test(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                       "swal({title: 'Are you sure?',text: 'Once deleted, you will not be able to recover this imaginary file!',icon: 'warning',buttons: true,dangerMode: true,}).then((willDelete) => {if (willDelete){swal('Poof! Your imaginary file has been deleted!', {icon: 'success',}); }else{swal('Your imaginary file is safe!');}});", true);
        }
    }
}