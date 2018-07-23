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
        public List<string> outputNames;

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
            outputNames = circuit.outputNames;
        }

        public LogicalCircuit(XmlNode node)
        {
            circuits = new List<CircuitBase>();
            wireGroups = new List<WireGroup>();
            gates = new List<Gate>();
            wires = new List<Wire>();
            locationSymbols = new List<CircuitSymbol>();
            outputNames = new List<string>();

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

        public struct CircuitPinOffset
        {
            public int OffsetX;
            public int OffsetY;
            public int SplitSpace;
        }

        public CircuitPinOffset OffsetCalculator(PinSide type)
        {
            int left = 0, top = 0, right = 0, bottom = 0;
            foreach(var circuitBase in circuits)
            {
                if(circuitBase.GetType() == typeof(Pin))
                {
                    Pin pin = (Pin)circuitBase;
                    switch(pin.Side)
                    {
                        case PinSide.Left:
                            left++;
                            break;
                        case PinSide.Top:
                            top++;
                            break;
                        case PinSide.Right:
                            right++;
                            break;
                        case PinSide.Bottom:
                            bottom++;
                            break;
                    }
                }
            }

            CircuitPinOffset offset = new CircuitPinOffset();

            switch (type)
            {
                case PinSide.Left:
                    offset.OffsetX = 0;
                    if(left == 1)
                    {
                        if(right <= 3)
                        {
                            offset.OffsetY = 2;
                        }
                        else
                        {
                            if (right % 2 != 0)
                            {
                                offset.OffsetY = right / 2;
                            }
                            else
                            {
                                offset.OffsetY = (right / 2) + 1;
                            }
                        }
                    }
                    else
                    {
                        offset.OffsetY = 1;
                        if(right <= 1)
                        {
                            offset.SplitSpace = 1;
                        }
                        else
                        {
                            int spacesize = left - 1;
                            int calc = right - left;
                            if(calc <= 0)
                            {
                                offset.SplitSpace = 0;
                            }
                            else
                            {
                                offset.SplitSpace = calc / spacesize;
                            }
                        }
                    }
                    break;
                case PinSide.Top:
                    offset.OffsetY = 0;
                    if (top == 1)
                    {
                        if (bottom % 2 != 0)
                        {
                            offset.OffsetX = bottom / 2;
                        }
                        else
                        {
                            offset.OffsetX = (bottom / 2) + 1;
                        }
                    }
                    else
                    {
                        offset.OffsetY = 1;
                        if (bottom <= 1)
                        {
                            offset.SplitSpace = 0;
                        }
                        else
                        {
                            int spacesize = top - 1;
                            int calc = bottom - top;
                            if(calc <= 0)
                            {
                                offset.SplitSpace = 0;
                            }
                            else
                            {
                                offset.SplitSpace = calc / spacesize;
                            }
                        }
                    }
                    break;
                case PinSide.Right:
                    offset.OffsetX = Math.Max(Math.Max(top, bottom) + 1, 3);
                    if (right == 1)
                    {
                        if(left <= 3)
                        {
                            offset.OffsetY = 2;
                        }
                        else
                        {
                            if (left % 2 != 0)
                            {
                                offset.OffsetY = left / 2;
                            }
                            else
                            {
                                offset.OffsetY = (left / 2) + 1;
                            }
                        }
                    }
                    else
                    {
                        offset.OffsetY = 1;
                        if (left <= 1)
                        {
                            offset.SplitSpace = 1;
                        }
                        else
                        {
                            int spacesize = left - 1;
                            int calc = left - right;
                            if (calc <= 0)
                            {
                                offset.SplitSpace = 0;
                            }
                            else
                            {
                                offset.SplitSpace = calc / spacesize;
                            }
                        }
                    }
                    break;
                case PinSide.Bottom:
                    offset.OffsetY = Math.Max(Math.Max(left, right) + 1, 4);
                    if (top == 1)
                    {
                        if (bottom % 2 != 0)
                        {
                            offset.OffsetX = bottom / 2;
                        }
                        else
                        {
                            offset.OffsetX = (bottom / 2) + 1;
                        }
                    }
                    else
                    {
                        offset.OffsetY = 1;
                        if (bottom <= 1)
                        {
                            offset.SplitSpace = 0;
                        }
                        else
                        {
                            int spacesize = top - 1;
                            int calc = bottom - top;
                            if (calc <= 0)
                            {
                                offset.SplitSpace = 0;
                            }
                            else
                            {
                                offset.SplitSpace = calc / spacesize;
                            }
                        }
                    }
                    break;
            }
            return offset;
        }

        /// <summary>
        /// This method is used if this logicalcircuit is used in ANOTHER logical circuit
        /// </summary>
        /// <returns></returns>
        public string WriteCircuitHDL(List<WireGroup> externalWireGroups)
        {
            string hdlOutput = Notation + "(";
            bool firstInput = true;

            Dictionary<string, string> leftMap = FindConnectedGroups(PinSide.Left, OffsetCalculator(PinSide.Left), externalWireGroups);
            Dictionary<string, string> topMap = FindConnectedGroups(PinSide.Top, OffsetCalculator(PinSide.Top), externalWireGroups);
            Dictionary<string, string> rightMap = FindConnectedGroups(PinSide.Right, OffsetCalculator(PinSide.Right), externalWireGroups);
            Dictionary<string, string> bottomMap = FindConnectedGroups(PinSide.Bottom, OffsetCalculator(PinSide.Bottom), externalWireGroups);

            if(leftMap.Count > 0)
            {
                for (int i = 0; i < leftMap.Count; i++)
                {
                    if(firstInput)
                    {
                        firstInput = false;
                        hdlOutput += leftMap.Keys.ElementAt(i) + "=" + leftMap.Values.ElementAt(i);
                    }
                    else
                    {
                        hdlOutput += "," + leftMap.Keys.ElementAt(i) + "=" + leftMap.Values.ElementAt(i);
                    }
                }
            }
            if (topMap.Count > 0)
            {
                for (int i = 0; i < topMap.Count; i++)
                {
                    if (firstInput)
                    {
                        firstInput = false;
                        hdlOutput += topMap.Keys.ElementAt(i) + "=" + topMap.Values.ElementAt(i);
                    }
                    else
                    {
                        hdlOutput += "," + topMap.Keys.ElementAt(i) + "=" + topMap.Values.ElementAt(i);
                    }
                }
            }
            if (rightMap.Count > 0)
            {
                for (int i = 0; i < rightMap.Count; i++)
                {
                    if (firstInput)
                    {
                        firstInput = false;
                        hdlOutput += rightMap.Keys.ElementAt(i) + "=" + rightMap.Values.ElementAt(i);
                    }
                    else
                    {
                        hdlOutput += "," + rightMap.Keys.ElementAt(i) + "=" + rightMap.Values.ElementAt(i);
                    }
                }
            }
            if (bottomMap.Count > 0)
            {
                for (int i = 0; i < bottomMap.Count; i++)
                {
                    if (firstInput)
                    {
                        firstInput = false;
                        hdlOutput += bottomMap.Keys.ElementAt(i) + "=" + bottomMap.Values.ElementAt(i);
                    }
                    else
                    {
                        hdlOutput += "," + bottomMap.Keys.ElementAt(i) + "=" + bottomMap.Values.ElementAt(i);
                    }
                }
            }

            hdlOutput += ");" + Environment.NewLine;

            return hdlOutput;
        }

        private Dictionary<string, string> FindConnectedGroups(PinSide side, CircuitPinOffset offset, List<WireGroup> groups)
        {
            List<Pin> pins = new List<Pin>();
            foreach (var circuitBase in circuits)
            {
                if (circuitBase.GetType() == typeof(Pin))
                {
                    Pin pin = (Pin)circuitBase;
                    if(pin.Side == side)
                    {
                        pins.Add(pin);
                    }
                }
            }

            List<Pin> sortedPins = new List<Pin>();
            sortedPins.Add(pins[0]);
            bool foundLocation = false;
            if(pins.Count > 1)
            {
                for (int i = 1; i < pins.Count; i++)
                {
                    foundLocation = false;
                    for(int j = 0; j < sortedPins.Count; j++)
                    {
                        if(pins[i].Symbol.Location.y < sortedPins[j].Symbol.Location.y)
                        {
                            sortedPins.Insert(j, pins[i]);
                            foundLocation = true;
                            break;
                        }
                        else if(pins[i].Symbol.Location.y == sortedPins[j].Symbol.Location.y)
                        {
                            if(pins[i].Symbol.Location.x < sortedPins[j].Symbol.Location.x)
                            {
                                sortedPins.Insert(j, pins[i]);
                                foundLocation = true;
                                break;
                            }
                        }
                    }
                    if (!foundLocation)
                    {
                        sortedPins.Add(pins[i]);
                    }
                }
            }

            Dictionary<string, string> returnDictionary = new Dictionary<string, string>();
            for (int i = 0; i < sortedPins.Count; i++)
            {
                Coords current = new Coords(Symbol.Location);
                if(side == PinSide.Left || side == PinSide.Right)
                {
                    current.x = current.x + offset.OffsetX;
                    current.y = current.y + offset.OffsetY + (i * offset.SplitSpace);
                }
                else
                {
                    current.x = current.x + offset.OffsetX + (i * offset.SplitSpace);
                    current.y = current.y + offset.OffsetY;
                }

                foreach (var group in groups)
                {
                    if (group.coords.Exists(x => x.x == current.x && x.y == current.y))
                    {
                        returnDictionary.Add(sortedPins[i].Name, group.inputList[0]);
                    }
                }
            }
            if (returnDictionary.Count != sortedPins.Count)
            {
                throw new Exception("Gate -- FindConnectedGroups: Unable to match a wire group to each input!");
            }
            return returnDictionary;
        }
    }
}
