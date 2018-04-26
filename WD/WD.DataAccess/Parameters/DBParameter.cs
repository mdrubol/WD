// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-18-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="DBParameter.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Data;



// namespace: WD.DataAccess.Parameters
//
// summary:	.


namespace WD.DataAccess.Parameters
{
    
    /// <summary>   A database parameter. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public class DBParameter
    {
        #region "Private Variables"

        /// <summary>   The name. </summary>
        private string name = string.Empty;

        /// <summary>   The value. </summary>
        private object value = null;

        private DbType type = DbType.String;

        /// <summary>   The parameter direction. </summary>
        private ParameterDirection paramDirection = ParameterDirection.Input;
        #endregion

        #region "Constructors"

        
        /// <summary>
        /// Defaule constructor. Paramete name, vale, type and direction needs to be assigned explicitly
        /// by using the public properties exposed.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        /// DBParameter param=new DBParameter();
        ///   param.ParameterName="F";
        ///   param.ParameterValue="XXX";
        /// }
        /// } 
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public DBParameter()
        {
        }

        
        /// <summary>
        /// Creates a parameter with the name and value specified. Default data type and direction is
        /// String and Input respectively.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        /// DBParameter param=new DBParameter("FirstName","XXXX");
        /// }
        /// } 
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="name">     Parameter name. </param>
        /// <param name="value">    Value associated with the parameter. </param>
        

        public DBParameter(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        
        /// <summary>
        /// Creates a parameter with the name, value and direction specified. Default data type is String.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        /// DBParameter param=new DBParameter("FirstName","XXXX",ParameterDirection.Input);
        /// }
        /// } 
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="name">             Parameter name. </param>
        /// <param name="value">            Value associated with the parameter. </param>
        /// <param name="paramDirection">   Parameter direction. </param>
        

        public DBParameter(string name, object value, ParameterDirection paramDirection)
        {
            this.name = name;
            this.value = value;
            this.paramDirection = paramDirection;
            
        }

        
        /// <summary>
        /// Creates a parameter with the name, value and Data type specified. Default direction is Input.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        /// DBParameter param=new DBParameter("FirstName","XXXX",DbType.AnsiString);
        /// }
        /// } 
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="name">     Parameter name. </param>
        /// <param name="value">    Value associated with the parameter. </param>
        /// <param name="dbType">   Data type. </param>
        ///
        /// ### <param name="dbProvider">   Data type. </param>
        

        public DBParameter(string name, object value, DbType dbType)
        {
            this.name = name;
            this.value = value;
            this.type = dbType;
            
        }

        
        /// <summary>
        /// Creates a parameter with the name, value, data type and direction specified.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        /// DBParameter param=new DBParameter("FirstName","XXXX",DbType.AnsiString,ParameterDirection.Input);
        /// }
        /// } 
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="name">             Parameter name. </param>
        /// <param name="value">            Value associated with the parameter. </param>
        /// <param name="dbType">           Data type. </param>
        /// <param name="paramDirection">   Parameter direction. </param>
        

        public DBParameter(string name, object value, DbType dbType, ParameterDirection paramDirection)
        {
            this.name = name;
            this.value = value;
            this.type = dbType;
            this.paramDirection = paramDirection;
        }
        #endregion

        #region "Public Properties"

        
        /// <summary>   Gets or sets the name of the parameter. </summary>
        ///
        /// <value> The name of the parameter. </value>
        

        public string ParameterName
        {
            get { return this.name; }
            set { this.name = value; }
        }

        
        /// <summary>   Gets or sets the value associated with the parameter. </summary>
        ///
        /// <value> The parameter value. </value>
        

        public object ParameterValue
        {
            get { return this.value; }
            set { this.value = value; }
        }

        
        /// <summary>   Gets or sets the type of the parameter. </summary>
        ///
        /// <value> The type. </value>
        

        public DbType Type
        {
            get { return this.type; }
            set { this.type = value; }

        }

        
        /// <summary>   Gets or sets the direction of the parameter. </summary>
        ///
        /// <value> The parameter direction. </value>
        

        public ParameterDirection ParamDirection
        {
            get
            {
                return this.paramDirection;
            }
            set
            {
                this.paramDirection = value;
            }
        }
        #endregion
    }

}
