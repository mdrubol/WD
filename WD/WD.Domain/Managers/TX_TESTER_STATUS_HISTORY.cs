using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WD.Domain.Managers
{
    public class TX_TESTER_STATUS_HISTORY
    {
        #region " Constants "
        const String TSHC9001 = "TSHC9001 - UpdateTable: ";
        const String TSHC9002 = "TSHC9002 - ConstructUpdateSQL: ";
        const String TSHC9004 = "TSHC9004 - InsertTable: ";
        const String TSHC9005 = "TSHC9005 - ConstructInsertSQL: ";
        #endregion
        #region " Properties "
        public string TESTER_ID { get; set; }
        public string TIMESTAMP { get; set; }
        public string SYS1 { get; set; }
        public string MITECS { get; set; }
        public string TESTPC { get; set; }
        public string HUB { get; set; }
        public string CBU { get; set; }
        public string INPUT_DTU { get; set; }
        public string OUTPUT_DTU { get; set; }
        public string SYS2 { get; set; }
        public string TIMESTAMP2 { get; set; }
        public string FEEDER { get; set; }
        public string DRAWER { get; set; }
        public string SLEDGARAGE { get; set; }
        public string INPUT_DTU_2 { get; set; }
        public string OUTPUT_DTU_2 { get; set; }
        public string MATCO_STATUS_1 { get; set; }
        public string MATCO_STATUS_2 { get; set; }
        public string PRINCIPAL { get; set; }
        public string TESTER_IPADDR { get; set; }
        public string RSM { get; set; }
        public string CELL_ENV_SERVER { get; set; }
        public string MAC_MASTER { get; set; }
        public string MAC_SLAVE { get; set; }
        public int SLOTQUAL1 { get; set; }
        public int SLOTQUAL2 { get; set; }

        //Added By Rajasekar 'ePWR 1102884
        public string PULLSW { get; set; }
        public string COOLFAN { get; set; }
        public string HEATER { get; set; }
        public string ABSORBFAN { get; set; }
        public string TEMPERATURE { get; set; }
        public string HBA { get; set; }
        public string CPU1_TEMP { get; set; }
        public string FAN2A { get; set; }
        //END

        //Start - Added by Liyun 27/1/15 [ePWR 1115022]
        public string SLOT_STATUS { get; set; }
        public string UTILIZATION { get; set; }
        public string DELTA_EXT_SENSOR { get; set; }
        public string LOCATION { get; set; }
        public string PRODUCT_FAMILY { get; set; }
        public string OPERATION { get; set; }
        public string ELAPSED_TIME { get; set; }
        //End - Added by Liyun 27/1/15 [ePWR 1115022]
        public string EXT_1_SENSOR { get; set; }   //Added by Liyun 21/7/15 [ePWR 1124367]
        public string EXT_2_SENSOR { get; set; }   //Added by Liyun 21/7/15 [ePWR 1124367]
        public string SYSTEM_STATUS_1 { get; set; }    //Added by Liyun 11/1/2016 [ePWR 1137837]
        public string SYSTEM_STATUS_2 { get; set; }    //Added by Liyun 11/1/2016 [ePWR 1137837]
        #endregion
        #region " ConstructInsertSQL "
        public void ConstructInsertSql(SqlCommand objInCommand, StringBuilder sbInColumn, StringBuilder sbInValue)
        {
            try
            {
                if (!(TESTER_ID == null))
                {
                    objInCommand.Parameters.Add("@TESTER_ID", SqlDbType.VarChar).Value = this.TESTER_ID;
                    sbInColumn.Append(",TESTER_ID");
                    sbInValue.Append(",@TESTER_ID");
                }
                if (!(TIMESTAMP == null))
                {
                    objInCommand.Parameters.Add("@TIMESTAMP", SqlDbType.VarChar).Value = this.TIMESTAMP;
                    sbInColumn.Append(",TIMESTAMP");
                    sbInValue.Append(",@TIMESTAMP");
                }
                if (!(SYS1 == null))
                {
                    objInCommand.Parameters.Add("@SYS1", SqlDbType.VarChar).Value = this.SYS1;
                    sbInColumn.Append(",SYS1");
                    sbInValue.Append(",@SYS1");
                }

                if (!(MITECS == null))
                {
                    objInCommand.Parameters.Add("@MITECS", SqlDbType.VarChar).Value = this.MITECS;
                    sbInColumn.Append(",MITECS");
                    sbInValue.Append(",@MITECS");
                }

                if (!(TESTPC == null))
                {
                    objInCommand.Parameters.Add("@TESTPC", SqlDbType.VarChar).Value = this.TESTPC;
                    sbInColumn.Append(",TESTPC");
                    sbInValue.Append(",@TESTPC");
                }

                if (!(HUB == null))
                {
                    objInCommand.Parameters.Add("@HUB", SqlDbType.VarChar).Value = this.HUB;
                    sbInColumn.Append(",HUB");
                    sbInValue.Append(",@HUB");
                }

                if (!(CBU == null))
                {
                    objInCommand.Parameters.Add("@CBU", SqlDbType.VarChar).Value = this.CBU;
                    sbInColumn.Append(",CBU");
                    sbInValue.Append(",@CBU");
                }

                if (!(INPUT_DTU == null))
                {
                    objInCommand.Parameters.Add("@INPUT_DTU", SqlDbType.VarChar).Value = this.INPUT_DTU;
                    sbInColumn.Append(",INPUT_DTU");
                    sbInValue.Append(",@INPUT_DTU");
                }

                if (!(OUTPUT_DTU == null))
                {
                    objInCommand.Parameters.Add("@OUTPUT_DTU", SqlDbType.VarChar).Value = this.OUTPUT_DTU;
                    sbInColumn.Append(",OUTPUT_DTU");
                    sbInValue.Append(",@OUTPUT_DTU");
                }

                if (!(SYS2 == null))
                {
                    objInCommand.Parameters.Add("@SYS2", SqlDbType.VarChar).Value = this.SYS2;
                    sbInColumn.Append(",SYS2");
                    sbInValue.Append(",@SYS2");
                }

                if (!(TIMESTAMP2 == null))
                {
                    objInCommand.Parameters.Add("@TIMESTAMP2", SqlDbType.VarChar).Value = this.TIMESTAMP2;
                    sbInColumn.Append(",TIMESTAMP2");
                    sbInValue.Append(",@TIMESTAMP2");
                }

                if (!(FEEDER == null))
                {
                    objInCommand.Parameters.Add("@FEEDER", SqlDbType.VarChar).Value = this.FEEDER;
                    sbInColumn.Append(",FEEDER");
                    sbInValue.Append(",@FEEDER");
                }

                if (!(DRAWER == null))
                {
                    objInCommand.Parameters.Add("@DRAWER", SqlDbType.VarChar).Value = this.DRAWER;
                    sbInColumn.Append(",DRAWER");
                    sbInValue.Append(",@DRAWER");
                }

                if (!(SLEDGARAGE == null))
                {
                    objInCommand.Parameters.Add("@SLEDGARAGE", SqlDbType.VarChar).Value = this.SLEDGARAGE;
                    sbInColumn.Append(",SLEDGARAGE");
                    sbInValue.Append(",@SLEDGARAGE");
                }

                if (!(INPUT_DTU_2 == null))
                {
                    objInCommand.Parameters.Add("@INPUT_2_DTU", SqlDbType.VarChar).Value = this.INPUT_DTU_2;
                    sbInColumn.Append(",INPUT_DTU_2");
                    sbInValue.Append(",@INPUT_2_DTU");
                }

                if (!(OUTPUT_DTU_2 == null))
                {
                    objInCommand.Parameters.Add("@OUTPUT_2_DTU", SqlDbType.VarChar).Value = this.OUTPUT_DTU_2;
                    sbInColumn.Append(",OUTPUT_DTU_2");
                    sbInValue.Append(",@OUTPUT_2_DTU");
                }

                if (!(MATCO_STATUS_1 == null))
                {
                    objInCommand.Parameters.Add("@MATCO_STATUS_1", SqlDbType.VarChar).Value = this.MATCO_STATUS_1;
                    sbInColumn.Append(",MATCO_STATUS_1");
                    sbInValue.Append(",@MATCO_STATUS_1");
                }

                if (!(MATCO_STATUS_2 == null))
                {
                    objInCommand.Parameters.Add("@MATCO_STATUS_2", SqlDbType.VarChar).Value = this.MATCO_STATUS_2;
                    sbInColumn.Append(",MATCO_STATUS_2");
                    sbInValue.Append(",@MATCO_STATUS_2");
                }

                if (!(PRINCIPAL == null))
                {
                    objInCommand.Parameters.Add("@PRINCIPAL", SqlDbType.VarChar).Value = this.PRINCIPAL;
                    sbInColumn.Append(",PRINCIPAL");
                    sbInValue.Append(",@PRINCIPAL");
                }

                if (!(TESTER_IPADDR == null))
                {
                    objInCommand.Parameters.Add("@TESTER_IPADDR", SqlDbType.VarChar).Value = this.TESTER_IPADDR;
                    sbInColumn.Append(",TESTER_IPADDR");
                    sbInValue.Append(",@TESTER_IPADDR");
                }

                if (!(RSM == null))
                {
                    objInCommand.Parameters.Add("@RSM", SqlDbType.VarChar).Value = this.RSM;
                    sbInColumn.Append(",RSM");
                    sbInValue.Append(",@RSM");
                }

                if (!(CELL_ENV_SERVER == null))
                {
                    objInCommand.Parameters.Add("@CELL_ENV_SERVER", SqlDbType.VarChar).Value = this.CELL_ENV_SERVER;
                    sbInColumn.Append(",CELL_ENV_SERVER");
                    sbInValue.Append(",@CELL_ENV_SERVER");
                }

                if (!(MAC_MASTER == null))
                {
                    objInCommand.Parameters.Add("@MAC_MASTER", SqlDbType.VarChar).Value = this.MAC_MASTER;
                    sbInColumn.Append(",MAC_MASTER");
                    sbInValue.Append(",@MAC_MASTER");
                }

                if (!(MAC_SLAVE == null))
                {
                    objInCommand.Parameters.Add("@MAC_SLAVE", SqlDbType.VarChar).Value = this.MAC_SLAVE;
                    sbInColumn.Append(",MAC_SLAVE");
                    sbInValue.Append(",@MAC_SLAVE");
                }

                //  'Edited by Liyun 24/11/14 [ePWR 1110793] if EMPTY or 0 populate it as NULL
                if (SLOTQUAL1 > 0)
                {
                    objInCommand.Parameters.Add("@SLOTQUAL1", SqlDbType.Int).Value = this.SLOTQUAL1;
                    sbInColumn.Append(",SLOTQUAL1");
                    sbInValue.Append(",@SLOTQUAL1");
                }
                else if (SLOTQUAL1 == 0)
                {
                    sbInColumn.Append(",SLOTQUAL1");
                    sbInValue.Append(",NULL");
                }

                //   'Edited by Liyun 24/11/14 [ePWR 1110793] if EMPTY or 0 populate it as NULL
                if (SLOTQUAL2 > 0)
                {
                    objInCommand.Parameters.Add("@SLOTQUAL2", SqlDbType.Int).Value = this.SLOTQUAL2;
                    sbInColumn.Append(",SLOTQUAL2");
                    sbInValue.Append(",@SLOTQUAL2");
                }
                else if (SLOTQUAL2 == 0)
                {
                    sbInColumn.Append(",SLOTQUAL2");
                    sbInValue.Append(",NULL");
                }

                //  'Added By Rajasekar ePWR 1102884

                if (!(PULLSW == null))
                {
                    objInCommand.Parameters.Add("@PULLSW", SqlDbType.VarChar).Value = this.PULLSW;
                    sbInColumn.Append(",PULLSW");
                    sbInValue.Append(",@PULLSW");
                }

                if (!(COOLFAN == null))
                {
                    objInCommand.Parameters.Add("@COOLFAN", SqlDbType.VarChar).Value = this.COOLFAN;
                    sbInColumn.Append(",COOLFAN");
                    sbInValue.Append(",@COOLFAN");
                }

                if (!(HEATER == null))
                {
                    objInCommand.Parameters.Add("@HEATER", SqlDbType.VarChar).Value = this.HEATER;
                    sbInColumn.Append(",HEATER");
                    sbInValue.Append(",@HEATER");
                }

                if (!(ABSORBFAN == null))
                {
                    objInCommand.Parameters.Add("@ABSORBFAN", SqlDbType.VarChar).Value = this.ABSORBFAN;
                    sbInColumn.Append(",ABSORBFAN");
                    sbInValue.Append(",@ABSORBFAN");
                }

                if (!(TEMPERATURE == null))
                {
                    objInCommand.Parameters.Add("@TEMPERATURE", SqlDbType.VarChar).Value = this.TEMPERATURE;
                    sbInColumn.Append(",TEMPERATURE");
                    sbInValue.Append(",@TEMPERATURE");
                }

                if (!(HBA == null))
                {
                    objInCommand.Parameters.Add("@HBA", SqlDbType.VarChar).Value = this.HBA;
                    sbInColumn.Append(",HBA");
                    sbInValue.Append(",@HBA");
                }

                if (!(CPU1_TEMP == null))
                {
                    objInCommand.Parameters.Add("@CPU1_TEMP", SqlDbType.VarChar).Value = this.CPU1_TEMP;
                    sbInColumn.Append(",CPU1_TEMP");
                    sbInValue.Append(",@CPU1_TEMP");
                }

                if (!(FAN2A == null))
                {
                    objInCommand.Parameters.Add("@FAN2A", SqlDbType.VarChar).Value = this.FAN2A;
                    sbInColumn.Append(",FAN2A");
                    sbInValue.Append(",@FAN2A");
                }
                //   'End

                //  'Start - Added by Liyun 27/1/15 [ePWR 1115022]
                if (!(SLOT_STATUS == null))
                {
                    objInCommand.Parameters.Add("@SLOT_STATUS", SqlDbType.VarChar).Value = this.SLOT_STATUS;
                    sbInColumn.Append(",SLOT_STATUS");
                    sbInValue.Append(",@SLOT_STATUS");
                }

                if (!(UTILIZATION == null))
                {
                    objInCommand.Parameters.Add("@UTILIZATION", SqlDbType.VarChar).Value = this.UTILIZATION;
                    sbInColumn.Append(",UTILIZATION");
                    sbInValue.Append(",@UTILIZATION");
                }

                if (!(DELTA_EXT_SENSOR == null))
                {
                    objInCommand.Parameters.Add("@DELTA_EXT_SENSOR", SqlDbType.VarChar).Value = this.DELTA_EXT_SENSOR;
                    sbInColumn.Append(",DELTA_EXT_SENSOR");
                    sbInValue.Append(",@DELTA_EXT_SENSOR");
                }

                if (!(LOCATION == null))
                {
                    objInCommand.Parameters.Add("@LOCATION", SqlDbType.VarChar).Value = this.LOCATION;
                    sbInColumn.Append(",LOCATION");
                    sbInValue.Append(",@LOCATION");
                }

                if (!(PRODUCT_FAMILY == null))
                {
                    objInCommand.Parameters.Add("@PRODUCT_FAMILY", SqlDbType.VarChar).Value = this.PRODUCT_FAMILY;
                    sbInColumn.Append(",PRODUCT_FAMILY");
                    sbInValue.Append(",@PRODUCT_FAMILY");
                }

                if (!(OPERATION == null))
                {
                    objInCommand.Parameters.Add("@OPERATION", SqlDbType.VarChar).Value = this.OPERATION;
                    sbInColumn.Append(",OPERATION");
                    sbInValue.Append(",@OPERATION");
                }

                if (!(ELAPSED_TIME == null))
                {
                    objInCommand.Parameters.Add("@ELAPSED_TIME", SqlDbType.VarChar).Value = this.ELAPSED_TIME;
                    sbInColumn.Append(",ELAPSED_TIME");
                    sbInValue.Append(",@ELAPSED_TIME");
                }
                // 'End - Added by Liyun 27/1/15 [ePWR 1115022]

                //  'Added by Liyun 21/7/15 [ePWR 1124367]
                if (!(EXT_1_SENSOR == null))
                {
                    objInCommand.Parameters.Add("@EXT_1_SENSOR", SqlDbType.VarChar).Value = this.EXT_1_SENSOR;
                    sbInColumn.Append(",EXT_1_SENSOR");
                    sbInValue.Append(",@EXT_1_SENSOR");
                }

                // 'Added by Liyun 21/7/15 [ePWR 1124367]
                if (!(EXT_2_SENSOR == null))
                {
                    objInCommand.Parameters.Add("@EXT_2_SENSOR", SqlDbType.VarChar).Value = this.EXT_2_SENSOR;
                    sbInColumn.Append(",EXT_2_SENSOR");
                    sbInValue.Append(",@EXT_2_SENSOR");
                }

                //'Added by Liyun 11/1/2016 [ePWR 1137837]
                if (!(SYSTEM_STATUS_1 == null))
                {
                    objInCommand.Parameters.Add("@SYSTEM_STATUS_1", SqlDbType.VarChar).Value = this.SYSTEM_STATUS_1;
                    sbInColumn.Append(",SYSTEM_STATUS_1");
                    sbInValue.Append(",@SYSTEM_STATUS_1");
                }

                if (!(SYSTEM_STATUS_2 == null))
                {
                    objInCommand.Parameters.Add("@SYSTEM_STATUS_2", SqlDbType.VarChar).Value = this.SYSTEM_STATUS_2;
                    sbInColumn.Append(",SYSTEM_STATUS_2");
                    sbInValue.Append(",@SYSTEM_STATUS_2");
                }
            }
            catch 
            {

            }
            //catch( WDShareEx As Security.WDSecurityException
            //    Throw WDShareEx
            //Catch ObjEx As Exception
            //    Throw New Security.WDSecurityException(TSHC9005 & ObjEx.Message, 9005, WD_SYSTEM_ERROR)
            //End Try
        }
        #endregion
    }
}