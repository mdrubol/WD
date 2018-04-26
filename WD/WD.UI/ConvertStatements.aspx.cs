using gudusoft.gsqlparser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using gudusoft.gsqlparser.Units;
using System.IO;

namespace VariableAnalyze
{
    public class TFunction
    {
        public string functionName;
        public string returnType;
        public TParameter[] parameters;
        public TVariable[] variables;
        public string lineNumber;
    }

    public class TParameter
    {
        public string paramName;
        public string dataType;
        public string lineNumber;
    }


    public class TVariable
    {
        public string varName;
        public string dataType;
        public string lineNumber;
    }


    public class variableAnalyze
    {
        private StringBuilder buffer = new StringBuilder();
        private Hashtable functionMap = new Hashtable();

        public string[] getFunctionNames()
        {
            return (string[])functionMap.Keys;
        }

        public TFunction getFunction(string functionName)
        {
            return (TFunction)functionMap[functionName];
        }

        public string AnalysisResult
        {
            get { return buffer.ToString(); }
        }




        public variableAnalyze(string sql, TDbVendor dbVendor)
        {
            TGSqlParser sqlparser = new TGSqlParser(dbVendor);
            sqlparser.SqlText.Text = sql;
            analyzeSQL(sqlparser);
        }

        public variableAnalyze(FileInfo file, TDbVendor dbVendor)
        {
            TGSqlParser sqlparser = new TGSqlParser(dbVendor);
            sqlparser.Sqlfilename = file.ToString();
            analyzeSQL(sqlparser);
        }

        private void analyzeSQL(TGSqlParser sqlparser)
        {
            int ret = sqlparser.Parse();

            if (ret != 0)
            {
                buffer.AppendLine(sqlparser.ErrorMessages);
                return;
            }

            analyzeSql(sqlparser);

            foreach (string key in functionMap.Keys)
            {
                TFunction function = (TFunction)functionMap[key];
                buffer.AppendLine(System.Environment.NewLine + "Function:" + function.functionName);
                buffer.AppendLine("Return Data Type:" + function.returnType);
                buffer.AppendLine("Line Number:" + function.lineNumber);

                if (function.parameters != null)
                {
                    foreach (TParameter param in function.parameters)
                    {
                        buffer.AppendLine(System.Environment.NewLine + "Parameter:" + param.paramName);
                        buffer.AppendLine("Data Type:" + param.dataType);
                        buffer.AppendLine("Line Number:" + param.lineNumber);
                    }
                }

                if (function.variables != null)
                {
                    foreach (TVariable variable in function.variables)
                    {
                        buffer.AppendLine(System.Environment.NewLine + "Variable:" + variable.varName);
                        buffer.AppendLine("Data Type:" + variable.dataType);
                        buffer.AppendLine("Line Number:" + variable.lineNumber);
                    }
                }
            }
        }

        private void analyzeSql(TGSqlParser sqlparser)
        {
            for (int i = 0; i < sqlparser.SqlStatements.Count(); i++)
            {
                if (sqlparser.SqlStatements[i] is TPlsqlStatement)
                {
                    TPlsqlStatement stmt = (TPlsqlStatement)sqlparser.SqlStatements[i];
                    if (stmt.SqlStatementType == TSqlStatementType.sstplsql_createpackage)
                    {
                        TLzPlsql_Package pkg = (TLzPlsql_Package)stmt.Root;
                        if (pkg._definitions != null)
                        {
                            for (int j = 0; j < pkg._definitions.Count(); j++)
                            {
                                if (pkg._definitions[j] is TLzPlsql_SubProgram)
                                {
                                    TLzPlsql_SubProgram procedure = (TLzPlsql_SubProgram)pkg._definitions[j];
                                    if (procedure._isFunction == true)
                                    {
                                        TFunction function = new TFunction();
                                        string functionName = procedure._procedure_name.AsText;
                                        string returnType = procedure._plsql_datatype.AsText;
                                        function.functionName = functionName;
                                        function.returnType = returnType;
                                        int index = procedure._procedure_name.ObjectNameToken.posinlist;
                                        int lineNumber = getLineNumber(stmt, index);
                                        function.lineNumber = lineNumber.ToString();
                                        functionMap.Add(functionName, function);

                                        if (procedure._parameters != null && procedure._parameters.Count() > 0)
                                        {
                                            List<TParameter> paramList = new List<TParameter>();
                                            for (int k = 0; k < procedure._parameters.Count(); k++)
                                            {
                                                if (procedure._parameters[k] is TLzPlsql_ParameterDecl)
                                                {
                                                    TLzPlsql_ParameterDecl param = (TLzPlsql_ParameterDecl)procedure._parameters[k];
                                                    string paramName = param._parameter_name.AsText;
                                                    string dataType = param._plsql_datatype.AsText;
                                                    TParameter parameter = new TParameter();
                                                    parameter.paramName = paramName;
                                                    parameter.dataType = dataType;
                                                    index = param._parameter_name.posinlist;
                                                    lineNumber = getLineNumber(stmt, index);
                                                    parameter.lineNumber = lineNumber.ToString();
                                                    paramList.Add(parameter);
                                                }
                                            }
                                            function.parameters = paramList.ToArray();
                                        }
                                        if (procedure._decl_stmts != null && procedure._decl_stmts.Count() > 0)
                                        {
                                            List<TVariable> variableList = new List<TVariable>();
                                            for (int k = 0; k < procedure._decl_stmts.Count(); k++)
                                            {
                                                if (procedure._decl_stmts[k] is TLzPlsql_VarDeclStmt)
                                                {
                                                    TLzPlsql_VarDeclStmt var = (TLzPlsql_VarDeclStmt)procedure._decl_stmts[k];
                                                    string varName = var._varname.AsText;
                                                    string dataType = var._typename.AsText;
                                                    TVariable variable = new TVariable();
                                                    variable.varName = varName;
                                                    variable.dataType = dataType;
                                                    index = var._varname.posinlist;
                                                    lineNumber = getLineNumber(stmt, index);
                                                    variable.lineNumber = lineNumber.ToString();
                                                    variableList.Add(variable);
                                                }
                                            }
                                            function.variables = variableList.ToArray();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static int getLineNumber(TPlsqlStatement stmt, int index)
        {
            int line = 1;
            for (int x = 0; x < index; x++)
            {
                string text = stmt.SourceTokenList[x].AsText;
                for (int y = 0; y < text.Length; y++)
                {
                    if (text[y] == '\n')
                        line++;
                }
            }
            return line;
        }
    }
}
namespace WD.UI
{
    public partial class ConvertStatements : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hash.Add("sql", @"\s+?\btop\s+?[(]\s+?\w+\s+?[)]|\s?\btop\s?[(]\s?\w+\s?[)]");
            hash.Add("oracle", @"\s+?\rownum\s+?<=\s+?\w+\s+?|\s?\rownum\s?<=\s?\w+\s?]");
            hash.Add("db2", @"\s+?\fetch\s+?first\s+?\d+\s+?\rows\s+?only|\s?\fetch\s?first\s?\d+\s?\rows\s?only]");
            hash.Add("numeric", @"\d+");
            if (!IsPostBack)
            {
                txtSource.Text = "SELECT TOP(10)* FROM TEMP";
                if (Request.QueryString["OP"] == "OL")
                {
                    navBar.Visible = false;
                }
            }
        }
        private string format(string sql, TDbVendor TDbVendor)
        {
            TGSqlParser sqlparser = new TGSqlParser(TDbVendor);
            sqlparser.SqlText.Text = sql;
            if (sqlparser.PrettyPrint() == 0)
            {
                return sqlparser.FormattedSqlText.Text;
            }
            else
            {
                return sql;
            }
        }
        protected void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {

                navBar.Visible = true;
                lblMessage.Visible = false;
                TDbVendor dbVendor;
                switch (ddlSource.SelectedItem.Value.ToLower())
                {
                    case "sql": dbVendor = TDbVendor.DbVMssql; break;
                    case "oracle": dbVendor = TDbVendor.DbVOracle; break;
                    case "db2": dbVendor = TDbVendor.DbVDB2; break;
                    default: dbVendor = TDbVendor.DbVMssql; break;

                }
                VariableAnalyze.variableAnalyze analysis = new VariableAnalyze.variableAnalyze(new FileInfo(ConfigurationManager.AppSettings["SqlIn"]), dbVendor);
                TraceError(analysis.AnalysisResult);
                TGSqlParser sqlparser = new TGSqlParser(dbVendor);
                sqlparser.SqlText.Text = txtSource.Text;
                int ret = sqlparser.CheckSyntax();
                if (ret == 0)
                {
                    string theSQL = format(txtSource.Text, dbVendor);
                    joinconverter.oracleJoinConverter c = new joinconverter.oracleJoinConverter(theSQL);
                    c.convert(dbVendor);
                    theSQL = (c.getQuery());
                    sqlparser = new TGSqlParser(dbVendor);
                    sqlparser.SqlText.Text = theSQL;
                    sqlparser.Parse();
                    if (sqlparser.ErrorCount != 0)
                    {
                        TraceError(sqlparser.ErrorMessages);
                        return;
                    }
                    if (sqlparser.SqlStatements.Count() == 0)
                    {
                        TraceError("no sql found");
                        return;
                    }
                    if (sqlparser.SqlStatements[0].SqlStatementType != TSqlStatementType.sstSelect)
                    {
                        TraceError("no select sql found");
                        return;
                    }
                    joinconverter.joinconveter.sqlserverjointoansi(sqlparser.SqlStatements[0] as TSelectSqlStatement);
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(ConfigurationManager.AppSettings["SqlIn"]))
                    {
                        writer.Write(sqlparser.SqlStatements[0].AsText);
                    }
                    Process();
                }
                else
                {
                    TraceError(String.Format("Syntax error detected: {0}", sqlparser.ErrorMessages));
                }

            }
            catch (Exception exc)
            {
                TraceError(exc.Message);
            }

        }
        private void TraceError(string message)
        {
            lblMessage.Visible = true;
            lblMessage.Text = message;

        }
        Hashtable hash = new Hashtable();
        
        public void Process()
        {
            try
            {
                
                string theSQL = txtSource.Text.ToString();
                bool IsActive = false;
                int topBottom = -1;
                string result = string.Empty;
                string replaceText = string.Empty;
                string key = string.Empty;
                string args = "-s = " + ddlSource.SelectedItem.Value;
                Regex regex = null;
                args += " -t =  " + ddlTarget.SelectedItem.Value;
                args += " -in = " + ConfigurationManager.AppSettings["SqlIn"];
                args += " -out = " + ConfigurationManager.AppSettings["SqlOut"];
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(ConfigurationManager.AppSettings["SqlExe"], args);
                startInfo.CreateNoWindow = Convert.ToBoolean(ConfigurationManager.AppSettings["CreateNoWindow"]);
                startInfo.UseShellExecute = Convert.ToBoolean(ConfigurationManager.AppSettings["UseShellExecute"]);
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                using (System.Diagnostics.Process exeProcess = System.Diagnostics.Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
                using (System.IO.StreamReader reader = new System.IO.StreamReader(ConfigurationManager.AppSettings["SqlOut"]))
                {
                    txtTarget.Text = string.Empty;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {

                        txtTarget.Text += line;
                        txtTarget.Text += Environment.NewLine;
                    }
                }
                #region SELECT TOP(N)* FROM TABLENAME
                switch (ddlTarget.SelectedItem.Value)
                {
                    case "db2":
                        //SQL TOP
                        key = "sql";
                        regex = new Regex(hash[key].ToString());
                        foreach (Match match in regex.Matches(theSQL.ToLower()))
                        {
                            replaceText = match.Value;
                            IsActive = true;
                        }
                        if (!string.IsNullOrEmpty(replaceText))
                        {
                            regex = new Regex(hash["numeric"].ToString());
                            foreach (Match match in regex.Matches(replaceText))
                            {
                                result = " FETCH FIRST " + match.Value + " ROWS ONLY";
                                TraceError(match.Value);
                                topBottom = 1;//1 means bottom addition
                            }
                        }
                        if (!IsActive)
                        {
                            //ORACLE FETCH
                            result = string.Empty;
                            key = "oracle";
                            regex = new Regex(hash[key].ToString());
                            foreach (Match match in regex.Matches(theSQL.ToLower()))
                            {
                                replaceText = match.Value;
                                IsActive = true;
                            }
                            if (!string.IsNullOrEmpty(replaceText))
                            {
                                regex = new Regex(hash["numeric"].ToString());
                                foreach (Match match in regex.Matches(replaceText))
                                {
                                    result = " FETCH FIRST " + match.Value + " ROWS ONLY";
                                    topBottom = 1;//1 means bottom addition
                                }
                            }
                        }

                        break;
                    case "sql":
                         result = string.Empty;
                         key = "oracle";
                         regex = new Regex(hash[key].ToString());//|\bTOP[\b(]
                        foreach (Match match in regex.Matches(theSQL.ToLower()))
                        {
                            replaceText = match.Value;
                            IsActive = true;
                        }
                        if (!string.IsNullOrEmpty(replaceText))
                        {
                            regex = new Regex(hash["numeric"].ToString());
                            foreach (Match match in regex.Matches(replaceText))
                            {
                                result = " TOP(" + match.Value + ")";
                                TraceError(match.Value);
                                topBottom = 0;//0 means top addition
                            }
                        }
                        if (!IsActive)
                        {
                            result = string.Empty;
                            key = "db2";
                            regex = new Regex(hash[key].ToString());//|\bTOP[\b(]
                            foreach (Match match in regex.Matches(theSQL.ToLower()))
                            {
                                replaceText = match.Value;
                                IsActive = true;
                            }
                            if (!string.IsNullOrEmpty(replaceText))
                            {
                                regex = new Regex(hash["numeric"].ToString());
                                foreach (Match match in regex.Matches(replaceText))
                                {
                                    result = " TOP(" + match.Value + ")";
                                    TraceError(match.Value);
                                    topBottom = 0;//0 means top addition
                                }
                            }
                        }
                        break;
                    case "oracle": 
                        key = "sql";
                        regex = new Regex(hash[key].ToString());
                        foreach (Match match in regex.Matches(theSQL.ToLower()))
                        {
                            replaceText = match.Value;
                            IsActive = true;
                        }
                        if (!string.IsNullOrEmpty(replaceText))
                        {
                            regex = new Regex(hash["numeric"].ToString());
                            foreach (Match match in regex.Matches(replaceText))
                            {
                                result = " rownum <= " + match.Value ;
                                TraceError(match.Value);
                                topBottom = 1;//1 means bottom addition
                            }
                        }
                        if (!IsActive)
                        {
                            //ORACLE FETCH
                            result = string.Empty;
                            key = "db2";
                            regex = new Regex(hash[key].ToString());
                            foreach (Match match in regex.Matches(theSQL.ToLower()))
                            {
                                replaceText = match.Value;
                                IsActive = true;
                            }
                            if (!string.IsNullOrEmpty(replaceText))
                            {
                                regex = new Regex(hash["numeric"].ToString());
                                foreach (Match match in regex.Matches(replaceText))
                                {
                                    result = " rownum <= " + match.Value;
                                    topBottom = 1;//1 means bottom addition
                                }
                            }
                        }
                        break;
                }
                #endregion
                if (IsActive)
                {
                    if (!txtTarget.Text.ToLower().Contains(result.ToLower()))
                    {
                        regex = new Regex(hash[key].ToString());
                        foreach (Match match in regex.Matches(txtTarget.Text.ToLower()))
                        {
                            replaceText = match.Value;
                            IsActive = true;
                        }
                        txtTarget.Text = txtTarget.Text.ToLower().Replace(replaceText.ToLower(), "");
                        if (topBottom == 0)
                        {
                            txtTarget.Text = txtTarget.Text.ToLower().Replace("select", "SELECT " + result + " ");
                        }
                        else if (topBottom == 1)
                        {
                            txtTarget.Text += result;
                        }
                    }
                }
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(ConfigurationManager.AppSettings["SqlOut"]))
                {
                    writer.Write(string.Empty);
                }
            }
            catch (Exception exc)
            {
                TraceError(exc.Message);
            }
        }

        void Process(TDbVendor vender) {
        
        
        }
    }

}