using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WD.DataAccess.Context;
using System.Net.Http;
using System.Net;
using Newtonsoft;
using Newtonsoft.Json;
namespace WD.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IWDService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select IWDService.svc or IWDService.svc.cs at the Solution Explorer and start debugging.
    public class IWDService : IIWDService
    {

        public System.Data.DataTable ExecuteDataTable(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteDataTable(commandText, commandType, aParams);
            }
            catch 
            {
                throw;
            }
        }

        public System.Data.DataTable ExecuteDataTableWithConnection(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbConnection connection , DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteDataTable(commandText, commandType, connection, aParams);
            }
            catch {

                throw;
            }
            
        }

        public System.Data.DataTable ExecuteDataTableWithTransaction(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbTransaction transaction, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteDataTable(commandText, commandType, transaction, aParams);
            }
            catch
            {

                throw;
            }
        }

        public System.Data.DataSet ExecuteDataSet(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteDataSet(commandText, commandType, aParams);
            }
            catch
            {

                throw;
            }
        }

        public System.Data.DataSet ExecuteDataSetWithConnection(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbConnection connection, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteDataSet(commandText, commandType, connection, aParams);
            }
            catch
            {

                throw;
            }
        }

        public System.Data.DataSet ExecuteDataSetWithTransaction(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbTransaction transaction, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteDataSet(commandText, commandType, transaction, aParams);
            }
            catch
            {

                throw;
            }
        }

        public object ExecuteScalar(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteScalar(commandText, commandType, aParams);
            }
            catch
            {

                throw;
            }
        }

        public object ExecuteScalarWithConnection(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbConnection connection, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteScalar(commandText, commandType, connection, aParams);
            }
            catch
            {

                throw;
            }
        }

        public object ExecuteScalarWithTransaction(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbTransaction transaction, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteScalar(commandText, commandType, transaction, aParams);
            }
            catch
            {

                throw;
            }
        }

        public int ExecuteNonQuery(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteNonQuery(commandText, commandType, aParams);
            }
            catch
            {

                throw;
            }
        }

        public int ExecuteNonQueryWithConnection(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbConnection connection, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteNonQuery(commandText, commandType, connection, aParams);
            }
            catch
            {

                throw;
            }
        }

        public int ExecuteNonQueryWithTransaction(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbTransaction transaction, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteNonQuery(commandText, commandType, transaction, aParams);

            }
            catch
            {

                throw;
            }
        }

        public ADODB.Recordset ExecuteRecordSet(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteRecordSet(commandText, commandType, aParams);
            }
            catch
            {

                throw;
            }
        }

        public ADODB.Recordset ExecuteRecordSetWithConnection(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbConnection connection, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteRecordSet(commandText, commandType, connection, aParams);
            }
            catch
            {

                throw;
            }
        }

        public ADODB.Recordset ExecuteRecordSetWithTransaction(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbTransaction transaction, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteRecordSet(commandText, commandType, transaction, aParams);
            }
            catch
            {

                throw;
            }
        }

        public System.Data.IDataReader ExecuteDataReader(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteDataReader(commandText, commandType, aParams);
            }
            catch
            {

                throw;
            }
        }

        public System.Data.IDataReader ExecuteDataReaderWithConnection(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbConnection connection, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteDataReader(commandText, commandType, connection, aParams);
            }
            catch
            {

                throw;
            }
        }

        public System.Data.IDataReader ExecuteDataReaderWithTransaction(DataAccess.Helpers.Connect aConnect, string commandText, System.Data.CommandType commandType, System.Data.IDbTransaction transaction, DataAccess.Parameters.DBParameter[] aParams = null)
        {
            try
            {
                DbContext dbContext = new DbContext(aConnect);
                return dbContext.ICommands.ExecuteDataReader(commandText, commandType, transaction, aParams);
            }
            catch
            {

                throw;
            }
        }





        
    }
}