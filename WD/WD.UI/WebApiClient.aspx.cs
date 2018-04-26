using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WD.UI
{
    public partial class WebApiClient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                BindGrid();
            }
        }
        string URI = "http://172.21.12.166:100/api/DataSet";
        private void BindGrid()
        {
            lblMessage.Visible = false;
            try
            {
                
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "text/json");
                    Wrapper input = new Wrapper();
                    input.AuthenticationToken = "UMeiS+n70CAAHQNA7TiW2g==";
                    input.Connect = new Connect() { DbType = 1 };
                    input.TheSql = new SqlStatement[1];
                    input.TheSql[0] = new SqlStatement() { CommandText = "SELECT * FROM TEMPEMPLOYEE", CommandType = 1 };
                    string pagesource = client.UploadString((new Uri(URI)), "POST", JsonConvert.SerializeObject(input));
                    grdResult.DataSource = JsonConvert.DeserializeObject<DataSet>(pagesource);
                    grdResult.DataBind();
                }
            }
            catch (Exception exc)
            {
                lblMessage.Visible = true;
                lblMessage.Text = exc.Message;
            }
        }


        public class Wrapper
        {
            /// <summary>
            /// Collection of SQL Statements with CommandText, CommandType and Collection of Paramters
            /// </summary>
            public SqlStatement[] TheSql { get; set; }
            /// <summary>
            /// Authentication Token
            /// </summary>
            public string AuthenticationToken { get; set; }
            /// <summary>
            /// Connection Class for DbContext Initialization
            /// </summary>
            public Connect Connect { get; set; }
        }
        public class SqlStatement
        {
            /// <summary>
            /// Open Sql Statement or Procedure Name
            /// </summary>
            public string CommandText { get; set; }
            /// <summary>
            /// CommandType for Text or StoredProcedure (1 or 4)
            /// </summary>
            public int CommandType { get; set; }
            /// <summary>
            /// Collection of Paramters
            /// </summary>
            public DBParameter[] Params { get; set; }
        }
        public class Connect
        {
            /// <summary>
            /// Connection Name Stored in Connection Settings of Client Application Web.Config or App.Config File
            /// </summary>
            public string ConnectionName { get; set; }
            /// <summary>
            /// Full Connection String for Client Application
            /// </summary>
            public string ConnectionString { get; set; }
            /// <summary>
            /// Provider Type SQL, Db2, Oracle, Oracle2 or TeraData
            /// </summary>
            public int DbType { get; set; }
        }
        public class DBParameter {
            //this id property is just for example

            public string ParameterName
            { get; set; }

            /// <summary>
            /// Gets or sets the value associated with the parameter.
            /// </summary>
            public object ParameterValue
            { get; set; }
        }

        protected void grdParamter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                paramList.Remove(paramList.Find(x => x.ParameterName == (string.IsNullOrEmpty(e.CommandArgument.ToString()) ? null : e.CommandArgument.ToString())));
                grdParamter.DataSource = paramList;
                grdParamter.DataBind();
            }
        }
        public static List<DBParameter> paramList = new List<DBParameter>();
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            paramList.Add(new DBParameter());
            grdParamter.DataSource = paramList;
            grdParamter.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            try
            {
                
                
              
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "text/json");
                    Wrapper input = new Wrapper();
                    input.AuthenticationToken = "UMeiS+n70CAAHQNA7TiW2g==";
                    input.Connect = new Connect() { DbType = Convert.ToInt32(ddlDbType.SelectedValue), ConnectionString = txtConnectionString.Text };
                    input.TheSql = new SqlStatement[1];
                    DBParameter[] aParams = new DBParameter[grdParamter.Rows.Count];
                    int index = 0;
                    for (index = 0; index < grdParamter.Rows.Count; index++)
                    {
                        aParams[index] = new DBParameter();
                        aParams[index].ParameterName = ((TextBox)grdParamter.Rows[index].FindControl("txtParameterName")).Text;
                        aParams[index].ParameterValue = ((TextBox)grdParamter.Rows[index].FindControl("txtParameterValue")).Text;
                    }
                    input.TheSql[0] = new SqlStatement() { CommandText = txtCommandText.Text, CommandType = Convert.ToInt32(ddlCommandType.SelectedValue), Params = aParams };
                    string pagesource = client.UploadString((new Uri(URI)), "POST", JsonConvert.SerializeObject(input));
                    grdResult.DataSource = JsonConvert.DeserializeObject<DataSet>(pagesource);
                    grdResult.DataBind();
                }
            }
            catch (Exception exc) {
                lblMessage.Visible = true;
                lblMessage.Text = exc.Message;
            
            }
        }

        protected void grdParamter_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}