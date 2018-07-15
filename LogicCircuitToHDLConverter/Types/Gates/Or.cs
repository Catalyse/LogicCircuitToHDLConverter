using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Or : Gate, ICanWriteHDL
    {
        public static readonly int GateIdentifierMin = 0x30200;
        public static readonly int GateIdentifierMax = 0x31200;

        public Or(CircuitSymbol _symbol) : base(_symbol) { }
    }
}
