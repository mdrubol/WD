using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WD.DataAccess.Helpers;
using WD.DataAccess.Parameters;

namespace WD.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IIWDService" in both code and config file together.
    [ServiceContract]
    public interface IIWDService
    {
        [OperationContract]
        DataTable ExecuteDataTable(Connect aConnect, string commandText, CommandType commandType, DBParameter[] aParams = null);
        [OperationContract]
        DataTable ExecuteDataTableWithConnection(Connect aConnect, string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null);
        [OperationContract]
        DataTable ExecuteDataTableWithTransaction(Connect aConnect, string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null);

        [OperationContract]
        DataSet ExecuteDataSet(Connect aConnect, string commandText, CommandType commandType, DBParameter[] aParams = null);
        [OperationContract]
        DataSet ExecuteDataSetWithConnection(Connect aConnect, string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null);
        [OperationContract]
        DataSet ExecuteDataSetWithTransaction(Connect aConnect, string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null);
        
        [OperationContract]
        object ExecuteScalar(Connect aConnect, string commandText, CommandType commandType, DBParameter[] aParams = null);
        [OperationContract]
        object ExecuteScalarWithConnection(Connect aConnect, string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null);
        [OperationContract]
        object ExecuteScalarWithTransaction(Connect aConnect, string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null);


        [OperationContract]
        int ExecuteNonQuery(Connect aConnect, string commandText, CommandType commandType, DBParameter[] aParams = null);
        [OperationContract]
        int ExecuteNonQueryWithConnection(Connect aConnect, string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null);
        [OperationContract]
        int ExecuteNonQueryWithTransaction(Connect aConnect, string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null);

        [OperationContract]
        ADODB.Recordset ExecuteRecordSet(Connect aConnect, string commandText, CommandType commandType, DBParameter[] aParams = null);
        [OperationContract]
        ADODB.Recordset ExecuteRecordSetWithConnection(Connect aConnect, string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null);
        [OperationContract]
        ADODB.Recordset ExecuteRecordSetWithTransaction(Connect aConnect, string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null);

        [OperationContract]
        IDataReader ExecuteDataReader(Connect aConnect, string commandText, CommandType commandType, DBParameter[] aParams = null);
        [OperationContract]
        IDataReader ExecuteDataReaderWithConnection(Connect aConnect, string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null);
        [OperationContract]
        IDataReader ExecuteDataReaderWithTransaction(Connect aConnect, string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null);


    }
}
