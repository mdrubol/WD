// ***********************************************************************
// Assembly         : WD.XLC.Domain
// Author           : shahid_k
// Created          : 04-27-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-21-2017
// ***********************************************************************
// <copyright file="ColumnConfig.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



// namespace: WD.XLC.Domain.Entities
//
// summary:	.


namespace WD.XLC.Domain.Entities
{
    
    /// <summary>   (Serializable) a column configuration. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>
    

    [Serializable]
    public class ColumnConfig
    {
        
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        

        public ColumnConfig() { }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="index">    Zero-based index of the. </param>
        

        public ColumnConfig(int index) { 
        
            this.ColumnName = "F" + (index).ToString();
            this.Index = index;
            this.DataType = "string";
            this.Length = "0";
            this.Format = string.Empty;
            this.FieldName = string.Empty;
            this.DefaultValue = string.Empty;
            this.IsPrimary = false;
        }

        
        /// <summary>   Gets or sets the zero-based index of this object. </summary>
        ///
        /// <value> The index. </value>
        

        public int Index { get; set; }

        
        /// <summary>   Gets or sets the name of the column. </summary>
        ///
        /// <value> The name of the column. </value>
        

        public string ColumnName { get; set; }

        
        /// <summary>   Gets or sets the type of the data. </summary>
        ///
        /// <value> The type of the data. </value>
        

        public string DataType { get; set; }

        
        /// <summary>   Gets or sets the default value. </summary>
        ///
        /// <value> The default value. </value>
        

        public string DefaultValue { get; set; }

        
        /// <summary>   Gets or sets the name of the field. </summary>
        ///
        /// <value> The name of the field. </value>
        

        public string FieldName { get; set; }

        
        /// <summary>   Gets or sets the format to use. </summary>
        ///
        /// <value> The format. </value>
        

        public string Format { get; set; }

        
        /// <summary>   Gets or sets the length. </summary>
        ///
        /// <value> The length. </value>
        

        public string Length { get; set; }

        
        /// <summary>   Gets or sets a value indicating whether this object is primary. </summary>
        ///
        /// <value> True if this object is primary, false if not. </value>
        

        public bool IsPrimary { get; set; }

        
        /// <summary>   Gets the length. </summary>
        ///
        /// <value> The i length. </value>
        

        public virtual int iLength
        {
            get { return string.IsNullOrEmpty(Length) ? 0 : Convert.ToInt32(Length); }
        }
    }
}
