using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WD.DataAccess.Abstract;
using WD.DataAccess.Enums;
using WD.DataAccess.Helpers;

namespace WD.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DataSetService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DataSetService.svc or DataSetService.svc.cs at the Solution Explorer and start debugging.
    public abstract class DataSetService : ICommands
    {
        public DataSetService(DBProvider dbType)
            : base(dbType)
        { 
        
        }
        public DataSetService(DBProvider dbType,string connectionName)
            : base(dbType, connectionName)
        {

        }
        public DataSetService(Connect aConnect)
            : base(aConnect)
        {
            
        }
       
      
    }
}
