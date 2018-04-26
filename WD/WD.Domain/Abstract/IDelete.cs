using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD.Domain.Abstract
{
    public interface IDelete
    {
        int Delete_Generic_Table(string query, ref string errorNumber, ref string errorMessage);
        int Delete_Generic_Object(Type o, string where, ref string errorNumber, ref string errorMessage);
        int Delete_Generic_ObjectBySN(Type o, string SERIAL_NUMBER, ref string errorNumber, ref string errorMessage);
    }
}
