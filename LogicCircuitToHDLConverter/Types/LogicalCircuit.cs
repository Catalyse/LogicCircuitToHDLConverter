using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class LogicalCircuit : CircuitBase
    {
        public string Name;
        public string Notation;
        public List<CircuitSymbol> locationSymbols;
        public List<CircuitBase> circuits;
        public List<WireGroup> wireGroups;
        public List<Wire> wires;
        public List<Gate> gates;

        public LogicalCircuit(LogicalCircuit circuit)
        {
            Name = circuit.Name;
            Notation = circuit.Notation;
            Id = circuit.Id;
            ParentId = circuit.Id;
            locationSymbols = circuit.locationSymbols;
            circuits = circuit.circuits;
            wireGroups = circuit.wireGroups;
            wires = circuit.wires;
            gates = circuit.gates;
        }

        public LogicalCircuit(XmlNode node)
        {
            circuits = new List<CircuitBase>();
            wireGroups = new List<WireGroup>();
            gates = new List<Gate>();
            wires = new List<Wire>();
            locationSymbols = new List<CircuitSymbol>();

            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.Name)
                {
                    case "lc:LogicalCircuitId":
                        Id = child.InnerText;
                        break;
                    case "lc:Name":
                        Name = child.InnerText;
                        break;
                    case "lc:Notation":
                        Notation = child.InnerText;
                        break;
                    default:
                        Console.WriteLine("Circuit Constructor: Unknown Element Type -- Ignoring");
                        break;
                }
            }

            if (Name == null || Id == null || Notation == null)
            {
                throw new Exception("Circuit Constructor: An invalid LogicalCircuit element has been detected in the save file, please check your project and try again!");
            }
        }
    }
}
