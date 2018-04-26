using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD.Domain.Abstract
{
    public interface IUpdate
    {
        int Update_Generic_Table(string query, ref string errorNumber, ref string errorMessage);
        int Update_Generic_Object(object o, string where, ref string errorNumber, ref string errorMessage);
        int Update_Generic_ObjectBySN(object o, string SERIAL_NUMBER, ref string errorNumber, ref string errorMessage);
    }
}
