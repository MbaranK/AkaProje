using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AkaProje
{
    public partial class tabloListelev2 : System.Web.UI.Page
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
                    SqlDataReader dr = sqlHelper.ExecuteReader(connection, $"SELECT name FROM sys.tables WHERE name LIKE '%_{kullanici}'");

                    List<string> tablolar = new List<string>();

                    while (dr.Read())
                    {
                        string tableName = (string)dr["name"];
                        tablolar.Add(tableName);
                    }
                    dr.Close();

                    ddlTablolar.DataSource = tablolar;
                    ddlTablolar.DataBind();
                    //btnKaydet.Enabled = false;
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
        }
        protected void btnListele_Click(object sender, EventArgs e)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlConnection connection = sqlHelper.OpenConnection();
            try
            {
                //SqlHelper sqlHelper = new SqlHelper();
                string selectedTableName = ddlTablolar.SelectedItem.ToString();
                DataTable dt = sqlHelper.ExecuteQuery(connection, "SELECT * FROM " + selectedTableName);
                ASPxGridView1.DataSource = dt;
                ASPxGridView1.DataBind();
                ASPxGridView1.Visible = true;
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

        //protected void ASPxGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        //{
        //    ds = (DataSet)Session["DataSet"];
        //    ASPxGridView gridView = (ASPxGridView)sender;
        //    DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
        //    DataRow row = dataTable.Rows.Find(e.Keys[0]);
        //    IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
        //    enumerator.Reset();
        //    while (enumerator.MoveNext())
        //        row[enumerator.Key.ToString()] = enumerator.Value;
        //    gridView.CancelEdit();
        //    e.Cancel = true;
        //}
        //protected void ASPxGridView1_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        //{
        //    ds = (DataSet)Session["DataSet"];
        //    ASPxGridView1 gridView = (ASPxGridView1)sender;
        //    DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
        //    DataRow row = dataTable.NewRow();
        //    IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
        //    enumerator.Reset();
        //    while (enumerator.MoveNext())
        //        if (enumerator.Key.ToString() != "Count")
        //            row[enumerator.Key.ToString()] = enumerator.Value;
        //    gridView.CancelEdit();
        //    e.Cancel = true;
        //    dataTable.Rows.Add(row);
        //}

        //protected void ASPxGridView1_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        //{
        //    int i = ASPxGridView1.FindVisibleIndexByKeyValue(e.Keys[ASPxGridView1.KeyFieldName]);
        //    Control c = ASPxGridView1.FindDetailRowTemplateControl(i, "ASPxGridView2");
        //    e.Cancel = true;
        //    ds = (DataSet)Session["DataSet"];
        //    ds.Tables[0].Rows.Remove(ds.Tables[0].Rows.Find(e.Keys[ASPxGridView1.KeyFieldName]));

        //}
    }
}