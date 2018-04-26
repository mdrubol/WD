using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using gudusoft.gsqlparser;
using gudusoft.gsqlparser.Units;


namespace joinconverter
{
    enum jointype { inner, left, right };

    class JoinCondition
    {
        public String lefttable, righttable, leftcolumn, rightcolumn;
        public jointype jt;
        public Boolean used;
        public String expr;
    }

    class FromClause
    {
        public TLzTable joinTable;
        public String joinClause;
        public String condition;
    }

    class getJoinConditionVisitor
    {
        List<JoinCondition> jrs = new List<JoinCondition>();

        public List<JoinCondition> getJrs()
        {
            return jrs;
        }

        private void analyzeJoinCondition(TLzCustomExpression expr)
        {
            TLzCustomExpression slexpr;
            TLzCustomExpression srexpr;

            if (expr.oper == TLzOpType.Expr_Comparison)
            {
                slexpr = (TLzCustomExpression)expr.lexpr;
                srexpr = (TLzCustomExpression)expr.rexpr;

                if (((slexpr.oper == TLzOpType.Expr_Attr) || (slexpr.oper == TLzOpType.Expr_OuterJoin) || (srexpr.oper == TLzOpType.Expr_OuterJoin && slexpr.oper == TLzOpType.Expr_Const))
                   && ((srexpr.oper == TLzOpType.Expr_Attr) || (srexpr.oper == TLzOpType.Expr_OuterJoin)) || (slexpr.oper == TLzOpType.Expr_OuterJoin && srexpr.oper == TLzOpType.Expr_Const))
                {
                    TLz_Attr lattr = null, rattr = null;
                    JoinCondition jr = new JoinCondition();
                    jr.used = false;
                    jr.jt = jointype.inner;

                    if (slexpr.oper == TLzOpType.Expr_Attr)
                    {
                        lattr = (TLz_Attr)slexpr.lexpr;
                    }
                    else if (slexpr.oper == TLzOpType.Expr_OuterJoin)
                    {
                        lattr = (TLz_Attr)((TLzCustomExpression)slexpr.lexpr).lexpr;
                        jr.jt = jointype.right;
                    }
                    jr.lefttable = lattr != null ? lattr.Prefix : null;
                    jr.leftcolumn = lattr != null ? lattr.lastname : null;

                    if (srexpr.oper == TLzOpType.Expr_Attr)
                    {
                        rattr = (TLz_Attr)srexpr.lexpr;
                    }
                    else if (srexpr.oper == TLzOpType.Expr_OuterJoin)
                    {
                        rattr = (TLz_Attr)((TLzCustomExpression)srexpr.lexpr).lexpr;
                        jr.jt = jointype.left;
                    }

                    jr.righttable = rattr != null ? rattr.Prefix : null;
                    jr.rightcolumn = rattr != null ? rattr.lastname : null;
                    jr.expr = expr.AsText;

                    if (slexpr.oper == TLzOpType.Expr_OuterJoin || srexpr.oper == TLzOpType.Expr_OuterJoin)
                        jr.expr = jr.expr.Replace("(+)", "");

                    if ((jr.lefttable != "") && (jr.righttable != ""))
                    {
                        expr.AsText = " ";
                        jrs.Add(jr);
                    }
                }
            }


            // Console.WriteLine(expr.oper.ToString());
            // Console.WriteLine(lexpr.oper.ToString()+" "+lexpr.AsText);
            // Console.WriteLine(rexpr.oper.ToString() + " " + rexpr.AsText);

        }

        public Boolean treenodevisitor(TLz_Node pnode, Boolean pIsLeafNode)
        {
            TLzCustomExpression expr = (TLzCustomExpression)pnode;
            analyzeJoinCondition(expr);
            return true;
        }
    }

    public class oracleJoinConverter
    {
        private string query;
        private List<TLzTable> leadingTables = new List<TLzTable>();
        private string errorMessage;
        private int errorNo = 0;

        public oracleJoinConverter(string sql)
        {
            this.query = sql;
        }

        public string getQuery()
        {
            return this.query;
        }

        public int convert(TDbVendor dbVendor)
        {
            TGSqlParser sqlparser = new TGSqlParser(dbVendor);
            sqlparser.SqlText.Text = this.query;
            int i = sqlparser.Parse();
            if (i != 0) return i;
            if (sqlparser.SqlStatements[0].SqlStatementType != TSqlStatementType.sstSelect) return 0;
            TSelectSqlStatement select = (TSelectSqlStatement)sqlparser.SqlStatements[0];
            analyzeSelect(select);
            if (errorMessage != null && !errorMessage.Equals(""))
                this.query = errorMessage;
            else this.query = select.AsText;
            return errorNo;
        }

        private TLzTable getTablebyName(string tbname, TLzTableList tableList)
        {

            for (int i = 0; i < tableList.Count(); i++)
            {
                if ((tbname.Equals(tableList[i].TableFullname, StringComparison.CurrentCultureIgnoreCase))
                    || (tbname.Equals(tableList[i].TableAlias, StringComparison.CurrentCultureIgnoreCase)))
                {

                    return tableList[i];

                }

            }
            return null;
        }

        private void addToLeadingTables(TLzTable table)
        {

            if (table == null) return;

            bool tableAlreadyExists = false;
            for (int i = 0; i < leadingTables.Count; i++)
            {
                if (table == leadingTables[i])
                {
                    tableAlreadyExists = true;
                    break;
                }
            }

            if (!tableAlreadyExists)
            {
                leadingTables.Add(table);
            }
        }

        private Boolean isNameOfTable(TLzTable table, String name)
        {
            return (name == null) ? false : string.Equals(table.Name, name, StringComparison.OrdinalIgnoreCase);
        }

        private Boolean isAliasOfTable(TLzTable table, String alias)
        {
            if (table.AliasClause == null)
            {
                return false;
            }
            else
                return (alias == null) ? false : string.Equals(table.AliasClause.aliastext, alias, StringComparison.OrdinalIgnoreCase);
        }

        private Boolean isNameOrAliasOfTable(TLzTable table, String str)
        {
            return isAliasOfTable(table, str) || isNameOfTable(table, str);
        }

        private Boolean areTableJoined(TLzTable lefttable, TLzTable righttable,
                List<JoinCondition> jrs)
        {

            Boolean ret = false;

            for (int i = 0; i < jrs.Count; i++)
            {
                JoinCondition jc = jrs[i];
                if (jc.used)
                {
                    continue;
                }
                ret = isNameOrAliasOfTable(lefttable, jc.lefttable)
                        && isNameOrAliasOfTable(righttable, jc.righttable);
                if (ret)
                    break;
                ret = isNameOrAliasOfTable(lefttable, jc.righttable)
                        && isNameOrAliasOfTable(righttable, jc.lefttable);
                if (ret)
                    break;
            }

            return ret;
        }

        private List<JoinCondition> getJoinCondition(TLzTable lefttable,
            TLzTable righttable, List<JoinCondition> jrs)
        {
            List<JoinCondition> lcjrs = new List<JoinCondition>();
            for (int i = 0; i < jrs.Count; i++)
            {
                JoinCondition jc = jrs[i];
                if (jc.used)
                {
                    continue;
                }

                if (isNameOrAliasOfTable(lefttable, jc.lefttable)
                        && isNameOrAliasOfTable(righttable, jc.righttable))
                {
                    lcjrs.Add(jc);
                    jc.used = true;
                    // lefttable.c = righttable.c(+), => lefttable left join
                    // righttable
                    // lefttable.c(+) = righttable.c, => lefttable right join
                    // righttable
                    //if (jc.jt == jointype.right)
                    //{
                    //    jc.jt = jointype.left;
                    //}
                    //else if (jc.jt == jointype.left)
                    //{
                    //    jc.jt = jointype.left;
                    //}
                }
                else if (isNameOrAliasOfTable(lefttable, jc.righttable)
                        && isNameOrAliasOfTable(righttable, jc.lefttable))
                {
                    lcjrs.Add(jc);
                    jc.used = true;
                    // righttable.c = lefttable.c(+), => lefttable right join
                    // righttable
                    // righttable.c(+) = lefttable.c, => lefttable left join
                    // righttable
                }
                else if ((jc.lefttable == null)
                        && (isNameOrAliasOfTable(lefttable, jc.righttable) || isNameOrAliasOfTable(righttable,
                                jc.righttable)))
                {
                    // 'Y' = righttable.c1(+)
                    lcjrs.Add(jc);
                    jc.used = true;
                }
                else if ((jc.righttable == null)
                        && (isNameOrAliasOfTable(lefttable, jc.lefttable) || isNameOrAliasOfTable(righttable,
                                jc.lefttable)))
                {
                    // lefttable.c1(+) = 'Y'
                    lcjrs.Add(jc);
                    jc.used = true;
                }
            }
            return lcjrs;
        }

        private String getJoinType(List<JoinCondition> jrs)
        {
            String str = "inner join";
            for (int i = 0; i < jrs.Count; i++)
            {
                if (jrs[i].jt == jointype.left)
                {
                    str = "left outer join";
                    break;
                }
                else if (jrs[i].jt == jointype.right)
                {
                    str = "right outer join";
                    break;
                }
            }

            return str;
        }

        private void analyzeSelect(TSelectSqlStatement select)
        {
            if (select.SelectSetType == TSelectSetType.sltNone)
            {
                if (select.Tables.Count() == 1) return;

                if (select.WhereClause == null)
                {
                    if (select.Tables.Count() > 1)
                    {
                        //cross join
                        String str = select.Tables[0].AsText;
                        for (int i = 1; i < select.Tables.Count(); i++)
                        {
                            str = str + "\ncross join " + select.Tables[i].AsText;
                        }

                        for (int k = select.JoinTables.Count() - 1; k > 0; k--)
                        {
                            select.JoinTables.Remove(k);
                        }
                        select.JoinTables[0].AsText = str;
                    }
                }
                else
                {

                    getJoinConditionVisitor v = new getJoinConditionVisitor();

                    //get join conditions
                    select.WhereClause.PreOrderTraverse(v.treenodevisitor);
                    List<JoinCondition> jrs = v.getJrs();

                    Boolean[] tableUsed = new Boolean[select.Tables.Count()];
                    for (int i = 0; i < select.Tables.Count(); i++)
                    {
                        tableUsed[i] = false;
                    }

                    // make first table to be the left most joined table
                    String fromclause = select.Tables[0].AsText;

                    tableUsed[0] = true;
                    Boolean foundTableJoined;
                    List<FromClause> fromClauses = new List<FromClause>();


                    for (; ; )
                    {
                        foundTableJoined = false;

                        for (int i = 0; i < select.Tables.Count(); i++)
                        {
                            TLzTable lcTable1 = select.Tables[i];

                            TLzTable leftTable = null, rightTable = null;
                            for (int j = i + 1; j < select.Tables.Count(); j++)
                            {
                                TLzTable lcTable2 = select.Tables[j];
                                if (areTableJoined(lcTable1, lcTable2, jrs))
                                {
                                    if (tableUsed[i] && (!tableUsed[j]))
                                    {
                                        leftTable = lcTable1;
                                        rightTable = lcTable2;
                                    }
                                    else if ((!tableUsed[i]) && tableUsed[j])
                                    {
                                        leftTable = lcTable2;
                                        rightTable = lcTable1;
                                    }

                                    if ((leftTable != null)
                                            && (rightTable != null))
                                    {
                                        List<JoinCondition> lcjrs = getJoinCondition(leftTable,
                                                rightTable,
                                                jrs);
                                        FromClause fc = new FromClause();
                                        fc.joinTable = rightTable;
                                        fc.joinClause = getJoinType(lcjrs);
                                        String condition = "";
                                        for (int k = 0; k < lcjrs.Count; k++)
                                        {
                                            condition += lcjrs[k].expr;
                                            if (k != lcjrs.Count - 1)
                                            {
                                                condition += " and ";
                                            }
                                        }
                                        fc.condition = condition;

                                        fromClauses.Add(fc);
                                        tableUsed[i] = true;
                                        tableUsed[j] = true;

                                        foundTableJoined = true;
                                    }
                                }
                            }
                        }

                        if (!foundTableJoined)
                        {
                            break;
                        }
                    }

                    // are all join conditions used?
                    for (int i = 0; i < jrs.Count; i++)
                    {
                        JoinCondition jc = jrs[i];
                        if (!jc.used)
                        {
                            for (int j = fromClauses.Count - 1; j >= 0; j--)
                            {
                                if (isNameOrAliasOfTable(fromClauses[j].joinTable,
                                        jc.lefttable)
                                        || isNameOrAliasOfTable(fromClauses[j].joinTable,
                                                jc.righttable))
                                {
                                    fromClauses[j].condition += " and "
                                            + jc.expr;
                                    jc.used = true;
                                    break;
                                }
                            }
                        }
                    }

                    errorNo = 0;
                    errorMessage = "";
                    for (int i = 0; i < select.Tables.Count(); i++)
                    {
                        if (!tableUsed[i])
                        {

                            errorNo++;
                            errorMessage += ("\r\nError " + errorNo + ", Message: This table has no join condition: "
                                            + select.Tables[i].AsText);
                        }
                    }
                    if (errorNo != 0)
                        return;


                    // link all join clause
                    for (int i = 0; i < fromClauses.Count; i++)
                    {
                        FromClause fc = fromClauses[i];
                        // fromclause += System.getProperty("line.separator") +
                        // fc.joinClause
                        // +" "+fc.joinTable.getFullNameWithAliasString()+" on "+fc.condition;
                        fromclause += "\n"
                                + fc.joinClause
                                + " "
                                + fc.joinTable.AsText
                                + " on "
                                + fc.condition;
                    }

                    for (int k = select.JoinTables.Count() - 1; k > 0; k--)
                    {
                        select.JoinTables.Remove(k);
                    }

                    select.JoinTables[0].AsText = fromclause;

                    if ((select.WhereClause.StartToken == null)
                            || (select.WhereClause.AsText.ToString().Trim().Length == 0))
                    {
                        // no where condition, remove WHERE keyword
                        select.WhereClause.AsText = " ";
                    }

                }
            }
            else
            {
                analyzeSelect(select.LeftStmt);
                analyzeSelect(select.RightStmt);
            }
        }

    }


    struct TLzJoinInfo
    {
        public string lefttable, righttable;
        public TSelectJoinType jointype;
        public TLzCustomExpression expr;
        public Boolean isDuplicated;
    }



    class joinconveter
    {
        static TLzJoinInfo[] gJoinInfos = new TLzJoinInfo[100];
        static int JoinInfoCount = 0;
        static string gTableName = "";
        static ArrayList gJoinExprs = new ArrayList();

        public static TLzTable findTableByNameOrAlias(string pName, TLzTableList pTables)
        {
            TLzTable ret = null;

            foreach (TLzTable t in pTables)
            {
                if (t.AliasClause != null)
                {
                    if (lzbasetype.MyCompareText(pName, t.AliasClause.aliastext) == 0)
                    {
                        ret = t;
                    }
                }

                if (ret == null)
                {
                    if (lzbasetype.MyCompareText(pName, t.TableName) == 0)
                    {
                        ret = t;
                    }
                }

                if (ret != null)
                {
                    break;
                }
            }

            return ret;
        }


        public static Boolean dofindJoinInfo(TLz_Node pnode, Boolean pIsLeafNode)
        {
            Boolean ret = true;
            TLzCustomExpression lcExpr;
            TLz_Attr lcAttr;
            if (pnode is TLzCustomExpression)
            {
                lcExpr = pnode as TLzCustomExpression;
                switch (lcExpr.oper)
                {
                    case TLzOpType.Expr_subquery:
                        lcExpr.IsVisitSubTree = false;
                        break;
                    case TLzOpType.Expr_Attr:
                        lcAttr = lcExpr.lexpr as TLz_Attr;
                        if (lcAttr.ObjectNameToken != null)
                        {
                            gTableName = lcAttr.ObjectNameToken.AsText;
                        }
                        lcExpr.IsVisitSubTree = false;
                        break;
                }

            }
            return ret;
        }


        public static void findJoinInfo(TLzCustomExpression pExpr)
        {
            if (pExpr.oper == TLzOpType.Expr_Leftjoin)
            {
                JoinInfoCount++;
                gJoinInfos[JoinInfoCount - 1].jointype = TSelectJoinType.sjtleft;
                gTableName = "";
                TLzCustomExpression lcExpr;
                lcExpr = pExpr.lexpr as TLzCustomExpression;
                lcExpr.PreOrderTraverse(dofindJoinInfo);
                gJoinInfos[JoinInfoCount - 1].lefttable = gTableName;

                gTableName = "";
                lcExpr = pExpr.rexpr as TLzCustomExpression;
                lcExpr.PreOrderTraverse(dofindJoinInfo);
                gJoinInfos[JoinInfoCount - 1].righttable = gTableName;

                gJoinInfos[JoinInfoCount - 1].expr = pExpr;


            }
            else if (pExpr.oper == TLzOpType.Expr_Rightjoin)
            {
                JoinInfoCount++;
                gJoinInfos[JoinInfoCount - 1].jointype = TSelectJoinType.sjtright;
                gTableName = "";
                TLzCustomExpression lcExpr;
                lcExpr = pExpr.lexpr as TLzCustomExpression;
                lcExpr.PreOrderTraverse(dofindJoinInfo);
                gJoinInfos[JoinInfoCount - 1].lefttable = gTableName;

                gTableName = "";
                lcExpr = pExpr.rexpr as TLzCustomExpression;
                lcExpr.PreOrderTraverse(dofindJoinInfo);
                gJoinInfos[JoinInfoCount - 1].righttable = gTableName;

                gJoinInfos[JoinInfoCount - 1].expr = pExpr;

            }

            if ((pExpr.oper == TLzOpType.Expr_Leftjoin) || (pExpr.oper == TLzOpType.Expr_Rightjoin))
            {
                gJoinInfos[JoinInfoCount - 1].isDuplicated = false;

                for (int i = 0; i < JoinInfoCount - 1; i++)
                {
                    if (
                        (gJoinInfos[JoinInfoCount - 1].jointype == gJoinInfos[i].jointype)
                        && (lzbasetype.MyCompareText(gJoinInfos[JoinInfoCount - 1].lefttable, gJoinInfos[i].lefttable) == 0)
                        && (lzbasetype.MyCompareText(gJoinInfos[JoinInfoCount - 1].righttable, gJoinInfos[i].righttable) == 0)
                        )
                    {
                        gJoinInfos[JoinInfoCount - 1].isDuplicated = true;
                        break;
                    }
                    else if (
                        (gJoinInfos[JoinInfoCount - 1].jointype != gJoinInfos[i].jointype)
                        && (lzbasetype.MyCompareText(gJoinInfos[JoinInfoCount - 1].lefttable, gJoinInfos[i].righttable) == 0)
                        && (lzbasetype.MyCompareText(gJoinInfos[JoinInfoCount - 1].righttable, gJoinInfos[i].lefttable) == 0)
                        )
                    {
                        gJoinInfos[JoinInfoCount - 1].isDuplicated = true;
                        break;
                    }

                }
            }


        }

        public static Boolean dofindjoins(TLz_Node pnode, Boolean pIsLeafNode)
        {
            Boolean ret = true;
            TLzCustomExpression lcExpr;

            if (pnode is TLzCustomExpression)
            {
                lcExpr = pnode as TLzCustomExpression;
                switch (lcExpr.oper)
                {
                    case TLzOpType.Expr_subquery:
                        lcExpr.IsVisitSubTree = false;
                        break;
                    case TLzOpType.Expr_Leftjoin:
                        gJoinExprs.Add(lcExpr);
                        lcExpr.IsVisitSubTree = false;
                        break;
                    case TLzOpType.Expr_Rightjoin:
                        gJoinExprs.Add(lcExpr);
                        lcExpr.IsVisitSubTree = false;
                        break;
                }

            }
            return ret;
        }

        public static void findjoins(TLzCustomExpression pExpr)
        {
            gJoinExprs.Clear();
            pExpr.PreOrderTraverse(dofindjoins);
        }

        public static int sqlserverjointoansi(TSelectSqlStatement pSelect)
        {
            int ret = 0;
            for (int i = 0; i < pSelect.ChildNodes.Count(); i++)
            {
                if (pSelect.ChildNodes[i] is TSelectSqlStatement)
                {
                    ret = ret + sqlserverjointoansi(pSelect.ChildNodes[i] as TSelectSqlStatement);
                }
            }

            string lcFromStr = "";
            if (pSelect.WhereClause != null)
            {
                TLzCustomExpression lcWhere = pSelect.WhereClause;
                findjoins(lcWhere);
                if (gJoinExprs.Count > 0)
                {
                    JoinInfoCount = 0;
                    for (int i = 0; i < gJoinExprs.Count; i++)
                    {
                        findJoinInfo(gJoinExprs[i] as TLzCustomExpression);
                    }

                    if (JoinInfoCount > 0)
                    {
                        for (int i = 0; i < JoinInfoCount; i++)
                        {
                            gJoinInfos[i].expr.opname.AsText = "=";
                            if (gJoinInfos[i].isDuplicated)
                            {
                                lcFromStr = lcFromStr + " and " + gJoinInfos[i].expr.AsText;
                                continue;
                            }

                            ret++;

                            TLzTable lcLeftTable, lcRightTable;
                            lcLeftTable = findTableByNameOrAlias(gJoinInfos[i].lefttable, pSelect.Tables);
                            lcRightTable = findTableByNameOrAlias(gJoinInfos[i].righttable, pSelect.Tables);
                            if (lcLeftTable == null) { continue; }
                            if (lcRightTable == null) { continue; }

                            if (gJoinInfos[i].jointype == TSelectJoinType.sjtleft)
                            {
                                if (i == 0)
                                {
                                    lcFromStr = lcLeftTable.AsText + " left join " + lcRightTable.AsText + " on " + gJoinInfos[i].expr.AsText;
                                }
                                else
                                {
                                    lcFromStr = lcFromStr + Environment.NewLine + " left join " + lcRightTable.AsText + " on " + gJoinInfos[i].expr.AsText;
                                }
                            }

                            if (gJoinInfos[i].jointype == TSelectJoinType.sjtright)
                            {
                                if (i == 0)
                                {
                                    lcFromStr = lcLeftTable.AsText + " right join " + lcRightTable.AsText + " on " + gJoinInfos[i].expr.AsText;
                                }
                                else
                                {
                                    lcFromStr = lcFromStr + Environment.NewLine + " right join " + lcRightTable.AsText + " on " + gJoinInfos[i].expr.AsText;
                                }
                            }

                            gJoinInfos[i].expr.IsParseAfterSetValue = false;
                            gJoinInfos[i].expr.AsText = " ";

                        }

                        pSelect.FromClauseText = lcFromStr;


                    }

                }
            }

            return ret;

        }
    }
}

