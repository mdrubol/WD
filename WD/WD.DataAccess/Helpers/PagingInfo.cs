// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 02-22-2017
//
// Last Modified By : shahid_k
// Last Modified On : 03-02-2017
// ***********************************************************************
// <copyright file="PagingInfo.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;



// namespace: WD.DataAccess.Helpers
//
// summary:	.


namespace WD.DataAccess.Helpers
{
    
    /// <summary>   Information about the paging. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    internal class PagingInfo
    {
        
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public PagingInfo() { }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="pageIndex">        . </param>
        /// <param name="pageSize">         . </param>
        /// <param name="totalItemCount">   . </param>
        

        public PagingInfo(int pageIndex, int pageSize, int totalItemCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
        }

        
        /// <summary>   Gets or sets the zero-based index of the page. </summary>
        ///
        /// <value> The page index. </value>
        

        public int PageIndex { get; set; }

        
        /// <summary>   Gets or sets the size of the page. </summary>
        ///
        /// <value> The size of the page. </value>
        

        public int PageSize { get; set; }

        
        /// <summary>   Gets or sets the number of total items. </summary>
        ///
        /// <value> The total number of item count. </value>
        

        public int TotalItemCount { get; set; }

        
        /// <summary>   Gets the number of total pages. </summary>
        ///
        /// <value> The total number of page count. </value>
        

        public int TotalPageCount
        {
            get
            {
                return (int)Math.Ceiling((double)TotalItemCount / (double)PageSize);
            }
        }
    }
}
