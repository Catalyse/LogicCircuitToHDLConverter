using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class CircuitSymbol
    {
        public string Id;
        public string CircuitId;//This is the circuit the symbol identifies and locates
        public string ParentId;
        public Coords Location;

        public CircuitSymbol(XmlNode node)
        {
            Location = new Coords();
            XmlNodeList children = node.ChildNodes;
            foreach (XmlNode child in children)
            {
                switch (child.Name)
                {
                    case "lc:CircuitSymbolId":
                        Id = child.InnerText;
                        break;
                    case "lc:CircuitId":
                        CircuitId = child.InnerText;
                        break;
                    case "lc:LogicalCircuitId":
                        ParentId = child.InnerText;
                        break;
                    case "lc:X":
                        Location.x = int.Parse(child.InnerText);
                        break;
                    case "lc:Y":
                        Location.y = int.Parse(child.InnerText);
                        break;
                    default:
                        Console.WriteLine("Wire Constructor: Unknown Element Type -- Ignoring");
                        break;
                }
            }
            if (Id == null || ParentId == null)
            {
                throw new Exception("CircuitSymbol Constructor: An invalid CircuitSymbol element has been detected in the save file, please check your project and try again!");
            }
        }
    }
}
