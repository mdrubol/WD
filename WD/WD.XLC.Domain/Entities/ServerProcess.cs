using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD.DataAccess.Attributes;

namespace WD.XLC.Domain.Entities
{
    [Custom(Name = "XLC_ServerProcess")]
    public class ServerProcess
    {
        public int ProcessId { get; set; }
        public string ServerId { get; set; }
        public string MachineId { get; set; }
    }
}
