using WD.DataAccess.Enums;
using System;
using System.Linq;
using WD.Domain.Abstract;
using WD.Domain.Concrete;
using WD.DataAccess.Abstract;
using WD.DataAccess.Helpers;

namespace WD.Domain.Managers
{
   public class CustomerManager:IDisposable
    {
       private static CustomerManager instance;
       public ISelect ISelect { get; set; }
       public ICommands ICommands { get; set; }
       public int DBType;
       #region Constructors
       private CustomerManager(int dbType)
       {
           DBType = dbType;
           ISelect = new SelectManager(dbType);
           ICommands = new SelectManager(dbType).ICommands;
       }
       private CustomerManager(int dbType, string connectionName)
       {
           DBType = dbType;
           ISelect = new SelectManager(dbType, connectionName);
           ICommands = new SelectManager(dbType, connectionName).ICommands;
       }
       private CustomerManager(Connect aConnect)
       {
           ISelect = new SelectManager(aConnect);
           ICommands = new SelectManager(aConnect).ICommands;
       }
       #endregion
       public static CustomerManager Instance(int aDBType)
        {
            if (instance == null)
            {
                instance = new CustomerManager(aDBType);

            }
            else if (instance.DBType != aDBType)
            {
                instance = new CustomerManager(aDBType);
            }
            
            return instance;
        }
       public static CustomerManager Instance(int aDBType, string connectionName)
        {
            if (instance == null)
            {
                instance = new CustomerManager(aDBType, connectionName);
            }
            else if (instance.DBType != aDBType)
            {
                instance = new CustomerManager(aDBType, connectionName);
            }
            return instance;
        }
       public static CustomerManager Instance(Connect aConnect)
        {
            if (instance == null)
            {
                instance = new CustomerManager(aConnect);
            }
            else if (instance.DBType != aConnect.DbProvider)
            {
                instance = new CustomerManager(aConnect);
            }
              return instance;
        }
       private bool IsDispose { get; set; }
       public void Dispose()
       {
           Dispose(true);
           GC.SuppressFinalize(this);
       }

       protected virtual void Dispose(bool disposing)
       {
           if (disposing)
           {
               
               // free managed resources
           }
           // free native resources if there are any.
       }


    }
}
