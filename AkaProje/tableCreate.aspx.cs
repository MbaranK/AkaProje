using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AkaProje
{
    public partial class tableCreate : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // Only create the dynamic controls during the initial page load
                ScriptManager1.RegisterAsyncPostBackControl(btnCogalt);
                //txtTekrar.Text = "1";
                //CreateDynamicControls();
            }
            else
            {
                // Recreate the dynamic controls during every postback, including partial postbacks
                RecreateDynamicControls();
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args.Value == "Seçiniz")
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void btnCogalt_Click(object sender, EventArgs e)
        {
            RecreateDynamicControls();
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string tableName = txtTablo.Text;
                string username = Session["kullaniciadi"].ToString();
                int numControls = int.Parse(txtTekrar.Text);

                string query = $"CREATE TABLE {tableName}_{username} (ID int PRIMARY KEY IDENTITY";

                for (int i = 1; i <= numControls; i++)
                {
                    string txtcolumnName = "txt" + i.ToString();
                    string ddlColumnType = "ddl" + i.ToString();
                    string chkAllowNulls = "chk" + i.ToString();


                    TextBox txtobj = (TextBox)pnlControls.FindControl(txtcolumnName);
                    DropDownList ddlobj = (DropDownList)pnlControls.FindControl(ddlColumnType);
                    CheckBox chkobj = (CheckBox)pnlControls.FindControl(chkAllowNulls);

                    string columnName = txtobj.Text;
                    string columnType = ddlobj.SelectedValue;
                    bool allowNulls = chkobj.Checked;

                    query += $", {columnName} {columnType} {(allowNulls ? "NULL" : "NOT NULL")}";
                }
                query += ");";

                SqlHelper sqlHelper = new SqlHelper();
                sqlHelper.ExecuteNonQuery(query);

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                       "swal('Başarılı', 'Tablonuzu başarıyla oluşturdunuz.', 'success').then(function(){window.location.href='http://localhost:49743/Default.aspx';});", true);
                
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected void CreateDynamicControls()
        {
            try
            {
                int numControls = int.Parse(txtTekrar.Text);

                for (int i = 0; i < numControls; i++)
                {
                    Label lbl = new Label();
                    lbl.ClientIDMode = ClientIDMode.Static;
                    lbl.ID = "lbl" + (i + 1).ToString();
                    lbl.Text = "Kolon " + (i + 1).ToString() + ":" + "<br>";
                    lbl.CssClass = "dynamic-control";

                    TextBox txt = new TextBox();
                    txt.ClientIDMode = ClientIDMode.Static;
                    txt.ID = "txt" + (i + 1).ToString();
                    txt.ViewStateMode = ViewStateMode.Inherit;
                    txt.CssClass = "dynamic-control form-control";

                    RequiredFieldValidator reqTxt = new RequiredFieldValidator();
                    reqTxt.ID = "reqTxt" + (i + 1).ToString();
                    reqTxt.ForeColor = System.Drawing.Color.Red;
                    reqTxt.Display = ValidatorDisplay.Dynamic;
                    reqTxt.ControlToValidate = txt.ID;
                    reqTxt.ErrorMessage = "Kolon İsmi Boş Bırakılamaz";

                    RegularExpressionValidator regExTxt = new RegularExpressionValidator();
                    regExTxt.ID = "regExTxt" + (i + 1).ToString();
                    regExTxt.ControlToValidate = txt.ID;
                    regExTxt.ForeColor = System.Drawing.Color.Red;
                    regExTxt.Display = ValidatorDisplay.Dynamic;
                    regExTxt.ErrorMessage = "Lütfen Geçerli Bir Kolon İsmi Giriniz.";
                    regExTxt.ValidationExpression = @"^[a-zA-Z_][a-zA-Z0-9_]*$";

                    Label lbl2 = new Label();
                    lbl2.ClientIDMode = ClientIDMode.Static;
                    lbl2.Text = "Data Tipi " + (i + 1).ToString() + ":";
                    lbl2.CssClass = "dynamic-control";

                    DropDownList ddl = new DropDownList();
                    ddl.ClientIDMode = ClientIDMode.Static;
                    ddl.ID = "ddl" + (i + 1).ToString();
                    ddl.ViewStateMode = ViewStateMode.Inherit;
                    ddl.CssClass = "dynamic-control";

                    ddl.Items.Add(new ListItem("Seçiniz", "Seçiniz"));
                    ddl.Items.Add(new ListItem("Kelime", "varchar(50)"));
                    ddl.Items.Add(new ListItem("Tarih", "datetime"));
                    ddl.Items.Add(new ListItem("Sayı", "int"));
                    ddl.Items.Add(new ListItem("Ondalıklı Sayı", "decimal(18,2)"));

                    RequiredFieldValidator reqDdl = new RequiredFieldValidator();
                    reqDdl.ID = "reqDdl" + (i + 1).ToString();
                    reqDdl.ControlToValidate = ddl.ID;
                    reqDdl.ForeColor = System.Drawing.Color.Red;
                    reqDdl.Display = ValidatorDisplay.Dynamic;
                    reqDdl.InitialValue = "Seçiniz";
                    reqDdl.ErrorMessage = "Lütfen Geçerli Data tipi Seçiniz";

                    // Create a new check box for the text box
                    CheckBox chk = new CheckBox();
                    chk.ClientIDMode = ClientIDMode.Static;
                    chk.ID = "chk" + (i + 1).ToString();
                    chk.Text = "Allow Nulls";
                    chk.CssClass = "dynamic-control mb-4";

                    pnlControls.Controls.Add(lbl);
                    pnlControls.Controls.Add(reqTxt);
                    pnlControls.Controls.Add(regExTxt);
                    pnlControls.Controls.Add(txt);
                    pnlControls.Controls.Add(lbl2);
                    pnlControls.Controls.Add(reqDdl);
                    pnlControls.Controls.Add(ddl);
                    pnlControls.Controls.Add(chk);

                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        private void RecreateDynamicControls()
        {
            // Remove the old dynamic controls from the pnlControls panel
            pnlControls.Controls.Clear();

            // Recreate the dynamic controls and add them to the pnlControls panel
            CreateDynamicControls();
        }
    }
}


