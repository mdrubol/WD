using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WD.UI
{

    public class CSVReader
    {
        //
        private string filePath;
        private StreamReader objReader;

        //add name space System.IO.Stream
        public CSVReader(string filePath) : this(filePath, null) { }

        public CSVReader(string filePath, Encoding enc)
        {
            this.filePath = filePath;
            //check the Pass Stream whether it is readable or not
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            objReader = (enc != null) ? new StreamReader(filePath, enc) : new StreamReader(filePath);
        }
        //parse the Line
        public string[] GetCSVLine()
        {
            string data = objReader.ReadLine();
            if (data == null) return null;
            if (data.Length == 0) return new string[0];
            //System.Collection.Generic
            System.Collections.ArrayList result = new System.Collections.ArrayList();
            //parsing CSV Data
            ParseCSVData(result, data);
            return (string[])result.ToArray(typeof(string));
        }

        private void ParseCSVData(System.Collections.ArrayList result, string data)
        {
            int position = -1;
            while (position < data.Length)
                result.Add(ParseCSVField(ref data, ref position));
        }

        private string ParseCSVField(ref string data, ref int StartSeperatorPos)
        {
            if (StartSeperatorPos == data.Length - 1)
            {
                StartSeperatorPos++;
                return "";
            }

            int fromPos = StartSeperatorPos + 1;
            if (data[fromPos] == '"')
            {
                int nextSingleQuote = GetSingleQuote(data, fromPos + 1);
                int lines = 1;
                while (nextSingleQuote == -1)
                {
                    data = data + "\n" + objReader.ReadLine();
                    nextSingleQuote = GetSingleQuote(data, fromPos + 1);
                    lines++;
                    if (lines > 20)
                        throw new Exception("lines overflow: " + data);
                }
                StartSeperatorPos = nextSingleQuote + 1;
                string tempString = data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1);
                tempString = tempString.Replace("'", "''");
                return tempString.Replace("\"\"", "\"");
            }

            int nextComma = data.IndexOf(',', fromPos);
            if (nextComma == -1)
            {
                StartSeperatorPos = data.Length;
                return data.Substring(fromPos);
            }
            else
            {
                StartSeperatorPos = nextComma;
                return data.Substring(fromPos, nextComma - fromPos);
            }
        }

        private int GetSingleQuote(string data, int SFrom)
        {
            int i = SFrom - 1;
            while (++i < data.Length)
                if (data[i] == '"')
                {
                    if (i < data.Length - 1 && data[i + 1] == '"')
                    {
                        i++;
                        continue;
                    }
                    else
                        return i;
                }
            return -1;
        }
    }

    //public partial class Default : System.Web.UI.Page
    //{

    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        string filePath = Server.MapPath("xms.txt");
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            CSVReader reader = new CSVReader(filePath);
    //            //get the header
    //            string[] headers = reader.GetCSVLine();
    //            DataTable dt = new DataTable();
    //            //add headers
    //            foreach (string strHeader in headers)
    //                dt.Columns.Add(strHeader);

    //            string[] data;
    //            while ((data = reader.GetCSVLine()) != null)
    //                dt.Rows.Add(data);

    //        }
    //        catch (Exception ex) //Error
    //        {

    //        }
    //        // Remove the key from memory. 
    //    }

    //}
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("EM68820160428092657.csv");
            try
            {
               // // Creates and opens an ODBC connection
               // string strConnString = "jdbc:odbc:Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + filePath + ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
               // string sql_select;
               // OdbcConnection conn;
               // conn = new OdbcConnection(strConnString.Trim());
               // conn.Open();
               // //Creates the select command text
               // //if (noofrows == -1)
               // //{
               //     sql_select = "select * from [xms]";
               //// }
               // //else
               //// {
               // //    sql_select = "select top " + noofrows + " * from [" + this.FileNevCSV.Trim() + "]";
               //// }
               // //Creates the data adapter
               // OdbcDataAdapter obj_oledb_da = new OdbcDataAdapter(sql_select, conn);
               // //Fills dataset with the records from CSV file
               // obj_oledb_da.Fill(ds, "csv");
               // //closes the connection
               // conn.Close();

                string header =  "No";
                string pathOnly = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileName(filePath);
                string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                          ";Extended Properties=\"Text;HDR=" + header + "\"";
                string sql = @"SELECT * FROM [" + fileName + "]";
                DataTable dataTable = new DataTable();
                using (System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(connectionString))
                {
                    using (System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand(sql, connection))
                    {
                        using (System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(command))
                        {
                            dataTable.Locale = System.Globalization.CultureInfo.CurrentCulture;
                            adapter.Fill(dataTable);
                            //return dataTable;
                        }
                    }
                }
                if (dataTable.Rows.Count > 0)
                {
                    List<string> iList = new List<string>();
                    iList.Add("XLC30");
                    iList.Add("XLC32");
                    iList.Add("XLC32");
                    iList.Add("XLC33");
                    iList.Add("XLC34");
                    iList.Add("XLC35");
                    iList.Add("XLC36");
                    iList.Add("XLC37");
                    iList.Add("XLC38");
                    iList.Add("XLC39");
                    iList.Add("XLC40");
                    iList.Add("XLC41");
                    iList.Add("XLC42");
                    iList.Add("XLC43");
                    iList.Add("XLC44");
                    iList.Add("XLC45");
                    iList.Add("XLC46");
                    iList.Add("XLC47");
                    iList.Add("XLC48");
                    iList.Add("XLC49");
                    iList.Add("XLC50");
                    iList.Add("XLC51");
                    iList.Add("XLC52");
                    iList.Add("XLC53");
                    iList.Add("XLC54");
                    iList.Add("XLC55");
                    iList.Add("XLC56");
                    iList.Add("XLC57");
                    iList.Add("XLC58");
                    iList.Add("XLC59");
                    foreach (DataRow row in dataTable.Select("DISTINCT(F1)"))
                    { 
                      
                    }
                }

            }
            catch (Exception ex) //Error
            {

            }
            // Remove the key from memory. 
        }
        static void DecryptFile(string sInputFilename,
                string sOutputFilename,
                string sKey)
        {
            RijndaelManaged DES = new RijndaelManaged();
            ////A 64 bit key and IV is required for this provider.
            ////Set secret key For DES algorithm.
            ////Set initialization vector.
            //DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename,
                                       FileMode.Open,
                                       FileAccess.Read,FileShare.Read,100,FileOptions.Encrypted);
            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread,
                                                     desdecrypt,
                                                     CryptoStreamMode.Read);
            //Print the contents of the decrypted file.
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            fsDecrypted.Flush();
            fsDecrypted.Close();
        }
    }
}