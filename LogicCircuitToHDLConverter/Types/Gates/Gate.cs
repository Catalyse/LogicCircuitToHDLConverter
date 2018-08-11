using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    /// <summary>
    /// This struct is used to indentify the first pin starting at the top left or top right.
    /// </summary>
    public struct GatePinOffset
    {
        public GatePinOffset(int x, int y)
        {
            OffsetX = x;
            OffsetY = y;
        }
        public int OffsetX;
        public int OffsetY;
    }

    public class Gate
    {
        public CircuitSymbol Symbol;
        public string HDLGateNotation;
        public Dictionary<Coords, WireGroup> gateMap;
        public string OutputName;

        public readonly Dictionary<int, GatePinOffset> LeftPinOffset = new Dictionary<int, GatePinOffset>
        {
            { 1, new GatePinOffset(0, 2) },
            { 2, new GatePinOffset(0, 1) },
            { 3, new GatePinOffset(0, 1) },
            { 4, new GatePinOffset(0, 1) },
            { 5, new GatePinOffset(0, 1) },
            { 6, new GatePinOffset(0, 1) },
            { 7, new GatePinOffset(0, 1) },
            { 8, new GatePinOffset(0, 1) },
            { 9, new GatePinOffset(0, 1) },
            { 10, new GatePinOffset(0, 1) },
            { 11, new GatePinOffset(0, 1) },
            { 12, new GatePinOffset(0, 1) },
            { 13, new GatePinOffset(0, 1) },
            { 14, new GatePinOffset(0, 1) },
            { 15, new GatePinOffset(0, 1) },
            { 16, new GatePinOffset(0, 1) },
            { 17, new GatePinOffset(0, 1) },
            { 18, new GatePinOffset(0, 1) }
        };
        public readonly Dictionary<int, GatePinOffset> RightPinOffset = new Dictionary<int, GatePinOffset>
        {
            { 2, new GatePinOffset(3, 2) },
            { 3, new GatePinOffset(3, 2) },
            { 4, new GatePinOffset(3, 2) },
            { 5, new GatePinOffset(3, 3) },
            { 6, new GatePinOffset(3, 3) },
            { 7, new GatePinOffset(3, 4) },
            { 8, new GatePinOffset(3, 4) },
            { 9, new GatePinOffset(3, 5) },
            { 10, new GatePinOffset(3, 5) },
            { 11, new GatePinOffset(3, 6) },
            { 12, new GatePinOffset(3, 6) },
            { 13, new GatePinOffset(3, 7) },
            { 14, new GatePinOffset(3, 7) },
            { 15, new GatePinOffset(3, 8) },
            { 16, new GatePinOffset(3, 8) },
            { 17, new GatePinOffset(3, 9) },
            { 18, new GatePinOffset(3, 9) }
        };

        public Gate(CircuitSymbol _symbol)
        {
            Symbol = _symbol;
            gateMap = new Dictionary<Coords, WireGroup>();
        }

        /// <summary>
        /// NOTE: There is a CUSTOM implementation of this method for the NOT gate
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputName"></param>
        /// <returns></returns>
        public string WriteGateHDL(List<WireGroup> groups, LogicalCircuit circuit)
        {
            int size = GetSize();
            string output = "";
            string tempName = "";
            string overrideOutputName = "";

            bool gateConnectsToOutput = false;
            Coords outputCoords = new Coords(Symbol.Location);
            outputCoords.x = outputCoords.x + RightPinOffset[size].OffsetX;
            outputCoords.y = outputCoords.y + RightPinOffset[size].OffsetY;
            foreach(var group in groups)
            {
                if (ConnectsToRightWireGroup(group))
                {
                    if(group.outputList.Count > 0)
                    {
                        gateConnectsToOutput = true;
                        overrideOutputName = group.outputList[0];
                        tempName = overrideOutputName;
                    }
                }
            }
            if (!gateConnectsToOutput)
            {
                tempName = OutputName;
            }

            Dictionary<int, string> inputs = FindConnectedGroups(groups);
            if (inputs.Count == size)
            {
                if (size > 2)
                {
                    if (gateConnectsToOutput)
                        tempName = Converter.CheckUnusedName(overrideOutputName, circuit);
                    else
                        tempName = Converter.CheckUnusedName(OutputName, circuit);

                    output += "\t" + HDLGateNotation + "(a=" + inputs[0] + ", b=" + inputs[1] + ", out=" + tempName + ");" + Environment.NewLine;
                    for (int i = 2; i < inputs.Count; i++)
                    {
                        if (i == (inputs.Count - 1))
                        {
                            output += "\t" + HDLGateNotation + "(a=" + tempName + ", b=" + inputs[i];

                            if (gateConnectsToOutput)
                                tempName = overrideOutputName;
                            else
                                tempName = OutputName;
                            output += ", out=" + tempName + ");" + Environment.NewLine;
                        }
                        else
                        {
                            output += "\t" + HDLGateNotation + "(a=" + tempName + ", b=" + inputs[i];
                            if (gateConnectsToOutput)
                                tempName = Converter.CheckUnusedName(overrideOutputName, circuit);
                            else
                                tempName = Converter.CheckUnusedName(OutputName, circuit);

                            output += ", out=" + tempName + ");" + Environment.NewLine;
                        }
                    }
                }
                else
                {
                    return "\t" + HDLGateNotation + "(a=" + inputs[0] + ", b=" + inputs[1] + ", out=" + tempName + ");" + Environment.NewLine;
                }
            }
            else
            {
                throw new Exception("Invalid Input Count Error: " + inputs.Count + " inputs were provided but the gate reports a size of " + size);
            }
            return output;
        }

        private Dictionary<int, string> FindConnectedGroups(List<WireGroup> groups)
        {
            Dictionary<int, string> returnDictionary = new Dictionary<int, string>();
            for(int i = 0; i < GetSize(); i++)
            {
                Coords current = new Coords(Symbol.Location);
                current.x = current.x + LeftPinOffset[GetSize()].OffsetX;
                if(i == 1 && GetSize() == 2)
                {
                    current.y = current.y + LeftPinOffset[GetSize()].OffsetY + 2;
                }
                else
                {
                    current.y = current.y + LeftPinOffset[GetSize()].OffsetY + i;
                }
                foreach(var group in groups)
                {
                    if (group.coords.Exists(x => x.x == current.x && x.y == current.y))
                    {
                        returnDictionary.Add(i, group.inputList[0]);
                    }
                }
            }
            if(returnDictionary.Count != GetSize())
            {
                throw new Exception("Gate -- FindConnectedGroups: Unable to match a wire group to each input!");
            }
            return returnDictionary;
        }

        /// <summary>
        /// This read the gate moniker and determines the number of left inputs
        /// </summary>
        /// <returns>Number of outputs for the gate</returns>
        public int GetSize()
        {
            var identifier = Symbol.CircuitId.Substring(32, 2);
            identifier = "0x" + identifier;
            return Convert.ToInt32(identifier, 16);//This is the size of the gate as reported by logiccircuit
        }

        public bool ConnectsToLeftWireGroup(WireGroup group)
        {
            int size = GetSize();
            Coords baseCoord = new Coords(Symbol.Location);
            if(size > 2)
            {
                for (int i = 0; i < size; i++)
                {
                    if(group.coords.Exists(x => x.x == baseCoord.x + LeftPinOffset[size].OffsetX && x.y == baseCoord.y + LeftPinOffset[size].OffsetY + i))
                    {
                        return true;
                    }
                }
            }
            else if (size == 2)
            {
                return group.coords.Exists(x => x.x == baseCoord.x + LeftPinOffset[size].OffsetX && x.y == baseCoord.y + LeftPinOffset[size].OffsetY) ||
                    group.coords.Exists(x => x.x == baseCoord.x + LeftPinOffset[size].OffsetX && x.y == baseCoord.y + LeftPinOffset[size].OffsetY + 2);
            }
            else
            {
                return group.coords.Exists(x => x.x == baseCoord.x + LeftPinOffset[1].OffsetX && x.y == baseCoord.y + LeftPinOffset[1].OffsetY);
            }
            return false;
        }

        public bool ConnectsToRightWireGroup(WireGroup group)
        {
            Coords baseCoord = new Coords(Symbol.Location);
            baseCoord.x = baseCoord.x + RightPinOffset[GetSize()].OffsetX;
            baseCoord.y = baseCoord.y + RightPinOffset[GetSize()].OffsetY;

            return group.coords.Exists(x => x.x == baseCoord.x && x.y == baseCoord.y);
        }

        /*
        public bool MapGroupToInput(List<WireGroup> groups)
        {
            int size = GetSize();
            Coords baseCoord = symbol.Location;
            if (size > 2)
            {
                for (int i = 0; i < size; i++)
                {
                    if (group.coords.Exists(x => x.x == baseCoord.x + LeftPinOffset[size].OffsetX && x.y == baseCoord.y + LeftPinOffset[size].OffsetY + i))
                    {
                        gateMap.Add(new Coords(baseCoord.x + LeftPinOffset[size].OffsetX, baseCoord.y + LeftPinOffset[size].OffsetY + i), group);
                        return true;
                    }
                }
            }
            else if (size == 2)
            {
                return group.coords.Exists(x => x.x == baseCoord.x + LeftPinOffset[size].OffsetX && x.y == baseCoord.y + LeftPinOffset[size].OffsetY) ||
                    group.coords.Exists(x => x.x == baseCoord.x + LeftPinOffset[size].OffsetX && x.y == baseCoord.y + LeftPinOffset[size].OffsetY + 2);
            }
            else
            {
                return group.coords.Exists(x => x.x == baseCoord.x + LeftPinOffset[1].OffsetX && x.y == baseCoord.y + LeftPinOffset[1].OffsetY);
            }
            return false;
        }*/
    }
}
