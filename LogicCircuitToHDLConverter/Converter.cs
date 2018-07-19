using System;
using System.Collections.Generic;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Converter
    {
        private Dictionary<int, string> GateMap = new Dictionary<int, string>
        {
            { 0x10000, "CLOCK" },
            { 0x20101, "NOT" },
            { 0x30200, "OR" },
            { 0x30201, "NOR" },
            { 0x40200, "AND" },
            { 0x40201, "NAND" },
            { 0x50200, "XOR" },
            { 0x50201, "NXOR" },
            { 0x80100, "LED" },
            { 0x80800, "8SEGDISPLAY" },
            { 0xa0200, "TRISTATE(BOTTOM)" },
            { 0xb0200, "TRISTATE(TOP)" }
        };

        public static Dictionary<string, Type> TypeMap = new Dictionary<string, Type>
        {
            { "LogicalCircuit" , typeof(LogicalCircuit) },
            { "CircuitButton" , typeof(CircuitButton) },
            { "CircuitProbe" , typeof(CircuitProbe) },
            { "CircuitSymbol" , typeof(CircuitSymbol) },
            { "Constant" , typeof(Constant) },
            { "GraphicsArray" , typeof(GraphicsArray) },
            { "LedMatrix" , typeof(LedMatrix) },
            { "Memory" , typeof(Memory) },
            { "Pin" , typeof(Pin) },
            { "Sensor" , typeof(Sensor) },
            { "Sound" , typeof(Sound) },
            { "Splitter" , typeof(Splitter) },
            { "Wire" , typeof(Wire) }
        };
        
        public static List<LogicalCircuit> ParseXMLDocument(string docLocation)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(docLocation);
            //doc.Load(@"C:/Users/Catalyse/Desktop/TestProject.CircuitProject");

            XmlNode primaryNode = doc.LastChild;

            XmlNodeList nodes = primaryNode.ChildNodes;

            List<CircuitBase> circuits = new List<CircuitBase>();
            List<Gate> gates = new List<Gate>();
            List<CircuitSymbol> symbols = new List<CircuitSymbol>();
            List<Wire> wires = new List<Wire>();
            List<LogicalCircuit> logicalCircuits = new List<LogicalCircuit>();

            for (int i = 0; i < nodes.Count; i++)
            {
                Type objectType = IdentifyObject(nodes[i]);
                if(objectType != null)
                {
                    if (objectType == typeof(LogicalCircuit))
                    {
                        logicalCircuits.Add(new LogicalCircuit(nodes[i]));
                    }
                    else if (objectType.IsSubclassOf(typeof(CircuitBase)))
                    {
                        circuits.Add(ConstructCircuitBaseType(nodes[i]));
                    }
                    else if (objectType == typeof(Gate))
                    {
                        gates.Add(ConstructGateType(nodes[i]));
                    }
                    else if (objectType == typeof(CircuitSymbol))
                    {
                        symbols.Add(new CircuitSymbol(nodes[i]));
                    }
                    else if (objectType == typeof(Wire))
                    {
                        wires.Add(new Wire(nodes[i]));
                    }
                }
            }
            logicalCircuits = SortByLogicalCircuit(circuits, gates, wires, logicalCircuits);
            
            foreach(var group in logicalCircuits)
            {
                circuits = ConnectSymbolsToCircuits(group.circuits, symbols, group);
                if(group.wires.Count > 0)
                {
                    group.wireGroups = CombineWires(group.wires);
                }
            }

            logicalCircuits = CheckForCircuitLink(logicalCircuits);

            for(int i = 0; i < logicalCircuits.Count; i++)
            {
                logicalCircuits[i] = FindWireGroupInputs(logicalCircuits[i]);
            }

            Console.WriteLine("Circuit Count: " + circuits.Count);
            Console.WriteLine("Gate Count: " + gates.Count);
            Console.WriteLine("Symbol Count:" + symbols.Count);

            return logicalCircuits;
        }

        /// <summary>
        /// This method iterates through a list of established output names to make sure there are no duplicates.
        /// If there are duplicates it will come up with a new name and return it.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="circuit"></param>
        /// <returns>A valid name</returns>
        public static string CheckUnusedName(string name, LogicalCircuit circuit)
        {
            if (!circuit.outputNames.Contains(name))
            {
                return name;
            }
            else
            {
                var i = 1;
                string newName;
                while (true)
                {
                    newName = name + i;
                    if (!circuit.outputNames.Contains(newName))
                    {
                        circuit.outputNames.Add(newName);
                        return newName;
                    }
                    i++;
                }
            }
        }

        public static LogicalCircuit FindWireGroupInputs(LogicalCircuit circuit)
        {
            foreach(var pin in circuit.circuits)
            {
                if(pin.GetType() == typeof(Pin))
                {
                    Pin temp = (Pin)pin;
                    Coords pinLocation = new Coords(temp.Symbol.Location);
                    if(temp.Type == PinType.Input)
                    {
                        pinLocation.x = pinLocation.x + temp.rightOffset.OffsetX;
                        pinLocation.y = pinLocation.y + temp.rightOffset.OffsetY;
                        for (int i = 0; i < circuit.wireGroups.Count; i++)
                        {
                            if (circuit.wireGroups[i].coords.Exists(x => x.x == pinLocation.x && x.y == pinLocation.y))
                            {
                                circuit.wireGroups[i].inputList.Add(temp.Name);
                                circuit.outputNames.Add(temp.Name);
                                break;
                            }
                        }
                    }
                    else if(temp.Type == PinType.Output)
                    {
                        pinLocation.x = pinLocation.x + temp.leftOffset.OffsetX;
                        pinLocation.y = pinLocation.y + temp.leftOffset.OffsetY;
                        for (int i = 0; i < circuit.wireGroups.Count; i++)
                        {
                            if (circuit.wireGroups[i].coords.Exists(x => x.x == pinLocation.x && x.y == pinLocation.y))
                            {
                                circuit.wireGroups[i].outputList.Add(temp.Name);
                                circuit.outputNames.Add(temp.Name);
                                break;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            foreach (var gate in circuit.gates)
            {
                Coords pinLocation = new Coords(gate.Symbol.Location);
                pinLocation.x = pinLocation.x + gate.RightPinOffset[gate.GetSize()].OffsetX;
                pinLocation.y = pinLocation.y + gate.RightPinOffset[gate.GetSize()].OffsetY;

                for (int i = 0; i < circuit.wireGroups.Count; i++)
                {
                    if (circuit.wireGroups[i].coords.Exists(x => x.x == pinLocation.x && x.y == pinLocation.y))
                    {
                        var name = CheckUnusedName(gate.HDLGateNotation, circuit);
                        gate.OutputName = name;
                        circuit.outputNames.Add(name);
                        circuit.wireGroups[i].inputList.Add(name);
                        break;
                    }
                }
            }
            foreach(var group in circuit.wireGroups)
            {
                if(group.inputList.Count > 1)
                {
                    throw new Exception("WireGroup Source Count Exception: There cannot be more than one data source on a wire group!");
                }
            }
            return circuit;
        }

        /// <summary>
        /// This method sorts all the instantiated parts into their appropriate logical circuit.
        /// This allows us to essnetially organize parts by where they will exist in hdl, and helps keep the processing time lower on 
        /// some of the more complicated connecting functions
        /// </summary>
        /// <param name="circuits">A list of all circuits in the project</param>
        /// <param name="gates">A list of all gates in the project</param>
        /// <param name="wires">A list of all ungrouped wires in the project</param>
        /// <param name="logicalCircuits">A list of all top level logical circuits</param>
        /// <returns>The logical circuits with all parts sorted appropriately</returns>
        public static List<LogicalCircuit> SortByLogicalCircuit(List<CircuitBase> circuits, List<Gate> gates, List<Wire> wires, List<LogicalCircuit> logicalCircuits)
        {
            Dictionary<string, LogicalCircuit> localMap = new Dictionary<string, LogicalCircuit>();
            foreach(var circuit in logicalCircuits)
            {
                localMap.Add(circuit.Id, circuit);
            }
            foreach(var circuit in circuits)
            {
                localMap[circuit.ParentId].circuits.Add(circuit);
            }
            foreach(var gate in gates)
            {
                localMap[gate.Symbol.ParentId].gates.Add(gate);
            }
            foreach(var wire in wires)
            {
                localMap[wire.ParentId].wires.Add(wire);
            }
            List<LogicalCircuit> returnList = new List<LogicalCircuit>();
            foreach(var circuit in localMap)
            {
                returnList.Add(circuit.Value);
            }
            return returnList;
        }

        /// <summary>
        /// Logical circuits can exist within other logical circuit maps, so we must go back after everything is sorted and check for links.
        /// Logical circuits are the only object that can have more than one circuitsymbol, and thus are duplicated when multiple are found.
        /// Each of these duplicates will have a unique circuitsymbol assigned in the .Symbol property.
        /// </summary>
        /// <param name="logicalCircuits">The list of all logical circuits in the project</param>
        /// <returns>The same list of logical circuits, but with them mapped to eachother with the appropriate circuitsymbol</returns>
        private static List<LogicalCircuit> CheckForCircuitLink(List<LogicalCircuit> logicalCircuits)
        {
            Dictionary<string, LogicalCircuit> localMap = new Dictionary<string, LogicalCircuit>();
            foreach (var circuit in logicalCircuits)
            {
                localMap.Add(circuit.Id, circuit);
            }
            foreach(var circuit in logicalCircuits)
            {
                foreach(var symbol in circuit.locationSymbols)//Every symbol against a logical circuit implies it exists within another circuit.
                {//If a logicalcircuit has no symbols then it is not referenced in other logical circuits
                    var newCircuit = new LogicalCircuit(circuit);
                    newCircuit.Symbol = symbol;
                    localMap[symbol.ParentId].circuits.Add(newCircuit);
                }
            }
            List<LogicalCircuit> returnList = new List<LogicalCircuit>();
            foreach (var circuit in localMap)
            {
                returnList.Add(circuit.Value);
            }
            return returnList;
        }

        /// <summary>
        /// This function maps circuitsymbols to their associated circuit.
        /// If a symbol does not have an associated circuit it is a gate.
        /// </summary>
        /// <param name="circuits">All circuits in a group</param>
        /// <param name="symbols">All symbols</param>
        /// <param name="parent">The logical circuit parent of the circuits.</param>
        /// <returns></returns>
        public static List<CircuitBase> ConnectSymbolsToCircuits(List<CircuitBase> circuits, List<CircuitSymbol> symbols, LogicalCircuit parent)
        {
            foreach(var symbol in symbols)
            {
                int index = circuits.FindIndex(x => x.Id == symbol.CircuitId);
                if (index != -1)
                {
                    circuits[index].Symbol = symbol;
                }
                else if(symbol.CircuitId == parent.Id)
                {
                    parent.locationSymbols.Add(symbol);
                }
            }
            return circuits;
        }

        /// <summary>
        /// This method identifies the type of object the xmlnode represents
        /// </summary>
        /// <param name="obj">The xml node you want to identify</param>
        /// <returns>The type of the xml node</returns>
        public static Type IdentifyObject(XmlNode obj)
        {
            Type objType;
            try
            {
                objType = TypeMap[obj.LocalName];
            }
            catch (KeyNotFoundException)
            {
                objType = null;
            }
            if (objType != null)
            {
                if(objType == typeof(CircuitSymbol))
                {
                    for(int i = 0; i < obj.ChildNodes.Count; i++)
                    {
                        if(obj.ChildNodes[i].LocalName == "CircuitId")
                        {
                            if (obj.ChildNodes[i].InnerText.Contains("00000000-0000-0000-0000-0000000"))
                            {
                                return typeof(Gate);
                            }
                        }
                    }
                    return objType;
                }
                else
                {
                    return objType;
                }
            }
            else return null;
        }

        /// <summary>
        /// This method identifies the type of gate the node represents and returns a constructed version of it.
        /// </summary>
        /// <param name="obj">The xml node you want built</param>
        /// <returns>A constructed gate type of the node</returns>
        public static Gate ConstructGateType(XmlNode obj)
        {
            for (int i = 0; i < obj.ChildNodes.Count; i++)
            {
                if (obj.ChildNodes[i].LocalName == "CircuitId")
                {
                    var identifier = obj.ChildNodes[i].InnerText.Substring(31);
                    switch (identifier.Substring(0, 1))
                    {
                        case "1":
                            return new Clock(new CircuitSymbol(obj));
                        case "2":
                            return new Not(new CircuitSymbol(obj));
                        case "3":
                            if (identifier.Substring(4) == "0")
                            {
                                return new Or(new CircuitSymbol(obj));
                            }
                            else
                            {
                                return new Nor(new CircuitSymbol(obj));
                            }
                        case "4":
                            if (identifier.Substring(4) == "0")
                            {
                                return new And(new CircuitSymbol(obj));
                            }
                            else
                            {
                                return new Nand(new CircuitSymbol(obj));
                            }
                        case "5":
                            if (identifier.Substring(4) == "0")
                            {
                                return new Xor(new CircuitSymbol(obj));
                            }
                            else
                            {
                                return new Nxor(new CircuitSymbol(obj));
                            }
                        case "8":
                            return new Invalid(new CircuitSymbol(obj));
                        case "a":
                        case "A":
                            return new TriStateBottom(new CircuitSymbol(obj));
                        case "b":
                        case "B":
                            return new TriStateTop(new CircuitSymbol(obj));
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// This method identifies the specific type of the node and returns a contructed object of that type
        /// </summary>
        /// <param name="obj">The node you want to build into an object</param>
        /// <returns>A constructed object of the node type</returns>
        public static CircuitBase ConstructCircuitBaseType(XmlNode obj)
        {
            Type temp = TypeMap[obj.LocalName];
            var newObj = Activator.CreateInstance(temp, obj);
            if (newObj.GetType().IsSubclassOf(typeof(CircuitBase)))
            {
                return (CircuitBase)newObj;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// This method iterates through all wires and groups them into a wire cluster.
        /// This allows us to see all objects connected to a specific wire network.
        /// </summary>
        /// <param name="wires">All wires you want to group</param>
        /// <returns>A set of grouped wires</returns>
        public static List<WireGroup> CombineWires(List<Wire> wires)
        {
            List<WireGroup> groups = new List<WireGroup>();
            
            //Create an initial group
            groups.Add(new WireGroup(wires[0]));
            //Create a bool to detect if we found a group for the wire
            bool groupFound;
            for(int i = 1; i < wires.Count; i++)
            {
                groupFound = false;
                foreach(var group in groups)
                {
                    if (group.ContainsConnectingWire(wires[i]))//See if we can add the current wire to an existing group
                    {
                        group.Add(wires[i]);
                        groupFound = true;
                        break;
                    }
                }
                if(!groupFound)//If no group is found create a new group
                {
                    groups.Add(new WireGroup(wires[i]));
                }
            }

            List<WireGroup> newGroups = new List<WireGroup>();
            if (groups.Count > 1)
            {
                bool groupMerged;
                int jStart = 1;
                while (true)
                {
                    groupMerged = false;
                    for(int i = 0; i < groups.Count-1; i++)
                    {
                        for(int j = jStart; j < groups.Count; j++)
                        {
                            if (groups[i].Intersection(groups[j]))
                            {
                                groups[i] = WireGroup.Merge(groups[i], groups[j]);//Merge the groups into the first element
                                groups.RemoveAt(j);//Remove the group that was merged
                                groupMerged = true;
                            }
                        }
                        if (groupMerged)//If we have merged a group we need to restart
                        {
                            break;
                        }
                        jStart++;
                    }
                    if (!groupMerged)//If we have not merged a group then all groups have been merged
                    {
                        break;
                    }
                }
            }
            foreach(var group in groups)//Extract each coordinate in the group.
            {
                group.Process();
            }
            return groups;
        }
    }
}
