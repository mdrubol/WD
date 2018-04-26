using System;
using WD.DataAccess.Context;
using WD.DataAccess.Logger;

namespace WD.WebUI._2._0
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ILogger.Info("hello" + new Random().Next(100).ToString());
            ILogger.Error("hello" + new Random().Next(100).ToString());
            ILogger.Warn("hello" + new Random().Next(100).ToString());
            ILogger.Debug("hello" + new Random().Next(100).ToString());
            ILogger.Fatal("hello" + new Random().Next(100).ToString());

          
        }
    }
}