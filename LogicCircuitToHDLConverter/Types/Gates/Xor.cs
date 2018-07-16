using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Xor : Gate
    {
        public static readonly int GateIdentifierMin = 0x50200;
        public static readonly int GateIdentifierMax = 0x51200;

        public Xor(CircuitSymbol _symbol) : base(_symbol)
        {
            HDLGateNotation = "Xor";
        }
    }
}
