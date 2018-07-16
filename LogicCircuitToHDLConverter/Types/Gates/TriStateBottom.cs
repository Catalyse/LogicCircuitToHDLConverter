using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class TriStateBottom : Gate
    {
        public static readonly int GateIdentifierMin = 0xA0200;
        public static readonly int GateIdentifierMax = 0xA0200;

        public TriStateBottom(CircuitSymbol _symbol) : base(_symbol)
        {
            HDLGateNotation = "UNSUPPORTED";
        }
    }
}
