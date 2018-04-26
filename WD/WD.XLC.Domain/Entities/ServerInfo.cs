using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WD.DataAccess.Attributes;

namespace WD.XLC.Domain.Entities
{
    [Serializable]
    [XmlRootAttribute("MyInstance", Namespace = "", IsNullable = false)]
    [Custom(Name = "XLC_ServerInfo")]
    public class ServerInfo
    {
        public ServerInfo() {

            DatabaseName = string.Empty;
            InitialCatalog = string.Empty;
            Password = string.Empty;
            UserID = string.Empty;
            DbProvider = 0;
            ServerName = string.Empty;
            ServerPort = string.Empty;
            CurrentSchema = string.Empty;
            CurrentProtocol = string.Empty;
            ConnString = string.Empty;
            ProviderString = String.Empty;
        }
        [WD.DataAccess.Attributes.Custom(Name = "ID")]
        public string Id { get; set; }
        public string DatabaseName { get; set; }
        public string InitialCatalog { get; set; }
        [WD.DataAccess.Attributes.Custom(Name = "UserPassword")]
        public string Password { get; set; }
        public string UserID { get; set; }
        public string ConnString { get; set; }
        public int DbProvider { get; set; }
        public string ServerName { get; set; }
        public string ServerPort { get; set; }
        public string CurrentSchema { get; set; }
        public string CurrentProtocol { get; set; }
        public int IntegratedSecurity { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode=true)]
        public DateTime CreatedOn { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ProviderString { get; set; }
        public int IsActive { get; set; }
        //    public virtual string ConnString
        //    {
        //      get
        //      {
        //          string ConnString = string.Empty;
        //          switch (CurrentDataModel)
        //          {
        //              case "SqlServer":

        //                  if (IntegratedSecurity == 0)
        //                  {
        //                      ConnString =
        //                          "Provider=" + ProviderString +
        //                          ";Data Source=" + ServerName +
        //                          ";Initial Catalog=" + InitialCatalog +
        //                          ";Integrated Security=SSPI;";
        //                  }
        //                  else
        //                  {
        //                      ConnString =
        //                          "Provider=" + ProviderString +
        //                          ";Password=" + Password +
        //                          ";User ID=" + UserID +
        //                          ";Data Source=" + ServerName +
        //                          ";Initial Catalog=" + InitialCatalog;
        //                  }

        //                  break;
        //              case "Oracle":
        //                 ConnString = "Provider=" +ProviderString +
        //                           ";Password=" + Password +
        //                           ";User ID=" +UserID +
        //                           ";Data Source=" +ServerName;
        //                  break;
        //              case "Access":
        //                  ConnString = "Provider=" + ProviderString +
        //                             ";Password=" + Password +
        //                             ";User ID=" + UserID +
        //                             ";Data Source=" + ServerName;
        //                  break;
        //              case "Db2":
        //                  ConnString= "Provider=" +ProviderString +
        //                                ";Pwd=" + Password +
        //                                 ";Uid=" +UserID +
        //                                 ";Hostname=" +ServerName +
        //                                 ";database=" +DatabaseName +
        //                                 ";CurrentSchema=" + CurrentSchema +
        //                                 ";HostVarParameters=true" +
        //                                 ";Protocol=" + CurrentProtocol +
        //                                 ";Port=" +ServerPort;
        //                  break;
        //              default: break;

        //          }
        //          return ConnString;

        //      }
        //}
    }
}