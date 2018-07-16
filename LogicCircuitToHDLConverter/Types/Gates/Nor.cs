using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Nor : Gate
    {
        public static readonly int GateIdentifierMin = 0x30201;
        public static readonly int GateIdentifierMax = 0x31201;

        public Nor(CircuitSymbol _symbol) : base(_symbol)
        {
            HDLGateNotation = "Nor";
        }
    }
}
