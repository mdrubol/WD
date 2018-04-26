using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD.Domain.Abstract
{
    public interface IGenericCommands:ISelect,IInsert,IUpdate,IDelete
    {
    }
}
