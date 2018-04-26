using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WD.DataAccess.Context;

namespace Test.UI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            DbContext dbContext = new DbContext(true);
            if (!IsPostBack) {
                try
                {
                    grd.DataSource = dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, "SELECT * FROM LU_Database_Status");
                    grd.DataBind();
                }
                catch (Exception exc) { 
                
                
                }
            }

        }
    }
}