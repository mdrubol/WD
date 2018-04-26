// ***********************************************************************
// Assembly         : WD.XLC.Domain
// Author           : shahid_k
// Created          : 04-27-2017
//
// Last Modified By : shahid_k
// Last Modified On : 06-08-2017
// ***********************************************************************
// <copyright file="Mapping.cs" company="Microsoft">
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
   
   /// <summary>    (Serializable) a mapping. </summary>
   ///
   /// <remarks>    Shahid K, 7/21/2017. </remarks>
   

   [Serializable]
   public class Mapping
    {
       /// <summary>    Default constructor. </summary>
       ///
       /// <remarks>    Shahid K, 7/21/2017. </remarks>
       public Mapping() {
           Columns = new List<ColumnConfig>();
       }
       public List<ColumnConfig> Columns { get; set; }
       public string Schema { get; set; }
    }
    
}
