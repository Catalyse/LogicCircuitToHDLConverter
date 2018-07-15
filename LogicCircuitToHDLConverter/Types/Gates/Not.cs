using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Not : Gate, ICanWriteHDL
    {
        public static readonly int GateIdentifierMin = 0x20100;
        public static readonly int GateIdentifierMax = 0x20100;

        public Not(CircuitSymbol _symbol) : base(_symbol) { }
    }
}
