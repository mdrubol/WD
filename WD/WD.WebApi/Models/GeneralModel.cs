using System;
using System.Collections.Generic;
using System.Linq;
using WD.DataAccess.Enums;

namespace WD.WebApi.Models
{
    public class GeneralModel<T>
        where T : class
    {
        public List<T> IList { get; set; }
        public int DbType { get; set; }
        public T Entity { get; set; }
    }
}