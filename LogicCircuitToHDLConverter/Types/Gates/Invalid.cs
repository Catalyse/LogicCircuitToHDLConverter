using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Invalid : Gate
    {
        public Invalid(CircuitSymbol _symbol) : base(_symbol) { }
    }
}
