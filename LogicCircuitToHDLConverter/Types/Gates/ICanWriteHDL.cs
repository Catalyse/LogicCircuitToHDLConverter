using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuitToHDLConverter
{
    public interface ICanWriteHDL
    {
        string WriteGateHDL();
    }
}
