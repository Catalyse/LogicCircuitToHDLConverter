using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Nand : Gate
    {
        public static readonly int GateIdentifierMin = 0x40201;
        public static readonly int GateIdentifierMax = 0x41201;
        
        public Nand(CircuitSymbol _symbol) : base(_symbol)
        {
            HDLGateNotation = "Nand";
        }
    }
}
