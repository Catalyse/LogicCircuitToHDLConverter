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
        public CircuitSymbol symbol;
        public string HDLGateNotation;
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
            symbol = _symbol;
        }

        public string WriteGateHDL(List<string> inputs, string outputName)
        {
            int size = GetSize();
            string output = "";
            int outputIterator = 1;
            if (inputs.Count == size)
            {
                if (size > 2)
                {
                    output += "\t" + HDLGateNotation + "(a=" + inputs[0] + ", b=" + inputs[1] + ", out=" + outputName + outputIterator + ");";
                    outputIterator++;
                    for (int i = 2; i < inputs.Count; i++)
                    {
                        output += "\t" + HDLGateNotation + "(a=" + outputName + (outputIterator - 1) + ", b=" + inputs[i] + ", out=" + outputName + outputIterator + ");";
                        outputIterator++;
                    }
                }
                else
                {
                    return "\t" + HDLGateNotation + "(a=" + inputs[0] + ", b=" + inputs[1] + ", out=" + outputName + ");";
                }
            }
            else
            {
                throw new Exception("Invalid Input Count Error: " + inputs.Count + " inputs were provided but the gate reports a size of " + size);
            }
            return output;
        }

        public int GetSize()
        {
            var identifier = symbol.CircuitId.Substring(32, 2);
            identifier = "0x" + identifier;
            return Convert.ToInt32(identifier, 16);//This is the size of the gate as reported by logiccircuit
        }

        public bool ConnectsToWireGroup(WireGroup group)
        {
            int size = GetSize();
            Coords baseCoord = symbol.Location;
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
    }
}
