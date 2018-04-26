using gudusoft.gsqlparser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Services;
using WD.DataAccess.Context;
using WD.DataAccess.Enums;
using WD.DataAccess.Helpers;
using WD.DataAccess.Logger;
namespace WD.WebAPI._2._0
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class DataSetController : System.Web.Services.WebService
    {

        private static string theSQL = "";
        [WebMethod]
        public string Get()
        {
            
            try
            {

                 DbContext DbContext = new DbContext(DBProvider.Oracle);

                 //DataTable dt = DbContext.ICommands.ExecuteDataTable("SELECT FirstName,LastName,MiddleName,Provider FROM tempEmployee");
                 //ILogger.Info("No Of Rows" + dt.Rows.Count);
                 //Dictionary<string, string> d = new Dictionary<string, string>();
                 //d.Add("FIRSTNAME", "FIRSTNAME");
                 //d.Add("LASTNAME", "LASTNAME");
                 //d.Add("MIDDLENAME", "MIDDLENAME");
                 //d.Add("PROVIDER", "PROVIDER");
                 //d.Remove("DATEOFJOINING");
                 //d.Remove("EMPLOYEEID");
                 //ILogger.Info(DateTime.UtcNow);
                 //DbContext.ICommands.BulkInsert(dt, "TempEmployee", 100, 30, d);
                 decimal count = (decimal)DbContext.ICommands.ExecuteScalar("SELECT count(*) from tempEmployee");
                 ILogger.Info("No Of Rows" + count);

            }
            catch (Exception exc)
            {
                ILogger.Error(exc);
                return Newtonsoft.Json.JsonConvert.SerializeObject(theSQL);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(theSQL);
        }

        void analize(string theSql)
        {
            TGSqlParser sqlparser = new TGSqlParser(TDbVendor.DbVMssql);
            sqlparser.SqlText.Text = theSql;
            int i = sqlparser.Parse();
            if (i == 0)
            {
                foreach (TCustomSqlStatement stmt in sqlparser.SqlStatements)
                {
                    analyzestmt(stmt);
                }
            }
            else
            {
                theSQL += sqlparser.ErrorMessages;
            }
        }
        public static void analyzestmt(TCustomSqlStatement psql)
        {
            switch (psql.SqlStatementType)
            {
                case TSqlStatementType.sstSelect:
                    analyzeselect((TSelectSqlStatement)psql);
                    break;
                case TSqlStatementType.sstDelete:
                    analyzedelete((TDeleteSqlStatement)psql);
                    break;
                case TSqlStatementType.sstInsert:
                    analyzeinsert((TInsertSqlStatement)psql);
                    break;
                case TSqlStatementType.sstUpdate:
                    analyzeupdate((TUpdateSqlStatement)psql);
                    break;
                case TSqlStatementType.sstAlterTable:
                    analyzealtertable((TAlterTableStatement)psql);
                    break;
                case TSqlStatementType.sstCreateTable:
                    analyzecreatetable((TCreateTableSqlStatement)psql);
                    break;
                default:
                    break;
            }

        }

        public static void analyzeselect(TSelectSqlStatement psql)
        {
            theSQL += "Select statement:";
            theSQL += SelectStmtInfo(psql);
        }

        public static string SelectStmtInfo(TSelectSqlStatement pSqlstmt)
        {
            string lcstr;

            lcstr = "";


            switch (pSqlstmt.SelectSetType)
            {
                case (TSelectSetType.sltNone): lcstr = "Select set type: none"; break;
                case (TSelectSetType.sltUnion):
                    lcstr = "\n Select set type: union";
                    lcstr = lcstr + "\n left statement: " + SelectStmtInfo(pSqlstmt.LeftStmt) + "\nright statement: " + "\r\n" + SelectStmtInfo(pSqlstmt.RightStmt);
                    break;
                case (TSelectSetType.sltUnionAll):
                    lcstr = "\n" + "Select set type: union all";
                    lcstr = lcstr + "\n" + "left statement: " + SelectStmtInfo(pSqlstmt.LeftStmt) + "\n" + "right statement: " + "\n" + SelectStmtInfo(pSqlstmt.RightStmt);
                    break;
                case (TSelectSetType.sltMinus):
                    {
                        lcstr = "\n" + "Select set type: minus";
                        lcstr = lcstr + "\n" + "left statement: " + SelectStmtInfo(pSqlstmt.LeftStmt) + "\n" + "right statement: " + "\n" + SelectStmtInfo(pSqlstmt.RightStmt);
                        break;
                    }
                case (TSelectSetType.sltIntersect):
                    {
                        lcstr = "\n" + "Select set type: insertsect";
                        lcstr = lcstr + "\n" + "left statement: " + SelectStmtInfo(pSqlstmt.LeftStmt) + "\n" + "right statement: " + "\n" + SelectStmtInfo(pSqlstmt.RightStmt);
                        break;
                    }
                default: break;
            }

            if (pSqlstmt.SelectSetType != TSelectSetType.sltNone)
            {
                lcstr = lcstr + "\n order by  clause:" + pSqlstmt.SortClauseText;
                lcstr = lcstr + "\n for upate  clause:" + pSqlstmt.ForUpdateClause;
                return lcstr;
            }


            lcstr = lcstr + "\n" + "Select distinct:" + pSqlstmt.SelectDistinctText;
            lcstr = lcstr + "\n" + "select clause:";
            lcstr = lcstr + "\n" + pSqlstmt.SelectClauseText;

            foreach (TLzField fld in pSqlstmt.Fields)
            {
                lcstr = lcstr + "\n" + "fieldtable:" + fld.FieldPrefix;
                lcstr = lcstr + "\n" + "fieldname:" + fld.FieldName;
                lcstr = lcstr + "\n" + "fieldalias:" + fld.FieldAlias;
            }

            lcstr = lcstr + "\n" + "from clause:";
            lcstr = lcstr + "\n" + pSqlstmt.FromClauseText;

            foreach (TLzTable tlb in pSqlstmt.Tables)
            {
                lcstr = lcstr + "\n" + "tableowner:" + tlb.TablePrefix;
                lcstr = lcstr + "\n" + "tablename:" + tlb.TableName;
                lcstr = lcstr + "\n" + "tablealias:" + tlb.TableAlias;
            }

            lcstr = lcstr + "\n" + "where clause:" + pSqlstmt.WhereClauseText;
            lcstr = lcstr + "\n" + "groupby clause:" + pSqlstmt.GroupbyClauseText;
            lcstr = lcstr + "\n" + "having clause:" + pSqlstmt.HavingClauseText;
            lcstr = lcstr + "\n" + "order by  clause:" + pSqlstmt.SortClauseText;
            lcstr = lcstr + "\n" + "for upate  clause:" + pSqlstmt.ForUpdateClauseText;


            lcstr = lcstr + "\n" + "top clause:" + pSqlstmt.topclauseText;
            lcstr = lcstr + "\n" + "limit clause:" + pSqlstmt.limitclauseText;

            return lcstr;

        }

        public static void analyzedelete(TDeleteSqlStatement psql)
        {
            theSQL += "Delete statement:";

            theSQL += DeleteStmtInfo(psql);
        }

        public static string DeleteStmtInfo(TDeleteSqlStatement pSqlstmt)
        {
            string lcStr;

            lcStr = "";
            if (pSqlstmt.Params.Count() > 0)
            {
                lcStr = "Parameters(bind variable):";
                foreach (TLzValue v in pSqlstmt.Params)
                    lcStr = lcStr + "\n" + "Name: " + v.ValueName + " Value:" + v.ValueStr;
            }

            lcStr = lcStr + "\n" + "table: " + pSqlstmt.TableText;
            lcStr = lcStr + "\n" + "where clause: " + pSqlstmt.WhereClauseText;
            lcStr = lcStr + "\n" + "returning clause: " + pSqlstmt.ReturningClauseText;

            return lcStr;
        }

        public static void analyzeinsert(TInsertSqlStatement psql)
        {
            theSQL += "Insert statement:";

            theSQL += InsertStmtInfo(psql);
        }

        public static string InsertStmtInfo(TInsertSqlStatement pSqlstmt)
        {
            string lcStr;

            lcStr = "";
            if (pSqlstmt.Params.Count() > 0)
            {
                lcStr = "Parameters(bind variable):";
                foreach (TLzValue v in pSqlstmt.Params)
                    lcStr = lcStr + "\n" + "Name: " + v.ValueName + " Value:" + v.ValueStr;
            }

            lcStr = lcStr + "\n" + "table: " + pSqlstmt.TableText;
            lcStr = lcStr + "\n" + "columns: " + pSqlstmt.ColumnListText;
            lcStr = lcStr + "\n" + "values: " + pSqlstmt.ValueListText;
            lcStr = lcStr + "\n" + "returning clause: " + pSqlstmt.ReturningClauseText;


            return lcStr;
        }

        public static void analyzeupdate(TUpdateSqlStatement psql)
        {
            theSQL += "Update statement:";

            theSQL += UpdateStmtInfo(psql);
        }

        public static string UpdateStmtInfo(TUpdateSqlStatement pSqlstmt)
        {
            string lcStr;

            lcStr = "";
            if (pSqlstmt.Params.Count() > 0)
            {
                lcStr = "Parameters(bind variable):";
                foreach (TLzValue v in pSqlstmt.Params)
                    lcStr = lcStr + "\r\n" + "Name: " + v.ValueName + " Value:" + v.ValueStr;
            }

            lcStr = lcStr + "\n" + "table: " + pSqlstmt.TableText;
            lcStr = lcStr + "\n" + "set clause: " + pSqlstmt.SetClauseText;
            lcStr = lcStr + "\n" + "where clause: " + pSqlstmt.WhereClauseText;
            lcStr = lcStr + "\n" + "returning clause: " + pSqlstmt.ReturningClauseText;


            return lcStr;
        }


        public static void analyzecreatetable(TCreateTableSqlStatement psql)
        {
            theSQL += "Create table";
            if (psql.Table.TableConstraints.Count() > 0)
            {
                theSQL += "Table level constraints";
                foreach (TLzConstraint lcct in psql.Table.TableConstraints)
                {
                    analyzeconstraint(lcct);
                }
            }

            foreach (TLzField lcfield in psql.Table.Fields)
            {

                theSQL += "Column name: " + lcfield.ColumnName.AsText;
                theSQL += "Column name: " + lcfield.FieldDataType.AsText;

                if (lcfield.ColumnConstraints.Count() > 0)
                {
                    theSQL += "Column level constraints";
                    foreach (TLzConstraint lcct in lcfield.ColumnConstraints)
                    {
                        analyzeconstraint(lcct);
                    }
                }
            }

        }

        public static void analyzealtertable(TAlterTableStatement psql)
        {
            theSQL += "Alter table";
            foreach (TLzAlterTableOption lcato in psql.AlterTableOptionList)
            {

                switch (lcato.atOption)
                {
                    case TLzAlterTableOptionType.atoAddConstraint:
                        theSQL += "Constraints in check constraints";
                        for (int m = 0; m < lcato._MssqlCheckConstraint.list1.Count(); m++)
                        {
                            TSourceToken lcst = (TSourceToken)lcato._MssqlCheckConstraint.list1[m];
                            theSQL += "\t" + lcst.AsText;
                        }
                        break;
                    default:
                        theSQL += "alter table option not handled yet";
                        break;
                }
            }
        }

        public static void analyzeconstraint(TLzConstraint pcon)
        {
            if ((pcon._name != null) & (pcon._name.AsText.Length > 0))
            {
                theSQL += "constraint name:" + pcon._name.AsText;
            }
            switch (pcon.ConstraintType)
            {
                case TLzConstraintType.ctPrimarykey:
                    theSQL += "Primary Key";

                    if (pcon.ColumnList != null)
                    {
                        theSQL += "Column in primary key";
                        for (int k = 0; k < pcon.ColumnList.Count(); k++)
                        {
                            TLz_SortGroupBy lcsgb = (TLz_SortGroupBy)pcon.ColumnList[k];

                            theSQL += "\t" + lcsgb.node.AsText;
                        }
                    }
                    break;
                case TLzConstraintType.ctForeignkey:
                    theSQL += "Foreign Key";
                    if (pcon.ColumnList != null)
                    {
                        theSQL += "Column in primary key";
                        for (int k = 0; k < pcon.ColumnList.Count(); k++)
                        {
                            TLz_Attr lcattr = (TLz_Attr)pcon.ColumnList[k];
                            theSQL += "\t" + lcattr.AsText;
                        }
                    }
                    if (pcon.RefClause != null)
                    {
                        analyzeconstraintref(pcon.RefClause);
                    }

                    break;
                case TLzConstraintType.ctUnique:
                    theSQL += "Unique key";
                    if (pcon.ColumnList != null)
                    {
                        theSQL += "Column in unique key";
                        for (int k = 0; k < pcon.ColumnList.Count(); k++)
                        {
                            TLz_SortGroupBy lcsgb = (TLz_SortGroupBy)pcon.ColumnList[k];
                            theSQL += "\t" + lcsgb.node.AsText;
                        }
                    }
                    break;
                case TLzConstraintType.ctDefault:
                    theSQL += "Default constraint";
                    theSQL += pcon.ColExpr.AsText;
                    break;
                case TLzConstraintType.ctCheck:
                    theSQL += "Check constraint";
                    theSQL += pcon.ColExpr.AsText;
                    break;
                case TLzConstraintType.ctNotnull:
                    theSQL += pcon.AsText;
                    break;
                case TLzConstraintType.ctNull:
                    theSQL += pcon.AsText;
                    break;
                default:
                    theSQL += "Constraint not handled yet";
                    theSQL += "\t" + pcon.AsText;
                    break;

            }
        }

        public static void analyzeconstraintref(TLzConstraintRefClause pref)
        {
            theSQL += "Constraint reference clause:";
            theSQL += "\t name:" + pref.q_name.AsText;
            if (pref._column_listnode != null)
            {
                theSQL += "column in reference clause:";
                for (int k = 0; k < pref._column_listnode.list1.Count(); k++)
                {
                    TLz_Attr lcattr = (TLz_Attr)pref._column_listnode.list1[k];
                    theSQL += "\t" + lcattr.AsText;
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            base.Dispose(disposing);
        }

    }
}