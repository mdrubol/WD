// ***********************************************************************
// Assembly         : WD.XLC.Domain
// Author           : shahid_k
// Created          : 05-18-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="CustomException.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;



// namespace: WD.XLC.Domain.Helpers
//
// summary:	.


namespace WD.XLC.Domain.Helpers
{
    
    /// <summary>   (Serializable) exception for signalling custom errors. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>
    

    [Serializable]
    public class CustomException : Exception
    {
        
        /// <summary>   Just create the exception. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        

        public CustomException()
            : base()
        {
        }

        
        /// <summary>   Create the exception with description. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="message">  Exception description. </param>
        

        public CustomException(String message)
            : base(message)
        {
        }

        
        /// <summary>   Create the exception with description and inner cause. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="message">          Exception description. </param>
        /// <param name="innerException">   Exception inner cause. </param>
        

        public CustomException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        
        /// <summary>
        /// Create the exception from serialized data. Usual scenario is when exception is occured
        /// somewhere on the remote workstation and we have to re-create/re-throw the exception on the
        /// local machine.
        /// </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="info">     Serialization info. </param>
        /// <param name="context">  Serialization context. </param>
        

        protected CustomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
