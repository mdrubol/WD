using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD.Domain.Abstract
{
    public interface IInsert
    {
        int Insert_Generic_Table(string query, ref string errorNumber, ref string errorMessage);
        int Insert_Generic_Object(object o, ref string errorNumber, ref string errorMessage);
    }
}
