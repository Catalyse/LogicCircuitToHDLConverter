using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Clock : Gate, ICanWriteHDL
    {
        public static readonly int GateIdentifierMin = 0x10000;
        public static readonly int GateIdentifierMax = 0x10000;

        public Clock(CircuitSymbol _symbol) : base(_symbol) { }
    }
}
