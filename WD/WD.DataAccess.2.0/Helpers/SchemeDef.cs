// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 05-19-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="SchemeDef.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

using System.Text;




// namespace: WD.DataAccess.Helpers
//
// summary:	.


namespace WD.DataAccess.Helpers
{
        
        /// <summary>
        /// The item specification class is used to define the columns contained within the delimited
        /// file to be opened.  For each column we need to know the data type (and I am using the jet
        /// data types here), the column number, the column name, data type, and column width (if the
        /// file is delimited using fixed widths alone) - a list of item specification is added to the
        /// schema definition trailing this class.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        [Serializable]
        public class ItemSpecification
        {
            
          

            
            /// <summary>   Values that represent jet data types. </summary>
            /// this enumeration is used to
            /// limit the type data property to
            /// a matching jet data type
            /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
            

            public enum JetDataType
            {
                /// <summary>
                /// 
                /// </summary>
                Bit,
                /// <summary>
                /// 
                /// </summary>
                Byte,
                /// <summary>
                /// 
                /// </summary>
                Short,
                /// <summary>
                /// 
                /// </summary>
                Long,
                /// <summary>
                /// 
                /// </summary>
                Currency,
                /// <summary>
                /// 
                /// </summary>
                Single,
                /// <summary>
                /// 
                /// </summary>
                Double,
                /// <summary>
                /// 
                /// </summary>
                DateTime,
                /// <summary>
                /// 
                /// </summary>
                Text,
                /// <summary>
                /// 
                /// </summary>
                Memo
            };


            
            /// <summary>   Gets or sets the column number. </summary>
            // the position of the column beginning with 1 to n
            /// <value> The column number. </value>
            

            public long ColumnNumber { get; set; }

            
            /// <summary>   Gets or sets the name. </summary>
            ///  the column name
            /// <value> The name. </value>
            

            public string Name { get; set; }

            
            /// <summary>   Gets or sets information describing the type. </summary>
            /// the data type
            /// <value> Information describing the type. </value>
            

            public JetDataType TypeData { get; set; }

            
            /// <summary>   Gets or sets the width of the column. </summary>
            /// optional column width for fixed width files
            /// <value> The width of the column. </value>
            

            public int ColumnWidth { get; set; }
        }

        
        /// <summary>
        /// The schema definition class is used to hold the contents of the schema.ini file used by the
        /// connection to open a delimited file (using an oledb connection).  The schema dialog is used
        /// to define a schema definition which is stored as a application property.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        [Serializable]
        public class SchemeDef
        {
            
            /// <summary>
            /// the constructor will create a default comma delimited file definition with an empty list of
            /// items specifications and will default to set the first row is a header row option to false.
            /// </summary>
            ///
            /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
            

            public SchemeDef()
            {
                DelimiterType = DelimType.CsvDelimited;
                ColumnDefinition = new List<ItemSpecification>();
                UsesHeader = FirstRowHeader.No;
            }

            
            /// <summary>   Values that represent Delimiter types. </summary>
            /// this enumeration is used to limit the delimiter types
            /// to one of the four we are interested in which are
            /// comma delimited, tab delimited, custom delimited
            /// (such as a pipe or an underscore), or fixed column
            /// widths
            /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
            

            public enum DelimType
            {

                /// <summary>   An enum constant representing the CSV delimited option. </summary>
                CsvDelimited,

                /// <summary>   An enum constant representing the tab delimited option. </summary>
                TabDelimited,

                /// <summary>   An enum constant representing the custom delimited option. </summary>
                CustomDelimited,

                /// <summary>   An enum constant representing the fixed width option. </summary>
                FixedWidth
            };

            
            /// <summary>   Values that represent first row headers. </summary>
            ///
            /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
            

            public enum FirstRowHeader
            {

                /// <summary>   An enum constant representing the yes option. </summary>
                Yes,

                /// <summary>   An enum constant representing the no option. </summary>
                No
            };

            
            /// <summary>   Gets or sets the type of the delimiter. </summary>
            /// The properties used to build the schema.ini file include the
            /// delimiter type, a custom delimiter (if used), a list of
            /// column definitions, and a determination as to whether
            /// the first row of the file contains header information rather
            /// than data
            /// <value> The type of the delimiter. </value>
            

            public DelimType DelimiterType { get; set; }

            
            /// <summary>   Gets or sets the custom delimiter. </summary>
            ///
            /// <value> The custom delimiter. </value>
            

            public string CustomDelimiter { get; set; }

            
            /// <summary>   Gets or sets the column definition. </summary>
            ///
            /// <value> The column definition. </value>
            

            public List<ItemSpecification> ColumnDefinition { get; set; }

            
            /// <summary>   Gets or sets the uses header. </summary>
            ///
            /// <value> The uses header. </value>
            

            public FirstRowHeader UsesHeader { get; set; }
        }



}
