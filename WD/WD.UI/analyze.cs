using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using gudusoft.gsqlparser;
using gudusoft.gsqlparser.Units;

/*
 * select, insert, update, delete
 * Table created, modified, deleted
 * Field created, modified, deleted
 * Constraints created, modified, deleted
 */


namespace WD.UI
{
   internal class analyzescript
    {
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: {0} scriptfile [/t <database type>]", Environment.GetCommandLineArgs()[0]);
                Console.WriteLine("/t: Option, set the database type. Support oracle, mssql and db2, the default type is oracle");
                return;
            }

            Array array = Array.CreateInstance(typeof(string), args.Length);

            for (int j = 0; j < array.Length; j++)
            {
                array.SetValue(args[j], j);
            }

            TDbVendor db = TDbVendor.DbVOracle;

            int index = Array.IndexOf(array, "/t");

            if (index != -1 && args.Length > index + 1)
            {
                if (args[index + 1].Equals("mssql"))
                {
                    db = TDbVendor.DbVMssql;
                }
                else if (args[index + 1].Equals("db2"))
                {
                    db = TDbVendor.DbVDB2;
                }
            }

            TGSqlParser sqlparser = new TGSqlParser(db);
            sqlparser.Sqlfilename = args[0];


            int i = sqlparser.Parse();

            if (i == 0)
            {
                foreach (TCustomSqlStatement stmt in sqlparser.SqlStatements)
                {
                    analyzestmt(stmt);
                    if (stmt != sqlparser.SqlStatements[sqlparser.SqlStatements.Count() - 1])
                        Console.WriteLine("\n");
                }
            }
            else
            {
                Console.WriteLine("Please make sure you are setting the correct database engine, current db engine is:" + db + Environment.NewLine + sqlparser.ErrorMessages);
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
                case TSqlStatementType.sstMssqlExec:
                    analyzeExec((TMssqlExecute)psql);
                    break;
                case TSqlStatementType.sstOracleComment:
                    analyzeCommentOn((TOracleCommentOnSqlStmt)psql);
                    break;
                case TSqlStatementType.sstMssqlDropindex:
                    analyzeMssqlDropIndex((TMssqlDropIndex)psql);
                    break;
                case TSqlStatementType.sstCreateView:
                    analyzeCreateView((TCreateViewSqlStatement)psql);
                    break;
                case TSqlStatementType.sstDb2CreateProcedure:
                    analyzeDb2CreateProcedure((TDb2CreateProcedure)psql);
                    break;
                case TSqlStatementType.sstplsql_block:
                    if (psql is TLzPlsql_Block)
                    {
                        analyzePlsql_Block((TLzPlsql_Block)psql);
                    }
                    else
                    {
                        analyzePlsqlStatement((TPlsqlStatement)psql);
                    }
                    break;
                case TSqlStatementType.sstPlsql_IfStmt:
                    analyzePlsql_IfStmt((TLzPlsql_IfStmt)psql);
                    break;
                case TSqlStatementType.sstPlsql_AssignStmt:
                    analyzePlsql_AssignStmt((TLzPlsql_AssignStmt)psql);
                    break;
                case TSqlStatementType.sstPlsql_ForallStmt:
                    analyzePlsql_ForallStmt((TLzPlsql_ForallStmt)psql);
                    break;
                case TSqlStatementType.sstPlsql_ExecImmeStmt:
                    analyzePlsql_ExecImmeStmt((TLzPlsql_ExecImmeStmt)psql);
                    break;
                case TSqlStatementType.sstPlsql_ProcBasicStmt:
                    analyzePlsql_ProcBasicStmt((TLzPlsql_ProcBasicStmt)psql);
                    break;
                case TSqlStatementType.sstplsql_createtrigger:
                    analyzePlsqlStatement((TPlsqlStatement)psql);
                    break;
                case TSqlStatementType.sstplsql_createprocedure:
                    analyzePlsqlStatement((TPlsqlStatement)psql);
                    break;
                case TSqlStatementType.sstplsql_createfunction:
                    analyzePlsqlStatement((TPlsqlStatement)psql);
                    break;
                case TSqlStatementType.sstOracleCreateSequence:
                    analyzeCreateSequence((TOracleCreateSequenceStmt)psql);
                    break;
                default:
                    Console.WriteLine("This type of sql statement not analyzed in this demo, contact info@sqlparser.com for an improved demo. {0}", System.Environment.NewLine + psql.SqlStatementType);
                    break;
            }

        }


        public static void analyzeDb2CreateProcedure(TDb2CreateProcedure stmt)
        {
            Console.WriteLine("DB2 create procedure statement:");
            for (int i = 0; i < stmt.SqlStatements.Count(); i++)
            {
                TCustomSqlStatement sql = stmt.SqlStatements[i];
                switch (sql.SqlStatementType)
                {
                    case TSqlStatementType.sstDb2SqlVariableDeclaration:
                        _TDb2SqlVariableDeclaration lcStmt = (_TDb2SqlVariableDeclaration)sql.fcRoot;
                        for (int j = 0; j < lcStmt._lstNames.Count(); j++)
                        {
                            TSourceToken st = (TSourceToken)lcStmt._lstNames[j];
                            Console.WriteLine("Variable: {0,-20}, Type: {1}", st.AsText, lcStmt._ndTypename.AsText);
                        }
                        break;
                    default:
                        // Console.WriteLine("Not analyzed yet: {0}",sql.SqlStatementType);
                        break;
                }
            }
        }

        public static void analyzeCreateView(TCreateViewSqlStatement stmt)
        {
            Console.WriteLine("create view statement:");
            Console.WriteLine("View name: {0}", stmt.ViewName);
            if (stmt.AliasClause != null)
            {
                _TOracleViewAliasClause va = (_TOracleViewAliasClause)stmt.AliasClause;
                for (int i = 0; i < va._lstItems.Count(); i++)
                {
                    _TOracleViewAliasItem vai = (_TOracleViewAliasItem)va._lstItems[i];
                    if (vai._stAlias != null)
                    {
                        Console.WriteLine("View alias: {0}", vai._stAlias.AsText);
                    }
                }
            }

            Console.WriteLine(Environment.NewLine + "Subquery:");
            analyzeselect(stmt.SubQuery);
        }

        public static void analyzeMssqlDropIndex(TMssqlDropIndex stmt)
        {
            Console.WriteLine("sql server drop index:");
            for (int i = 0; i < stmt._lstDropIndexItemList.Count(); i++)
            {
                _TMssqlDropIndexItem item = (_TMssqlDropIndexItem)stmt._lstDropIndexItemList[i];
                Console.WriteLine("Index name: {0}", item._ndIndexName.AsText);
                if (item._ndObjectName != null)
                {
                    Console.WriteLine("object name: {0}", item._ndObjectName.AsText);
                }
            }
        }

        public static void analyzeCommentOn(TOracleCommentOnSqlStmt cmt)
        {
            Console.WriteLine("oracle comment on statement:");
            Console.WriteLine("Object type: {0}", cmt.dbObjType.ToString());
            Console.WriteLine("Object name: {0}", cmt.objectName.AsText);
            Console.WriteLine("Message: {0}", cmt.messageText);

        }

        public static void analyzeselect(TSelectSqlStatement psql)
        {
            Console.WriteLine("Select statement:");

            Console.WriteLine(SelectStmtInfo(psql));
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

            if (pSqlstmt.SelectDistinct != null)
            {
                lcstr = lcstr + "\n" + "Select distinct:" + pSqlstmt.SelectDistinctText;
            }
            lcstr = lcstr + "\n" + "select clause:";
            lcstr = lcstr + "\nColumns";


            foreach (TLzField fld in pSqlstmt.Fields)
            {
                lcstr = lcstr + "\n\tFullname:" + fld.FieldFullname;
                lcstr = lcstr + "\n\tPrefix:" + fld.FieldPrefix;
                lcstr = lcstr + "\tColumn:" + fld.FieldName;
                lcstr = lcstr + "\talias:" + fld.FieldAlias;
            }

            lcstr = lcstr + "\n\n" + "from clause:";
            lcstr = lcstr + "\n" + pSqlstmt.FromClauseText + "\n";

            foreach (TLzTable tlb in pSqlstmt.Tables)
            {
                if (tlb.TableAttr.ServerNameToken != null)
                {
                    lcstr = lcstr + "\n" + "server:" + tlb.TableAttr.ServerNameToken.AsText;
                }

                if (tlb.TableAttr.DatabaseNameToken != null)
                {
                    lcstr = lcstr + "\n" + "database:" + tlb.TableAttr.DatabaseNameToken.AsText;
                }

                if (tlb.TableAttr.SchemaNameToken != null)
                {
                    lcstr = lcstr + "\n" + "schema:" + tlb.TableAttr.SchemaNameToken.AsText;
                }

                if (tlb.TableAttr.ObjectNameToken != null)
                {
                    lcstr = lcstr + "\n" + "object:" + tlb.TableAttr.ObjectNameToken.AsText;
                }

                //lcstr = lcstr + "\n" + "tableowner:" + tlb.TablePrefix;
                //lcstr = lcstr + "\n" + "tablename:" + tlb.TableName;
                lcstr = lcstr + "\n" + "object alias:" + tlb.TableAlias;
            }

            if (pSqlstmt.WhereClause != null)
            {
                lcstr = lcstr + "\n" + "where clause:" + pSqlstmt.WhereClauseText;
            }
            if (pSqlstmt.GroupbyClause != null)
            {
                lcstr = lcstr + "\n" + "groupby clause:" + pSqlstmt.GroupbyClauseText;
                lcstr = lcstr + "\n" + "having clause:" + pSqlstmt.HavingClauseText;
            }

            if (pSqlstmt.SortClause != null)
            {
                lcstr = lcstr + "\n" + "order by  clause:" + pSqlstmt.SortClauseText;
            }

            if (pSqlstmt.ForUpdateClause != null)
            {
                lcstr = lcstr + "\n" + "for upate  clause:" + pSqlstmt.ForUpdateClauseText;
            }


            if (pSqlstmt.topclause != null)
            {
                lcstr = lcstr + "\n" + "top clause:" + pSqlstmt.topclauseText;
            }

            if (pSqlstmt.limitClause != null)
            {
                lcstr = lcstr + "\n" + "limit clause:" + pSqlstmt.limitclauseText;
            }

            return lcstr;

        }

        public static void analyzedelete(TDeleteSqlStatement psql)
        {
            Console.WriteLine("Delete statement:");

            Console.WriteLine(DeleteStmtInfo(psql));
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
            Console.WriteLine("Insert statement:");
            if (psql.InsertIntoValues != null && psql.InsertIntoValues.Count() > 0)
            {
                Console.WriteLine(InsertAllStmtInfo(psql));
            }
            else if (psql.Insert_clause_when_then_list != null && psql.Insert_clause_when_then_list.Count() > 0)
            {
                Console.WriteLine(InsertAllStmtInfo(psql));
            }
            else
            {
                Console.WriteLine(InsertStmtInfo(psql));
            }
        }

        private static string InsertAllStmtInfo(TInsertSqlStatement pSqlstmt)
        {
            string lcStr = "";
            if (pSqlstmt.InsertIntoValues != null)
            {
                for (int i = 0; i < pSqlstmt.InsertIntoValues.Count(); i++)
                {
                    TLz_InsertIntoValue insertIntoValue = (TLz_InsertIntoValue)pSqlstmt.InsertIntoValues[i];
                    lcStr += "insert into table:";
                    lcStr += insertIntoValue.Table.AsText;
                    if (insertIntoValue.Fields != null && insertIntoValue.Fields.Count() > 0)
                    {
                        lcStr += "\ncolumns:";
                        for (int j = 0; j < insertIntoValue.Fields.Count(); j++)
                        {
                            lcStr = lcStr + "\n\t" + insertIntoValue.Fields[j].AsText;
                        }
                    }
                    if (insertIntoValue.Fields != null && insertIntoValue.Values.Count() > 0)
                    {
                        TLzFieldList values = (TLzFieldList)insertIntoValue.Values;
                        lcStr = lcStr + "\nvalues: ";
                        for (int j = 0; j < values.Count(); j++)
                        {
                            lcStr = lcStr + "\n\t" + values[j].AsText;
                        }
                    }
                    lcStr += "\n";
                }
            }
            if (pSqlstmt.Insert_clause_when_then_list != null)
            {
                for (int k = 0; k < pSqlstmt.Insert_clause_when_then_list.Count(); k++)
                {
                    TLz_InsertCondition insertCondition = (TLz_InsertCondition)pSqlstmt.Insert_clause_when_then_list[k];
                    if (insertCondition._ndCondition != null)
                    {
                        lcStr += "condition:\n";
                        lcStr += (insertCondition._ndCondition.AsText + "\n");
                    }
                    TLz_List intoValues = insertCondition._lstInsertIntoValues;
                    if (intoValues != null && intoValues.Count() > 0)
                    {
                        for (int i = 0; i < intoValues.Count(); i++)
                        {
                            TLz_InsertIntoValue insertIntoValue = (TLz_InsertIntoValue)intoValues[i];
                            lcStr += "insert into table:";
                            lcStr += insertIntoValue.Table.AsText;
                            if (insertIntoValue.Fields != null && insertIntoValue.Fields.Count() > 0)
                            {
                                lcStr += "\ncolumns:";
                                for (int j = 0; j < insertIntoValue.Fields.Count(); j++)
                                {
                                    lcStr = lcStr + "\n\t" + insertIntoValue.Fields[j].AsText;
                                }
                            }
                            if (insertIntoValue.Fields != null && insertIntoValue.Values.Count() > 0)
                            {
                                TLzFieldList values = (TLzFieldList)insertIntoValue.Values;
                                lcStr = lcStr + "\nvalues: ";
                                for (int j = 0; j < values.Count(); j++)
                                {
                                    lcStr = lcStr + "\n\t" + values[j].AsText;
                                }
                            }
                            lcStr += "\n";
                        }
                    }
                }
            }
            TSelectSqlStatement subquery = pSqlstmt.subquery;
            if (subquery != null)
            {
                lcStr += "select query details:\n";
                lcStr += SelectStmtInfo(subquery);
            }
            return lcStr;
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

            lcStr = lcStr + "\ntable: " + pSqlstmt.Table.AsText;
            lcStr = lcStr + "\ncolumns: ";
            for (int i = 0; i < pSqlstmt.Fields.Count(); i++)
            {
                lcStr = lcStr + "\n\t" + pSqlstmt.Fields[i].AsText;
            }

            TLzFieldList values = (TLzFieldList)pSqlstmt.MultiValues[0];
            lcStr = lcStr + "\nvalues: ";
            for (int i = 0; i < values.Count(); i++)
            {
                lcStr = lcStr + "\n\t" + values[i].AsText;
            }

            if (pSqlstmt.ReturningClause != null)
            {
                lcStr = lcStr + "\nreturning clause: " + pSqlstmt.ReturningClauseText;
            }


            return lcStr;
        }

        public static void analyzeupdate(TUpdateSqlStatement psql)
        {
            Console.WriteLine("Update statement:");

            Console.WriteLine(UpdateStmtInfo(psql));
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
            lcStr = lcStr + "\nset clause: ";
            for (int i = 0; i < pSqlstmt.Fields.Count(); i++)
            {
                lcStr = lcStr + "\n\tColumn:" + pSqlstmt.Fields[i].FieldExpr.lexpr.AsText + "\tvalue:" + pSqlstmt.Fields[i].FieldExpr.rexpr.AsText;
            }

            lcStr = lcStr + "\n" + "where clause: " + pSqlstmt.WhereClauseText;
            if (pSqlstmt.ReturningClause != null)
            {
                lcStr = lcStr + "\n" + "returning clause: " + pSqlstmt.ReturningClauseText;
            }


            return lcStr;
        }


        public static void analyzecreatetable(TCreateTableSqlStatement psql)
        {
            Console.WriteLine("Create table, table name:" + psql.Table.AsText);

            foreach (TLzField lcfield in psql.Table.Fields)
            {

                Console.WriteLine("Column name: " + lcfield.ColumnName.AsText);
                Console.WriteLine("Column type: " + lcfield.FieldDataType.AsText);

                if (lcfield.ColumnConstraints.Count() > 0)
                {
                    Console.WriteLine("Column level constraints");
                    foreach (TLzConstraint lcct in lcfield.ColumnConstraints)
                    {
                        analyzeconstraint(lcct);
                    }
                }
            }

            if (psql.Table.TableConstraints.Count() > 0)
            {
                Console.WriteLine("Table level constraints");
                foreach (TLzConstraint lcct in psql.Table.TableConstraints)
                {
                    analyzeconstraint(lcct);
                }
            }


        }

        public static void analyzeaField(TLzField pfield)
        {
            if (pfield.FieldType == TLzFieldType.lftColumn)
            {
                Console.WriteLine("Column name: " + pfield.ColumnName.AsText);
                if (pfield.FieldDataType != null)
                {
                    Console.WriteLine("Column type: " + pfield.FieldDataType.AsText);
                }

                if (pfield.ColumnConstraints.Count() > 0)
                {
                    Console.WriteLine("Column level constraints");
                    foreach (TLzConstraint lcct in pfield.ColumnConstraints)
                    {
                        analyzeconstraint(lcct);
                    }
                }
            }
            else
            {
                Console.WriteLine("Column: " + pfield.AsText);
            }

        }

        public static void analyzealtertable(TAlterTableStatement psql)
        {
            Console.WriteLine("Alter table: " + psql.Table.TableName);
            foreach (TLzAlterTableOption lcato in psql.AlterTableOptionList)
            {

                switch (lcato.atOption)
                {
                    case TLzAlterTableOptionType.atoAddColumn:
                        Console.WriteLine("Add column");
                        foreach (TLzField field in lcato.fields)
                        {
                            analyzeaField(field);
                        }
                        break;
                    case TLzAlterTableOptionType.atoAddConstraint:
                        Console.WriteLine("add constraint");
                        foreach (TLzConstraint constraint in lcato.TableConstraintList)
                        {
                            analyzeconstraint(constraint);
                        }
                        break;
                    case TLzAlterTableOptionType.atoModifyColumn:
                        Console.WriteLine("modify column");
                        foreach (TLzField field in lcato.fields)
                        {
                            analyzeaField(field);
                        }
                        break;
                    case TLzAlterTableOptionType.atoAlterColumn:
                        Console.WriteLine("alter column:" + lcato._ndColumnName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoDropColumn:
                        Console.WriteLine("drop column:");
                        foreach (TLz_Attr columnName in lcato.ColumnNameList)
                        {
                            Console.WriteLine(columnName.AsText);
                        }
                        break;
                    case TLzAlterTableOptionType.atoSetUnUsedColumn:
                        Console.WriteLine("set unused column:");
                        foreach (TLz_Attr columnName in lcato.ColumnNameList)
                        {
                            Console.WriteLine(columnName.AsText);
                        }
                        break;
                    case TLzAlterTableOptionType.atoDropUnUsedColumn:
                        Console.WriteLine("drop unused column");
                        break;
                    case TLzAlterTableOptionType.atoDropColumnsContinue:
                        Console.WriteLine("drop column continue");
                        break;
                    case TLzAlterTableOptionType.atoRenameColumn:
                        Console.WriteLine("rename column from {0} to {1}", lcato._ndColumnName.AsText, lcato._ndNewColumnName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoChangeColumn:
                        Console.WriteLine("change column {0}", lcato._ndColumnName.AsText);
                        Console.WriteLine("new column definition:");
                        analyzeaField(lcato.fields[0]);
                        break;
                    case TLzAlterTableOptionType.atoRenameTable:
                        Console.WriteLine("rename table, new table name: {0}", lcato._ndColumnName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoAddConstraintIndex:
                        Console.WriteLine("add constraint index");
                        foreach (TLz_Attr columnName in lcato.ColumnNameList)
                        {
                            Console.WriteLine("column name: {0}", columnName.AsText);
                        }
                        break;
                    case TLzAlterTableOptionType.atoAddConstraintPK:
                        Console.WriteLine("add constraint primary key");
                        foreach (TLz_Attr columnName in lcato.ColumnNameList)
                        {
                            Console.WriteLine("column name: {0}", columnName.AsText);
                        }
                        break;
                    case TLzAlterTableOptionType.atoAddConstraintUnique:
                        Console.WriteLine("add constraint unique");
                        foreach (TLz_Attr columnName in lcato.ColumnNameList)
                        {
                            Console.WriteLine("column name: {0}", columnName.AsText);
                        }
                        break;
                    case TLzAlterTableOptionType.atoAddConstraintFK:
                        Console.WriteLine("add constraint foreign key");
                        foreach (TLz_Attr columnName in lcato.ColumnNameList)
                        {
                            Console.WriteLine("column name: {0}", columnName.AsText);
                        }
                        if (lcato.refclause != null)
                        {
                            analyzeconstraintref(lcato.refclause);
                        }
                        break;
                    case TLzAlterTableOptionType.atoModifyConstraint:
                        Console.WriteLine("modify constraint, name: {0}", lcato._stConstraintName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoRenameConstraint:
                        Console.WriteLine("rename constraint from {0} to {1}", lcato._stConstraintName.AsText, lcato._stNewConstraintName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoDropConstraint:
                        Console.WriteLine("drop constraint, name: {0}", lcato._stConstraintName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoDropConstraintPK:
                        Console.WriteLine("drop primary key");
                        break;
                    case TLzAlterTableOptionType.atoDropConstraintFK:
                        Console.WriteLine("drop foreign key, constraint name: {0}", lcato._stConstraintName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoDropConstraintUnique:
                        if (lcato.DBVendor == TDbVendor.DbVOracle)
                        {
                            Console.WriteLine("drop contraint unqiue, columns:");
                            foreach (TLz_Attr columnName in lcato.ColumnNameList)
                            {
                                Console.WriteLine("column name: {0}", columnName.AsText);
                            }
                        }
                        else if (lcato.DBVendor == TDbVendor.DbVDB2)
                        {
                            Console.WriteLine("drop contraint unqiue, name: {0}", lcato._stConstraintName.AsText);
                        }
                        break;
                    case TLzAlterTableOptionType.atoDropConstraintCheck:
                        Console.WriteLine("drop contraint check, name: {0}", lcato._stConstraintName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoDropConstraintPartitioningKey:
                        Console.WriteLine("drop constraint Partitioning Key");
                        break;
                    case TLzAlterTableOptionType.atoDropConstraintRestrict:
                        Console.WriteLine("drop Constraint Restrict");
                        break;
                    case TLzAlterTableOptionType.atoDropConstraintIndex:
                        Console.WriteLine("drop Constraint index, name: {0}", lcato._stIndexName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoDropConstraintKey:
                        Console.WriteLine("drop Constraint key, name: {0}", lcato._stIndexName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoAlterConstraintFK:
                        Console.WriteLine("alter constraint foreign key, name: {0}", lcato._stConstraintName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoAlterConstraintCheck:
                        Console.WriteLine("alter constraint check, name: {0}", lcato._stConstraintName.AsText);
                        break;
                    case TLzAlterTableOptionType.atoCheckConstraint:
                        Console.WriteLine("Constraints in check constraints");
                        for (int m = 0; m < lcato._MssqlCheckConstraint.list1.Count(); m++)
                        {
                            TSourceToken lcst = (TSourceToken)lcato._MssqlCheckConstraint.list1[m];
                            Console.WriteLine("\t" + lcst.AsText);
                        }
                        break;
                    default:
                        Console.WriteLine("alter table option not handled yet");
                        break;
                }
            }
        }

        public static void analyzeconstraint(TLzConstraint pcon)
        {
            if ((pcon._name != null) & (pcon._name.AsText.Length > 0))
            {
                Console.WriteLine("constraint name:" + pcon._name.AsText);
            }
            switch (pcon.ConstraintType)
            {
                case TLzConstraintType.ctPrimarykey:
                    Console.WriteLine("Primary Key");

                    if (pcon.ColumnNameList.Count() > 0)
                    {
                        Console.WriteLine("Column in primary key");
                        for (int k = 0; k < pcon.ColumnNameList.Count(); k++)
                        {

                            Console.WriteLine("\t" + pcon.ColumnNameList[k].AsText);
                        }
                    }
                    break;
                case TLzConstraintType.ctForeignkey:
                    Console.WriteLine("Foreign Key");
                    if (pcon.ColumnList != null)
                    {
                        Console.WriteLine("Columns");
                        for (int k = 0; k < pcon.ColumnNameList.Count(); k++)
                        {

                            Console.WriteLine("\t" + pcon.ColumnNameList[k].AsText);
                        }
                    }
                    if (pcon.RefClause != null)
                    {
                        analyzeconstraintref(pcon.RefClause);
                    }

                    break;
                case TLzConstraintType.ctReference:
                    if (pcon.RefClause != null)
                    {
                        analyzeconstraintref(pcon.RefClause);
                    }
                    break;
                case TLzConstraintType.ctUnique:
                    Console.WriteLine("Unique key");
                    if (pcon.ColumnList != null)
                    {
                        Console.WriteLine("Column in unique key");
                        for (int k = 0; k < pcon.ColumnNameList.Count(); k++)
                        {
                            Console.WriteLine("\t" + pcon.ColumnNameList[k].AsText);
                        }
                    }
                    break;
                case TLzConstraintType.ctDefault:
                    Console.WriteLine("Default constraint");
                    Console.WriteLine(pcon.ColExpr.AsText);
                    break;
                case TLzConstraintType.ctCheck:
                    Console.WriteLine("Check constraint");
                    Console.WriteLine(pcon.ColExpr.AsText);
                    break;
                case TLzConstraintType.ctNotnull:
                    Console.WriteLine(pcon.AsText);
                    break;
                case TLzConstraintType.ctNull:
                    Console.WriteLine(pcon.AsText);
                    break;
                default:
                    Console.WriteLine("Constraint not handled yet");
                    Console.WriteLine("\t" + pcon.AsText);
                    break;

            }
        }

        public static void analyzeconstraintref(TLzConstraintRefClause pref)
        {
            Console.WriteLine("Constraint reference clause:");
            Console.WriteLine("\t referced object:" + pref.q_name.AsText);
            if (pref._column_listnode != null)
            {
                Console.WriteLine("reference columns:");
                for (int k = 0; k < pref._column_listnode.list1.Count(); k++)
                {
                    TLz_Attr lcattr = (TLz_Attr)pref._column_listnode.list1[k];
                    Console.WriteLine("\t" + lcattr.AsText);
                }
            }
        }

        public static void analyzeExec(TMssqlExecute pStmt)
        {
            // Console.WriteLine(pStmt.AsText);
            if (pStmt.ModuleName != null)
            {
                Console.WriteLine("procedure name:" + pStmt.ModuleName.AsText);
            }
            Console.WriteLine("exec type:" + pStmt.ExecType);
            Console.WriteLine("Parameters:" + pStmt.Parameters.Count());
            TGSqlParser sqlparser1 = new TGSqlParser(TDbVendor.DbVMssql);
            TLz_MssqlExecParam param;
            String sqltext;
            for (int i = 0; i < pStmt.Parameters.Count(); i++)
            {
                param = (TLz_MssqlExecParam)pStmt.Parameters[i];

                if (param._ndParamValue.AsText.StartsWith("N'", true, new System.Globalization.CultureInfo("en-US")))
                {
                    sqltext = param._ndParamValue.AsText.Substring(2, param._ndParamValue.AsText.Length - 3);
                    if (sqltext.StartsWith("INSERT", true, new System.Globalization.CultureInfo("en-US"))
                        || sqltext.StartsWith("UPDATE", true, new System.Globalization.CultureInfo("en-US"))
                        || sqltext.StartsWith("SELECT", true, new System.Globalization.CultureInfo("en-US"))
                        )
                    {
                        sqlparser1.SqlText.Text = sqltext.Replace(Environment.NewLine, "").Replace("''", "'");

                        int ret = sqlparser1.Parse();
                        if (ret == 0)
                        {
                            // Console.WriteLine(sqlparser1.SqlStatements[0].SqlStatementType);
                            // analyzeinsert((TInsertSqlStatement)sqlparser1.SqlStatements[0]);
                            analyzestmt(sqlparser1.SqlStatements[0]);
                        }
                        else
                        {
                            Console.WriteLine("parse error:" + sqlparser1.ErrorMessages);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Param" + i + ":" + sqltext);
                    }
                }
                else
                {
                    Console.WriteLine("Param" + i + ":" + param._ndParamValue.AsText);
                }
            }


        }

        public static void analyzePlsql_Block(TLzPlsql_Block pStmt)
        {
            if (pStmt._declSect != null)
            {
                Console.WriteLine("Delcare section:\n");
            }

            if (pStmt._procSect != null)
            {
                //Console.WriteLine("size: " + pStmt._procSect.Count());
                for (int i = 0; i < pStmt._procSect.Count(); i++)
                {
                    //  Console.WriteLine("type: "+ ((TCustomSqlStatement)pStmt._procSect[i]).SqlStatementType);
                    analyzestmt((TCustomSqlStatement)pStmt._procSect[i]);
                }
            }
        }

        public static void analyzePlsql_IfStmt(TLzPlsql_IfStmt pStmt)
        {
            Console.WriteLine("if condition: {0}", pStmt._condition.AsText);
            foreach (TCustomSqlStatement sql in ((TlzPlsql_ProcSect)pStmt.thenprocsect).Stmts)
            {
                // Console.WriteLine("type: " + sql.SqlStatementType);
                analyzestmt(sql);
            }
        }

        public static void analyzePlsql_AssignStmt(TLzPlsql_AssignStmt pStmt)
        {
            Console.WriteLine("plsql assignment: left: {0}, right: {1}", pStmt._leftside.AsText, pStmt._rightside.AsText);
        }

        public static void analyzePlsql_ForallStmt(TLzPlsql_ForallStmt pStmt)
        {
            Console.WriteLine("plsql for statement");
            analyzestmt(((TLzPlsql_SqlStmt)(pStmt._selectstmt))._sqlstmt as TCustomSqlStatement);
        }

        public static void analyzePlsql_ProcBasicStmt(TLzPlsql_ProcBasicStmt pStmt)
        {
            Console.WriteLine("basic statement: {0}", pStmt._expr.AsText);
        }

        public static void analyzePlsql_ExecImmeStmt(TLzPlsql_ExecImmeStmt pStmt)
        {
            Console.WriteLine("plsql Exec Immediate statement: {0}", pStmt._expr.AsText);
            if (pStmt._using_list != null)
            {
                for (int i = 0; i < pStmt._using_list.Count(); i++)
                {
                    TLz_DummyNode node = pStmt._using_list[i] as TLz_DummyNode;
                    if (node.node1 != null)
                    {
                        Console.WriteLine("using clause: {0}", node.node1.AsText);
                    }
                }
            }
            //    analyzestmt(((TLzPlsql_SqlStmt)(pStmt._selectstmt))._sqlstmt as TCustomSqlStatement);
        }

        public static void analyzePlsqlStatement(TPlsqlStatement pStmt)
        {
            TCustomSqlStatement plsql = pStmt.ChildNodes[0] as TCustomSqlStatement;

            switch (plsql.SqlStatementType)
            {
                case TSqlStatementType.sstplsql_block:
                    analyzePlsql_Block((TLzPlsql_Block)plsql);
                    break;
                case TSqlStatementType.sstplsql_createtrigger:
                    analyzePlsql_CreateTrigger((TLzPlsql_Trigger)plsql);
                    break;
                case TSqlStatementType.sstPlsql_ProcedureDecl:
                    analyzePlsql_CreateProcedure((TLzPlsql_SubProgram)plsql);
                    break;
                default:
                    break;
            }
        }

        private static void analyzePlsql_CreateProcedure(TLzPlsql_SubProgram pStmt)
        {
            if (pStmt._procedure_name != null)
            {
                if (pStmt._isFunction)
                    Console.WriteLine("plsql function name:" + pStmt._procedure_name.AsText);
                else
                    Console.WriteLine("plsql procedure name:" + pStmt._procedure_name.AsText);
            }
        }

        private static void analyzePlsql_CreateTrigger(TLzPlsql_Trigger pStmt)
        {
            if (pStmt._ndTriggername != null)
            {
                Console.WriteLine("plsql trigger name:" + pStmt._ndTriggername.AsText);
            }

            if (pStmt._plsql_block != null)
            {
                analyzePlsql_Block(pStmt._plsql_block);
            }
        }

        public static void analyzeCreateSequence(TOracleCreateSequenceStmt pStmt)
        {
            Console.WriteLine("sequence name {0}", pStmt.sequenceName.AsPrettyText);
            for (int i = 0; i < pStmt.options.Count(); i++)
            {
                TLz_SequenceOption option = pStmt.options[i] as TLz_SequenceOption;
                Console.WriteLine("option type: {0}", option.sequenceOptionType);
                if (option._ndValue != null)
                {
                    Console.WriteLine("option value: {0}", option._ndValue.AsText);
                }
            }
        }


    }
}

