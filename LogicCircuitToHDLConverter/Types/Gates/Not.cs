using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Not : Gate
    {
        public static readonly int GateIdentifierMin = 0x20100;
        public static readonly int GateIdentifierMax = 0x20100;

        public Not(CircuitSymbol _symbol) : base(_symbol)
        {
            HDLGateNotation = "Not";
        }
        
        public new string WriteGateHDL(List<string> inputs, string outputName)
        {
            if (inputs.Count == 1)
            {
                return "\t" + HDLGateNotation + "(in=" + inputs[0] + ", out=" + outputName + ");";
            }
            else
            {
                throw new Exception("Invalid Input Count Error: " + inputs.Count + " inputs were provided but the gate reports a maximum size of 1");
            }
        }
    }
}
