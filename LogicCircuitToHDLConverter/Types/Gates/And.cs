using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class And : Gate
    {
        public static readonly int GateIdentifierMin = 0x40200;
        public static readonly int GateIdentifierMax = 0x41200;
        
        public And(CircuitSymbol _symbol) : base(_symbol)
        {
            HDLGateNotation = "And";
        }
    }
}
