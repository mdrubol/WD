// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 02-24-2017
// ***********************************************************************
// <copyright file="CustomAttribute.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Runtime.CompilerServices;



// namespace: WD.DataAccess.Attributes
//
// summary:	.


namespace WD.DataAccess.Attributes
{
    
    /// <summary>   Attribute for custom. This class cannot be inherited. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public sealed class CustomAttribute : Attribute
    {
       
       /// <summary>    Constructor. </summary>
       ///
       /// <remarks>    Shahid Kochak, 7/20/2017. </remarks>
       ///
       /// <param name="order"> (Optional) </param>
       

       public CustomAttribute([CallerLineNumber] int order = 0) 
       { 
           Order = order; 
       } 

       
       /// <summary>    Gets or sets the order. </summary>
       ///
       /// <value>  The order. </value>
       

       public int Order { get; private set; } 

       
       /// <summary>    Gets or sets a value indicating whether this object is primary. </summary>
       ///
       /// <value>  True if this object is primary, false if not. </value>
       

       public bool IsPrimary { get; set; } 

        
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        

        public string Name { get; set; }

        
        /// <summary>   Gets or sets the type of the data. </summary>
        ///
        /// <value> The type of the data. </value>
        

        public object DataType { get; set; }

        
        /// <summary>   Gets or sets a value indicating whether this object is read only. </summary>
        ///
        /// <value> True if this object is read only, false if not. </value>
        

        public bool IsReadOnly { get; set; }
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size { get; set; }

        public bool NotMapped { get; set; }

    }
}