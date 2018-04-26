// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 04-11-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="DataRowMapper.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WD.DataAccess.Attributes;



// namespace: WD.DataAccess.Helpers
//
// summary:	.


namespace WD.DataAccess.Helpers
{
    
    /// <summary>   A data row mapper. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    ///
    /// ### <typeparam name="T">    . </typeparam>
    

    public class DataRowMapper : IDisposable
    {
         /// <summary>  The mappings. </summary>
         Dictionary<string, PropertyInfo> mappings;
         /// <summary>  An object to process. </summary>
         object T;

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="reader">   . </param>
        /// <param name="T">        . </param>
        

        public DataRowMapper(IDataReader reader, object T)
        {
            this.T = T;
            this.mappings = Mappings(reader, T);
        }

        
        /// <summary>   Mappings. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="reader">   . </param>
        /// <param name="T">        . </param>
        ///
        /// <returns>   A Dictionary&lt;string,PropertyInfo&gt; </returns>
        

        static Dictionary<string, PropertyInfo> Mappings(IDataReader reader, object T)
        {

            var columns = Enumerable.Range(0, reader.FieldCount);
            var properties = T.GetType()
                            .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetMethod.IsVirtual == false)
                            .Select(prop => new
                            {
                                prop,
                                attr = prop.GetCustomAttributes(typeof(CustomAttribute)).FirstOrDefault()
                            })
                            .Select(x => new
                            {
                                name = x.attr == null ? x.prop.Name : ((CustomAttribute)x.attr).Name,
                                x.prop
                            });
            return columns
                  .Join(properties, reader.GetName, x => x.name, (index, x) => new
                  {
                      x.name,
                      prop = !x.prop.CanWrite ? null : x.prop
                  }, StringComparer.InvariantCultureIgnoreCase)
                 .Where(x => x.prop != null) // only settable properties accounted for
                 .ToDictionary(x => x.name, x => x.prop);
        }

        
        /// <summary>   Map from. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="record">   . </param>
        ///
        /// <returns>   An object. </returns>
        

        public object MapFrom(IDataRecord record)
        {
            var element = Activator.CreateInstance(T.GetType());
            foreach (var map in mappings)
                map.Value.SetValue(element, ChangeType(record[map.Key], map.Value.PropertyType));
            return element;
        }

        
        /// <summary>   Change type. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="value">        . </param>
        /// <param name="targetType">   . </param>
        ///
        /// <returns>   An object. </returns>
        

        static object ChangeType(object value, Type targetType)
        {
            object result = null;
            if (value == null || value == DBNull.Value)
                return result;
            try
            {
                result = Convert.ChangeType(value, Nullable.GetUnderlyingType(targetType) ?? targetType);

            }
            catch
            {
                result = null;
            }
            return result;
        }

        
        /// <summary>   Finaliser. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        ~DataRowMapper()
        {
        Dispose(false);
        }

        
        /// <summary>
        /// Releases the unmanaged resources used by the WD.DataAccess.Helpers.DataRowMapper and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="disposing">    True to release both managed and unmanaged resources; false to
        ///                             release only unmanaged resources. </param>
        

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (mappings != null)
                    {
                        mappings = null;
                    }
                }

                disposed = true;
            }
        }

        /// <summary>   to detect redundant calls. </summary>
        private bool disposed = false;

        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
