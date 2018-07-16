using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Nxor : Gate
    {
        public static readonly int GateIdentifierMin = 0x50201;
        public static readonly int GateIdentifierMax = 0x51201;

        public Nxor(CircuitSymbol _symbol) : base(_symbol)
        {
            HDLGateNotation = "Nxor";
        }
    }
}
