using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WD.DataAccess.Context;
using WD.DataAccess.Enums;

namespace WD.WebAPI._2._0
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            WD.DataAccess.Context.DbContext dbContext= new DbContext(false);

            grdConnections.DataSource = dbContext.ICommands.ExecuteDataTable("SELECT * FROM TEMPEMPLOYEE");
            grdConnections.DataBind();
         
        }
    }
}