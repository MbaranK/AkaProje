using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AkaProje
{
    public partial class tabloListele : System.Web.UI.Page
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
                    btnKaydet.Enabled = false;
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
            else
            {
                
                CreateDynamicControls();
            }
        }

        protected void btnListele_Click(object sender, EventArgs e)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlConnection connection = sqlHelper.OpenConnection();
            try
            {
                //SqlHelper sqlHelper = new SqlHelper();
                string selectedTableName = ddlTablolar.SelectedItem.ToString();
                DataTable dt = sqlHelper.ExecuteQuery(connection,"SELECT * FROM " + selectedTableName);
                gridView.DataSource = dt;
                gridView.DataBind();
                gridView.Visible = true;
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

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {

            GridViewRow row = gridView.SelectedRow;
            for (int i = 4; i < row.Cells.Count; i++)
            {
                TextBox textBox = new TextBox();
                textBox.Text = row.Cells[i].Text;
                row.Cells[i].Controls.Clear();
                row.Cells[i].Controls.Add(textBox);
            }

        }

        protected void onEdit(object sender, GridViewEditEventArgs e)
        {
            
                gridView.EditIndex = e.NewEditIndex;
                
                BindGrid();
            ((TextBox)gridView.Rows[e.NewEditIndex].Cells[3].Controls[0]).Enabled = false;


        }

       

        protected void onRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlConnection connection = sqlHelper.OpenConnection();
            SqlTransaction transaction = sqlHelper.BeginTrans(connection);
            try
            {
                GridViewRow row = gridView.Rows[e.RowIndex];
                string id = row.Cells[3].Text;

                //SqlHelper sqlHelper = new SqlHelper();
                sqlHelper.ExecuteNonQuery(connection, "DELETE FROM" + " " + ddlTablolar.SelectedValue + " " + "WHERE ID = " + id + "",null,transaction);
                sqlHelper.CommitTrans(transaction);
                BindGrid();
            }
            catch (Exception ex)
            {
                sqlHelper.RollbackTrans(transaction);
                throw ex;
            }

            finally
            {
                sqlHelper.CloseConnection(connection);
            }

        }

        protected void OnRowCancel(object sender, GridViewCancelEditEventArgs e)
        {
            gridView.EditIndex = -1;
            BindGrid();
        }

        protected void onRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlConnection connection = sqlHelper.OpenConnection();
            SqlTransaction transaction = sqlHelper.BeginTrans(connection);
            try
            {
                GridViewRow row = gridView.Rows[e.RowIndex];
                string id = ((TextBox)row.Cells[3].Controls[0]).Text;
                string selectedTableName = ddlTablolar.SelectedItem.ToString();

                StringBuilder query = new StringBuilder();
                query.Append("UPDATE ");
                query.Append(selectedTableName);
                query.Append(" SET ");

                for (int i = 4; i < row.Cells.Count; i++)
                {
                    string columnName = gridView.HeaderRow.Cells[i].Text;
                    string value = ((TextBox)row.Cells[i].Controls[0]).Text;
                    query.Append(columnName);
                    query.Append(" = '");
                    value = value.Replace(",", ".");
                    query.Append(value);                    
                    query.Append("'");

                    if (i < row.Cells.Count - 1)
                    {
                        query.Append(", ");
                    }
                }
                query.Append(" WHERE ID = ");
                query.Append(id);

                //SqlHelper sqlHelper = new SqlHelper();
                sqlHelper.ExecuteNonQuery(connection,query.ToString(),null,transaction);
                sqlHelper.CommitTrans(transaction);
                gridView.EditIndex = -1;
                BindGrid();
            }
            catch (Exception)
            {
                sqlHelper.RollbackTrans(transaction);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                         "swal('Hata!', '" + "Güncellenirken hata oluştu, verileri inceleyip tekrar deneyiniz." + "', 'error')", true);
            }

            finally
            {
                sqlHelper.CloseConnection(connection);
            }

        }
        protected void btnVeriEkle_Click(object sender, EventArgs e)
        {
            btnKaydet.Enabled = true;
            CreateDynamicControls();
            pnlControl2.Visible = true;
            
        }
        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlConnection connection = sqlHelper.OpenConnection();
            SqlTransaction transaction = sqlHelper.BeginTrans(connection);
            try
            {
                string selectedTableName = ddlTablolar.SelectedItem.ToString();
                //SqlHelper sqlHelper = new SqlHelper();
                int kolonSayi = sqlHelper.ExecuteScalar(connection,"SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID('" + selectedTableName + "')",null,transaction);
                string query = $"INSERT INTO " + selectedTableName + " VALUES (";
                for (int i = 1; i < kolonSayi; i++)
                {
                    string txtDataName = "txt" + (i + 1).ToString();
                    string ddlColumnType = "ddl" + (i + 1).ToString();

                    TextBox txtobj = (TextBox)pnlControl2.FindControl(txtDataName);
                    DropDownList ddlobj = (DropDownList)pnlControl2.FindControl(ddlColumnType);

                   

                    string dataName = txtobj.Text;
                    string columnType = ddlobj.SelectedValue;

                    if(columnType == "Ondalıklı Sayı")
                    {
                        dataName = dataName.Replace(",", ".");
                    }

                    if (columnType == "Kelime" || columnType == "Tarih")
                    {
                        query += $"'{dataName}', ";
                    }
                    else
                    {
                        query += $"{dataName}, ";
                    }

                    //if (columnType == "Sayı")
                    //{
                    //    txtobj.Attributes.Add("onblur", $"validateInput('{columnType}', '{dataName}')");
                    //}

                }
                query = query.TrimEnd(' ', ',');
                query += ")";
                sqlHelper.ExecuteNonQuery(connection,query,null,transaction);
                sqlHelper.CommitTrans(transaction);
                BindGrid();
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
        private void BindGrid()
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlConnection connection = sqlHelper.OpenConnection();
            try
            {
                string selectedTableName = ddlTablolar.SelectedItem.ToString();
                DataTable dt = sqlHelper.ExecuteQuery(connection,"SELECT * FROM " + selectedTableName);
                gridView.DataSource = dt;
                gridView.DataBind();
                gridView.Visible = true;
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
                //SqlHelper sqlHelper = new SqlHelper();
                List<string> kolonİsimleri = new List<string>();
                SqlDataReader dr = sqlHelper.ExecuteReader(connection, query, parameters,transaction);
                while (dr.Read())
                {
                    kolonİsimleri.Add(dr.GetString(0));
                }
                dr.Close();

                pnlControl2.Controls.Clear();

                int kolonSayi = sqlHelper.ExecuteScalar(connection,"SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID('" + selectedTableName + "')",null,transaction);

                for (int i = 1; i < kolonSayi; i++)
                {
                    Label lbl = new Label();
                    lbl.ClientIDMode = ClientIDMode.Static;
                    lbl.ID = "lbl" + (i + 1).ToString();
                    lbl.Text = kolonİsimleri[i] + ":" + "<br>";
                    lbl.ViewStateMode = ViewStateMode.Inherit;
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
                    reqTxt.ErrorMessage = "Lütfen Değer Giriniz.";

                    string query2 = ($"SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{selectedTableName}' AND COLUMN_NAME = '{kolonİsimleri[i]}'");
                    string dataTipi = sqlHelper.ExecuteScalarString(connection,transaction,query2).ToString();

                    DropDownList ddl = new DropDownList();
                    ddl.ClientIDMode = ClientIDMode.Static;
                    ddl.ID = "ddl" + (i + 1).ToString();
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
                    ddl.Enabled = false;
                    ddl.ViewStateMode = ViewStateMode.Inherit;
                    ddl.CssClass = "dynamic-control";

                    pnlControl2.Controls.Add(lbl);
                    pnlControl2.Controls.Add(reqTxt);
                    pnlControl2.Controls.Add(txt);
                    pnlControl2.Controls.Add(ddl);
                    pnlControl2.Visible = false;

                   
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