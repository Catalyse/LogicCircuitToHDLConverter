using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class TriStateTop : Gate
    {
        public static readonly int GateIdentifierMin = 0xB0200;
        public static readonly int GateIdentifierMax = 0xB0200;

        public TriStateTop(CircuitSymbol _symbol) : base(_symbol)
        {
            HDLGateNotation = "UNSUPPORTED";
        }
    }
}
